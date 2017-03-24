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
		Tasks.Task<char?> peeked;
		public Uri.Locator Resource { get { return this.backend?.Resource; } }
		public Tasks.Task<Text.Position> Position { get; private set; }
		public bool Opened { get { return this.backend?.Opened ?? false; } }
		async Tasks.Task<bool> EmptyHelper() {
			return !((this.peeked.NotNull() && (await this.peeked).HasValue) || (this.backend.NotNull() && !await this.backend.Empty));
		}
		public Tasks.Task<bool> Empty { get { return this.EmptyHelper(); } }
		public bool Readable { get { return this.backend?.Readable ?? false; } }
		public event Action<char> OnRead;
		protected TextReader(ICharacterInDevice backend)
		{
			this.backend = backend;
			this.Position = Tasks.Task.FromResult(new Text.Position(0, 0));
		}
		~TextReader()
		{
			this.Close().Wait();
		}
		async Tasks.Task<char?> ReadBackend()
		{
			char? result = await this.backend.Read();
			if (result.HasValue && result == '\r' && (await this.backend.Peek()).HasValue && await this.backend.Peek() == '\n')
				result = await this.backend.Read();
			return result;
		}
		async Tasks.Task<Text.Position> GetPosition(Tasks.Task<char?> next)
		{
			var c = await next;
			return c.HasValue ? await this.Position + c.Value : await this.Position;
		}
		public async Tasks.Task<char?> Peek() {
			return (this.peeked.NotNull() ? await this.peeked : null) ?? await (this.peeked = this.ReadBackend());
		}
		public async Tasks.Task<char?> Read()
		{
			var next = this.Peek();
			this.peeked = null;
			this.Position = this.GetPosition(next);
			var onRead = this.OnRead; // Save OnRead so that it reflects the state when the read command was issued not when the result arrives.
			var result = await next;
			if (result.HasValue && onRead.NotNull())
				onRead(result.Value);
			return result;
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
			return TextReader.Open(CharacterInDevice.Open(content));
		}
		public static ITextReader From(Generic.IEnumerable<char> content)
		{
			return TextReader.Open(CharacterInDevice.Open(content));
		}
		public static ITextReader From(Generic.IEnumerator<char> content)
		{
			return TextReader.Open(CharacterInDevice.Open(content));
		}
		#endregion
	}
}
