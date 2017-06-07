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
    public class MovieDetailFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "Movie";
            return inflater.Inflate(Resource.Layout.MovieDetail, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var movieTitleTextView = View.FindViewById<TextView>(Resource.Id.MovieTitleTextView);
            var movieYearTextView = View.FindViewById<TextView>(Resource.Id.MovieYearTextView);
            int movieId = Arguments.GetInt("MovieID", -1);
            if (movieId == -1) return;

            movieTitleTextView.Text = "Movie #" + movieId.ToString();

            var client = TraktApiHelper.Client;
            var movie = await client.Movies.GetMovieAsync(movieId.ToString());
            movieTitleTextView.Text = movie.Title;
            movieYearTextView.Text = movie.Year.ToString();

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
                    await client.Checkins.CheckIntoMovieAsync(movie);
                    checkInButton.Enabled = false;
                }
                catch (TraktCheckinException)
                {
                    Toast.MakeText(Context, "Checking in is not possible while you are already watching something else.", ToastLength.Short).Show();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };
        }
    }
}