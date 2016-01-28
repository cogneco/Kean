// 
//  Tests.cs
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
using Kean;
using Kean.Extension;
using NUnit.Framework;

namespace Kean.Xml.Dom.Test
{
    [TestFixture]
    public abstract class Tests<T> :
        Factory<T>
		where T : Tests<T>, new()
	{
        [Test]
        public void Valid001()
        {
			this.Verify("valid001");
        }
        [Test]
        public void Valid002()
        {
			this.Verify("valid002");
        }
        [Test]
        public void Valid003()
        {
			this.Verify("valid003");
        }
        [Test]
        public void Valid004()
        {
			this.Verify("valid004");
        }
        [Test]
        public void Valid005()
        {
			this.Verify("valid005");
        }
        [Test]
        public void Valid006()
        {
			this.Verify("valid006");
        }
        [Test]
        public void Valid007()
        {
			this.Verify("valid007");
        }
		[Test]
		public void Valid008()
		{
			this.Verify("valid008");
		}
		[Test]
		public void Valid009()
		{
			this.Verify("valid009");
		}
		protected override void Run()
        {
            this.Run(
				this.Valid009,
				this.Valid008,
				this.Valid007,
                this.Valid006,
                this.Valid005,
                this.Valid004,
                this.Valid003,
                this.Valid002,
				this.Valid001
                );
        }
    }
}

