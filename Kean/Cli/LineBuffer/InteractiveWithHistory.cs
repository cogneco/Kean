// 
//  InteractiveWithHistory.cs
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
using Integer = Kean.Math.Integer;

namespace Kean.Cli.LineBuffer
{
	public class InteractiveWithHistory :
		Interactive
	{
		int position = 0;
		int Position
		{
			get { return position; }
			set { this.position = Integer.Modulo(value, this.size); }
		}
		int last = 0;
		int size = 50;
		int first = 0;
		string[] history;

		public InteractiveWithHistory(ITerminal terminal) :
			base(terminal)
		{
			this.history = new string[this.size];
		}

		protected override void OnUp()
		{
			if (this.Position != this.first)
			{
				if (this.position == this.last)
					this.history[this.Position] = this.Line;
				this.Replace(this.history[Integer.Modulo(--this.Position, this.size)]);
			}
		}

		protected override void OnDown()
		{
			if (this.Position != this.last)
				this.Replace(this.history[++this.Position % this.size]);
		}

		protected override void OnEnter()
		{
			if (this.Line.NotEmpty() && this.Line != this.history[Integer.Modulo(this.last - 1, this.size)])
			{
				this.history[this.last] = this.Line;
				this.last = (this.last + 1) % this.size;
				if (this.last == this.first)
					this.first = (this.first + 1) % this.size;
			}
			this.position = this.last;
			base.OnEnter();
		}
	}
}
