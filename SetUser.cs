using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace RatingManipulationC
{
	class SetUser
	{
		public User ByName(ClientContext clientContext, List list, ListItem item)
		{
			clientContext.Load(clientContext.Web.SiteUsers, i => i.IncludeWithDefaultProperties(u => u.Title, u => u.Id));
			clientContext.ExecuteQuery();
			User user = clientContext.Web.SiteUsers.GetById(1);
			bool isUserPresent = false;
			int numberOfTries = 5;
			string userName;
			Console.Clear();
			Console.WriteLine("Site: " + clientContext.Web.Title);
			Console.WriteLine("List: " + list.Title);
			Console.WriteLine("Item: " + item.DisplayName);
			Console.WriteLine();
			Console.WriteLine("Enter the User name");
			string skipReadingUserName = Console.ReadLine();
			do
			{
				if (skipReadingUserName == "" || numberOfTries == 0)
					return DefaultUser(clientContext, list, item);
				userName = skipReadingUserName;
				numberOfTries--;
				foreach (User userSearch in clientContext.Web.SiteUsers)
					if (userSearch.Title == userName)
					{	//if site-user with exact name is present.
						user = userSearch;
						isUserPresent = true;
						break;
					}
					else if (userSearch.Title.Split(' ')[0] == userName)
					{	//if site-user with FirstName is equal to the specified name is present.
						user = userSearch;
						isUserPresent = true;
					}
				if (!isUserPresent)
				{
					Console.Clear();
					Console.WriteLine("Site: " + clientContext.Web.Title);
					Console.WriteLine("List: " + list.Title);
					Console.WriteLine("Item: " + item.DisplayName);
					Console.WriteLine();
					Console.WriteLine("The User name is miss-splled \nOr\nUser does not exists.\nPlease re-enter the User Name/Title");
					string skipReadingItemName = Console.ReadLine();
				}
			} while (!isUserPresent);
			clientContext.Load(user);
			clientContext.ExecuteQuery();
			return user;
		}

		//function to get default user.
		private User DefaultUser(ClientContext clientContext, List list, ListItem item)
		{
			Console.WriteLine("User selection skiped hence default user user1 is being selected\nDo you wish to continue Y/N");
			string reselect = Console.ReadLine();
			if (reselect != "Y" && reselect != "y" && reselect != "")
				return ByName(clientContext, list, item);
			else
			{
				string loginName = "i:0#.w|sharepoint\\user1";
				User defaultUser = clientContext.Web.EnsureUser(loginName);
				clientContext.Load(defaultUser);
				clientContext.ExecuteQuery();
				return defaultUser;
			}
		}
	}
}
