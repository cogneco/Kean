// 
//  Simple.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2013 Simon Mika
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
using Kean;
using Kean.Extension;
using IO = Kean.IO;

namespace Kean.Cli.LineBuffer
{
	public class Simple : 
		Abstract
	{
		IO.Text.Builder builder = "";
		protected override string Line { get { return this.builder; } }
		int length;
		protected override int LineLength { get { return this.length; } }
		public Simple(ITerminal terminal) :
			base(terminal)
		{ }

		protected override void Insert(char c)
		{
			this.length++;
			this.builder.Append(c);
		}
		protected override void ClearLine()
		{
			this.length = 0;
			this.builder = "";
		}
	}
}
