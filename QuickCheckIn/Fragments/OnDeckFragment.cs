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
using Android.Support.V7.App;
using TraktApiSharp.Requests.Params;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class OnDeckFragment : TraktItemListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "On Deck To Watch";
            return inflater.Inflate(Resource.Layout.OnDeck, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            
            var client = TraktApiHelper.Client;
            var watchedShows = await client.Users.GetWatchedShowsAsync("me");
            watchedShows = watchedShows.OrderByDescending(show => show.LastWatchedAt);
            var episodesOnDeck = new List<TraktItem>();
            foreach (var show in watchedShows)
            {
                var watchedProgress = await client.Shows.GetShowWatchedProgressAsync(show.Show.Ids.Trakt.ToString());
                if(watchedProgress.NextEpisode != null) // TODO: Or check for watchedProgress.Completed?
                {
                    episodesOnDeck.Add(new TraktItem(show.Show, watchedProgress.NextEpisode));
                }
            }

            var onDeckListView = View.FindViewById<ListView>(Resource.Id.OnDeckListView);
            SetUpListView(onDeckListView, episodesOnDeck.ToArray());
        }
    }
}