// 
//  Server.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO.Net.Telnet
{
	public class Server :
		IByteDevice
	{
		IByteDevice backend;
		bool? echo;
		public bool Echo
		{
			get { return this.echo.HasValue ? this.echo.Value : false; }
			set
			{
				this.backend.Write(new byte[] { (byte)Command.IAC, (byte)(value ? Command.WILL : Command.WONT), (byte)Option.Echo });
				this.backend.Write(new byte[] {
					(byte)Command.IAC,
					(byte)(value ? Command.WILL : Command.WONT),
					(byte)Option.SuppressGoAhead
				});
				this.echo = value;
			}
		}
		public Server(IByteDevice backend)
		{
			this.backend = backend;
			this.AutoFlush = true;
		}
		~Server()
		{
			Error.Log.Wrap((Func<bool>)this.Close)();	
		}
		void Filter()
		{
			byte? result = this.backend.Read();
			if (result.HasValue)
				switch ((Command)result.Value)
				{
					case Command.SE: // End of subnegotiation parameters
						break;
					case Command.NOP: // No operation
						break;
					case Command.DM: // Data mark
						break;
					case Command.BRK:  // Break
						break;
					case Command.IP:  // Suspend
						break;
					case Command.AO:  // Abort output
						break;
					case Command.AYT: // Are you there
						break; 
					case Command.EC: // Erase character
						break; 
					case Command.EL: // Erase line
						break;
					case Command.GA: // Go ahead
						break;
					case Command.SB: // Subnegotiation
						this.backend.Read(); // option
						this.backend.Read(); // supplied (0) / required (1)
						Collection.List<byte> value = new Kean.Collection.List<byte>();
						byte? v;
						while ((v = this.backend.Read()).HasValue && (Command)v.Value != Command.IAC)
							value.Add(v.Value);
						this.backend.Read(); // SE

						break;
					case Command.WILL: // Will
						{
							byte? option = this.backend.Read();  // option
							if (option.HasValue)
								switch ((Option)option.Value)
								{
									case Option.WindowSize:
									case Option.TerminalType:
									default:
										this.backend.Write(new byte[] { (byte)Command.IAC, (byte)Command.WONT, option.Value });
										break;
									case Option.TerminalSpeed:
										this.backend.Write(new byte[] {
											(byte)Command.IAC,
											(byte)Command.SB,
											option.Value,
											1,
											(byte)Command.IAC,
											(byte)Command.SE
										});
										break;
									case Option.SuppressGoAhead:
										this.backend.Write(new byte[] { (byte)Command.IAC, (byte)Command.DO, option.Value });
										break;
								}
						}
						break; 
					case Command.WONT: // Wont
						this.backend.Read();  // option
						break;
					case Command.DO: // Do
						{
							byte? option = this.backend.Read();  // option
							if (option.HasValue)
								switch ((Option)option.Value)
								{
									case Option.WindowSize:
									case Option.TerminalSpeed:
									case Option.TerminalType:
									default:
										this.backend.Write(new byte[] { (byte)Command.IAC, (byte)Command.WONT, option.Value });
										break;
									case Option.Echo:
									case Option.SuppressGoAhead:
										if (!this.echo.HasValue || !this.echo.Value)
											this.backend.Write(new byte[] { (byte)Command.IAC, (byte)Command.WILL, option.Value });
										this.echo = true;
										break;
								}
						}
						break;
					case Command.DONT: // Dont
						{
							byte? option = this.backend.Read();  // option
							if (option.HasValue)
								switch ((Option)option.Value)
								{
									case Option.Echo:
									case Option.SuppressGoAhead:
										if (!this.echo.HasValue || this.echo.Value)
											this.backend.Write(new byte[] { (byte)Command.IAC, (byte)Command.WONT, option.Value });
										this.echo = false;
										break;
								}
						}
						break;
					case Command.IAC: // Interpret as command
						this.Filter();
						break;
				}	
		}
		#region IByteInDevice Members
		public byte? Peek()
		{
			byte? result = this.backend.Peek();
			while (result == 255)
			{
				this.Filter();
				result = this.backend.Peek();
			}
			return result;
		}
		public byte? Read()
		{
			byte? result;
			try
			{
				result = this.backend.Read();
				while (result == 255)
				{
					this.Filter();
					result = this.backend.Read();
				}
			}
			catch (NullReferenceException)
			{
				result = null;
			}
			return result;
		}
		#endregion
		#region IByteOutDevice Members
		public bool Write(System.Collections.Generic.IEnumerable<byte> buffer)
		{
			return this.backend.Write(buffer);
		}
		#endregion
		#region IInDevice Members
		public bool Empty { get { throw new System.NotImplementedException(); } }
		#endregion
		#region IOutDevice Members
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public bool Flush()
		{
			return this.backend.NotNull() && this.backend.Flush();
		}
		#endregion
		#region IByteDevice Members
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		public bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		#endregion
		#region IDevice Members
		public Uri.Locator Resource { get { return this.backend.Resource; } }
		public bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public bool Close()
		{
			bool result;
			if (result = this.backend.NotNull() && this.backend.Close())
				this.backend = null;
			return result;
		}
		#endregion
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
	}
}
