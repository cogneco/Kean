// 
//  Interval.cs
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
    public class Interval :
        Interval<byte>
    {
        public Interval()
        {
        }
        public override byte Generate()
        {
            int result;
            int length = this.Ceiling - this.Floor;
            int bits = Kean.Math.Integer.Ceiling(Kean.Math.Double.Logarithm(length + 1, 2));
            do
                result = (int)this.Next(bits);
            while (result > length);
            return (byte)(result + this.Floor);
        }
        public override byte[] Generate(int count)
        {
            byte[] result = new byte[count];
            int numberOfUlongs = Kean.Math.Integer.Ceiling(count / 8f);
            byte[] buffer = new byte[numberOfUlongs * 8];
            ulong[] ulongbuffer = new ulong[numberOfUlongs];
            for (int i = 0; i < numberOfUlongs; i++)
                ulongbuffer[i] = this.Next();
            for (int i = 0; i < numberOfUlongs; i++)
                Array.Copy(BitConverter.GetBytes(ulongbuffer[i]), 0, buffer, i * 8, 8);
            byte floor = this.Floor;
            byte ceiling = this.Ceiling;
            for (int i = 0; i < count; i++)
                result[i] = (byte)(floor + (float)buffer[i] * (ceiling - floor) / 255f);
            return result;
        }
    }
}

