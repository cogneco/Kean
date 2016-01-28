// 
//  Server.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Kean.IO.Net.Telnet
{
	enum Command :
		byte
	{
		/// <summary>
		/// End of subnegotiation parameters
		/// </summary>
		SE = 240,
		/// <summary>
		/// No operation
		/// </summary>
		NOP = 241,
		/// <summary>
		/// Data mark
		/// </summary>
		DM = 242,
		/// <summary>
		/// Break
		/// </summary>
		BRK = 243,
		/// <summary>
		/// Suspend
		/// </summary>
		IP = 244,
		/// <summary>
		/// Abort output
		/// </summary>
		AO = 245,
		/// <summary>
		/// Are you there
		/// </summary>
		AYT = 246,
		/// <summary>
		/// Erase character
		/// </summary>
		EC = 247,
		/// <summary>
		/// Erase line
		/// </summary>
		EL = 248,
		/// <summary>
		/// Go ahead
		/// </summary>
		GA = 249,
		/// <summary>
		/// Subnegotiation
		/// </summary>
		SB = 250,
		/// <summary>
		/// Will
		/// </summary>
		WILL = 251,
		/// <summary>
		/// Wont
		/// </summary>
		WONT = 252,
		/// <summary>
		/// Do
		/// </summary>
		DO = 253,
		/// <summary>
		/// Dont
		/// </summary>
		DONT = 254,
		/// <summary>
		/// Interpret as command
		/// </summary>
		IAC = 255,
	}
}
