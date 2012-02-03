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
using System.Net;
using W0utan.Gasq.Enums;

namespace W0utan.Gasq.Interfaces
{
    /// <summary>
    /// Interface for server query classes.
    /// </summary>
    public interface IServerQuery
    {
        int PingServer(int timeOut = 100);
        /// <summary>
        /// Query all server infos.
        /// </summary>
        /// <param name="gs"></param>
        void QueryServerInfo(ref GameServer gs);
    }
}