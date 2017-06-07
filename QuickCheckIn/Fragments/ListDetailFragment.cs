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
using Dspeckmann.QuickCheckIn.Adapters;
using TraktApiSharp.Objects.Get.Users.Lists;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Watchlist;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class ListDetailFragment : Android.Support.V4.App.Fragment
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
            if(Arguments.GetBoolean("Watchlist"))
            {
                listTitleTextView.Text = "Watchlist";
                var list = await client.Users.GetWatchlistAsync("me");
                var adapter = new WatchlistItemAdapter(Context, list.ToArray());
                listItemListView.Adapter = adapter;
                listItemListView.ItemClick += (sender, e) =>
                {
                    // TODO: Determine type and start transaction
                };
            }
            else
            {
                var listId = Arguments.GetInt("ListID", -1);
                var listName = Arguments.GetString("ListName");
                if(listId == -1)
                {
                    return; // TODO: Finish? Toast?
                }
                listTitleTextView.Text = listName;
                var list = await client.Users.GetCustomListItemsAsync("me", Arguments.GetInt("ListID").ToString());
                var adapter = new ListItemAdapter(Context, list.ToArray());
                listItemListView.Adapter = adapter;
                listItemListView.ItemClick += (sender, e) =>
                {
                    // TODO: Determine type and start transaction
                };
            }
        }
    }
}