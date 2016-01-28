// 
//  IError.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2009 Simon Mika
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

namespace Kean.Error
{
	public interface IError
	{
		DateTime Time { get; }
		Level Level { get; }
		string Title { get; }
		string Message { get; }
		string AssemblyName { get; }
		string AssemblyVersion { get; }
		string Type { get; }
		string Method { get; }
		string Filename { get; }
		int Line { get; }
		int Column { get; }
	}
}
