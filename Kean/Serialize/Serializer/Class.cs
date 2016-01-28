//
//  Class.cs
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
using Kean;
using Kean.Extension;
using Kean.Reflect.Extension;
using Kean.Serialize.Extension;

namespace Kean.Serialize.Serializer
{
	public class Class :
		ISerializer
	{
		public Class()
		{
		}
		#region ISerializer Members
		public ISerializer Find(Reflect.Type type, bool deserialize)
		{
			return type.Category == Reflect.TypeCategory.Class ? this : null;
		}
		public Data.Node Serialize(IStorage storage, Reflect.Type type, object data, Uri.Locator resource)
		{
			Data.Node result;
			Uri.Locator l = storage.Resolver.Update(data, resource);
			if (l.NotNull())
				result = new Data.Link(l);
			else
			{
				result = new Data.Branch(data, type);
				foreach (Reflect.Property property in data.GetProperties())
				{
					ParameterAttribute[] attributes = property.GetAttributes<ParameterAttribute>();
					if (attributes.Length == 1 && property.Data.NotNull())
					{
						string name = attributes[0].Name ?? property.Name.Convert(Casing.Pascal, storage.Casing);
						if (resource.NotNull())
						{
							l = resource.Copy();
							l.Fragment = (l.Fragment.NotEmpty() ? l.Fragment + "/" : "") + name;
						}
						(result as Data.Branch).Nodes.Add(storage.Serialize(property.Type, property.Data, l).UpdateName(name).UpdateAttribute(attributes[0]).UpdateLocator(resource));
					}
				}
			}
			return result;
		}
		public object Deserialize(IStorage storage, Data.Node data, object result)
		{
			if (result.IsNull())
				try
				{
					result = data.Type.Create();
				}
				catch (System.MissingMethodException e)
				{
					if (data.Type == data.OriginalType)
						new Exception.CreateAbstract(e, data.Type, data.Region).Throw();
					else
						new Exception.UnknownType(e, data.OriginalType, data.Region).Throw();
				}
			if (result.NotNull())
			{
				Reflect.Property[] properties = result.GetProperties();
				if (data is Data.Branch)
					foreach (Data.Node node in (data as Data.Branch).Nodes)
					{
						string name = node.Name.Convert(storage.Casing, Casing.Pascal);
						Reflect.Property property = properties.Find(p =>
						{
							Kean.Serialize.ParameterAttribute[] attributes;
							return (attributes = p.GetAttributes<Kean.Serialize.ParameterAttribute>()).Length > 0 && attributes[0].Name.NotEmpty() ? attributes[0].Name == node.Name : p.Name == name;
						});
						if (property.IsNull())
							new Exception.PropertyMissing(data.Type, name, node.Region).Throw();
						else if (property.Writable)
							storage.Deserialize(node, property.Type, d => property.Data = d);
						else if (property.Readable && (property.Type.Category == Reflect.TypeCategory.Class || property.Type.Category == Reflect.TypeCategory.Array || property.Type.Category == Reflect.TypeCategory.Interface))
							storage.DeserializeContent(node.DefaultType(property.Type), property.Data);
						else
							new Exception.PropertyNotWriteable(data.Type, name, node.Region).Throw();
					}
			}
			return result;
		}
		#endregion
	}
}