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
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class ShowDetailFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "Show";
            return inflater.Inflate(Resource.Layout.ShowDetail, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var showTitleTextView = View.FindViewById<TextView>(Resource.Id.ShowTitleTextView);
            var showYearTextView = View.FindViewById<TextView>(Resource.Id.ShowYearTextView);
            int showId = Arguments.GetInt("ShowID", -1);
            if (showId == -1) return; // Finish? Show Toast? // Don't finish but rather pop back stack or whatever

            showTitleTextView.Text = "Show #" + showId.ToString();

            var client = TraktApiHelper.Client;
            var show = await client.Shows.GetShowAsync(showId.ToString());
            showTitleTextView.Text = show.Title;
            showYearTextView.Text = show.Year.ToString();

            var seasons = await client.Seasons.GetAllSeasonsAsync(showId.ToString());
            var seasonListView = View.FindViewById<ListView>(Resource.Id.SeasonListView);
            var seasonAdapter = new SeasonAdapter(Context, seasons.ToArray());
            seasonListView.Adapter = seasonAdapter;
            seasonListView.ItemClick += (sender, e) =>
            {
                var seasonDetailFragment = new SeasonDetailFragment();
                var bundle = new Bundle();
                bundle.PutInt("ShowID", showId);
                var adapter = (SeasonAdapter)seasonListView.Adapter;
                bundle.PutInt("SeasonNumber", seasonAdapter[e.Position].Number.Value); // HasValue?
                seasonDetailFragment.Arguments = bundle;

                var transaction = FragmentManager.BeginTransaction();
                transaction.Hide(this);
                transaction.Add(Resource.Id.MainFrameLayout, seasonDetailFragment);
                transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
        }
    }
}