// 
//  Comparer.cs
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
using NUnit.Framework;
using Kean;
using Kean.Extension;
using Kean.Collection.Extension;
using Kean.Reflect.Extension;

namespace Kean.Serialize.Test
{
	public abstract class Verifier
	{
		protected abstract string Extension { get; }

		protected abstract Uri.Locator CorrectBase { get; }

		protected abstract Uri.Locator CreatedBase { get; }

		internal protected IFactory Factory { get; set; }

		public Storage Storage { get; private set; }

		protected Verifier(Storage storage)
		{
			this.Storage = storage;
		}

		internal void VerifyStore (Reflect.Type type)
		{
			Uri.Locator created = this.CreatedName(type);
			Uri.Locator resource = this.CorrectName(type);
			this.Factory.Store(this.Factory.Create(type), created);
			this.Factory.VerifyResource(created, resource, "Serializing test \"{0}\" failed.", this.Factory.Name(type));
		}

		internal void VerifyLoad (Reflect.Type type)
		{
			Uri.Locator resource = this.CorrectName(type);
			this.Factory.Verify(this.Storage.Load<object>(resource), "Deserialization text \"{0}\" failed.", this.Factory.Name(type));
		}

		protected virtual Uri.Locator CorrectName (Reflect.Type type)
		{
			return this.CorrectBase + (this.Factory.Name(type) + "." + this.Extension);
		}

		protected Uri.Locator CreatedName (Reflect.Type type)
		{
			return this.CreatedBase + (this.Factory.Name(type) + "." + this.Extension);
		}

		string ReferencePath (string filename)
		{
			return "" + filename;
		}

		string ReferencePath (Reflect.Type type)
		{
			return this.ReferencePath(this.CreatedName(type));
		}
	}
}

