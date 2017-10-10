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
using TraktApiSharp.Objects.Get.Shows.Seasons;
using Dspeckmann.QuickCheckIn;
using TraktApiSharp.Objects.Get.Users.Lists;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class ListOverviewFragment : TraktItemListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "Lists";
            return inflater.Inflate(Resource.Layout.ListOverview, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            
            var client = TraktApiHelper.Client;
            var watchlist = await client.Users.GetWatchlistAsync("me");
            var lists = new List<TraktList>();
            lists.Add(new TraktList() { Name = "Watchlist", ItemCount = watchlist.Items.Count(), Description = "Your Watchlist", Ids = new TraktListIds() { Trakt = 0 } }); // TODO: Improve?
            var customLists = await client.Users.GetCustomListsAsync("me");
            lists.AddRange(customLists);
            var listListView = View.FindViewById<ListView>(Resource.Id.ListListView);
            SetUpListView(listListView, lists.Select(list => new TraktItem(list)).ToArray());
        }
    }
}