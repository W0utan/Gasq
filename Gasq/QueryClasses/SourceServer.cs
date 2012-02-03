/*
    Gasq - Game Server Query Library. Small library to query game server information.
    Copyright (C) 2012  _w0utan_

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using W0utan.Gasq.Exceptions;
using W0utan.Gasq.Helper;
using W0utan.Gasq.Interfaces;

namespace W0utan.Gasq.QueryClasses
{
    /// <summary>
    /// Queries Source server like Counter Strike and Counter Strike Source.
    /// TODO Atm only Counter Strike Source is supported!
    /// </summary>
    internal class SourceServer : IServerQuery
    {
        private int receiveTimeout=2000; //std receiveTimeout
        private IPAddress IP = null;
        private int Port = 0;

        /// <summary>
        /// Gets or sets the receiveTimeout for the requests.
        /// </summary>
        public int ReceiveTimeout 
        {
            get {return receiveTimeout;} 
            set {receiveTimeout=value;}
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ip">Server IP</param>
        /// <param name="port">Server Port</param>
        public SourceServer(string ip, int port)
        {
            IP = IPAddress.Parse(ip);
            Port = port;
        }

        /// <summary>
        /// Creates a new UDPClient and set the receiveTimeout and Ttl (=100)
        /// </summary>
        /// <returns></returns>
        private UdpClient CreateUDPClient()
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Client.ReceiveTimeout = receiveTimeout;
            udpClient.Ttl = 100;
            return udpClient;
        }

        /// <summary>
        /// Tries to ping the server via A2A_PING. If that fails it tries
        /// an ICMP echo reply message.
        /// </summary>
        /// <param name="timeOut">timeOut for the Ping</param>
        /// <returns></returns>
        public int PingServer(int timeOut=100)
        {
            DateTime dtStart;
            DateTime dtEnd;

            using(var udpClient = CreateUDPClient())
            {
                udpClient.Client.ReceiveTimeout = timeOut;

                //Send A2A_PING
                byte[] send_buffer = { 0xFF, 0xFF, 0xFF, 0xFF, 0x69 };

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                udpClient.Connect(IP, Port);
                dtStart = DateTime.Now;

                udpClient.Send(send_buffer, send_buffer.Length);
                dtEnd = DateTime.Now;
            
                try
                {
                    var responseData = udpClient.Receive(ref RemoteIpEndPoint);
                }
                catch (SocketException ex) //If A2A_PING doesn't work, try via ICMP
                {
                    udpClient.Close();
                    Ping p = new Ping();
                    try
                    {
                        dtStart = DateTime.Now;
                        PingReply reply = p.Send(IP, Port);
                        if (reply.Status == IPStatus.Success)
                        {
                            dtEnd = DateTime.Now;
                        }
                    }
                    catch 
                    { 
                        throw new GSQException("No data received! SocketException thrown, see InnerException", ex);
                    }
                
                }
            }
            return dtEnd.Subtract(dtStart).Milliseconds;
        }

        /// <summary>
        /// Tries to get all the server infos.
        /// </summary>
        /// <param name="gs">GameServer instance to fill with the information.</param>
        public void QueryServerInfo(ref GameServer gs)
        {
            if (gs == null)
                throw new ArgumentException("Parameter gs must be not null!");

            //Send A2S_INFO  --->  -1, 'T', "Source Engine Query"
            byte[] send_buffer = { 0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 
                                     0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00 };

            using(var udpClient = CreateUDPClient())
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                udpClient.Connect(IP, Port);
                udpClient.Send(send_buffer, send_buffer.Length);
                byte[] responseData;
                try
                {
                    responseData = udpClient.Receive(ref RemoteIpEndPoint);
                }
                catch (Exception ex)
                {
                    throw new GSQException("No data received! SocketException thrown, see InnerException", ex);
                }
                

                using (MemoryStream data = new MemoryStream(responseData))
                { 
                    if (data.Length <= 0)
                        throw new GSQException("No data received!");

                    data.Position = 4; //First 4 0xFF bytes...unimportant

                    //Type and Version
                    gs.Type = (byte)data.ReadByte();
                    gs.Version = (byte)data.ReadByte();
                    if (gs.Type != 'I') //TODO Maybe check version
                        throw new GSQException("Server is not a Source Server!");

                    //Read Name, Map, Game dir and Description
                    gs.Name             = data.ReadUTFString();
                    gs.Map              = data.ReadUTFString();
                    gs.GameDirectory    = data.ReadUTFString();
                    gs.GameDescription  = data.ReadUTFString();

                    //Steam App ID
                    byte[] bufBytesArr  = new byte[2];
                    bufBytesArr[0]      = (byte)data.ReadByte();
                    bufBytesArr[1]      = (byte)data.ReadByte();
                    gs.AppID            = BitConverter.ToInt16(bufBytesArr, 0);

                    //Player Info
                    gs.PlayerCount      = (ushort)data.ReadByte();
                    gs.MaxPlayerCount   = (ushort)data.ReadByte();
                    gs.BotCount         = (ushort)data.ReadByte();

                    //Server Type
                    char cDedicated = (char)data.ReadByte();
                    if (cDedicated == 'l')
                        gs.ServerType = Enums.EServerType.Listen;
                    else if(cDedicated == 'd')
                        gs.ServerType = Enums.EServerType.Dedicated;
                    else if (cDedicated == 'p')
                        gs.ServerType = Enums.EServerType.SourceTV;

                    //Operating System the server is hosted on
                    char cHost  = (char)data.ReadByte();
                    if (cHost == 'l')
                        gs.OS = Enums.EHostOS.Linux;
                    else if (cHost == 'w')
                        gs.OS = Enums.EHostOS.Windows;

                    //Password and VACSecured?
                    gs.IsPasswordProtected  = data.ReadByte() == 0x01;
                    gs.IsVACSecured         = data.ReadByte() == 0x01;

                    //Game Version
                    gs.GameVersion = data.ReadUTFString();
                }
            }
        }
    }
}
