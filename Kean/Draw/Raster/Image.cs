// 
//  Image.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011 Simon Mika
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
using Buffer = Kean.Buffer;
using Collection = Kean.Collection;
using Geometry2D = Kean.Math.Geometry2D;
using Kean.Extension;
using Kean.Draw.Raster.Extension;
using Reflect = Kean.Reflect;
using Kean.Reflect.Extension;

namespace Kean.Draw.Raster
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public abstract class Image :
		Draw.Image
	{
		protected int DistanceRadius { get { return 1; } }
		Cairo.Image cairo;
		Cairo.Image Cairo
		{
			get
			{
				if (this.cairo.IsNull())
					this.cairo = this.CreateCairoImage(this.buffer, this.Size);
				return this.cairo; 
			}
		}
		public override Canvas Canvas { get { return this.Cairo.Canvas; } }
		Buffer.Sized buffer;
		public Buffer.Sized Buffer { get { return this.buffer; } }
		public IntPtr Pointer { get { return this.buffer; } }
		public int Length { get { return this.buffer.Size; } }
		public abstract void Apply (Action<Color.Bgr> action);
		public abstract void Apply (Action<Color.Yuv> action);
		public abstract void Apply (Action<Color.Monochrome> action);
		protected Image(Image original) :
			base(original)
		{
			this.buffer = original.buffer.Copy();
		}

		protected Image (Buffer.Sized buffer, Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop) :
			base(size, coordinateSystem, crop)
		{
			this.buffer = buffer;
		}
		protected virtual Cairo.Image CreateCairoImage (Buffer.Sized buffer, Geometry2D.Integer.Size size)
		{
			return null;
		}
		public override Draw.Image ResizeTo (Geometry2D.Integer.Size size)
		{
			Draw.Image result = null;
			using (Bgra resized = new Bgra((Geometry2D.Integer.Size)size) { CoordinateSystem = this.CoordinateSystem })
			{
				using (System.Drawing.Bitmap bitmap = this.Convert<Raster.Bgra>().GdiBitmap())
				{
					using (System.Drawing.Bitmap b = resized.GdiBitmap())
					using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
					{
						g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
						g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
						g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; 
						g.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, (int)size.Width, (int)size.Height), new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height), System.Drawing.GraphicsUnit.Pixel);
					}
				}
				if (this is Monochrome)
					result = resized.Convert<Monochrome>();
				else if (this is Bgr)
					result = resized.Convert<Bgr>();
				else if (this is Bgra)
					result = resized.Convert<Bgra>();
				else if (this is Yuv420)
					result = resized.Convert<Yuv420>();
				else if (this is Yvu420)
					result = resized.Convert<Yvu420>();
				else if (this is Yuv444)
					result = resized.Convert<Yuv444>();
				else if (this is Yuv422)
					result = resized.Convert<Yuv422>();
				else if (this is Yuyv)
					result = resized.Convert<Yuyv>();
			}
			return result;
		}
		/// <summary>
		/// Copy a specified region of the current image. The transform specifies the part of current image to be copied.
		/// The transform is map which sends a rectangle of size Resolution centered at origo to a transformed (scaled, rotated, translated) one 
		/// in the current image. 
		/// </summary>
		/// <param name="size">Result bitmap size</param>
		/// <param name="transform">Transform</param>
		/// <returns></returns>
		public override Draw.Image Copy (Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform)
		{
			transform = (Geometry2D.Single.Transform)this.Transform * transform * (Geometry2D.Single.Transform)this.Transform.Inverse;
			Geometry2D.Single.Transform mappingTransform = Geometry2D.Single.Transform.CreateTranslation(this.Size.Width / 2, this.Size.Height / 2) * transform;
			Geometry2D.Single.Point upperLeft = mappingTransform * new Geometry2D.Single.Point(-size.Width / 2, -size.Height / 2);
			Geometry2D.Single.Point upperRight = mappingTransform * new Geometry2D.Single.Point(size.Width / 2, -size.Height / 2);
			Geometry2D.Single.Point downLeft = mappingTransform * new Geometry2D.Single.Point(-size.Width / 2, size.Height / 2);
			Geometry2D.Single.Point downRight = mappingTransform * new Geometry2D.Single.Point(size.Width / 2, size.Height / 2);
			Geometry2D.Single.Box source = Geometry2D.Single.Box.Bounds(upperLeft, upperRight, downLeft, downRight);
			Geometry2D.Single.Transform mappingTransformInverse = mappingTransform.Inverse;
			upperLeft = mappingTransformInverse * source.LeftTop;
			upperRight = mappingTransformInverse * source.RightTop;
			downLeft = mappingTransformInverse * source.LeftBottom;
			downRight = mappingTransformInverse * source.RightBottom;
			return this.Copy(size, source, upperLeft, upperRight, downLeft);
		}
		Image Copy (Geometry2D.Single.Size size, Geometry2D.Single.Box source, Geometry2D.Single.Point upperLeft, Geometry2D.Single.Point upperRight, Geometry2D.Single.Point lowerLeft)
		{
			Bgra result = new Bgra((Geometry2D.Integer.Size)size.Ceiling());
			using (System.Drawing.Bitmap bitmap = this.GdiBitmap())
			{
				using (System.Drawing.Bitmap b = result.GdiBitmap())
				using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
				{
					Geometry2D.Single.Point offset = new Geometry2D.Single.Point(result.Size.Width / 2, result.Size.Height / 2);
					upperLeft += offset;
					upperRight += offset;
					lowerLeft += offset;
					g.DrawImage(bitmap, new System.Drawing.PointF[3] {
						new System.Drawing.PointF(upperLeft.X, upperLeft.Y),
						new System.Drawing.PointF(upperRight.X, upperRight.Y),
						new System.Drawing.PointF(lowerLeft.X, lowerLeft.Y)
					}, new System.Drawing.RectangleF(source.Left, source.Top, source.Width, source.Height), System.Drawing.GraphicsUnit.Pixel);
				}
			}
			return result;
		}
		public override T Convert<T> ()
		{
			T result = null;
			Reflect.Type type = typeof(T);
			if (type == this.Type() || type == typeof(Image))
				result = this.Copy() as T;
			else if (type.Inherits<Image>())
			{
				if (type.Inherits<Packed>())
				{
					if (type == typeof(Bgr))
						result = new Bgr(this) as T;
					else if (type == typeof(Bgra))
						result = new Bgra(this) as T;
					else if (type == typeof(Monochrome))
						result = new Monochrome(this) as T;
					else if (type == typeof(Yuyv))
						result = new Yuyv(this) as T;
					else if (type == typeof(Uyvy))
						result = new Uyvy(this) as T;
				}
				else
				{
					if (type == typeof(Yuv420))
						result = new Yuv420(this) as T;
					else if (type == typeof(Yvu420))
						result = new Yvu420(this) as T;
					else if (type == typeof(Yuv444))
						result = new Yuv444(this) as T;
					else if (type == typeof(Yuv422))
						result = new Yuv422(this) as T;
				}
			}
			else if (type.Inherits<Cairo.Image>())
			{
				if (type == typeof(Cairo.Image) || type == this.cairo.GetType())
					result = this.Cairo as T;
				else if (type == typeof(Cairo.Bgra))
					result = this.Convert<Bgra>().Cairo as T;
			}
			return result;
		}
		#region Save
		public bool Save (Uri.Locator url)
		{
			Compression compression;
			switch (url.Path.Extension)
			{
				case "jpg":
				case "jpeg":
					compression = Compression.Jpeg;
					break;
				case "bmp":
					compression = Compression.Bmp;
					break;
				default:
				case "png":
					compression = Compression.Png;
					break;
			}
			return this.Save(url, compression);
		}
		public bool Save (Uri.Locator url, Compression compression)
		{
			bool result;
			try
			{
				if (url.Scheme == "file")
				{
					System.IO.Directory.CreateDirectory(url.Path.FolderPath.PlatformPath);
					using (var stream = System.IO.File.Open(System.IO.Path.GetFullPath(url.PlatformPath), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Write))
						result = this.Save(stream, compression);
				}
				else
					using (var device = IO.BlockDevice.Create(url))
						result = this.Save(device, compression);
			}
			catch (System.Runtime.InteropServices.ExternalException)
			{
				result = false;
			}
			return result;
		}
		public bool Save (IO.IByteOutDevice device, Compression compression)
		{
			using (System.IO.Stream stream = IO.Wrap.ByteStream.Wrap(device))
				return this.Save(stream, compression);
		}
		public bool Save (IO.IBlockOutDevice device, Compression compression)
		{
			using (System.IO.Stream stream = IO.Wrap.BlockStream.Wrap(device))
				return this.Save(stream, compression);
		}
		public bool Save (System.IO.Stream stream, Compression compression)
		{
			bool result = true;
			Image converted;
			if (this is Bgr || this is Bgra)
				converted = this;
			else
				converted = this.Convert<Bgra>();
			using (System.Drawing.Bitmap bitmap = converted.GdiBitmap())
			{
				if (converted.CoordinateSystem == CoordinateSystem.Default)
					bitmap.Save(stream, compression.ImageFormat());
				else
					using (System.Drawing.Bitmap final = new System.Drawing.Bitmap(converted.Size.Width, converted.Size.Height))
					{
						bitmap.RotateFlip(converted.CoordinateSystem.FlipType());
						using (System.Drawing.Graphics canvas = System.Drawing.Graphics.FromImage(final))
						{
							canvas.DrawImage(bitmap, new System.Drawing.Rectangle(0, 0, (int)converted.Size.Width, (int)converted.Size.Height), new System.Drawing.Rectangle(0, 0, (int)converted.Size.Width, (int)converted.Size.Height), System.Drawing.GraphicsUnit.Pixel);
						}
						try
						{
							bitmap.Save(stream, compression.ImageFormat());
						}
						catch (System.Runtime.InteropServices.ExternalException)
						{
							result = false;
						}
					}
			}
			return result;
		}
		#endregion
		#region Metric
		public override float Distance (Draw.Image other)
		{
			return other is Image ? this.buffer.Distance((other as Image).buffer) : float.MaxValue;
		}
		#endregion
		#region Object Overrides
		public override bool Equals (object other)
		{
			return other is Image && this.Equals(other as Image);
		}
		public override bool Equals (Draw.Image other)
		{
			return base.Equals(other) && other is Image &&
			this.buffer.NotNull() == (other as Image).buffer.NotNull() &&
			(this.buffer as Collection.IVector<byte>).Equals((other as Image).buffer as Collection.IVector<byte>);
		}
		public override int GetHashCode ()
		{
			return this.buffer.GetHashCode() ^ this.Size.GetHashCode() ^ this.CoordinateSystem.GetHashCode();
		}
		public override string ToString ()
		{
			return "<" + this.Size.ToString() + ">";
		}
		#endregion
		#region Static Create
		public static T Create<T> (Geometry2D.Integer.Size size) where T : Image
		{
			T result = null;
			Type type = typeof(T);
			if (type.IsSubclassOf(typeof(Packed)))
			{
				if (type == typeof(Bgr))
					result = new Bgr(size) as T;
				else if (type == typeof(Bgra))
					result = new Bgra(size) as T;
				else if (type == typeof(Monochrome))
					result = new Monochrome(size) as T;
				else if (type == typeof(Yuyv))
					result = new Yuyv(size) as T;
			}
			else
			{
				if (type == typeof(Yuv420))
					result = new Yuv420(size) as T;
				else if (type == typeof(Yvu420))
					result = new Yvu420(size) as T;
				else if (type == typeof(Yuv444))
					result = new Yuv444(size) as T;
				else if (type == typeof(Yuv422))
					result = new Yuv422(size) as T;
			}
			return result;
		}
		#endregion
		#region Static Open
		public static Image OpenResource (System.Reflection.Assembly assembly, string name)
		{
			name = assembly.GetName().Name + "." + name.Replace('\\', '.').Replace('/', '.');
			assembly.GetManifestResourceNames();
			Image result;
			using (System.IO.Stream stream = assembly.GetManifestResourceStream(name))
				result = Image.Open(stream);
			return result;
		}
		public static T OpenResource<T> (System.Reflection.Assembly assembly, string name) where T : Raster.Image
		{
			Image result = Image.OpenResource(assembly, name);
			return result.NotNull() ? result.As<T>() : null;
		}
		public static Image OpenResource (string name)
		{
			string[] splitted = name.Split(new char[] { ':' }, 2);
			Image result;
			if (splitted.Length > 1)
				result = Image.OpenResource(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(splitted[0])), splitted[1]);
			else
				result = Image.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), name);
			return result;
		}
		public static T OpenResource<T> (string name) where T : Raster.Image
		{
			string[] splitted = name.Split(new char[] { ':' }, 2);
			T result;
			if (splitted.Length > 1)
				result = Image.OpenResource(System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(splitted[0])), splitted[1]).As<T>();
			else
				result = Image.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), name).As<T>();
			return result;
		}
		public static Image Open (string filename)
		{
			Image result;
			try
			{
				using (System.IO.Stream stream = new System.IO.StreamReader(filename).BaseStream)
					result = Image.Open(stream);
			}
			catch (System.IO.IOException)
			{
				result = null;
			}
			return result;
		}
		public static T Open<T> (string filename) where T : Raster.Image
		{
			return Image.Open(filename).As<T>();
		}
		public static Image Open (IO.IByteInDevice device)
		{
			return Image.Open(IO.Wrap.ByteStream.Open(device));
		}
		public static Image Open (System.IO.Stream stream)
		{
			Image result;
			try
			{
				result = new System.Drawing.Bitmap(stream).AsImage();
			}
			catch (ArgumentException)
			{
				result = null;
			}
			return result;
		}
		public static T Open<T> (System.IO.Stream stream) where T : Raster.Image
		{
			return Image.Open(stream).As<T>();
		}
		#endregion
		#region IDisposable Members Override
		public override void Dispose ()
		{
			if (this.cairo.NotNull())
			{
				this.cairo.Dispose();
				this.cairo = null;
			}
			if (this.buffer.NotNull())
			{
				this.buffer.Dispose();
				this.buffer = null;
			}
		}
		#endregion
	}
}
