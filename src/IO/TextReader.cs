// Copyright (C) 2011, 2017	Simon Mika <simon@mika.se>
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
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;

namespace Kean.IO
{
	public class TextReader :
		ITextReader
	{
		ICharacterInDevice backend;
		public Uri.Locator Resource { get { return this.backend?.Resource; } }
		public Text.Position Position { get; private set; }
		public bool Opened { get { return this.backend?.Opened ?? false; } }
		public Tasks.Task<bool> Empty { get { return this.backend?.Empty; } }
		public bool Readable { get { return this.backend.NotNull() && this.backend.Readable; } }
		public char Last { get; private set; }
		public event Action<char> OnNext;
		protected TextReader(ICharacterInDevice backend)
		{
			this.backend = backend;
			this.Position = new Text.Position(1, 0);
		}
		~TextReader()
		{
			this.Close().Wait();
		}
		public async Tasks.Task<bool> Next()
		{
			this.Position += this.Last;
			char? next = await this.backend.Read();
			if (!next.HasValue)
				this.Last = '\0';
			else if (next == '\r' && (await this.backend.Peek()).HasValue && await this.backend.Peek() == '\n')
			{
				await this.backend.Read();
				this.Last = '\n';
			}
			/*
			else if (next == '\r')
			{
				this.Last = '\n';
			}*/
			else
				this.Last = (char)next;
			if (next.HasValue)
				this.OnNext.Call(this.Last);
			return next.HasValue;
		}
		public async Tasks.Task<bool> Close()
		{
			bool result;
			if (result = this.backend.NotNull() && await this.backend.Close())
				this.backend = null;
			return result;
		}
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close().Wait();
		}
		#endregion
		#region Static Open
		public static ITextReader Open(ICharacterInDevice backend)
		{
			return backend.NotNull() ? new TextReader(backend) : null;
		}
		public static ITextReader Open(Uri.Locator resource)
		{
			return TextReader.Open(CharacterDevice.Open(resource));
		}
		public static ITextReader From(string content)
		{
			return TextReader.Open(TextCharacterInDevice.Open(content));
		}
		public static ITextReader From(Generic.IEnumerable<char> content)
		{
			return TextReader.Open(TextCharacterInDevice.Open(content));
		}
		public static ITextReader From(Generic.IEnumerator<char> content)
		{
			return TextReader.Open(TextCharacterInDevice.Open(content));
		}
		#endregion
	}
}
