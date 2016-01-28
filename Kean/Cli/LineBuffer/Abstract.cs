// 
//  Abstract.cs
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

namespace Kean.Cli.LineBuffer
{
	public abstract class Abstract :
		Synchronized,
		IDisposable
	{
		protected ITerminal terminal;
		bool exit = false;
		bool executing;
		protected bool Executing
		{
			get { lock (this.Lock) return this.executing; }
			set { lock (this.Lock) this.executing = value; }
		}


		public Func<string, bool> Execute { get; set; }
		public Func<string, string> Complete { get; set; }
		public Func<string, string> Help { get; set; }
		public Func<string, string> Error { get; set; }
		public Func<string, bool> RequestType { get; set; }
		string prompt;
		public string Prompt 
		{
			get { return this.prompt; }
			set { this.PromptLength = (this.prompt = value).Length; }
		}
		protected int PromptLength { get; private set; }

		protected abstract string Line { get; }
		protected abstract int LineLength { get; }

		protected Abstract(ITerminal terminal)
		{
			this.terminal = terminal;
			this.terminal.Command += this.OnCommand;
		}
		#region Input
		protected virtual void OnCommand(EditCommand command)
		{
			switch (command)
			{
				case EditCommand.None:
					break;
				case EditCommand.Home:
					this.OnHome();
					break;
				case EditCommand.LeftArrow:
					this.OnLeft();
					break;
				case EditCommand.Copy:
					break;
				case EditCommand.Exit:
					this.OnExit();
					break;
				case EditCommand.End:
					this.OnEnd();
					break;
				case EditCommand.RightArrow:
					this.OnRight();
					break;
				case EditCommand.Backspace:
					this.OnBackspace();
					break;
				case EditCommand.Tab:
					this.OnTab();
					break;
				case EditCommand.Quit:
				case EditCommand.Enter:
					this.OnEnter();
					break;
				case EditCommand.DownArrow:
					this.OnDown();
					break;
				case EditCommand.UpArrow:
					this.OnUp();
					break;
				case EditCommand.Paste:
					break;
				case EditCommand.Cut:
					break;
				case EditCommand.Redo:
					break;
				case EditCommand.Undo:
					break;
				case EditCommand.Delete:
					this.OnDelete();
					break;
			}
		}
		protected virtual void OnHome() { }
		protected virtual void OnLeft() { }
		protected virtual void OnExit() 
		{
			this.exit = true;
			this.Close();
		}
		protected virtual void OnEnd() { }
		protected virtual void OnRight() { }
		protected virtual void OnBackspace() { }
		protected virtual void OnTab() { }
		protected virtual void OnEnter() 
		{
			this.Executing = true;
			string line = this.Line;
			this.ClearLine();
			if (this.terminal.Echo)
				this.terminal.Out.WriteLine();
			if (line.StartsWith("?"))
			{
				if (this.RequestType.NotNull())
					this.RequestType(line.Substring(1));
			}
			else if (this.Execute.NotNull())
				this.Execute(line);
			this.Executing = false;
			this.terminal.Out.Write(this.Prompt);
		}
		protected virtual void OnDown() { }
		protected virtual void OnUp() { }
		protected virtual void OnDelete() { }

		public void Read()
		{
			this.terminal.Out.Write(this.Prompt);
			while (this.terminal.In.Next() && !this.exit)
				this.Insert(this.terminal.In.Last);
		}
		protected abstract void Insert(char c);
		protected abstract void ClearLine();
		#endregion
		#region Output
		public virtual void WriteLine(string value)
		{
			lock (this.Lock)
				this.terminal.Out.WriteLine(value);
		}
		#endregion
		#region Close & Dispose
		public void Close()
		{
			lock (this.Lock)
				if (this.terminal.NotNull())
				{
					this.terminal.Close();
					this.terminal = null;
				}
		}

		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#endregion
	}
}