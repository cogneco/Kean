//
//  Storage.cs
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
using Kean.Extension;
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
using Kean.Reflect.Extension;

namespace Kean.Serialize
{
	public abstract class Storage :
	Storer
	{
		IRebuilder rebuilder;
		public abstract string ContentType { get; }
		protected Storage(Resolver resolver, IRebuilder rebuilder, params ISerializer[] serializers) :
			base(resolver, serializers)
		{
			this.rebuilder = rebuilder ?? new Rebuilder.Identity();
		}
		protected abstract bool StoreImplementation(Data.Node value, IO.IByteOutDevice device);
		public bool Store(Data.Node value, IO.IByteOutDevice device)
		{
			return device.NotNull() && this.StoreImplementation(this.rebuilder.Store(this, value), device);
		}
		public bool Store<T>(T value, Uri.Locator resource, string name = null)
		{
			return this.Store(value, IO.ByteDevice.Create(resource), name);
		}
		public bool Store<T>(T value, IO.IBlockOutDevice device, string name = null)
		{
			using (var byteDevice = IO.Wrap.BlockByteOutDevice.Wrap(device))
				return this.Store(value, byteDevice, name);
		}
		public bool Store<T>(T value, IO.IByteOutDevice device, string name = null)
		{
			return this.Store(typeof(T), value, device, name);
		}
		public bool Store(System.Type type, object value, IO.IByteOutDevice device, string name = null)
		{
			bool result = device.NotNull();
			if (result)
			{
				Data.Node data = this.Serialize(type, value, device.Resource);
				if (name.NotEmpty() && data.NotNull())
					data.Name = name;
				result = this.Store(data, device);
			}
			return result;
		}
		protected abstract Data.Node LoadImplementation(IO.IByteInDevice device);
		public Data.Node Load(IO.IByteInDevice device)
		{
			Data.Node node;
			return (node = this.LoadImplementation(device)).NotNull() ? this.rebuilder.Load(this, node.UpdateLocators(device.Resource)) : null;
		}
		public T Load<T>(Uri.Locator resource)
		{
			return this.Load<T>(IO.ByteDevice.Open(resource));
		}
		public T Load<T>(IO.IByteInDevice device)
		{
			Data.Node node;
			return device.IsNull() ? default(T) : (T)(this.Resolver[device.Resource] ?? ((node = this.Load(device)).NotNull() ? this.Deserialize(null, node.DefaultType(typeof(T))) : null));
		}
		public bool LoadInto(object result, IO.IByteInDevice device)
		{
			return this.DeserializeContent(this.Load(device).DefaultType(result.GetType()), result);
		}
	}
}