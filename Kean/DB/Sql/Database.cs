//
//  Database.cs
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
//  You should have received data copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Kean;
using Kean.Extension;
using Collection = Kean.Collection;
using Uri = Kean.Uri;
using Serialize = Kean.Serialize;
using Data = System.Data;
using Reflect = Kean.Reflect;

namespace Kean.DB.Sql
{
	public class Database :
		DB.Database
	{
		Database(Uri.Locator locator, Data.IDbConnection connection) :
            this(locator, connection, null, null, null)
		{
		}
		Data.IDbConnection connection;
		Database(Uri.Locator locator, Data.IDbConnection connection, Serialize.Resolver resolver, Serialize.IRebuilder rebuilder, params Serialize.ISerializer[] serializers) :
 			base(locator, resolver, rebuilder, serializers)
		{
			this.connection = connection;
		}
		public override bool Close()
		{
			bool result;
			if (result = this.connection.NotNull())
			{
				this.connection.Close();
				this.connection = null;
			}
			return result;
		}

		#region implemented abstract members of Database
		protected override DB.Table<T> New<T>(string name)
		{
			return new Table<T>(this.connection, name);
		}
		#endregion

		#region Static Open
		public static Database Open(Uri.Locator locator)
		{
			string connectionString = null;
			connectionString =
				"Server=" + locator.Authority.Endpoint +
				";Database=" + locator.Path[0] +
				";User ID=" + locator.Authority.User.Name +
				";Password=" + locator.Authority.User.Password;
			Console.WriteLine(connectionString);
			Data.IDbConnection connection = null;
			switch (locator.Scheme)
			{
				case "mysql":
					connection = new global::MySql.Data.MySqlClient.MySqlConnection(connectionString);
					break;
			}
			connection.Open();
			Console.WriteLine(connection.State);
			return connection.NotNull() ? new Database(locator, connection) : null;
		}
		#endregion

	}
}
