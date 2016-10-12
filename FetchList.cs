using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace RatingManipulationC
{
	class FetchList
	{
		public List ByName(ClientContext clientContext, Web web)
		{
			clientContext.Load(web.Lists, l => l.IncludeWithDefaultProperties(i => i.Title));
			clientContext.ExecuteQuery();
			List list = web.Lists[0];
			int numberOfTries = 5;
			bool isListPresent = false;
			string listName;
			Console.WriteLine("Site: " + web.Title);
			Console.WriteLine();
			Console.WriteLine("Enter the List Name To Select");
			string skipReadingListName = Console.ReadLine();
			do
			{
				numberOfTries--;
				if (skipReadingListName == "" || numberOfTries == 0)
					return DefaultList(clientContext);
				listName = skipReadingListName;
				//a function to ensure the list is present in current web.
				CustomListCheck(clientContext, listName, out list, out isListPresent);

				if (!isListPresent)
				{
					Console.Clear();
					Console.WriteLine("Site: " + web.Title);
					Console.WriteLine();
					Console.WriteLine("The List name is miss-splled!\nOr\nList with name '" + listName + "' is not Present!.\nPlease re-enter the List Name/Title\nOr\nPress EnterKey to skip and use DefaultList");
					skipReadingListName = Console.ReadLine();
				}
			} while (!isListPresent);
			clientContext.Load(list);
			clientContext.ExecuteQuery();
			return list;
		}

		//function to get default list.
		private List DefaultList(ClientContext clientContext)
		{
			//alert user that default list is being selected.
			Console.WriteLine("List name skiped hence default list 'TestingList' is being selected\nDo you wish to continue Y/N");
			string reselect = Console.ReadLine();
			if (reselect != "Y" && reselect != "y" && reselect != "")
				ByName(clientContext, clientContext.Web);
			List defaultList = clientContext.Web.Lists.GetByTitle("TestingList");
			clientContext.Load(defaultList);
			clientContext.ExecuteQuery();
			return defaultList;
		}

		//function to check whether list with listName specified is present or not and return with the list.
		private void CustomListCheck(ClientContext clientContext, string listName, out List customList, out bool isPresent)
		{
			isPresent = false;
			clientContext.Load(clientContext.Web.Lists);
			clientContext.ExecuteQuery();
			ListCollection lists = clientContext.Web.Lists;
			clientContext.Load(lists);
			clientContext.Load(lists, t => t.IncludeWithDefaultProperties(l => l.Title));
			clientContext.ExecuteQuery();
			customList = lists[0];
			foreach (List list in lists)
				if (list.Title == listName)
				{
					customList = clientContext.Web.Lists.GetByTitle(listName);
					isPresent = true;
					break;
				}
		}
	}
}
