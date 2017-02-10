// Copyright (C) 2012, 2016	Simon Mika <simon@mika.se>
//
// This file is part of Kean.
//
// Kean is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
//
// Kean is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with Kean.	If not, see <http://www.gnu.org/licenses/>.
//

using System;
using Generic = System.Collections.Generic;
using Tasks = System.Threading.Tasks;
using Kean.Extension;
using Kean.IO.Extension;

namespace Kean.IO
{
	public class TextIndenter :
		ITextIndenter
	{
		ITextWriter backend;
		bool lineIndented;
		string currentIndention = "";
		public bool Format { get; set; }
		public Uri.Locator Resource { get { return this.backend?.Resource; } }
		public bool Opened { get { return this.backend?.Opened ?? false; } }
		public bool Writable { get { return this.backend?.Writable ?? false; } }
		public bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public char[] NewLine { get; set; } = new char[] { '\n' };
		public char[] Indention { get; set; } = new char[] { '\t' };
		public TextIndenter(ITextWriter backend)
		{
			this.backend = backend;
			this.Format = true;
		}
		~TextIndenter()
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
		public bool Increase()
		{
			return (this.currentIndention += this.Indention).NotEmpty();
		}
		public bool Decrease()
		{
			return (this.currentIndention = this.currentIndention.Substring(this.Indention.Length)).NotNull();
		}
		bool WriteIndent()
		{
			return !this.Format || this.lineIndented || (this.lineIndented = this.backend.Write(this.currentIndention));
		}
		public bool Write(Generic.IEnumerator<char> buffer)
		{
			return this.WriteIndent() && this.backend.Write(buffer);
		}
		public async Tasks.Task<bool> Flush()
		{
			return this.backend.NotNull() && await this.backend.Flush();
		}
	}
}
