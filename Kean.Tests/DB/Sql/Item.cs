using System;

namespace Kean.DB.Sql.Test
{
	public class Item :
	DB.Item<Item>
	{
		[DB.Index]
		public string Name { get; set; }
		[DB.Data]
		public string Description { get; set; }
		public Item()
		{
		}
		public override string ToString()
		{
			return string.Format("[Item: Key={0}, Name={1}, Description={2}]", this.Key, this.Name, this.Description);
		}
	}
}

