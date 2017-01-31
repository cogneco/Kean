// 
//  Y.cs
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
using Kean;
using Kean.Extension;
using Single = Kean.Math.Single;

namespace Kean.Draw.Color
{
	public struct Monochrome :
		IColor,
		System.IEquatable<Monochrome>
	{
		public byte Y;
		public Monochrome(byte y)
		{
			this.Y = y;
		}
		public Monochrome(float y)
		{
			this.Y = (byte)Math.Single.Clamp(y * 255, 0, 255);
		}
		public Monochrome(double y)
		{
			this.Y = (byte)Math.Double.Clamp(y * 255, 0, 255);
		}
		#region Casts
		public static implicit operator Monochrome(byte value)
		{
			unsafe { return *((Monochrome*)&value); }
		}
		public static implicit operator byte(Monochrome value)
		{
			unsafe { return *((byte*)&value); }
		}
		public static implicit operator Monochrome(float value)
		{
			return new Monochrome(value);
		}
		public static implicit operator float(Monochrome value)
		{
			return value.Y / 255.0f;
		}
		public static implicit operator Monochrome(double value)
		{
			return new Monochrome(value);
		}
		public static implicit operator double(Monochrome value)
		{
			return value.Y / 255.0;
		}
		#endregion
		#region IColor Members
		public IColor Copy()
		{
			return new Monochrome(this.Y);
		}
		public void Set<T>(T color) where T : IColor
		{
			Monochrome c = color.Convert<Monochrome>();
			this.Y = c.Y;
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Monochrome))
				result = (T)(IColor)this;
			else if (typeof(T) == typeof(Yuv))
				Color.Convert.FromY((Yuv v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Bgr))
				Color.Convert.FromY((Bgr v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Bgra))
			{
				Bgr color = new Bgr();
				Color.Convert.FromY((Bgr v) => color = (Bgr)(IColor)v)(this);
				result = (T)(IColor)new Bgra(color,255);
			}
			return result;
		}
		public IColor Blend(float factor, IColor other)
		{
			Monochrome c = other.Convert<Monochrome>();
			return new Monochrome((byte)(this.Y * (1 - factor) + c.Y * factor));
		}
		public float Distance(IColor other)
		{
			Monochrome c = other.Convert<Monochrome>();
			return Single.SquareRoot(Single.Squared(this.Y - c.Y));
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this.Y.ToString();
		}
		public override bool Equals(object other)
		{
			return other is Monochrome && this.Equals((Monochrome)other);
		}
		#endregion
		#region IEquatable<Y> Members
		public bool Equals(Monochrome other)
		{
			return this.Y == other.Y;
		}
		public override int GetHashCode()
		{
			return this.Y.GetHashCode();
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Monochrome left, Monochrome right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Monochrome left, Monochrome right)
		{
			return !(left == right);
		}
		#endregion
	}
}
