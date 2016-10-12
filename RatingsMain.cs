using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net;
using System.Security;
using System.Web;

namespace RatingManipulationC
{
	class RatingsMain
	{
		static void Main(string[] args)
		{
			ClientContext clientContext = new ClientContext("Your site address");
			SecureString password = new SecureString();
			foreach (char c in "Passw0rd".ToCharArray())
				password.AppendChar(c);
			clientContext.Credentials = new NetworkCredential("administrator user name", password, "sharepoint domain name");
			Web web = clientContext.Web;
			clientContext.Load(web);
			clientContext.ExecuteQuery();
			Console.Title = "Ratings";
			FetchList getList = new FetchList();
			List list = getList.ByName(clientContext, web);
			Console.Title = "Ratings:" + list.Title;
			FetchItem getItem = new FetchItem();
			ListItem item = getItem.ByName(clientContext, list);
			Console.Title = "Ratings:" + list.Title + ">" + item.DisplayName;
			SetUser getUser = new SetUser();
			User user = getUser.ByName(clientContext, list, item);
			Console.Title = "Ratings:" + list.Title + ">" + item.DisplayName + " (" + user.Title + ")";
			ChangeRating reCalculate = new ChangeRating();
			reCalculate.NewRatings(clientContext, list, item, user);
			list.Update();
			clientContext.ExecuteQuery();
			Console.ReadLine();
			clientContext.Dispose();
		}
	}
}

