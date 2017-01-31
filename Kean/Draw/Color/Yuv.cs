// 
//  Yuv.cs
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
	public struct Yuv :
		IColor,
		System.IEquatable<Yuv>
	{
		public byte Y;
		public byte U;
		public byte V;
		public Yuv(byte y, byte u, byte v)
		{
			this.Y = y;
			this.U = u;
			this.V = v;
		}
		#region IColor Members
		public IColor Copy()
		{
			return new Yuv(this.Y, this.U, this.V);
		}
		public void Set<T>(T color) where T : IColor
		{
			Yuv c = color.Convert<Yuv>();
			this.Y = c.Y;
			this.U = c.U;
			this.V = c.V;
		}
		public T Convert<T>() where T : IColor, new()
		{
			T result = default(T);
			if (typeof(T) == typeof(Monochrome))
				Color.Convert.FromYuv((Monochrome v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Yuv))
				result = (T)(IColor)this;
			else if (typeof(T) == typeof(Bgr))
				Color.Convert.FromYuv((Bgr v) => result = (T)(IColor)v)(this);
			else if (typeof(T) == typeof(Bgra))
			{
				Bgr color = new Bgr();
				Color.Convert.FromYuv((Bgr v) => color = (Bgr)(IColor)v)(this);
				result = (T)(IColor)new Bgra(color, 255);
			}
			return result;
		}
		public IColor Blend(float factor, IColor other)
		{
			Yuv c = other.Convert<Yuv>();
			return new Yuv((byte)(this.Y * (1 - factor) + c.Y * factor), (byte)(this.U * (1 - factor) + c.U * factor), (byte)(this.V * (1 - factor) + c.V * factor));
		}
		public float Distance(IColor other)
		{
			Yuv c = other.Convert<Yuv>();
			return Single.SquareRoot((Math.Single.Squared(this.Y - c.Y) + Single.Squared(this.U - c.U) + Single.Squared(this.V - c.V)) / 3);
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			return this.Y + " " + this.U + " " + this.V;
		}
		public override bool Equals(object other)
		{
			return other is Yuv && this.Equals((Yuv)other);
		}
		public override int GetHashCode()
		{
			return 33 * (33 * this.Y.GetHashCode() ^ this.U.GetHashCode()) ^ this.V.GetHashCode();
		}
		#endregion
		#region IEquatable<Yuv> Members
		public bool Equals(Yuv other)
		{
			return this.Y == other.Y && this.U == other.U && this.V == other.V;
		}
		#endregion
		#region Comparison Operators
		public static bool operator ==(Yuv left, Yuv right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Yuv left, Yuv right)
		{
			return !(left == right);
		}
		#endregion
	}
}
