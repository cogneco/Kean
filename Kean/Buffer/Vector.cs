// 
//  Vector.cs
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
using InteropServices = System.Runtime.InteropServices;

namespace Kean.Buffer
{
	public class Vector<T> :
		Sized,
		Collection.IVector<T>
		where T : struct
	{
		T[] data;
		protected internal T[] Data 
		{
			get { lock (this.Lock) return this.data; }
			set { lock (this.Lock) this.data = value; }
		}
		bool isPinned = false;
		bool recyclable;
		InteropServices.GCHandle handle;
		public Vector(int size) : this(size, null) { }
		public Vector(int size, Action<IntPtr> free) : this(Vector<T>.Find(size), free) 
		{
			this.recyclable = true;  // only recycle the once we allocate our self is both safer and leads to higher recycle rate
		}
		public Vector(T[] data) : this(data, null) { }
		public Vector(T[] data, Action<IntPtr> free) :
			this(InteropServices.GCHandle.Alloc(data, InteropServices.GCHandleType.Pinned), InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), data.Length * InteropServices.Marshal.SizeOf(typeof(T)), free)
		{
			this.Data = data;
		}
		Vector(InteropServices.GCHandle handle, IntPtr data, int size, Action<IntPtr> free) :
			base(data, size, free)
		{
			this.handle = handle;
			this.isPinned = true;
		}
		protected override void Dispose(bool disposing)
		{
			lock (this.Lock ?? new object())
			{
				if (this.data.NotNull())
				{
					base.Dispose(disposing);
					if (this.isPinned && this.handle.IsAllocated)
					{
						this.handle.Free();
						this.isPinned = false;
					}
					T[] data = this.data;
					this.data = null; // must set this to null before recycle so that we don't recycle already recycled arrays
					if (this.recyclable && Vector.Recycle)
						Vector<T>.recycle.Recycle(data);
				}
			}
		}
		public override void CopyFrom(Sized other, int start, int destination, int length)
		{
			if (typeof(T) == typeof(byte))
				System.Runtime.InteropServices.Marshal.Copy(new IntPtr(((IntPtr)other).ToInt32() + start), this.Data as byte[], destination, length);
			else if (typeof(T) == typeof(int))
				System.Runtime.InteropServices.Marshal.Copy(new IntPtr(((IntPtr)other).ToInt32() + start), this.Data as int[], destination, length);
			else if (typeof(T) == typeof(float))
				System.Runtime.InteropServices.Marshal.Copy(new IntPtr(((IntPtr)other).ToInt32() + start), this.Data as float[], destination, length);
			else if (typeof(T) == typeof(double))
				System.Runtime.InteropServices.Marshal.Copy(new IntPtr(((IntPtr)other).ToInt32() + start), this.Data as double[], destination, length);
			else
				base.CopyFrom(other, start, destination, length);
		}
		#region IVector<T> Members
		T Collection.IVector<T>.this[int index]
		{
			get { lock (this.Lock) return this.Data[index]; }
			set { lock (this.Lock) this.Data[index] = value; }
		}
		int Collection.IVector<T>.Count
		{
			get { lock (this.Lock) return this.Data.Length; }
		}
		System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator()
		{
			for (int i = 0; i < this.Data.Length; i++)
				yield return this.Data[i];
		}
		#endregion
		#region IEquatable<IVector<T>> Members
		bool IEquatable<Collection.IVector<T>>.Equals(Collection.IVector<T> other)
		{
			bool result = this.Data.Length == other.Count;
			for (int i = 0; result && i < this.Data.Length; i++)
				result &= this.Data[i].Equals(other[i]);
			return result;
		}
		#endregion
		public unsafe static implicit operator void*(Vector<T> pointer)
		{
			return ((IntPtr)pointer).ToPointer();
		}
		public static implicit operator IntPtr(Vector<T> pointer)
		{
			return (IntPtr)(Pointer)pointer;
		}
		static int Index(int size)
		{
			return size < 10000 ? 0 : size < 100000 ? 1 : 2;
		}
		static T[] Find(int size)
		{
			T[] result = Vector<T>.recycle.Find(size);
			return result.NotNull() ? result.Initialize(default(T)) : new T[size];
		}
		static Vector()
		{
			Vector.OnFree += Vector<T>.Free;
		}
		static Recycle.IBin<T[], int> recycle = new Recycle.Bins<T[], int>(
			10,
			3,
			(item, size) => item.Length == size,
			null,
			item => Vector<T>.Index(item.Length),
			size => Vector<T>.Index(size)
			);
		public static void Free()
		{
			Vector<T>.recycle.Free();
		}
	}
	public class Vector
	{
		public static bool Recycle { get; set; }
		internal static event Action OnFree;
		static Vector()
		{
			Vector.Recycle = true;
		}
		public static void Free()
		{
			Vector.OnFree.Call();
		}
	}
}
