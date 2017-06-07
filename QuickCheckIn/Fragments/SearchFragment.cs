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
using TraktApiSharp.Enums;
using Dspeckmann.QuickCheckIn;
using Dspeckmann.QuickCheckIn.Adapters;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class SearchFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            (Activity as AppCompatActivity).SupportActionBar.Title = "Search";
            return inflater.Inflate(Resource.Layout.Search, container, false);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var client = TraktApiHelper.Client;
            var searchEditText = View.FindViewById<EditText>(Resource.Id.SearchEditText);
            var searchResultListView = View.FindViewById<ListView>(Resource.Id.SearchResultListView);
            searchEditText.TextChanged += async (sender, e) =>
            {
                if (searchEditText.Text.Length > 1)
                {
                    var apiResult = await client.Search.GetTextQueryResultsAsync(TraktSearchResultType.Movie | TraktSearchResultType.Show, searchEditText.Text);
                    searchResultListView.Adapter = new SearchResultAdapter(Context, apiResult.Items.ToArray());
                }
                else
                {
                    searchResultListView.Adapter = null;
                }
            };
            searchResultListView.ItemClick += (sender, e) =>
            {
                var adapter = searchResultListView.Adapter as SearchResultAdapter;
                if (adapter == null) return;

                var searchResult = adapter[e.Position];

                if (searchResult.Type == TraktSearchResultType.Movie)
                {
                    var movieDetailFragment = new MovieDetailFragment();
                    var bundle = new Bundle();
                    bundle.PutInt("MovieID", (int)searchResult.Movie.Ids.Trakt);
                    movieDetailFragment.Arguments = bundle;

                    var transaction = FragmentManager.BeginTransaction();
                    transaction.Hide(this);
                    transaction.Add(Resource.Id.MainFrameLayout, movieDetailFragment); // Name
                    transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                }
                else if (searchResult.Type == TraktSearchResultType.Show)
                {
                    var showDetailFragment = new ShowDetailFragment();
                    var bundle = new Bundle();
                    bundle.PutInt("ShowID", (int)searchResult.Show.Ids.Trakt);
                    showDetailFragment.Arguments = bundle;

                    var transaction = FragmentManager.BeginTransaction();
                    transaction.Hide(this);
                    transaction.Add(Resource.Id.MainFrameLayout, showDetailFragment); // Name
                    transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                }
            };
        }
    }
}