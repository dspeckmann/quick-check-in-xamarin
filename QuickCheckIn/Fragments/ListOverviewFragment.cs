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
using Dspeckmann.QuickCheckIn.Adapters;
using TraktApiSharp.Objects.Get.Users.Lists;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class ListOverviewFragment : Android.Support.V4.App.Fragment
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
            lists.Add(new TraktList() { Name = "Watchlist", ItemCount = watchlist.Items.Count(), Description = "Your Watchlist" }); // TODO: Why does ItemCount not work?
            var customLists = await client.Users.GetCustomListsAsync("me");
            lists.AddRange(customLists);
            var listAdapter = new ListAdapter(Context, lists.ToArray());
            var listListView = View.FindViewById<ListView>(Resource.Id.ListListView);
            listListView.Adapter = listAdapter;
            
            listListView.ItemClick += (sender, e) =>
            {
                var listDetailFragment = new ListDetailFragment();
                var bundle = new Bundle();
                if(listAdapter[e.Position].Ids != null && listAdapter[e.Position].Ids.HasAnyId)
                {
                    var list = listAdapter[e.Position];
                    bundle.PutBoolean("Watchlist", false);
                    bundle.PutInt("ListID", (int)list.Ids.Trakt);
                    bundle.PutString("ListName", list.Name);
                }
                else
                {
                    bundle.PutBoolean("Watchlist", true);
                }
                
                listDetailFragment.Arguments = bundle;

                var transaction = FragmentManager.BeginTransaction();
                transaction.Hide(this);
                transaction.Add(Resource.Id.MainFrameLayout, listDetailFragment);
                transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
        }
    }
}