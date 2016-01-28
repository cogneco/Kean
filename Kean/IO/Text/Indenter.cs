//
//  Indenter.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2012 Simon Mika
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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Error = Kean.Error;

namespace Kean.IO.Text
{
	public class Indenter :
	Abstract.CharacterWriter
	{
		ICharacterWriter backend;
		bool lineIndented;
		string indention = "";
		public bool Format { get; set; }
		public Indenter(ICharacterWriter backend)
		{
			this.backend = backend;
			this.Format = true;
		}
		public bool AddIndent()
		{
			return (this.indention += "\t").NotEmpty();
		}
		public bool RemoveIndent()
		{
			return (this.indention = this.indention.Substring(1)).NotNull();
		}
		bool WriteIndent()
		{
			return !this.Format || this.lineIndented || (this.lineIndented = this.backend.Write(this.indention));
		}
		#region implemented abstract and virtual members of Kean.IO.Abstract.CharacterWriter
		public override Uri.Locator Resource { get { return this.backend.NotNull() ? this.backend.Resource : null; } }
		public override bool Opened { get { return this.backend.NotNull() && this.backend.Opened; } }
		public override bool Writable { get { return this.backend.NotNull() && this.backend.Writable; } }
		public override bool Close()
		{
			bool result;
			if (result = this.backend.NotNull() && this.backend.Close())
				this.backend = null;
			return result;
		}
		public override bool Write(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.WriteIndent() && this.backend.Write(buffer);
		}
		public override bool WriteLine()
		{
			return !(this.lineIndented = !(!this.Format || this.backend.WriteLine()));
		}
		public override bool WriteLine(params char[] buffer)
		{
			return this.WriteIndent() && !(this.lineIndented = !(this.Format ? this.backend.WriteLine(buffer) : this.backend.Write(buffer)));
		}
		public override bool WriteLine(string value)
		{
			return this.WriteIndent() && !(this.lineIndented = !(this.Format ? this.backend.WriteLine(value) : this.backend.Write(value)));
		}
		public override bool AutoFlush
		{
			get { return this.backend.AutoFlush; }
			set { this.backend.AutoFlush = value; }
		}
		public override bool Flush()
		{
			return this.backend.NotNull() && this.backend.Flush();
		}
		#endregion
	}
}

