using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace RatingManipulationC
{
	class FetchItem
	{
		string itemName;
		int numberOfTries = 5;
		public ListItem ByName(ClientContext clientContext, List list)
		{
			Console.Clear();
			Console.WriteLine("Site: " + clientContext.Web.Title);
			Console.WriteLine("List: " + list.Title);
			Console.WriteLine();
			Console.WriteLine("Enter The Name of Item which you need to Rate");
			string skipReadingItemName = Console.ReadLine();
			ListItem item = list.GetItemById(1);
			bool isItemPresent = false;
			CamlQuery query = CamlQuery.CreateAllItemsQuery();
			ListItemCollection items = list.GetItems(query);
			clientContext.Load(items, i => i.IncludeWithDefaultProperties(t => t.DisplayName));
			clientContext.ExecuteQuery();
			do
			{
				numberOfTries--;
				if (skipReadingItemName == "" || numberOfTries == 0)
					return DefaultItem(clientContext, list);
				itemName = skipReadingItemName;
				foreach (ListItem itemCheck in items)
					if (itemCheck.DisplayName == itemName)
					{
						item = itemCheck;
						isItemPresent = true;
						break;
					}
				if (!isItemPresent)
				{
					Console.Clear();
					Console.WriteLine("Site: " + clientContext.Web.Title);
					Console.WriteLine("List: " + list.Title);
					Console.WriteLine();
					Console.WriteLine("The Item name Provided is either miss-spelled or does not exists\nPlease re-enter the Item Name/DisplayName");
					skipReadingItemName = Console.ReadLine();

				}
			} while (!isItemPresent);
			clientContext.Load(item);
			clientContext.ExecuteQuery();
			return item;
		}

		//function to get default item.
		private ListItem DefaultItem(ClientContext clientcontext, List list)
		{
			ListItemCollection items = list.GetItems(CamlQuery.CreateAllItemsQuery());
			clientcontext.Load(items, i => i.IncludeWithDefaultProperties(t => t.DisplayName));
			clientcontext.ExecuteQuery();
			ListItem defaultItem = items[0];
			clientcontext.Load(defaultItem);
			clientcontext.ExecuteQuery();
			//alert the default item is being selected.
			Console.WriteLine("Item name skiped hence Default item i.e. first item in list\n'" + defaultItem.DisplayName + "' is being selected\nDo you wish to continue Y/N");
			string reselect = Console.ReadLine();
			if (reselect != "y" && reselect != "Y" && reselect != "")
				ByName(clientcontext, list);
			return defaultItem;
		}
	}
}
