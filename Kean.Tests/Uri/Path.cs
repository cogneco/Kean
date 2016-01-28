// 
//  Path.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010-2011 Simon Mika
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
using Target = Kean.Uri;

namespace Kean.Uri.Test
{
	[TestFixture]
	public class Path :
		Kean.Test.Fixture<Path>
	{
		protected override void Run()
		{
			this.Run(
				this.EqualityNull,
				this.Equality,
				this.Space,
				this.Plus,
				this.Hash,
				this.Filename,
				this.Folder,
				this.Extension,
				this.Stem
			);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Path path = null;
			Verify(path, Is.Null);
			Verify(path == null, Is.True);
		}

		[Test]
		public void Equality()
		{
			Target.Path path = "folderA/folderB/file.extension";
			Verify(path, Is.Not.Null);
			Verify(path != null, Is.True);
			Verify((string)path, Is.EqualTo("/folderA/folderB/file.extension"));
			Verify(path == "folderA/folderB/file.extension", Is.True);
		}

		[Test]
		public void Space()
		{
			Target.Path path = "folder A/folder B/file C.extension";
			Verify(path, Is.Not.Null);
			Verify(path != null, Is.True);
			Verify((string)path, Is.EqualTo("/folder%20A/folder%20B/file%20C.extension"));
			Verify(path == "folder%20A/folder%20B/file%20C.extension", Is.True);
		}

		[Test]
		public void Plus()
		{
			Target.Path path = "folder+A/folder+B/file+C.extension";
			Verify(path, Is.Not.Null);
			Verify(path != null, Is.True);
			Verify((string)path, Is.EqualTo("/folder+A/folder+B/file+C.extension"));
			Verify(path == "folder+A/folder+B/file+C.extension", Is.True);
		}

		[Test]
		public void Hash()
		{
			Target.Path path = "folderA/folderB/file.extension";
			Target.Path path2 = "folderA/folderB/file.extension2";
			Verify(path, Is.Not.EqualTo(path2));
			Verify(path.GetHashCode(), Is.Not.EqualTo(path2.GetHashCode()));
		}

		[Test]
		public void Filename()
		{
			Target.Path path = "$(Pictures)/file.extension";
			Target.Path path2 = "$(Pictures)/folder/file.extension2";
			Verify(path.Filename, Is.EqualTo("file.extension"));
			Verify(path2.Filename, Is.EqualTo("file.extension2"));
		}

		[Test]
		public void Folder()
		{
			Target.Path path = "$(Pictures)/file.extension";
			Target.Path path2 = "$(Pictures)";
			Target.Path path3 = "$(Pictures)/";
			Target.Path path4 = "$(Pictures)/folder";
			Target.Path path5 = "$(Pictures)/folder/";
			Verify(path.Folder, Is.False);
			Verify(path2.Folder, Is.False);
			Verify(path3.Folder, Is.True);
			Verify(path4.Folder, Is.False);
			Verify(path5.Folder, Is.True);
		}

		[Test]
		public void Extension()
		{
			Target.Path path = "$(Pictures)/file.extension";
			Target.Path path2 = "$(Pictures)/folder/file.extension2";
			Verify(path.Extension, Is.EqualTo("extension"));
			Verify(path2.Extension, Is.EqualTo("extension2"));
		}

		[Test]
		public void Stem()
		{
			Target.Path path = "$(Pictures)/file.extension";
			Target.Path path2 = "$(Pictures)/folder/file.extension2";
			Verify(path.Stem, Is.EqualTo("file"));
			Verify(path2.Stem, Is.EqualTo("file"));
		}
	}
}
