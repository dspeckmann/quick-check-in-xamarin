using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TraktApiSharp.Exceptions;
using Dspeckmann.QuickCheckIn;
using TraktApiSharp.Objects.Get.Users.Lists;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Watchlist;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class ListDetailFragment : TraktItemListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "List";
            return inflater.Inflate(Resource.Layout.ListDetail, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var listTitleTextView = View.FindViewById<TextView>(Resource.Id.ListTitleTextView);
            var listItemListView = View.FindViewById<ListView>(Resource.Id.ListItemListView);
            
            var client = TraktApiHelper.Client;
            var listId = Arguments.GetInt("ListID", -1);
            if(listId == -1)
            {
                FragmentManager.PopBackStack();
                return;
            }
            else if(listId == 0)
            {
                listTitleTextView.Text = "Watchlist";
                var list = await client.Users.GetWatchlistAsync("me");
                SetUpListView(listItemListView, list.Select(listItem => new TraktItem(listItem)).ToArray());
            }
            else
            {
                var listName = Arguments.GetString("ListName");
                listTitleTextView.Text = listName;
                var list = await client.Users.GetCustomListItemsAsync("me", Arguments.GetInt("ListID").ToString());
                SetUpListView(listItemListView, list.Select(listItem => new TraktItem(listItem)).ToArray());
            }
        }
    }
}