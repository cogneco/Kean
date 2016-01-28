using System;
using Kean.DB.Extension;
using Generic = System.Collections.Generic;
namespace Kean.DB.Sql.Test
{
	public class Program
	{
		public static void Run(string[] args)
		{
			Console.WriteLine("Hello World!");
			DB.Sql.Database.Register("mysql", c => new global::MySql.Data.MySqlClient.MySqlConnection(c));
			using (Database database = Database.Open("mysql://kean:password@localhost/keanTest/"))
			{
				//database.Create<Item>();
				DB.ITable<Item> table = database.Get<Item>();
				table.Create(new Item() { Name = "Test Name A", Description = "This is a description of A." });
				table.Create(new Item() { Name = "Test Name B", Description = "This is a description of B." });
				table.Create(new Item() { Name = "Test Name C", Description = "This is a description of C." });
				table.Create(new Item() { Name = "Test Name D", Description = "This is a description of D." });
				table.Create(new Item() { Name = "Test Name E", Description = "This is a description of E." });
				table.Create(new Item() { Name = "Test Name F", Description = "This is a description of F." });

				Item item0 = table.Read(1);
				Console.WriteLine(item0);
				item0.Description += " With some more.";
				Generic.IEnumerable<Item> items = table.Filter(item => item.Key > 2).Filter(item => item.Key > 5).Sort(item => item.Name, false).Limit(8, 2).Read();
				foreach (Item item in items)
					Console.WriteLine(item);
				Console.ReadLine();
			}
		}
	}
}
