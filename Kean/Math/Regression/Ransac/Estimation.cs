// 
//  Estimation.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2011 Anders Frisk
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
using Collection = Kean.Collection;
using Kean.Extension;

namespace Kean.Math.Regression.Ransac
{
    public class Estimation<Domain, Range, Transform>
    {
        public Domain[] InlierDomain { get; private set; }
        public Range[] InlierRange { get; private set; }
        public Transform Mapping { get; private set; }
        public bool[] Mask { get; private set; }
        public int Count { get { return this.InlierDomain.Length; } }
        public Estimation(Domain[] inlierDomain, Range[] inlierRange, Transform mapping, bool[] mask)
        {
            this.InlierDomain = inlierDomain;
            this.InlierRange = inlierRange;
            this.Mapping = mapping;
            this.Mask = mask;
        }
        public override string ToString()
        {
            return "Count: " + this.Count + "  Transform: " + this.Mapping.ToString();
        }
    }
}
