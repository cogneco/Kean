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
using Kean;
using Kean.Extension;
using Geometry2D = Kean.Math.Geometry2D;
using Geometry3D = Kean.Math.Geometry3D;
using Error = Kean.Error;
using Integer = Kean.Math.Integer;

namespace Kean.Draw
{
	[System.Runtime.InteropServices.ComVisible(true)]
	public abstract class Image :
		IEquatable<Image>,
		IDisposable
	{
		public abstract Canvas Canvas { get; }

		public bool IsValidIn(int x, int y)
		{
			return (x >= 0 && x < this.Size.Width && y >= 0 && y < this.Size.Height);
		}
		public Geometry2D.Integer.Size Size { get; private set; }
		Geometry2D.Integer.Transform transform = Geometry2D.Integer.Transform.Identity;
		protected Geometry2D.Integer.Transform Transform { get { return this.transform; } }
		#region CoordinateSystem
		CoordinateSystem coordinateSystem;
		[Notify("CoordinateSystemChanged")]
		public CoordinateSystem CoordinateSystem
		{
			get { return this.coordinateSystem; }
			set
			{
				if (this.coordinateSystem != value)
				{
					this.coordinateSystem = value;
					this.transform = Geometry2D.Integer.Transform.CreateScaling(
						(value & CoordinateSystem.XLeftward) == CoordinateSystem.XLeftward ? -1 : 1,
						(value & CoordinateSystem.YUpward) == CoordinateSystem.YUpward ? -1 : 1);
					this.CoordinateSystemChanged.Call(this.coordinateSystem);
				}
			}
		}
		public event Action<CoordinateSystem> CoordinateSystemChanged;
		#endregion
		#region Crop
		Geometry2D.Integer.Shell crop;
		[Notify("CropChanged")]
		public virtual Geometry2D.Integer.Shell Crop 
		{
			get { return this.crop; }
			set
			{
				if (this.crop != value)
				{
					this.crop = value;
					this.CropChanged.Call(this.crop);
				}
			}
		}
		public event Action<Geometry2D.Single.Shell> CropChanged;
		#endregion
		#region Wrap
		bool wrap;
		[Notify("WrapChanged")]
		public bool Wrap
		{
			get { return this.wrap; }
			set
			{
				if (this.wrap != value)
				{
					this.wrap = value;
					this.WrapChanged.Call(this.wrap);
				}
			}
		}
		public event Action<bool> WrapChanged;
		#endregion
		#region Peek & Poke
		public abstract IColor this[int x, int y] { get; set; }
		public IColor this[float x, float y]
		{
			get
			{
				var topLeft = this[Integer.Floor(x), Integer.Floor(y)];
				var bottomLeft = this[Integer.Floor(x), Integer.Ceiling(y)];
				var topRight = this[Integer.Ceiling(x), Integer.Floor(y)];
				var bottomRight = this[Integer.Ceiling(x), Integer.Ceiling(y)];
				var deltaX = x - Integer.Floor(x);
				var deltaY = y - Integer.Floor(y);
				return topLeft.Blend(deltaX, topRight).Blend(deltaY, bottomLeft.Blend(deltaX, bottomRight));
			}
		}
		public IColor this[Geometry2D.Integer.Point position]
		{
			get { return this[position.X, position.Y]; }
			set { this[position.X, position.Y] = value; }
		}
		public IColor this[Geometry2D.Single.Point position]
		{
			get { return this[position.X, position.Y]; }
		}
		#endregion
		protected Image()
		{ }
		protected Image(Image original) :
			this(original.Size, original.CoordinateSystem, original.Crop, original.Wrap) 
		{ }
		protected Image(Geometry2D.Integer.Size size, CoordinateSystem coordinateSystem, Geometry2D.Integer.Shell crop = new Geometry2D.Integer.Shell(), bool wrap = false)
		{
			this.Size = size;
			this.CoordinateSystem = coordinateSystem;
			this.Crop = crop;
			this.Wrap = wrap;
		}
		public abstract T Convert<T>() where T : Image;
		public T As<T>() where T : Image
		{
			T result;
			if (this is T)
				result = this as T;
			else
			{
				result = this.Convert<T>();
				this.Dispose();
			}
			return result;
		}

		public void ProjectPointWise(Draw.Image source, Geometry3D.Single.Transform transform, Geometry2D.Single.Size fieldOfView)
		{
			float zPlane = (float)this.Size.Width / (Math.Single.Tangens(fieldOfView.Width / 2f) * 2f);
			var cam = Geometry2D.Single.Transform.CreateTranslation(-(source.Size.Width) / 2f, -(source.Size.Height) / 2f);
			for (int y = 0; y < this.Size.Height; y++)
				for (int x = 0; x < this.Size.Width; x++)
				{
					var p = cam * new Geometry2D.Single.Point(x, y);
					var t = p.Project(transform, zPlane);
					var d = cam.Inverse * t;
					this[x, y] = source[d.X, d.Y]; 
				}
		}

		public void Project(Draw.Image source, Geometry3D.Single.Transform camera, Geometry2D.Single.Size fieldOfView)
		{
			float focalLengthX = (float)this.Size.Width / (Math.Single.Tangens(fieldOfView.Width / 2f) * 2f);
			float height = 2 * focalLengthX * Math.Single.Tangens(fieldOfView.Height / 2f);
			var transform = Geometry3D.Single.Transform.CreateRotation(camera, new Geometry3D.Single.Point(source.Size.Width / 2f, source.Size.Height / 2f, focalLengthX));
			var pointTransform = transform * Geometry3D.Single.Transform.CreateTranslation((source.Size.Width - this.Size.Width) / 2f, (source.Size.Height - this.Size.Height) / 2f, 0);
			var cam = transform * new Geometry3D.Single.Point((source.Size.Width) / 2f, (source.Size.Height) / 2f, focalLengthX);
			Project(source, pointTransform, cam); 
		}

		protected virtual void Project(Draw.Image source, Geometry3D.Single.Transform pointTransform, Geometry3D.Single.Point cam)
		{
			for (int y = 0; y < this.Size.Height; y++)
				for (int x = 0; x < this.Size.Width; x++)
				{
					var p = pointTransform * new Geometry3D.Single.Point(x, y, 0);
					var d = cam + (Geometry3D.Single.Point)(p - cam) * (cam.Z / (cam.Z - p.Z));
					this[x, y] = source[d.X, d.Y];
				}
		}
		public Draw.Image ResizeWithin(Geometry2D.Integer.Size restriction)
		{
			return this.ResizeTo((Geometry2D.Integer.Size)((Geometry2D.Single.Size)this.Size * Math.Single.Minimum((float)restriction.Width / (float)this.Size.Width, (float)restriction.Height / (float)this.Size.Height)));
		}
		public abstract Image ResizeTo(Geometry2D.Integer.Size size);
		public abstract Image Create(Geometry2D.Integer.Size size);
		public abstract Image Copy();
		public abstract Image Copy(Geometry2D.Integer.Size size, Geometry2D.Single.Transform transform);
		public abstract Image Shift(Geometry2D.Integer.Size offset);
		public virtual void Flush()
		{
		}
		public virtual bool Finish()
		{
			return true;
		}
		public abstract float Distance(Image other);
		public override bool Equals(object other)
		{
			return other is Image && this.Equals(other as Image);
		}
		public virtual bool Equals(Image other)
		{
			return other.NotNull() && this.Size == other.Size && this.Distance(other) < 10 * float.Epsilon;
		}
		~Image()
		{
			Error.Log.Wrap((Action)this.Dispose)();
		}
		#region IDisposable Members
		public virtual void Dispose()
		{
		}
		#endregion
	}
}
