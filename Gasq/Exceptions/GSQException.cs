﻿/*
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

namespace W0utan.Gasq.Exceptions
{
    /// <summary>
    /// Exception thrown if something went wrong ;)
    /// </summary>
    public class GSQException : Exception
    {
        public GSQException() : base() { }
        public GSQException(string msg) : base(msg) { }
        public GSQException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
