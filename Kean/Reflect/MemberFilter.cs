// 
//  MemberFilter.cs
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

using System;

namespace Kean.Reflect
{
	[Flags]
	public enum MemberFilter
	{
		Default = 0x69,
		Instance = 0x0001,
		Static = 0x0002,
		NonPublic = 0x0004,
		Public = 0x0008,
		Field = 0x0010,
		Property = 0x0020,
		Event = 0x0040,
		Method = 0x0080,
		Constructor = 0x0100,
		All = 0xffff,
	}
}
