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
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class EpisodeDetailFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "Episode";
            return inflater.Inflate(Resource.Layout.EpisodeDetail, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var episodeTitleTextView = View.FindViewById<TextView>(Resource.Id.EpisodeTitleTextView);
            var episodeNumberTextView = View.FindViewById<TextView>(Resource.Id.EpisodeNumberTextView);
            int movieId = Arguments.GetInt("ShowID", -1);
            int seasonNumber = Arguments.GetInt("SeasonNumber", -1);
            int episodeNumber = Arguments.GetInt("EpisodeNumber", -1);
            if (movieId == -1 || seasonNumber == 1 || episodeNumber == -1) return;

            var client = TraktApiHelper.Client;
            var episode = await client.Episodes.GetEpisodeAsync(movieId.ToString(), seasonNumber, episodeNumber);
            episodeTitleTextView.Text = episode.Title;
            episodeNumberTextView.Text = $"S{episode.SeasonNumber.Value}E{episode.Number.Value}";

            var checkInButton = View.FindViewById<Button>(Resource.Id.CheckInButton);

            try
            {
                var watching = await client.Users.GetWatchingAsync("me");
                if (watching == null)
                {
                    checkInButton.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            checkInButton.Click += async (sender, e) =>
            {
                try
                {
                    await client.Checkins.CheckIntoEpisodeAsync(episode);
                    checkInButton.Enabled = false;
                }
                catch (TraktCheckinException)
                {
                    Toast.MakeText(Context, "Checking in is not possible while you are already watching something else.", ToastLength.Short).Show(); // TODO: Use string resources
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };
        }
    }
}