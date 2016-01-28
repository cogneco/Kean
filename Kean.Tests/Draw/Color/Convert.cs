// 
//  Convert.cs
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
using NUnit.Framework;

using Target = Kean.Draw.Color;

namespace Kean.Draw.Test.Color
{
	public class Convert :
		Kean.Test.Fixture<Convert>
	{
		protected override void Run ()
		{
			this.Run (
				this.FromBgr, 
				this.FromBgra,
				this.FromY,
				this.FromYuv
			);
		}

		[Test]
		public void FromBgr ()
		{
			Target.Bgr value = new Target.Bgr (120, 100, 80);
			Expect (value.Convert<Target.Monochrome> (), Is.EqualTo (new Target.Monochrome (95)));
			Expect (value.Convert<Target.Yuv> (), Is.EqualTo (new Target.Yuv (95, 141, 116)));
			Expect (value.Convert<Target.Bgr> (), Is.EqualTo (new Target.Bgr (120, 100, 80)));
			Expect (value.Convert<Target.Bgra> (), Is.EqualTo (new Target.Bgra (120, 100, 80, 255)));
		}

		[Test]
		public void FromBgra ()
		{
			Target.Bgra value = new Target.Bgra (120, 100, 80, 60);
			Expect (value.Convert<Target.Monochrome> (), Is.EqualTo (new Target.Monochrome (95)));
			Expect (value.Convert<Target.Yuv> (), Is.EqualTo (new Target.Yuv (95, 141, 116)));
			Expect (value.Convert<Target.Bgr> (), Is.EqualTo (new Target.Bgr (120, 100, 80)));
			Expect (value.Convert<Target.Bgra> (), Is.EqualTo (new Target.Bgra (120, 100, 80, 60)));
		}

		[Test]
		public void FromY ()
		{
			Target.Monochrome value = new Target.Monochrome (95);
			Expect (value.Convert<Target.Monochrome> (), Is.EqualTo (new Target.Monochrome (95)));
			Expect (value.Convert<Target.Yuv> (), Is.EqualTo (new Target.Yuv (95, 128, 128)));
			Expect (value.Convert<Target.Bgr> (), Is.EqualTo (new Target.Bgr (95, 95, 95)));
			Expect (value.Convert<Target.Bgra> (), Is.EqualTo (new Target.Bgra (95, 95, 95, 255)));
		}

		[Test]
		public void FromYuv ()
		{
			Target.Yuv value = new Target.Yuv (95, 141, 116);
			Expect (value.Convert<Target.Monochrome> (), Is.EqualTo (new Target.Monochrome (95)));
			Expect (value.Convert<Target.Yuv> (), Is.EqualTo (new Target.Yuv (95, 141, 116)));
			Expect (value.Convert<Target.Bgr> (), Is.EqualTo (new Target.Bgr (118, 99, 78)));
			Expect (value.Convert<Target.Bgra> (), Is.EqualTo (new Target.Bgra (118, 99, 78, 255)));
		}
	}
}
