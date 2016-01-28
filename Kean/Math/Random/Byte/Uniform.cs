//
//  Uniform.cs
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

namespace Kean.Math.Random.Byte
{
	public class Uniform :
		Generator<byte>
	{
		public Uniform()
		{
		}
		public override byte Generate()
		{
			unchecked
			{
				return (byte)this.Next();
			}
		}
        public override byte[] Generate(int count)
        {
            byte[] result = new byte[count];
            int numberOfUlongs = Kean.Math.Integer.Ceiling(count / 8f);
            byte[] buffer = new byte[numberOfUlongs * 8];
            ulong[] source = new ulong[numberOfUlongs];
            for (int i = 0; i < count; i++)
                source[i] = this.Next();
            for(int i = 0; i < source.Length; i++)
                Array.Copy(BitConverter.GetBytes(source[i]),0, buffer, i, i * 8);
            Array.Copy(buffer, result, count);
            return result;
        }
	}
}