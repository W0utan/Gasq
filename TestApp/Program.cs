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
using System.Net.Sockets;
using System.Net;
using System.Threading;
using W0utan.Gasq;
using W0utan.Gasq.Enums;
using W0utan.Gasq.Exceptions;

namespace W0utan
{
    class TestApp
    {
        static Object thisLock = new Object();
        static void Main(string[] args)
        {
                GameServer[] gsArray = {
                                           new GameServer("62.213.68.113", 27015, EGame.Source),
                                           new GameServer("188.138.88.35", 31000, EGame.Source),
                                           new GameServer("84.200.9.104", 27015, EGame.Source),
                                           new GameServer("78.138.99.214", 27025, EGame.Source),
                                           new GameServer("84.201.1.20", 27015, EGame.Source),
                                           new GameServer("193.192.59.120", 27150, EGame.Source),
                                           new GameServer("62.141.42.97", 27055, EGame.Source),
                                           new GameServer("222.56.16.77", 27016, EGame.Source),
                                           new GameServer("218.93.192.12", 27019, EGame.Source),
                                           new GameServer("103.23.148.67", 27064, EGame.Source),
                                           new GameServer("85.227.251.250",27015 , EGame.Source),
                                           new GameServer("217.78.19.179", 27015, EGame.Source),
                                           new GameServer("90.225.86.248", 27015, EGame.Source)
                                       };

                Thread[] threads = new Thread[gsArray.Length];

                for (int i = 0; i < gsArray.Length; i++)
                {
                    threads[i] = new Thread(new ParameterizedThreadStart(PrintGame), 0);
                    threads[i].Start(gsArray[i]);
                }
            Console.ReadKey();
        }

        static void PrintGame(object obj)
        {
            GameServer gs = (GameServer)obj;
            try
            {
                gs.GetInfo();
                lock (thisLock)
                {
                    Console.WriteLine("Dir:\t" + gs.GameDirectory);
                    Console.WriteLine("Name:\t" + gs.Name);
                    Console.WriteLine("Map:\t" + gs.Map);
                    Console.WriteLine("Vers:\t" + gs.GameVersion);
                    Console.WriteLine("Player:\t" + gs.PlayerCount + "/" + gs.MaxPlayerCount);

                    Console.WriteLine("");
                }
            }
            catch (GSQException ex)
            {
                Console.Write(gs.IP + ": ");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
