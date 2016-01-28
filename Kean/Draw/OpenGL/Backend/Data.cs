//
//  Data.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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

using Kean.Collection.Extension;
using System;
using Collection = Kean.Collection;
using Error = Kean.Error;
using GL = OpenTK.Graphics.OpenGL.GL;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Raster = Kean.Draw.Raster;
using Kean.Extension;

namespace Kean.Draw.OpenGL.Backend
{
	public abstract class Data :
		Resource,
		IData
	{
		protected int Identifier { get; private set; }
		protected OpenTK.Graphics.OpenGL.PixelFormat PixelFormat { get; private set; }
		protected OpenTK.Graphics.OpenGL.PixelInternalFormat PixelInternalFormat { get; private set; }
		protected OpenTK.Graphics.OpenGL.PixelType PixelType { get; private set; }
		public TextureType Type { get; private set; }
		public int PixelSize { get; private set; }
		protected Data(Context context) :
			base(context)
		{
			this.Identifier = this.CreateIdentifier();
		}
		protected bool SetType<T>() 
			where T : struct
		{
			TextureType type;
			int pixelSize;
			OpenTK.Graphics.OpenGL.PixelFormat pixelFormat;
			OpenTK.Graphics.OpenGL.PixelInternalFormat pixelInternalFormat;
			OpenTK.Graphics.OpenGL.PixelType pixelType;
			Reflect.Type t = typeof(T);
			if (t.Implements<IColor>() || t == typeof(byte))
			{
				switch (pixelSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)))
				{
					case 4:
						type = TextureType.Rgba;
						pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
						pixelInternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba;
						break;
					case 3:
						type = TextureType.Rgb;
						pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
						pixelInternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgb;
						break;
					default:
					case 1:
						type = TextureType.Monochrome;
						pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
						pixelInternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Luminance;
						break;
				}
				pixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
			}
			else
			{
				if (t == typeof(float))
				{
					pixelSize = sizeof(float);
					pixelType = OpenTK.Graphics.OpenGL.PixelType.Float;
				} 
				else
				{
					pixelSize = 1;
					pixelType = OpenTK.Graphics.OpenGL.PixelType.UnsignedByte;
				}
				type = TextureType.Monochrome;
				pixelFormat = OpenTK.Graphics.OpenGL.PixelFormat.Luminance;
				pixelInternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat.Luminance;
			}
			bool result = type == this.Type && this.PixelSize != 0;
			if (!result)
			{
				this.Type = type;
				this.PixelSize = pixelSize;
				this.PixelFormat = pixelFormat;
				this.PixelInternalFormat = pixelInternalFormat;
				this.PixelType = pixelType;
			}
			return result;
		}
		protected void FixCall(Action<IntPtr> action, object data)
		{
			this.Use();
			System.Runtime.InteropServices.GCHandle handle = System.Runtime.InteropServices.GCHandle.Alloc(data, System.Runtime.InteropServices.GCHandleType.Pinned);
			action(handle.AddrOfPinnedObject());
			handle.Free();
			this.UnUse();
		}
		#region Implementors Interface
		protected abstract int CreateIdentifier();
		protected abstract void Allocate(IntPtr pointer);
		protected abstract void Load(IntPtr pointer);
		public abstract void Use();
		public abstract void UnUse();
		protected internal override void Delete()
		{
			this.Identifier = 0;
			base.Delete();
		}
		#endregion
		protected override void Dispose(bool disposing)
		{
			if (this.Identifier != 0)
				this.Delete();
		}
	}
}
