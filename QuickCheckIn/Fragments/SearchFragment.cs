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
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class SearchFragment : TraktItemListFragment
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
            SetUpListView(searchResultListView, new TraktItem[0]); // We cannot call this everytime the user enters a character, because it would add more and more event handlers
            searchEditText.TextChanged += async (sender, e) =>
            {
                if (searchEditText.Text.Length > 1)
                {
                    // TODO: Allow filtering type? Allow lists? or extra button "search lists" on lists overview?
                    var apiResult = await client.Search.GetTextQueryResultsAsync(TraktSearchResultType.Movie | TraktSearchResultType.Show, searchEditText.Text);
                    searchResultListView.Adapter = new TraktItemAdapter(Context, apiResult.Items.Select(searchResult => new TraktItem(searchResult)).ToArray());
                }
                else
                {
                    searchResultListView.Adapter = null;
                }
            };
        }
    }
}