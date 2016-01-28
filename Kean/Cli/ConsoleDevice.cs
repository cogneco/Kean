// 
//  ConsoleStream.cs
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
using Kean;
using Kean.Extension;
using Uri = Kean.Uri;

namespace Kean.Cli
{
	class ConsoleDevice :
		IO.ICharacterInDevice,
		IO.ICharacterOutDevice
	{
		public bool LocalEcho { get; set; }
		public bool Opened { get { return true; } }
		public bool Empty { get { return !Console.KeyAvailable; } }
		public Uri.Locator Resource { get { return new Uri.Locator("console://", "/"); } }
		public event Action<EditCommand> Command;
		public ConsoleDevice()
		{
			this.LocalEcho = true;
			if (Kean.Environment.IsWindows)
				try
				{
					int w = Console.WindowWidth;
				}
				catch (System.IO.IOException)
				{
					ConsoleDevice.AllocateConsole();
				}
		}
		public char? Read()
		{
			char? result = null;
			do
			{
				ConsoleKeyInfo key = Console.ReadKey(this.LocalEcho);
				switch (key.Key)
				{
					case ConsoleKey.Home:
						this.Command.Call(EditCommand.Home);
						break;
					case ConsoleKey.LeftArrow:
						this.Command.Call(EditCommand.LeftArrow);
						break;
					case ConsoleKey.Delete:
						this.Command.Call(EditCommand.Delete);
						break;
					case ConsoleKey.End:
						this.Command.Call(EditCommand.End);
						break;
					case ConsoleKey.RightArrow:
						this.Command.Call(EditCommand.RightArrow);
						break;
					case ConsoleKey.Backspace:
						this.Command.Call(EditCommand.Backspace);
						break;
					case ConsoleKey.Tab:
						this.Command.Call(EditCommand.Tab);
						break;
					case ConsoleKey.Enter:
						this.Command.Call(EditCommand.Enter);
						break;
					case ConsoleKey.DownArrow:
						this.Command.Call(EditCommand.DownArrow);
						break;
					case ConsoleKey.UpArrow:
						this.Command.Call(EditCommand.UpArrow);
						break;
					default:
						result = key.KeyChar;
						break;
				}
			}
			while (!result.HasValue);
			return result;
		}
		public char? Peek()
		{
			int result = Console.In.Peek();
			return result > 0 ? (char?)result : null;
		}
		public bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			foreach (char c in buffer)
				Console.Write(c);
			return true;
		}
		public bool Close()
		{
			return true;
		}
		#region ITerminal Members
		public bool Readable { get { return true; } }
		public bool Writable { get { return true; } }
		#endregion
		#region IOutDevice Members
		public bool AutoFlush
		{
			get { return true; }
			set { ; }
		}
		public bool Flush()
		{
			return true;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
		static extern int AllocateConsole();
	}
}
