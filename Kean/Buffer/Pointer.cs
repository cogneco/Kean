// 
//  Pointer.cs
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

namespace Kean.Buffer
{
	/// <summary>
	/// Used to handle a memory area for use by unmanaged code so that it can be garbage collected.
	/// </summary>    
	public class Pointer :
		Synchronized,
		IDisposable
	{
		Action<IntPtr> free;
		IntPtr pointer;

		public unsafe Pointer(void* pointer) : this(new System.IntPtr(pointer)) { }
		public Pointer(IntPtr pointer) : this(pointer, null) { }
		public Pointer(IntPtr pointer, Action<IntPtr> free)
		{
			this.free = free;
			this.pointer = pointer;
		}
		/// <summary>
		/// Destructor
		/// </summary>
		~Pointer()
		{
			Error.Log.Wrap((Action<bool>)this.Dispose)(false);
		}
		/// <summary>
		/// Disposes Memory object.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Frees all resources associated with object.
		/// </summary>
		/// <param name="disposing">Does not affect anything. Needed for function to have the right interface.</param>
		protected virtual void Dispose(bool disposing)
		{
			lock (this.Lock)
				if (this.free != null)
				{
					this.free(this.pointer);
					this.free = null;
				}
		}
		public unsafe static implicit operator void*(Pointer pointer)
		{
			return pointer.pointer.ToPointer();
		}
		public unsafe static implicit operator Pointer(void* pointer)
		{
			return new Pointer(pointer);
		}
		public static implicit operator IntPtr(Pointer pointer)
		{
			return pointer.pointer;
		}
		public static implicit operator Pointer(IntPtr pointer)
		{
			return new Pointer(pointer);
		}
	}
}


