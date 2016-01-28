//
//  CharacterWriter.cs
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
using Generic = System.Collections.Generic;

namespace Kean.IO.Abstract
{
	public abstract class CharacterWriter :
		ICharacterWriter
	{
		public char[] NewLine { get; set; }
		public abstract Uri.Locator Resource { get; }
		public abstract bool Opened { get; }
		public abstract bool Writable { get; }
		protected CharacterWriter()
		{
			this.NewLine = new char[] { '\n' };
		}
		~CharacterWriter()
		{
			Error.Log.Wrap((Func<bool>)this.Close)();
		}
		public abstract bool Close();
		#region IDisposable Members
		void IDisposable.Dispose()
		{
			this.Close();
		}
		#endregion
		#region ICharacterWriter Members
		public bool Write(params char[] buffer)
		{
			return this.Write((System.Collections.Generic.IEnumerable<char>)buffer);
		}
		public virtual bool Write(string value)
		{
			return value.IsNull() || this.Write(value.ToCharArray());
		}
		public bool Write<T>(T value) where T : IConvertible
		{
			return this.Write(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public bool Write(string format, params object[] arguments)
		{
			return this.Write(string.Format(format, arguments));
		}
		public abstract bool Write(Generic.IEnumerable<char> buffer);
		public virtual bool WriteLine()
		{
			return this.Write('\n'); // The newline characters are converted by bool Write(Generic.IEnumerable<char> buffer)
		}
		public virtual bool WriteLine(params char[] buffer)
		{
			return this.Write((System.Collections.Generic.IEnumerable<char>)buffer.Merge(this.NewLine));
		}
		public virtual bool WriteLine(string value)
		{
			return this.Write(new System.Text.StringBuilder(value).Append('\n').ToString()); // The newline characters are converted by bool Write(Generic.IEnumerable<char> buffer)
		}
		public bool WriteLine<T>(T value) where T : IConvertible
		{
			return this.WriteLine(value.ToString((IFormatProvider)System.Globalization.CultureInfo.InvariantCulture.GetFormat(typeof(T))));
		}
		public bool WriteLine(string format, params object[] arguments)
		{
			return this.WriteLine(string.Format(format, arguments));
		}
		public bool WriteLine(System.Collections.Generic.IEnumerable<char> buffer)
		{
			return this.Write((System.Collections.Generic.IEnumerable<char>)buffer) && this.WriteLine();
		}
		#endregion
		#region IOutDevice Members
		public virtual bool AutoFlush { get; set; }
		public virtual bool Flush()
		{
			return true;
		}
		#endregion
	}
}

