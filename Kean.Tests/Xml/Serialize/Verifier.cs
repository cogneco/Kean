//
//  Verifier.cs
//
//  Author:
//       Simon Mika <smika@hx.se>
//
//  Copyright (c) 2013 Simon Mika
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
using Uri = Kean.Uri;

namespace Kean.Xml.Serialize.Test
{
	public class Verifier :
		Kean.Serialize.Test.Verifier
	{
		protected override string Extension { get { return "xml"; } }
		protected override Uri.Locator CorrectBase { get { return "assembly://Kean.Tests/Xml/Serialize/Xml"; } }
		protected override Uri.Locator CreatedBase { get { return Uri.Locator.FromPlatformPath(System.Environment.CurrentDirectory) + "Xml"; } }
		public Verifier() :
			base(new Storage())
		{
		}
	}
}

