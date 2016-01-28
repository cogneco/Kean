// 
//  Bgr.cs
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

using System;
using Kean.Extension;
using Single = Kean.Math.Single;

namespace Kean.Draw.Color
{
	public struct Bgr :
		IColor, 
		System.IEquatable<Bgr>
	{
		public byte Blue;
		public byte Green;
		public byte Red;
		public Bgr(byte blue, byte green, byte red)
		{
			this.Blue = blue;
			this.Green = green;
			this.Red = red;
		}

		#region IColor Members
		public IColor Copy()
		{
			return new Bgr(this.Blue, this.Red, this.Green);
		}
		public void Set<T>(T color) where T : IColor
		{
			Bgr c = color.Convert<Bgr>();
			this.Blue = c.Blue;
			this.Green = c.Green;
			this.Red = c.Red;
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Monochrome))
				Color.Convert.FromBgr((Monochrome v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Yuv))
				Color.Convert.FromBgr((Yuv v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Bgr))
				result = (T)(IColor)this;
			else if (typeof(T) == typeof(Bgra))
				result = (T)(IColor)new Bgra(this, 255);
			return result;
		}
		public IColor Blend(float factor, IColor other)
		{
			Bgr c = other.Convert<Bgr>();
			return new Bgr((byte)(this.Blue * (1 - factor) + c.Blue * factor), (byte)(this.Green * (1 - factor) + c.Green * factor), (byte)(this.Red * (1 - factor) + c.Red * factor));
		}
		public float Distance(IColor other)
		{
			Bgr c = other.Convert<Bgr>();
			float result = Single.SquareRoot((Single.Squared(this.Blue - c.Blue) + Single.Squared(this.Green - c.Green) + Single.Squared(this.Red - c.Red)) / 3);
			if (result.NotNumber())
				Console.WriteLine(this + " " + other);
			return result;
		}
		#endregion
		#region Object Overides
		public override string ToString()
		{
			return this.Blue + " " + this.Green + " " + this.Red;
		}
		public override bool Equals(object other)
		{
			return other is Bgr && this.Equals((Bgr)other);
		}
		public override int GetHashCode()
		{
			return 33 * (33 * this.Blue.GetHashCode() ^ this.Green.GetHashCode()) ^ this.Red.GetHashCode();
		}
		#endregion
		#region IEquatable<Bgr> Members
		public bool Equals(Bgr other)
		{
			return this.Blue == other.Blue && this.Green == other.Green && this.Red == other.Red;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Bgr left, Bgr right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Bgr left, Bgr right)
		{
			return !(left == right);
		}
		#endregion
	}
}
