// 
//  Buffer.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Kean.Extension;

namespace Kean.IO.Text
{
	public class Buffer
	{
		Collection.IList<char> backend = new Collection.Linked.List<char>();
		public int Length { get; private set; }
		public int Cursor { get; private set; }
		public int CursorToEnd { get { return this.Length - this.Cursor; } }
		public string LeftOfCursor { get { return this.Substring(0, this.Cursor); } }
		public string RightOfCursor { get { return this.Substring(this.Cursor); } }

		public Buffer()
		{ }

		public bool MoveCursorLeft()
		{
			bool result;
			if (result = this.Cursor > 0)
				this.Cursor--;
			return result;
		}
		public bool MoveCursorRight()
		{
			bool result;
			if (result = this.Cursor < this.Length)
				this.Cursor++;
			return result;
		}
		public int MoveCursorHome()
		{
			int result = -this.Cursor;
			this.Cursor = 0;
			return result;
		}
		public int MoveCursorEnd()
		{
			int result = this.Length - this.Cursor;
			this.Cursor = this.Length;
			return result;
		}

		public override string ToString()
		{
			return this.backend.Fold<char, string>((c, result) => result + c, "");
		}
		public bool Insert(string value)
		{
			bool result = true;
			foreach (char c in value)
				result = this.Insert(c);
			return result;
		}
		public bool Insert(char c)
		{
			bool result;
			if (result = this.Cursor == this.Length)
				this.backend.Add(c);
			else
				this.backend.Insert(this.Cursor, c);
			this.Length++;
			this.Cursor++;
			return result;
		}
		public string Substring(int index, int length)
		{
			string result = "";
			if (length != 0)
			{
				int count = this.backend.Count;
				int sliceIndex = (index >= 0) ? index : count + index;
				int sliceLength = (length > 0) ? length : count + length - sliceIndex;
				result = ((Collection.IVector<char>)this.backend.Slice(sliceIndex, sliceLength)).Fold<char, string>((c, res) => res + c, "");
			}
			return result;
		}
		public string Substring(int index)
		{
			return index >= 0 ?
				this.Substring(index, this.backend.Count - index) :
				this.Substring(this.backend.Count + index, -index);
		}
		public bool Delete()
		{
			bool result;
			if (result = this.Cursor < this.Length)
			{
				this.Length--;
				this.backend.Remove(this.Cursor);
			}
			return result;
		}
		public bool Backspace()
		{
			bool result;
			if (result = this.Cursor > 0)
			{
				this.Length--;
				this.Cursor--;
				this.backend.Remove(this.Cursor);
			}
			return result;
		}
		public void Clear()
		{
			this.Length = 0;
			this.Cursor = 0;
			this.backend.Clear();
		}
		#region Casts
		public static implicit operator string(Buffer buffer)
		{
			return buffer.NotNull() ? buffer.ToString() : null;
		}
		public static implicit operator Buffer(string value)
		{
			Buffer result = new Buffer();
			if (value.NotEmpty())
			{
				result.backend.Add(value.ToCharArray());
				result.Cursor = result.Length = value.Length;
			}
			return result;
		}
		#endregion
	}
}
