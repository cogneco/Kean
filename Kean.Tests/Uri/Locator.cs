// 
//  Locator.cs
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
	public class Locator :
		Kean.Test.Fixture<Locator>
	{
		protected override void Run()
		{
			this.Run(
				this.EqualityNull,
				this.SchemeOnly,
				this.Equality,
				this.PathAbsolute,
				this.PathAbsoluteWithoutResource,
				this.PathRelative,
				this.NoPath,
				this.NoPathWithQuery,
				this.RootPathWithQuery,
				this.NoPathWithQueryAndFragment,
				this.NoPathAndQueryWithFragment,
				this.FromPlatformPath,
				this.FromNetworkPlatformPath,
				this.SpaceEscaped,
				this.SpaceSpace,
				this.Plus,
				this.RelativeResolve);
		}

		[Test]
		public void EqualityNull()
		{
			Target.Locator locator = null;
			Verify(locator, Is.Null);
			Verify(locator == null, Is.True);
		}

		[Test]
		public void SchemeOnly()
		{
			Target.Locator locator = "schemeA://";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA"));
			locator = "schemeA+schemeB://";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
		}

		[Test]
		public void Equality()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment";
			Verify(locator, Is.Not.Null);
			Verify(locator != null, Is.True);
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo("/folderA/folderB/file.extension"));
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"));
			Verify((string)locator.Fragment, Is.EqualTo("fragment"));
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folderA/folderB/file.extension?keyA=valueA&keyB=valueB#fragment", Is.True);
		}

		[Test]
		public void SpaceEscaped()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200";
			Verify(locator, Is.Not.Null);
			Verify(locator != null, Is.True);
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo("/folder%20a/folder%20b/file%200.extension"));
			Verify((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"));
			Verify((string)locator.Fragment, Is.EqualTo("fragment 0"));
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200", Is.True);
		}

		[Test]
		public void SpaceSpace()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder a/folder b/file 0.extension?key a=value a&keyB=valueB#fragment 0";
			Verify(locator, Is.Not.Null);
			Verify(locator != null, Is.True);
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo("/folder%20a/folder%20b/file%200.extension"));
			Verify((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"));
			Verify((string)locator.Fragment, Is.EqualTo("fragment 0"));
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder%20a/folder%20b/file%200.extension?key+a=value+a&keyB=valueB#fragment%200"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folder a/folder b/file 0.extension?key a=value a&keyB=valueB#fragment 0", Is.True);
		}

		[Test]
		public void Plus()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0";
			Verify(locator, Is.Not.Null);
			Verify(locator != null, Is.True);
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo("/folder+a/folder+b/file+0.extension"));
			Verify((string)locator.Query, Is.EqualTo("key+a=value+a&keyB=valueB"));
			Verify((string)locator.Fragment, Is.EqualTo("fragment+0"));
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key a=value a&keyB=valueB#fragment+0", Is.True);
		}

		[Test]
		public void PathAbsolute()
		{
			Target.Locator locator = "/folderA/folderB/file.extension";
			Verify((string)locator.Scheme, Is.Null);
			Verify((string)locator.Authority, Is.Null);
			Verify((string)locator.Path, Is.EqualTo("/folderA/folderB/file.extension"));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("/folderA/folderB/file.extension"));
			Verify(locator == "/folderA/folderB/file.extension", Is.True);
		}

		[Test]
		public void PathAbsoluteWithoutResource()
		{
			Target.Locator locator = "/folderA/folderB/";
			Verify((string)locator.Scheme, Is.Null);
			Verify((string)locator.Authority, Is.Null);
			Verify((string)locator.Path, Is.EqualTo("/folderA/folderB/"));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("/folderA/folderB/"));
			Verify(locator == "/folderA/folderB/", Is.True);
		}

		[Test]
		public void PathRelative()
		{
			Target.Locator locator = "./folderA/folderB/file.extension";
			Verify((string)locator.Scheme, Is.Null);
			Verify((string)locator.Authority, Is.Null);
			Verify((string)locator.Path, Is.EqualTo("./folderA/folderB/file.extension"));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("./folderA/folderB/file.extension"));
			Verify(locator == "./folderA/folderB/file.extension", Is.True);
		}

		[Test]
		public void NoPath()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo(""));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80", Is.True);
		}

		[Test]
		public void NoPathWithQuery()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo(""));
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB", Is.True);
		}

		[Test]
		public void RootPathWithQuery()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo("/"));
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80/?keyA=valueA&keyB=valueB", Is.True);
		}

		[Test]
		public void NoPathWithQueryAndFragment()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo(""));
			Verify((string)locator.Query, Is.EqualTo("keyA=valueA&keyB=valueB"));
			Verify((string)locator.Fragment, Is.EqualTo("fragment"));
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80?keyA=valueA&keyB=valueB#fragment", Is.True);
		}

		[Test]
		public void NoPathAndQueryWithFragment()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80#fragment";
			Verify((string)locator.Scheme, Is.EqualTo("schemeA+schemeB"));
			Verify((string)locator.Authority, Is.EqualTo("name:password@example.com:80"));
			Verify((string)locator.Path, Is.EqualTo(""));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.EqualTo("fragment"));
			Verify((string)locator, Is.EqualTo("schemeA+schemeB://name:password@example.com:80#fragment"));
			Verify(locator == "schemeA+schemeB://name:password@example.com:80#fragment", Is.True);
		}

		[Test]
		public void FromPlatformPath()
		{
			string platformPath = Kean.Environment.IsWindows ? "C:\\Windows\\System32\\etc\\hosts" : "/C:/Windows/System32/etc/hosts"; 
			Target.Locator locator = Target.Locator.FromPlatformPath(platformPath);
			Verify((string)locator.Scheme, Is.EqualTo("file"));
			Verify((string)locator.Authority, Is.Null);
			Verify((string)locator.Path, Is.EqualTo("/C:/Windows/System32/etc/hosts"));
			Verify((string)locator.Query, Is.EqualTo(""));
			Verify((string)locator.Fragment, Is.Null);
			Verify((string)locator, Is.EqualTo("file:///C:/Windows/System32/etc/hosts"));
			Verify(locator == "file:///C:/Windows/System32/etc/hosts", Is.True);
			Verify((string)locator.PlatformPath, Is.EqualTo(platformPath));
			
		}

		[Test]
		public void FromNetworkPlatformPath()
		{
			if (Kean.Environment.IsWindows)
			{
				string platformPath = "\\\\SERVER\\Windows\\System32\\etc\\hosts";
				Target.Locator locator = Target.Locator.FromPlatformPath(platformPath);
				Verify((string)locator.Scheme, Is.EqualTo("file"));
				Verify((string)locator.Authority, Is.EqualTo("server"));
				Verify((string)locator.Path, Is.EqualTo("/Windows/System32/etc/hosts"));
				Verify((string)locator.Query, Is.EqualTo(""));
				Verify((string)locator.Fragment, Is.Null);
				Verify((string)locator, Is.EqualTo("file://server/Windows/System32/etc/hosts"));
				Verify(locator == "file://server/Windows/System32/etc/hosts", Is.True);
				Verify((string)locator.PlatformPath, Is.EqualTo(platformPath));
			}
		}

		[Test]
		public void RelativeResolve()
		{
			Target.Locator locator = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB#fragment+0";
			Target.Locator file = "schemeA+schemeB://name:password@example.com:80/folder+a/folder+b/file+0.extension?key+a=value+a&keyB=valueB";
			Target.Locator relative = locator.Relative(file);
			Verify(relative == "#fragment+0", Is.True);
			Target.Locator resolve = relative.Resolve(file);
			Verify(resolve == locator, Is.True);
		}
	}
}
