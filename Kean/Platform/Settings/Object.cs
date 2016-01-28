// 
//  Object.cs
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
using Kean;
using Kean.Extension;
using Kean.Reflect.Extension;
using Reflect = Kean.Reflect;
using Collection = Kean.Collection;
using Kean.Collection.Extension;

namespace Kean.Platform.Settings
{
	class Object :
		Member,
        System.Collections.Generic.IEnumerable<Member>
	{
		internal Parser Parser { get; set; }

		protected override char Delimiter { get { return '.'; } }
		object backend;
		#region Sorted vectors of members for help
		internal Collection.IReadOnlyVector<Property> Properties
		{
			get
			{
				return new Collection.Wrap.ReadOnlyVector<Property>(this.Members.Fold((m, l) =>
				{
					if (m is Property)
						l.Add(m as Property);
					return l;
				}, new Collection.List<Property>()));
			}
		}
		internal Collection.IReadOnlyVector<Method> Methods
		{
			get
			{
				return new Collection.Wrap.ReadOnlyVector<Method>(this.Members.Fold((m, l) =>
				{
					if (m is Method)
						l.Add(m as Method);
					return l;
				}, new Collection.List<Method>()));
			}
		}
		internal Collection.IReadOnlyVector<Object> Objects
		{
			get
			{
				return new Collection.Wrap.ReadOnlyVector<Object>(this.Members.Fold((m, l) =>
				{
					if (m is Object)
						l.Add(m as Object);
					return l;
				}, new Collection.List<Object>()));
			}
		}
		#endregion
		Collection.IList<Member> members;
		Collection.IList<Member> Members
		{
			get
			{
				if (this.members.IsNull())
				{
					this.members = new Collection.Sorted.List<Member>();
					if (this.backend.NotNull())
					{
						foreach (Reflect.Member member in this.backend.GetMembers(Reflect.MemberFilter.Instance | Reflect.MemberFilter.Public | Reflect.MemberFilter.Method | Reflect.MemberFilter.Property))
						{
							MemberAttribute[] attributes = member.GetAttributes<MemberAttribute>();
							if (attributes.Length == 1)
							{
								if (attributes[0] is PropertyAttribute)
									this.members.Add(new Property(attributes[0] as PropertyAttribute, member as Reflect.Property, this));
								else if (attributes[0] is MethodAttribute)
									this.members.Add(new Method(attributes[0] as MethodAttribute, member as Reflect.Method, this));
								else if (attributes[0] is ObjectAttribute && member is Reflect.Property && (member as Reflect.Property).Readable && (member as Reflect.Property).Data.NotNull())
									this.members.Add(new Object(attributes[0] as ObjectAttribute, member as Reflect.Property, this));
							}
						}
						if (this.Parser.NotNull())
							this.members.Add(new Object("settings", "", "", this.Parser, this));
						if (this.backend is IReload)
						{
							(this.backend as IReload).Reload += () => this.members = null;
							if (this.backend is IDynamic)
								foreach (Tuple<string, string, string, object> dynamic in (this.backend as IDynamic).GetDynamic())
									this.members.Add(new Object(dynamic.Item1, dynamic.Item2, dynamic.Item3, dynamic.Item4, this));
						}
						if (this.backend is Root)
							(this.backend as Root).Object = this;
					}

				}
				return this.members;
			}
		}
		public Object(object backend) :
			base(null, null, null)
		{
			this.backend = backend;
		}
		public Object(string name, string description, string usage, object backend, Object parent) :
			base(new ObjectAttribute(name, description, usage), null, parent)
		{
			this.backend = backend;
		}
		public Object(ObjectAttribute attribute, Reflect.Property backend, Object parent) :
			base(attribute, backend, parent)
		{
			this.backend = backend.Data;
		}
		internal Reflect.Event GetEvent(string name)
		{
			return this.backend.GetEvent(name);
		}
		public override bool Execute(Parser editor, string[] parameters)
		{
			if (parameters.Length == 0 || parameters.Length == 1 && parameters[0].IsEmpty())
				editor.Current = this;
			else
				editor.Error(this, "Error Parsing:", parameters);
			return true;
		}
		public override string Complete(string[] parameters)
		{
			string incomplete = parameters.Length > 0 ? parameters[0] ?? "" : "";
			Collection.List<string> alternatives = new Collection.List<string>();
			foreach (Member member in this)
				if (member.Name.StartsWith(incomplete))
					alternatives.Add(member.Name + (member is Object ? "." : " "));
			string result = "";
			if (alternatives.Count > 0)
				for (int i = 0; i < alternatives[0].Length && alternatives.All(s => s[i] == alternatives[0][i]); i++)
					result += alternatives[0][i];
			return result;
		}
		public override string Help(string[] parameters)
		{
			string incomplete = parameters.Length > 0 ? parameters[0] ?? "" : "";
			Collection.List<Tuple<string, string>> results = new Collection.List<Tuple<string, string>>();
			foreach (Member member in this)
				if (member.Name.StartsWith(incomplete))
					results.Add(Tuple.Create(member.Name, member.Description));
			return results.Fold((m, r) => r + m.Item1 + "\t" + m.Item2 + "\n", "");
		}
		public override bool RequestType(Parser editor)
		{
			Collection.List<string> results = new Collection.List<string>();
			foreach (Member member in this)
					results.Add(member.Name);
			editor.TypeResponse(this, "object : (" + results.Join(", ") + ")");
			return true;
		}
		#region IEnumerable<Member> Members
		public System.Collections.Generic.IEnumerator<Member> GetEnumerator()
		{
			foreach (Member member in this.Members)
				yield return member;
		}
		#endregion
		#region IEnumerable Members
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (this as System.Collections.IEnumerable).GetEnumerator();
		}
		#endregion
	}
}
