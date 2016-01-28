// 
//  Abstract.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2012 Simon Mika
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

using Error = Kean.Error;

namespace Kean.Math.Exception
{
    public class Abstract : 
        Error.Exception
    {
        internal Abstract(Error.Level level, string title, string message, params string[] arguments) : this(null, level, title, message, arguments) { }
        internal Abstract(System.Exception innerException, Error.Level level, string title, string message, params string[] arguments) : base(innerException, level, title, message, arguments) { }
    }
}
