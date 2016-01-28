// 
//  Missing.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012 Simon Mika
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
using Uri = Kean.Uri;
using NUnit.Framework;
namespace Kean.Xml.Serialize.Test
{
    public class Missing :
		Kean.Test.Fixture<Missing>
    {
        Serialize.Storage storage;
        void Test(Uri.Locator locator)
        {
            Verify(this.storage.Load<object>(locator), Is.Null, "Deserialize Missing {0}", locator);
            if (locator.Scheme == "file")
                Verify(this.storage.Store<object>(new Object(), locator), Is.True, "Serialize Missing {0}", locator);
            else
                Verify(this.storage.Store<object>(new Object(), locator), Is.False, "Serialize Missing {0}", locator);
        }
        public override void Setup()
        {
            storage = new Storage();
            base.Setup();
        }
        protected override void Run()
        {
            this.Run(
                this.Disk,
                this.Web
            );
        }
        [Test]
        public void Disk()
        {
            if (Kean.Environment.IsWindows)
                this.Test("file:///missing/missing.xml");
        }
        [Test]
        public void Web()
        {
            this.Test("http://kean.hx.se/dev/missing.xml");
        }
    }
}
