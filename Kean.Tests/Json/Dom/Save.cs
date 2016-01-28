//
//  Open.cs
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
using Kean;
using Kean.Extension;
using Uri = Kean.Uri;
using NUnit.Framework;

namespace Kean.Json.Dom.Test
{
	public class Save :
		Tests<Save>
	{
		protected override void Verify(string name)
		{
			Uri.Locator filename = Uri.Locator.FromPlatformPath(System.IO.Path.GetFullPath(name + ".json"));
			this.Create(name).Save(filename);
			Uri.Locator resource = "assembly:///Json/Dom/Data/" + name + ".json";
			VerifyAsResource(filename.PlatformPath, resource.Path, "Json save test \"{0}\" failed.", name);
		}
	}
}
