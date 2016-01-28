// 
//  Exception.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2010 Simon Mika
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
using Target = Kean.Error;
using NUnit.Framework;

namespace Kean.Error.Test
{
	[TestFixture]
	public class Error :
		Kean.Test.Fixture<Error>
	{
		public Error ()
		{
		}
		[Test]
		public void MetaDataImmediateThrow()
		{
			DateTime before = DateTime.Now;
			try
			{
				throw new Exception.Test(Target.Level.Critical, "Title", "Message {0} {1}", "argument0", "argument1");
			}
			catch (Exception.Abstract e)
			{
				Verify((e as Target.IError).Time, Is.LessThanOrEqualTo(DateTime.Now));
				Verify((e as Target.IError).Time, Is.GreaterThanOrEqualTo(before));
				Verify((e as Target.IError).Level, Is.EqualTo(Target.Level.Critical));
				Verify((e as Target.IError).Title, Is.EqualTo("Title"));
				Verify((e as Target.IError).Message, Is.EqualTo("Message argument0 argument1"));
				Verify((e as Target.IError).AssemblyName, Is.EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
				Verify((e as Target.IError).AssemblyVersion, Is.EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
				Verify((e as Target.IError).Type, Is.EqualTo("Kean.Error.Test.Error"));
				Verify((e as Target.IError).Method, Is.EqualTo("MetaDataImmediateThrow"));
			}
		}
		[Test]
		public void MetaDataFilteredThrow()
		{
			DateTime before = DateTime.Now;
			try
			{
				new Exception.Test(Target.Level.Critical, "Title", "Message {0} {1}", "argument0", "argument1").Throw();
			}
			catch (Exception.Abstract e)
			{
				Verify((e as Target.IError).Time, Is.LessThanOrEqualTo(DateTime.Now));
				Verify((e as Target.IError).Time, Is.GreaterThanOrEqualTo(before));
				Verify((e as Target.IError).Level, Is.EqualTo(Target.Level.Critical));
				Verify((e as Target.IError).Title, Is.EqualTo("Title"));
				Verify((e as Target.IError).Message, Is.EqualTo("Message argument0 argument1"));
				Verify((e as Target.IError).AssemblyName, Is.EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
				Verify((e as Target.IError).AssemblyVersion, Is.EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
				Verify((e as Target.IError).Type, Is.EqualTo("Kean.Error.Test.Error"));
				Verify((e as Target.IError).Method, Is.EqualTo("MetaDataFilteredThrow"));
			}
		}
		[Test]
		public void MetaDataIndirectThrow()
		{
			DateTime before = DateTime.Now;
			try
			{
				Exception.Test.Check(Target.Level.Critical, "Title", "Message {0} {1}", "argument0", "argument1");
			}
			catch (Exception.Abstract e)
			{
				Verify((e as Target.IError).Time, Is.LessThanOrEqualTo(DateTime.Now));
				Verify((e as Target.IError).Time, Is.GreaterThanOrEqualTo(before));
				Verify((e as Target.IError).Level, Is.EqualTo(Target.Level.Critical));
				Verify((e as Target.IError).Title, Is.EqualTo("Title"));
				Verify((e as Target.IError).Message, Is.EqualTo("Message argument0 argument1"));
				Verify((e as Target.IError).AssemblyName, Is.EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
				Verify((e as Target.IError).AssemblyVersion, Is.EqualTo(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
				Verify((e as Target.IError).Type, Is.EqualTo("Kean.Error.Test.Error"));
				Verify((e as Target.IError).Method, Is.EqualTo("MetaDataIndirectThrow"));
			}
		}
		protected override void Run()
		{
			this.Run(
				this.MetaDataImmediateThrow,
				this.MetaDataFilteredThrow,
				this.MetaDataIndirectThrow
			);
		}
	}
}

