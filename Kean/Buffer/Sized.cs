//
//  Sized.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2009-2011 Simon Mika
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

namespace Kean.Buffer
{
	public class Sized :
		Pointer,
		Collection.IVector<byte>
	{
		int size;
		public int Size
		{
			get
			{
				lock (this.Lock)
					return this.size;
			}
		}
		public Sized(IntPtr data, int size) :
			this(data, size, null)
		{
		}
		public Sized(IntPtr data, int size, Action<IntPtr> free) :
			base(data, free)
		{
			this.size = size;
		}

		#region IVector<byte> Members
		byte Collection.IVector<byte>.this [int index]
		{
			get
			{
				lock (this.Lock)
				{
					if (index < 0 || index >= this.Size)
						throw new Exception.IndexOutOfRange();
					unsafe
					{
						return ((byte*)this)[index];
					}
				}
			}
			set
			{
				lock (this.Lock)
				{
					if (index < 0 || index >= this.Size)
						throw new Exception.IndexOutOfRange();
					unsafe
					{
						((byte*)this)[index] = value;
					}
				}
			}
		}
		int Collection.IVector<byte>.Count { get { return this.Size; } }
		System.Collections.Generic.IEnumerator<byte> System.Collections.Generic.IEnumerable<byte>.GetEnumerator()
		{
			for (int i = 0; i < this.Size; i++)
				yield return (this as Collection.IVector<byte>)[i];
		}
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			for (int i = 0; i < this.Size; i++)
				yield return (this as Collection.IVector<byte>)[i];
		}
		bool IEquatable<Collection.IVector<byte>>.Equals(Collection.IVector<byte> other)
		{
			bool result = other.NotNull() && this.Size == other.Count;
			for (int i = 0; result && i < this.Size; i++)
				result &= (this as Collection.IVector<byte>)[i] == other[i];
			return result;
		}
		#endregion

		public unsafe static implicit operator void*(Sized pointer)
		{
			return ((IntPtr)pointer).ToPointer();
		}
		public static implicit operator IntPtr(Sized pointer)
		{
			return (IntPtr)(Pointer)pointer;
		}
		public Sized Copy()
		{
			Vector<byte> result = new Vector<byte>(this.Size);
			System.Runtime.InteropServices.Marshal.Copy(this, result.Data, 0, this.Size);
			return result;
		}
		public virtual void CopyFrom(Sized other, int start, int destination, int length)
		{
			unsafe
			{
				byte* a = (byte*)this;
				byte* b = (byte*)other;
				for (int i = 0; i < length; i++)
					*(a + destination + i) = *(b + start + i);
			}
		}
		public float Distance(Sized other)
		{
			float result = 0;
			if (other.NotNull())
			{
				result = this.Size != other.Size ? float.MaxValue : 0;
				if (result == 0)
				{
					unsafe
					{
						byte* a = (byte*)this;
						byte* b = (byte*)other;
						int size = this.Size;
						for (int i = 0; i < size; i++, a++, b++)
							result += (*a - *b) * (*a - *b);
					}
					result /= this.Size;
					result = Math.Single.SquareRoot(result);
				}
			}
			else
				result = float.MaxValue;
			return result;
		}
		public Tuple<int, float> Discrepance(Sized other)
		{
			Tuple<int, float> result;
			float error = 0;
			int position = -1;
			if (other.NotNull())
			{
				error = this.Size != other.Size ? float.MaxValue : 0;
				if (error == 0)
					unsafe
					{
						byte* a = (byte*)this;
						byte* b = (byte*)other;
						int size = this.Size;
						for (int i = 0; i < size; i++, a++, b++)
						{
							float localError = System.Math.Abs(*a - *b);
							if (localError > error)
							{
								error = localError;
								position = i;
							}
						}
					}
			}
			else
				error = float.MaxValue;
			result = Tuple.Create<int, float>(position, error);
			return result;
		}
	}
}