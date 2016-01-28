//
//  Document.cs
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
using Collection = Kean.Collection;
using Kean.Collection.Extension;
using Uri = Kean.Uri;
namespace Kean.Xml.Dom
{
	public class Document :
		IEquatable<Document>
	{
		public float Version { get; set; }
		public string Encoding { get; set; }
		public bool? Standalone { get; set; }
		Element root;
		public Element Root
		{
			get { return this.root; }
			set
			{
				if (this.root.NotNull())
					this.root.Document = null;
				this.root = value;
				if (this.root.NotNull())
				{
					this.root.Parent = null;
					this.root.Document = this;
				}
			}
		}
		public Document()
		{
			this.Version = 1.0f;
			this.Encoding = "utf-8";
		}
		public Document(Element root) :
			this()
		{
			this.Root = root;
		}
		public bool Save(Uri.Locator resource)
		{
			using (IO.ICharacterWriter writer = IO.CharacterWriter.Create(resource))
				return this.Save(writer);
		}
		public bool Save(IO.IByteOutDevice device)
		{
			using (IO.ICharacterDevice characterDevice = IO.CharacterDevice.Open(IO.ByteDeviceCombiner.Open(device)))
				return this.Save(characterDevice);
		}
		public bool Save(IO.ICharacterDevice device)
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
		public static explicit operator string(Document document)
		{
			return document.ToString();
		}
		public static explicit operator Document(string data)
		{
			return Document.Open(Sax.Parser.Open(new IO.Text.Reader(data)));
		}

		#region Static Open

		public static Document Open(Sax.Parser parser)
		{
			Document result = null;
			if (parser.NotNull())
			{
				result = new Document();
				parser.OnDeclaration += (version, encoding, standalone, region) =>
				{
					result.Version = version;
					result.Encoding = encoding;
					result.Standalone = standalone;
				};
				result.Root = Element.Open(parser);
			}
			return result;
		}
		public static Document OpenResource(System.Reflection.Assembly assembly, Uri.Path resource)
		{
			using (Sax.Parser parser = Sax.Parser.Open(assembly, resource))
				return Document.Open(parser);
		}
		public static Document OpenResource(Uri.Path resource)
		{
			return Document.OpenResource(System.Reflection.Assembly.GetCallingAssembly(), resource);
		}
		public static Document Open(System.IO.Stream stream)
		{
			return Document.Open(Sax.Parser.Open(stream));
		}
		public static Document Open(IO.IByteInDevice device)
		{
			return Document.Open(Sax.Parser.Open(device));
		}
		public static Document Open(Uri.Locator resource)
		{
			using (Sax.Parser parser = Sax.Parser.Open(resource))
				return Document.Open(parser);
		}

		#endregion

		#region Object Overrides

		public override bool Equals(object other)
		{
			return this.Equals(other as Document);
		}
		public override int GetHashCode()
		{
			return this.Version.GetHashCode() ^
				this.Encoding.Hash() ^
				this.Standalone.GetHashCode() ^
				this.Root.GetHashCode();
		}
		public override string ToString()
		{
			IO.Text.Writer result = new IO.Text.Writer();
			this.Save(new Writer.Formatting(result));
			return result;
		}

		#endregion

		#region IEquatable<Document> Members

		public bool Equals(Document other)
		{
			return other.NotNull() &&
				this.Version == other.Version &&
				this.Encoding == other.Encoding &&
				this.Standalone == other.Standalone &&
				this.Root == other.Root;
		}

		#endregion

		#region Equality Operators

		public static bool operator ==(Document left, Document right)
		{
			return left.Same(right) || left.NotNull() && left.Equals(right);
		}
		public static bool operator !=(Document left, Document right)
		{
			return !(left == right);
		}

		#endregion

	}
}
