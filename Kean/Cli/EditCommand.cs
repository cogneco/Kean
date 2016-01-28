// 
//  EditAction.cs
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

namespace Kean.Cli
{
	public enum EditCommand : 
		byte
	{
		None = 0,
		Home = 1,		// a
		LeftArrow = 2,	// b
		Copy = 3,		// c
		Exit = 4,		// d
		End = 5,		// e
		RightArrow = 6, // f
		// 7			// g
		Backspace = 8,	// h
		Tab = 9,		// i
		Enter = 10,		// j
		// 11			// k
		// 12			// l
		DownArrow = 13, // m
		// 14			// n
		UpArrow = 15,	// o
		// 16			// p
		Quit = 17,		// q
		// 18			// r
		// 19			// s
		// 20			// t
		// 21			// u
		Paste = 22,		// v
		// 23			// w
		Cut = 24,		// x
		Redo = 25,		// y
		Undo = 26,		// z
		Delete = 127,
	}
}
