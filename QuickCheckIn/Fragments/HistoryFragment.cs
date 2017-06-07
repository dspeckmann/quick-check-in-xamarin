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
using TraktApiSharp.Enums;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class HistoryFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // TODO: Funktioniert nur beim ersten Mal.
            // Entweder muss der Titel anders gesetzt werden oder die Daten anders gecached werden (und dann transaction.replace statt .add)
            (Activity as AppCompatActivity).SupportActionBar.Title = "History";
            return inflater.Inflate(Resource.Layout.History, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var client = TraktApiHelper.Client;
            var history = await client.Users.GetWatchedHistoryAsync("me");
            var historyListView = View.FindViewById<ListView>(Resource.Id.HistoryListView);
            var historyAdapter = new HistoryAdapter(Context, history.ToArray());
            historyListView.Adapter = historyAdapter;
            historyListView.ItemClick += (sender, e) =>
            {
                var historyItem = historyAdapter[e.Position];
                Android.Support.V4.App.Fragment newFragment = null;
                if(historyItem.Type == TraktSyncItemType.Movie)
                {
                    newFragment = new MovieDetailFragment();
                    var bundle = new Bundle();
                    bundle.PutInt("MovieId", (int)historyItem.Movie.Ids.Trakt);
                    newFragment.Arguments = bundle;
                }
                else if(historyItem.Type == TraktSyncItemType.Episode)
                {
                    newFragment = new EpisodeDetailFragment();
                    var bundle = new Bundle();
                    bundle.PutInt("ShowID", (int)historyItem.Show.Ids.Trakt);
                    bundle.PutInt("SeasonNumber", historyItem.Episode.SeasonNumber.Value); // HasValue?
                    bundle.PutInt("EpisodeNumber", historyItem.Episode.Number.Value); // HasValue?
                    newFragment.Arguments = bundle;
                }

                if (newFragment != null)
                {
                    var transaction = FragmentManager.BeginTransaction();
                    transaction.Hide(this);
                    transaction.Add(Resource.Id.MainFrameLayout, newFragment);
                    transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                }
            };
        }
    }
}