//
//  ContextNotCreatable.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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

namespace Kean.Draw.OpenGL.Exception
{
    public class ContextNotCreatable : 
        Exception
    {
        internal ContextNotCreatable(System.Exception inner)
            : base(inner, Error.Level.Recoverable, "Unable to Create Context", "Unable to create context: \"{0}\"", inner.Message)
        { }
    }
}
