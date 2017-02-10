// Copyright (C) 2011	Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Tasks = System.Threading.Tasks;
using Generic = System.Collections.Generic;
using Kean.Extension;

namespace Kean.IO
{
	public class TextWriter :
		ITextWriter
	{
		ICharacterOutDevice backend;
		Generic.IEnumerator<char> buffer;
		readonly object bufferLock = new object();
		public Uri.Locator Resource { get { return this.backend?.Resource; } }
		public bool Opened { get { return this.backend?.Opened ?? false; } }
		public bool Writable { get { return this.backend?.Writable ?? false; } }
		public char[] NewLine { get; set; } = new char[] { '\n' };
		public bool AutoFlush
		{
			get { return this.backend?.AutoFlush ?? false; }
			set { this.backend.AutoFlush = value; }
		}
		protected TextWriter(ICharacterOutDevice backend)
		{
			this.backend = backend;
		}
		~TextWriter()
		{
			this.Close().Wait();
		}
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		public async Tasks.Task<bool> Close()
		{
			bool result;
			if (result = this.backend.NotNull() && await this.backend.Close())
				this.backend = null;
			return result;
		}
		#region implemented abstract and virtual members of Kean.IO.Abstract.CharacterWriter
		public bool Write(Generic.IEnumerator<char> buffer)
		{
			this.buffer = this.buffer.Append(new NewLineConverter(buffer, this.NewLine));
			if (this.AutoFlush)
				this.Flush().Start(); // Perform writing and flushing in the background. TODO: Is start needed?
			return this.Writable;
		}
		public async Tasks.Task<bool> Flush()
		{
			return this.backend.NotNull() &&
				await this.backend.Write(this.buffer) &&
				(this.buffer = null).NotNull() &&
				await this.backend.Flush();
		}
		#endregion
		#region NewLineConverter Class
		class NewLineConverter :
			Generic.IEnumerator<char>
		{
			Generic.IEnumerator<char> backend;
			char[] newLine;
			int index;
			#region Constructors
			public NewLineConverter(Generic.IEnumerator<char> backend, params char[] newLine)
			{
				this.backend = backend;
				this.newLine = newLine;
			}
			#endregion
			#region IEnumerator<char> Members
			public char Current { get; private set; }
			#endregion
			#region IDisposable Members
			public void Dispose()
			{
				this.backend.Dispose();
			}
			#endregion
			#region IEnumerator Members
			object System.Collections.IEnumerator.Current { get { return this.Current; } }
			public bool MoveNext()
			{
				bool result = true;
				if (this.index > 0)
				{
					this.Current = this.newLine[this.index++];
					if (this.index >= this.newLine.Length)
						this.index = 0;
				}
				else if (this.backend.MoveNext())
				{
					if (this.backend.Current == '\n')
					{
						this.Current = this.newLine[0];
						if (this.newLine.Length > 1)
							this.index = 1;
					}
					else
						this.Current = this.backend.Current;
				}
				else
					result = false;
				return result;
			}
			public void Reset()
			{
				this.backend.Reset();
				this.index = 0;
			}
			#endregion
		}
		#endregion
		#region Static Open & Create
		public static ITextWriter Open(Uri.Locator resource)
		{
			return TextWriter.Open(CharacterDevice.Open(resource));
		}
		public static ITextWriter Create(Uri.Locator resource)
		{
			return TextWriter.Open(CharacterDevice.Create(resource));
		}
		public static ITextWriter Open(ICharacterOutDevice backend)
		{
			return backend.NotNull() ? new TextWriter(backend) : null;
		}
		#endregion
	}
}
