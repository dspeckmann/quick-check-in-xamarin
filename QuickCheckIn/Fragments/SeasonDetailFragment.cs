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
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class SeasonDetailFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "Season";
            return inflater.Inflate(Resource.Layout.SeasonDetail, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var seasonTitleTextView = View.FindViewById<TextView>(Resource.Id.SeasonTitleTextView);
            int showId = Arguments.GetInt("ShowID", -1);
            int seasonNumber = Arguments.GetInt("SeasonNumber", -1);
            if (showId == -1 || seasonNumber == -1) return; // Finish? Show Toast?

            seasonTitleTextView.Text = $"Show #{showId} Season #{seasonNumber}";

            var client = TraktApiHelper.Client;
            var season = await client.Seasons.GetSeasonAsync(showId.ToString(), seasonNumber);

            var episodeListView = View.FindViewById<ListView>(Resource.Id.EpisodeListView);
            var episodeAdapter = new EpisodeAdapter(Context, season.ToArray());
            episodeListView.Adapter = episodeAdapter;
            episodeListView.ItemClick += (sender, e) =>
            {
                var selectedEpisode = episodeAdapter[e.Position];
                var episodeDetailFragment = new EpisodeDetailFragment();
                var bundle = new Bundle();
                bundle.PutInt("ShowID", showId);
                bundle.PutInt("SeasonNumber", selectedEpisode.SeasonNumber.Value);
                bundle.PutInt("EpisodeNumber", selectedEpisode.Number.Value);
                episodeDetailFragment.Arguments = bundle;

                var transaction = FragmentManager.BeginTransaction();
                transaction.Hide(this);
                transaction.Add(Resource.Id.MainFrameLayout, episodeDetailFragment);
                transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };
        }
    }
}