// 
//  NullableTypes.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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
using NUnit.Framework;
using Kean.Extension;

namespace Kean.Serialize.Test
{
	public class NullableTypes<C> :
		Factory<NullableTypes<C>, C>
			where C : Verifier, new()
	{
		protected override void  Run()
		{
			this.Run(
				this.Byte,
				this.SignedByte,
				this.Short,
				this.UnsignedShort,
				this.Integer,
				this.UnsignedInteger,
				this.Long,
				this.UnsignedLong,
				this.Single,
				this.Double,
				this.Decimal,
				this.Char,
				this.DateTime,
				this.DateTimeOffset,
				this.TimeSpan,
				this.Boolean,
				this.Enumerator,
				this.Structure,
				this.Nullable
			);
		}
		[Test] 
		public void Byte()
		{
			this.Test(typeof(byte?));
		}
		[Test] 
		public void SignedByte()
		{
			this.Test(typeof(sbyte?));
		}
		[Test] 
		public void Short()
		{
			this.Test(typeof(short?));
		}
		[Test] 
		public void UnsignedShort()
		{
			this.Test(typeof(ushort?));
		}
		[Test]
		public void Integer()
		{
			this.Test(typeof(int?));
		}
		[Test]
		public void UnsignedInteger()
		{
			this.Test(typeof(uint?));
		}
		[Test]
		public void Long()
		{
			this.Test(typeof(long?));
		}
		[Test]
		public void UnsignedLong()
		{
			this.Test(typeof(ulong?));
		}
		[Test] 
		public void Single()
		{
			this.Test(typeof(float?));
		}
		[Test]
		public void Double()
		{
			this.Test(typeof(double?));
		}
		[Test]
		public void Decimal()
		{
			this.Test(typeof(decimal?));
		}
		[Test] 
		public void Char()
		{
			this.Test(typeof(char?));
		}
		[Test]
		public void DateTime()
		{
			this.Test(typeof(DateTime?));
		}
		[Test]
		public void DateTimeOffset()
		{
			this.Test(typeof(DateTimeOffset?));
		}
		[Test]
		public void TimeSpan()
		{
			this.Test(typeof(TimeSpan?));
		}
		[Test]
		public void Boolean()
		{
			this.Test(typeof(bool?));
		}
		[Test] 
		public void Enumerator()
		{
			this.Test(typeof(Data.Enumerator?));
		}
		[Test]
		public void Structure()
		{
			this.Test(typeof(Data.Structure?));
		}
		[Test]
		public void Nullable()
		{
			this.Test(typeof(Data.Nullable));
		}
	}
}
