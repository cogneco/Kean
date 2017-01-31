// 
//  Bgra.cs
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
using Single = Kean.Math.Single;

namespace Kean.Draw.Color
{
	public struct Bgra :
		IColor, 
		System.IEquatable<Bgra>
	{
		public byte Blue { get { return this.Color.Blue; } set { this.Color.Blue = value; } }
		public byte Green { get { return this.Color.Green; } set { this.Color.Green = value; } }
		public byte Red { get { return this.Color.Red; } set { this.Color.Red = value; } }
		public Bgr Color;
		public byte Alpha;
		public Bgra(byte blue, byte green, byte red) : this(blue, green, red, 255) { }
		public Bgra(byte blue, byte green, byte red, byte alpha) : this(new Bgr(blue, green, red), alpha) { }
		public Bgra(Bgr color, byte alpha)
		{
			this.Color = color;
			this.Alpha = alpha;
		}
		public string HexString
		{
			get { return this.Color.HexString + this.Alpha.ToString("x02"); }
		}
		#region IColor Members
		public void Set<T>(T color) where T : IColor
		{
			Bgra c = color.Convert<Bgra>();
			this.Alpha = c.Alpha;
			this.Color.Set(c.Color);
		}
		public IColor Copy()
		{
			return new Bgra(this.Color, this.Alpha);
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Monochrome))
				Draw.Color.Convert.FromBgr((Monochrome v) => result = (T)(IColor)v)(this.Color);
			else if (typeof(T) == typeof(Yuv))
				Draw.Color.Convert.FromBgr((Yuv v) => result = (T)(IColor)v)(this.Color);
			else if (typeof(T) == typeof(Bgr))
				result = (T)(IColor)this.Color;
			else if (typeof(T) == typeof(Bgra))
				result = (T)(IColor)this;
			return result;
		}
		public IColor Blend(float factor, IColor other)
		{
			Bgra c = other.Convert<Bgra>();
			return new Bgra(this.Color.Blend(factor, c.Color).Convert<Bgr>(), (byte)(this.Alpha * (1 - factor) + c.Alpha * factor));
		}
		public float Distance(IColor other)
		{
			Bgra c = other.Convert<Bgra>();
			return Single.SquareRoot((3 * Single.Squared(this.Color.Distance(c.Color)) + Single.Squared(this.Alpha - c.Alpha)) / 4);
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this.Color.ToString() + " " + this.Alpha;
		}
		public override bool Equals(object other)
		{
			return other is Bgra && this.Equals((Bgra)other);
		}
		public override int GetHashCode()
		{
			return 33 * this.Color.GetHashCode() ^ this.Alpha.GetHashCode();
		}
		#endregion
		#region IEquatable<Bgra> Members
		public bool Equals(Bgra other)
		{
			return this.Color == other.Color && this.Alpha == other.Alpha;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Bgra left, Bgra right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Bgra left, Bgra right)
		{
			return !(left == right);
		}
		#endregion
	}
}
