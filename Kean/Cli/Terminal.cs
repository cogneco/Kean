// 
//  Terminal.cs
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
using Geometry2D = Kean.Math.Geometry2D;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Cli
{
	public class Terminal :
		ITerminal
	{
		public char[] NewLine
		{
			get { return this.Out.NewLine; }
			set { this.Out.NewLine = value; }
		}
		#region Constructors
		protected Terminal(IO.IByteDevice device) :
			this(IO.CharacterDevice.Open(device))
		{
		}
		protected Terminal(IO.ICharacterDevice device) :
			this(device, device)
		{
		}
		protected Terminal(IO.IByteInDevice read, IO.IByteOutDevice write) :
			this(IO.CharacterDevice.Open(IO.ByteDeviceCombiner.Open(read, write)))
		{
		}
		protected Terminal(IO.ICharacterInDevice inDevice, IO.ICharacterOutDevice outDevice)
		{
			this.In = IO.CharacterReader.Open(IO.Filter.CharacterInDevice.Open(inDevice, this.FilterInput));
			this.Out = IO.CharacterWriter.Open(outDevice) ?? new IO.Null.CharacterWriter();
			if (this.Out.NotNull())
				this.Out.AutoFlush = true;
		}
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { throw new NotImplementedException(); } }
		public bool Opened { get { return this.In.Opened && this.Out.Opened; } }
		public bool Close()
		{
			this.OnCommand(EditCommand.Quit);
			return this.In.Close() && this.Out.Close();
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region ITerminal Members
		public IO.ICharacterReader In { get; private set; }
		public IO.ICharacterWriter Out { get; private set; }
		public virtual bool Echo { get; set; }
		public event Action<EditCommand> Command;
		public virtual bool MoveCursor(Geometry2D.Integer.Size delta)
		{
			return false;
		}
		public virtual bool MoveCursor(int delta)
		{
			return false;
		}
		public virtual bool ClearLine()
		{
			return false;
		}
		public virtual bool ClearLineFromCursor()
		{
			return false;
		}
		public virtual bool Home()
		{
			this.Out.Write('\r');
			return true;
		}
		public virtual bool End()
		{
			return false;
		}
		public virtual bool Clear()
		{
			return false;
		}
		#endregion
		protected virtual void OnCommand(EditCommand action)
		{
			this.Command.Call(action);
		}
		char[] FilterInput(Func<char?> read)
		{
			Collection.IList<char> buffer = new Collection.List<char>();
			char? next;
			while (buffer.Count <= 0 && (next = read()).HasValue)
				switch (next.Value)
				{
					case '\0':
						break;
					case '\x04':
						this.OnCommand(EditCommand.Exit);
						break;
					case '\b':
						this.OnCommand(EditCommand.Backspace);
						break;
					case '\t':
						this.OnCommand(EditCommand.Tab);
						break;
					case '\n':
						this.OnCommand(EditCommand.Enter);
						break;
					case '\r':
						this.OnCommand(EditCommand.Enter);
						if ((next = read()).HasValue && (next.Value != '\n' && next.Value != '\0'))
							buffer.Add(next.Value);
						break;
					default:
						buffer.Add(next.Value);
						break;
				}
			return buffer.ToArray();
		}
		#region Static Open
		public static Terminal Open(IO.IByteDevice device)
		{
			return Terminal.Open(IO.CharacterDevice.Open(device));
		}
		public static Terminal Open(IO.ICharacterDevice device)
		{
			return Terminal.Open(device, device);
		}
		public static Terminal Open(IO.IByteInDevice read, IO.IByteOutDevice write)
		{
			return Terminal.Open(IO.CharacterDevice.Open(IO.ByteDeviceCombiner.Open(read, write)));
		}
		public static Terminal Open(IO.ICharacterInDevice inDevice, IO.ICharacterOutDevice outDevice)
		{
			return inDevice.NotNull() || outDevice.NotNull() ? new Terminal(inDevice, outDevice) : null;
		}
		#endregion
	}
}
