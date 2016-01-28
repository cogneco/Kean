// 
//  Fixture.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2012-2013 Simon Mika
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
using Kean;
using Kean.Extension;
using NUnit.Framework;
using Reflect = Kean.Reflect;

namespace Kean.Draw.Raster.Test
{
	public abstract class Fixture<T> :
		Kean.Test.Fixture<T>
		where T : Fixture<T>, new()
	{
		public float Tolerance { get; private set; }

		protected Fixture () :
			this(0.01f)
		{
		}

		protected Fixture (float tolerance)
		{
			this.Tolerance = tolerance;
		}

		public void Verify (Draw.Image image, string resource)
		{
			this.Verify (image, resource, Is.LessThanOrEqualTo (this.Tolerance));
		}

		public void Verify (Draw.Image image, string resource, NUnit.Framework.Constraints.Constraint constraint)
		{
			using (Image correct = Image.OpenResource(System.Reflection.Assembly.GetAssembly(typeof(T)), resource))
				this.Verify (image, correct, constraint);
		}

		public void Verify (Draw.Image image, Raster.Image correct)
		{
			this.Verify (image, correct, Is.LessThanOrEqualTo (this.Tolerance));
		}

		public void Verify (Draw.Image image, Raster.Image correct, NUnit.Framework.Constraints.Constraint constraint)
		{
			try 
			{
				Expect (correct, Is.Not.Null);
				this.Verify (correct.Distance (image), constraint);
			}
			catch (NUnit.Framework.AssertionException) 
			{
				using (Raster.Image raster = image.Convert<Raster.Image>())
					raster.Save (Uri.Locator.FromRelativePlatformPath(this.CurrentTestStep + ".png"));
				throw;
			} 
		}
	}
}
