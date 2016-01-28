// 
//  IFactory.cs
//  
//  Author:
//       Simon Mika <smika@hx.se>
//  
//  Copyright (c) 2011-2012 Simon Mika
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

namespace Kean.Serialize.Test
{
	public interface IFactory
	{
		T Create<T>();
		object Create(Reflect.Type type);
		string Name(Reflect.Type type);
		bool Store<T>(T value, Uri.Locator locator);
		void Verify(object value, string message, params object[] arguments);
		void Verify(object actual, NUnit.Framework.Constraints.Constraint constraint, string message, params object[] arguments);
		void VerifyResource(Uri.Locator filename, Uri.Locator resource, string message, params object[] arguments);
	}
}
