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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W0utan.Gasq.Data;
using W0utan.Gasq.Enums;
using W0utan.Gasq.QueryClasses;
using W0utan.Gasq.Interfaces;
using System.Net;

namespace W0utan.Gasq
{
    /// <summary>
    /// Represents a game server with all its information.
    /// </summary>
    public class GameServer
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="ip">Server IP.</param>
        /// <param name="port">Server port.</param>
        /// <param name="gameType">Game running on this server.</param>
        public GameServer(string ip, int port, EGame gameType)
        {
            IP = ip;
            Port = port;
            GameType = gameType;
        }

        /// <summary>
        /// Gets or sets the game type.
        /// </summary>
        public EGame GameType{ get; set; }

        //Standard data
        /// <summary>
        /// Gets or sets the server IP.
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// Gets or sets the server port.
        /// </summary>
        public int Port { get; set; }
        
        //###################################
        // Used by this games:
        // Valve Source Server
        //###################################

        /// <summary>
        /// Server Type.
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Server Version
        /// </summary>
        public byte Version { get; set; }
        /// <summary>
        /// Name of the server/match
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Name of the map running on this server.
        /// </summary>
        public string Map { get; set; }
        /// <summary>
        /// Server directory.
        /// </summary>
        public string GameDirectory { get; set; }
        /// <summary>
        /// Description of this server/match
        /// </summary>
        public string GameDescription { get; set; }
        /// <summary>
        /// Application ID/Steam Application ID.
        /// </summary>
        public short AppID { get; set; }
        /// <summary>
        /// Amount of players online.
        /// </summary>
        public ushort PlayerCount { get; set; }
        /// <summary>
        /// Mayimum amount of possible players.
        /// </summary>
        public ushort MaxPlayerCount { get; set; }
        /// <summary>
        /// Number of active bots on this server.
        /// </summary>
        public ushort BotCount { get; set; }
        /// <summary>
        /// Type of server.
        /// </summary>
        public EServerType ServerType { get; set; }
        /// <summary>
        /// Operating system this server runs on.
        /// </summary>
        public EHostOS OS { get; set; }
        /// <summary>
        /// Does this server need a password?
        /// </summary>
        public bool IsPasswordProtected { get; set; }
        /// <summary>
        /// Is this server VAC secured?
        /// </summary>
        public bool IsVACSecured { get; set; }
        /// <summary>
        /// Version of the game/server.
        /// </summary>
        public string GameVersion { get; set; }

        /// <summary>
        /// Tries to get all the server infos.
        /// </summary>
        /// <param name="gs">GameServer instance to fill with the information.</param>
        public GameServer GetInfo()
        {
            IServerQuery query=null;
            GameServer gameServer = null;

            switch (GameType)
            {
                case EGame.Source:
                    query = new SourceServer(IP,Port);
                    var self = this;
                    query.QueryServerInfo(ref self);
                    break;
                default:
                    break;
            }

            return gameServer;
        }

        /// <summary>
        /// Tries to ping the server.
        /// </summary>
        /// <returns></returns>
        public int Ping()
        {
            IServerQuery query = null;
            int iPingResult = -1;
            switch (GameType)
            {
                case EGame.Source:
                    query = new SourceServer(IP, Port);
                    break;
                default:
                    break;
            }

            iPingResult = query.PingServer();

            return iPingResult;
        }
    }
}
