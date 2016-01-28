// 
//  Interactive.cs
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
using Text = Kean.IO.Text;

namespace Kean.Cli.LineBuffer
{
	public class Interactive :
		Abstract
	{
		protected Text.Buffer Buffer { get; set; }
		bool help;

		protected override string Line { get { return this.Buffer.ToString(); } }
		protected override int LineLength { get { return this.Buffer.Length; } }

		public Interactive(ITerminal terminal) :
			base(terminal)
		{
			this.Buffer = new Text.Buffer();
		}
		protected override void Insert(char c)
		{
			this.help = false;
			this.terminal.Out.Write(c);
			if (!this.Buffer.Insert(c))
				this.ReplaceFromCursor(this.Buffer.Substring(this.Buffer.Cursor));
		}
		protected override void OnCommand(EditCommand command)
		{
			if (command != EditCommand.Tab)
				this.help = false;
			base.OnCommand(command);
		}
		protected override void OnDelete()
		{
			if (this.Buffer.Delete())
				this.ReplaceFromCursor(this.Buffer.Substring(this.Buffer.Cursor) + " ");
		}

		protected override void OnBackspace()
		{
			if (this.Buffer.Backspace())
			{
				this.MoveCursor(-1);
				this.ReplaceFromCursor(this.Buffer.Substring(this.Buffer.Cursor) + " ");
			}
		}
		protected override void OnLeft()
		{
			if (this.Buffer.MoveCursorLeft())
				this.MoveCursor(-1);
		}
		protected override void OnRight()
		{
			if (this.Buffer.MoveCursorRight())
				this.MoveCursor(1);
		}
		protected override void OnHome()
		{
			this.MoveCursor(this.Buffer.MoveCursorHome());
		}
		protected override void OnEnd()
		{
			this.MoveCursor(this.Buffer.MoveCursorEnd());
		}
		protected override void ClearLine()
		{
			this.Buffer.Clear();
		}
		protected override void OnTab()
		{
			string leftOfCursor = this.Buffer.LeftOfCursor;
			if ((this.help || this.Complete.IsNull()) && this.Help.NotNull())
			{
				this.terminal.Out.WriteLine();
				this.terminal.Out.Write(this.Help(leftOfCursor) + this.Prompt + this.Line);
				this.MoveCursor(-this.Buffer.CursorToEnd);
				this.help = false;
			}
			else if (this.Complete.NotNull())
			{
				string result = this.Complete(leftOfCursor);
				if (result == leftOfCursor)
					this.help = true;
				else if (result.StartsWith(leftOfCursor))
				{
					string delta = result.Substring(leftOfCursor.Length);
					if (this.Buffer.RightOfCursor.StartsWith("."))
						this.Buffer.Delete();
					this.terminal.Out.Write(delta + this.Buffer.RightOfCursor);
					this.MoveCursor(-this.Buffer.CursorToEnd);
					this.Buffer.Insert(delta);
				}
			}
		}
		public override void WriteLine(string value)
		{
			lock (this.Lock)
			{
				if (this.Executing)
					this.terminal.Out.WriteLine(value);
				else
				{
					this.terminal.Home();
					this.terminal.Out.WriteLine(value.PadRight(this.PromptLength + this.LineLength));
					this.terminal.Out.Write(this.Prompt + this.Line);
				}
			}
		}
		protected virtual bool MoveCursor(int delta)
		{
			return delta == 0 || this.terminal.MoveCursor(delta) || 
				this.terminal.Out.Write(delta > 0 ? this.Buffer.Substring(this.Buffer.Cursor - delta, delta) : new string('\b', -delta));
		}
		protected virtual bool ReplaceFromCursor(string value)
		{
			return this.terminal.Out.Write(value) && this.MoveCursor(-value.Length);
		}
		protected virtual void Replace(string line)
		{
			int oldLength = this.Buffer.Length;
			this.OnHome();
			this.Buffer = line;
			this.terminal.Out.Write(((string)this.Buffer).PadRight(oldLength));
			if (oldLength > this.Buffer.Length)
				this.MoveCursor(this.Buffer.Length - oldLength);
		}
	}
}
