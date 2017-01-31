//
//  Node.cs
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
using Uri = Kean.Uri;

namespace Kean.Json.Dom
{
	public abstract class Item :
		IEquatable<Item>
	{
		public Uri.Region Region { get; internal set; }
		public Collection Parent { get; internal set; }
		protected Item()
		{
		}
		protected Item(Uri.Region region)
		{
			this.Region = region;
		}
		#region Save
		public bool Save(Uri.Locator resource)
		{
			using (IO.IByteDevice device = IO.ByteDevice.Create(resource))
				return this.Save(device);
		}
		public bool Save(IO.IBlockOutDevice device)
		{
			using (IO.IByteOutDevice byteDevice = IO.Wrap.BlockByteOutDevice.Wrap(device))
			{
				byteDevice.AutoFlush = false;
				return this.Save(byteDevice);
			}
		}
		public bool Save(IO.IByteOutDevice device)
		{
			using (IO.ICharacterOutDevice characterDevice = IO.CharacterDevice.Open(IO.ByteDeviceCombiner.Open(device)))
				return this.Save(characterDevice);
		}
		public bool Save(IO.ICharacterOutDevice device)
		{
			using (IO.ICharacterWriter writer = IO.CharacterWriter.Open(device))
				return this.Save(writer);
		}
		public bool Save(IO.ICharacterWriter writer)
		{
			return writer.NotNull() && this.Save(new Writer.Formatting(writer));
		}
		public bool Save(IWriter writer)
		{
			return writer.Write(this);
		}
		#endregion
		#region Static Open
		public static Item Open(Sax.Parser parser)
		{
			Collection result = null;
			Collection current = null;
			parser.OnObjectStart += (label, labelRegion, region) =>
			{ 
				Object next = new Object(region);
				if (result.IsNull())
					result = next;
				else
					current.Add(label, labelRegion, next);
				current = next;
			};
			parser.OnObjectEnd += region =>
			{
				if (current.Region.NotNull() && region.NotNull())
					current.Region = new Uri.Region(current.Region.Resource, current.Region.Start, region.End);
				current = current.Parent;
			};
			parser.OnArrayStart += (label, labelRegion, region) =>
			{ 
				Array next = new Array(region);
				if (result.IsNull())
					result = next;
				else
					current.Add(label, labelRegion, next);
				current = next;
			};
			parser.OnArrayEnd += region =>
			{
				if (current.Region.NotNull() && region.NotNull())
					current.Region = new Uri.Region(current.Region.Resource, current.Region.Start, region.End);
				current = current.Parent;
			};
			parser.OnNull += (label, labelRegion, region) => current.Add(label, labelRegion, new Null(region));
			parser.OnBoolean += (label, labelRegion, value, region) => current.Add(label, labelRegion, new Boolean(value, region));
			parser.OnNumber += (label, labelRegion, value, region) => current.Add(label, labelRegion, new Number(value, region));
			parser.OnString += (label, labelRegion, value, region) => current.Add(label, labelRegion, new String(value, region));

			return parser.Parse() ? result : null;
		}
		public static Item OpenResource(System.Reflection.Assembly assembly, Uri.Path path)
		{
			return Item.Open(Sax.Parser.Open(assembly, path));
		}
		public static Item OpenResource(Uri.Path path)
		{
			return Item.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), path);
		}
		public static Item Open(System.IO.Stream stream)
		{
			return Item.Open(Sax.Parser.Open(stream));
		}
		public static Item Open(Uri.Locator resource)
		{
			return Item.Open(Sax.Parser.Open(resource));
		}
		public static Item Open(IO.IByteInDevice device)
		{
			return Item.Open(Sax.Parser.Open(device));
		}
		public static Item Open(IO.ICharacterInDevice device)
		{
			return Item.Open(Sax.Parser.Open(device));
		}
		#endregion
		#region Object Overrides
		public override string ToString()
		{
			IO.Text.Writer result = new IO.Text.Writer();
			return this.Save(new Writer.Formatting(result) { Format = false }) ? result : null;
		}
		#endregion
		#region Equals
		public override bool Equals(object other)
		{
			return base.Equals(other as Item);
		}
		public abstract bool Equals(Item other);
		public static bool operator ==(Item left, Item right)
		{
			return left.NotNull() ? left.Equals(right) : right.IsNull();
		}
		public static bool operator !=(Item left, Item right)
		{
			return left.NotNull() ? !left.Equals(right) : right.NotNull();
		}
		#endregion
		#region Static Create
		public static Item Create(object value)
		{
			Item result = null;
			if (value.IsNull())
				result = new Null();
			else if (value is string)
				result = value as string;
			else if (value is bool)
				result = (bool)value;
			else if (value is decimal)
				result = (decimal)value;
			else if (value is int)
				result = (int)value;
			else if (value is long)
				result = (long)value;
			else if (value is float)
				result = (float)value;
			else if (value is double)
				result = (double)value;
			return result;
		}
		#endregion
		#region Casts
		public static explicit operator string(Item node)
		{
			return node.ToString();
		}
		public static implicit operator Item(string value)
		{
			return value.IsNull() ? null : (String)value;
		}
		public static implicit operator Item(bool value)
		{
			return (Boolean)value;
		}
		public static implicit operator Item(decimal value)
		{
			return (Number)value;
		}
		public static implicit operator Item(int value)
		{
			return (Number)value;
		}
		public static implicit operator Item(long value)
		{
			return (Number)value;
		}
		public static implicit operator Item(float value)
		{
			return (Number)value;
		}
		public static implicit operator Item(double value)
		{
			return (Number)value;
		}
		public static implicit operator Item(Item[] items)
		{
			return (Array)items;
		}
		#endregion
	}
}

