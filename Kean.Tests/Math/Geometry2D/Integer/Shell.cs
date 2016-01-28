// 
//  Size.cs
//  
//  Author:
//       Anders Frisk <andersfrisk77@gmail.com>
//  
//  Copyright (c) 2012 Anders Frisk
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
using Target = Kean.Math.Geometry2D;
using Kean.Extension;

namespace Kean.Math.Geometry2D.Test.Integer
{
	[TestFixture]
	public class Shell :
		Kean.Test.Fixture<Shell>
	{
		protected override void Run()
		{
			this.Run(
				this.StringCasts
				);
		}
		[Test]
		public void StringCasts()
		{
			string textFromValue = new Target.Integer.Shell(10, 10, 10, 10);
			Verify(textFromValue, Is.EqualTo("10"));
			textFromValue = new Target.Integer.Shell(10, 10, 20, 20);
			Verify(textFromValue, Is.EqualTo("10, 20"));
			textFromValue = new Target.Integer.Shell(10, 20, 10, 20);
			Verify(textFromValue, Is.EqualTo("10, 20, 10, 20"));
		}
	}
}
