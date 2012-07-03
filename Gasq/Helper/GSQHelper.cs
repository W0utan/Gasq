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
using System.Collections.Generic;
using System.IO;

namespace W0utan.Gasq.Helper
{
    /// <summary>
    /// Little Helper Class for different purposes.
    /// </summary>
    public static class GSQHelper
    {
        /// <summary>
        /// Reads a UTF string from a MemoryStream. Method will read until it finds 0x0 
        /// or reaches the end of the stream.
        /// </summary>
        /// <param name="ms">MemoryStream to read a UTF string from.</param>
        /// <returns></returns>
        public static string ReadUTFString(this MemoryStream ms)
        {   
            int buffer = ms.ReadByte();
            List<byte> byteBuf = new List<byte>();
            while(buffer != 0 && buffer != -1)
            {
                byteBuf.Add((byte)buffer);
                buffer = ms.ReadByte();
            }
            var tempString = System.Text.Encoding.UTF8.GetString(byteBuf.ToArray());

            return tempString.Trim();
        }
    }
}
