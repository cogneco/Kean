//
//  Bgra.cs
//
//  Author:
//      Simon Mika <smika@hx.se
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
using Geometry2D = Kean.Math.Geometry2D;

namespace Kean.Draw.Cairo.Test
{
	[TestFixture]
	public class Bgra :
		Draw.Raster.Test.Fixture<Bgra>
	{
		public Bgra() :
			base(0.01f)
		{
		}
		protected override void Run()
		{
			this.Run(
				this.Create
			);
		}
		[Test]
		public void Create()
		{
			using (Cairo.Raster image = new Cairo.Bgra(new Geometry2D.Integer.Size(128, 256)))
			using (Draw.Raster.Bgra raster = new Draw.Raster.Bgra(image.Buffer, image.Size, image.CoordinateSystem, image.Crop))
				Verify(raster, "Draw.Cairo.Correct.Bgra.Create.png");
		}
	}
}
