using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class TraktItemListFragment : Android.Support.V4.App.Fragment
    {
        protected void SetUpListView(ListView listView, TraktItem[] source)
        {
            listView.Adapter = new TraktItemAdapter(Context, source);
            listView.ItemClick += (sender, e) =>
            {
                Android.Support.V4.App.Fragment newFragment = null;
                Bundle bundle = new Bundle();

                var adapter = (TraktItemAdapter)listView.Adapter; // TODO: Error handling? Which type of casting?
                var item = adapter[e.Position];

                switch (item.Type)
                {
                    case TraktItemType.Movie:
                        newFragment = new MovieDetailFragment();
                        bundle.PutInt("MovieID", (int)item.Movie.Ids.Trakt);
                        break;
                    case TraktItemType.Show:
                        newFragment = new ShowDetailFragment();
                        bundle.PutInt("ShowID", (int)item.Show.Ids.Trakt);
                        break;
                    case TraktItemType.Season:
                        newFragment = new SeasonDetailFragment();
                        bundle.PutInt("ShowID", (int)item.Show.Ids.Trakt);
                        bundle.PutInt("SeasonNumber", item.Season.Number.Value); // TODO: HasValue?
                        break;
                    case TraktItemType.Episode:
                        newFragment = new EpisodeDetailFragment();
                        bundle.PutInt("ShowID", (int)item.Show.Ids.Trakt);
                        bundle.PutInt("SeasonNumber", item.Episode.SeasonNumber.Value);
                        bundle.PutInt("EpisodeNumber", item.Episode.Number.Value);
                        break;
                    case TraktItemType.List:
                        newFragment = new ListDetailFragment();
                        bundle.PutInt("ListID", (int)item.List.Ids.Trakt);
                        bundle.PutString("ListName", item.List.Name);
                        break;
                }
                
                if (newFragment != null)
                {
                    newFragment.Arguments = bundle;
                    var transaction = FragmentManager.BeginTransaction();
                    transaction.Hide(this);
                    transaction.Add(Resource.Id.MainFrameLayout, newFragment); // TODO: Name?
                    transaction.SetTransition((int)FragmentTransit.FragmentOpen);
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                }
            };
        }
    }
}