//
//  Preprocessor.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using IO = Kean.IO;
using Uri = Kean.Uri;
using Generic = System.Collections.Generic;

namespace Kean.Xml.Serialize
{
	class Preprocessor :
		Dom.Processor
	{
		public Preprocessor()
		{
		}
		protected override Generic.IEnumerator<Dom.Node> Process(Dom.ProcessingInstruction instruction)
		{
			Generic.IEnumerator<Dom.Node> result;
			switch (instruction.Target)
			{
				case "include":
					result = this.Process(this.LoadAll(((Uri.Locator)instruction.Value).Resolve((Uri.Locator)instruction.Region.Resource)));
					break;
				default:
					result = base.Process(instruction);
					break;
			}
			return result;
		}
		Generic.IEnumerator<Dom.Node> LoadAll(Uri.Locator locator)
		{
			return (locator.Scheme == "file" && locator.Authority == null) ? this.LoadAll(locator.Path) : this.Load(locator);
		}
		Generic.IEnumerator<Dom.Node> LoadAll(Uri.Path path)
		{
			string platformPath = System.IO.Path.GetFullPath(path.FolderPath.PlatformPath);
			if (System.IO.Directory.Exists(platformPath))
				foreach (string file in System.IO.Directory.GetFiles(platformPath, path.Last.Head).Sort())
				{
					Dom.Document document = Dom.Document.Open(Uri.Locator.FromPlatformPath(file));
					if (document.NotNull() && document.Root.NotNull())
						yield return document.Root;
				}
		}
		Generic.IEnumerator<Dom.Node> Load(Uri.Locator locator)
		{
			Dom.Document document = Dom.Document.Open(locator);
			if (document.NotNull() && document.Root.NotNull())
				yield return document.Root;
		}
	}
}
