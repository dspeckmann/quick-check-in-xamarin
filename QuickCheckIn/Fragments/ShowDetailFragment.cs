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
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class ShowDetailFragment : TraktItemListFragment
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
            SetUpListView(seasonListView, seasons.Select(season => new TraktItem(show, season)).ToArray());
        }
    }
}