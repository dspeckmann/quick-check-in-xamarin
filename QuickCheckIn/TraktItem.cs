using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraktApiSharp.Objects.Get.Movies;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Users.Lists;
using TraktApiSharp.Objects.Get.Watchlist;
using TraktApiSharp.Objects.Get.History;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Enums;
using Dspeckmann.QuickCheckIn.Fragments;
using Android.OS;

namespace Dspeckmann.QuickCheckIn
{
    // TODO: Person as Type?
    public class TraktItem
    {
        public TraktItemType Type { get; private set; }
        public TraktMovie Movie { get; private set; }
        public TraktShow Show { get; private set; }
        public TraktSeason Season { get; private set; }
        public TraktEpisode Episode { get; private set; }
        public TraktList List { get; private set; }

        public TraktItem(TraktMovie movie)
        {
            Type = TraktItemType.Movie;
            Movie = movie;
        }

        public TraktItem(TraktShow show)
        {
            Type = TraktItemType.Show;
            Show = show;
        }

        public TraktItem(TraktShow show, TraktSeason season)
        {
            Type = TraktItemType.Season;
            Show = show;
            Season = season;
        }

        public TraktItem(TraktShow show, TraktEpisode episode)
        {
            Type = TraktItemType.Episode;
            Show = show;
            Episode = episode;
        }

        public TraktItem(TraktList list)
        {
            Type = TraktItemType.List; // TODO: Type Watchlist?
            List = list;
        }

        public TraktItem(TraktListItem listItem)
        {
            // TODO: No switch possible because of implementation?
            if(listItem.Type == TraktListItemType.Movie)
            {
                // TODO: Redundant?
                Type = TraktItemType.Movie;
                Movie = listItem.Movie;
            }
            else if (listItem.Type == TraktListItemType.Show)
            {
                Type = TraktItemType.Show;
                Show = listItem.Show;
            }
            else if (listItem.Type == TraktListItemType.Season)
            {
                Type = TraktItemType.Season;
                Show = listItem.Show;
                Season = listItem.Season;
            }
            else if(listItem.Type == TraktListItemType.Episode)
            {
                Type = TraktItemType.Episode;
                Show = listItem.Show;
                Episode = listItem.Episode;
            }

            // TODO: Default operation?
        }

        public TraktItem(TraktWatchlistItem watchlistItem)
        {
            if (watchlistItem.Type == TraktSyncItemType.Movie)
            {
                Type = TraktItemType.Movie;
                Movie = watchlistItem.Movie;
            }
            else if (watchlistItem.Type == TraktSyncItemType.Show)
            {
                Type = TraktItemType.Show;
                Show = watchlistItem.Show;
            }
            else if (watchlistItem.Type == TraktSyncItemType.Season)
            {
                Type = TraktItemType.Season;
                Show = watchlistItem.Show;
                Season = watchlistItem.Season;
            }
            else if (watchlistItem.Type == TraktSyncItemType.Episode)
            {
                Type = TraktItemType.Episode;
                Show = watchlistItem.Show;
                Episode = watchlistItem.Episode;
            }
        }

        public TraktItem(TraktHistoryItem historyItem)
        {
            if (historyItem.Type == TraktSyncItemType.Movie)
            {
                Type = TraktItemType.Movie;
                Movie = historyItem.Movie;
            }
            else if (historyItem.Type == TraktSyncItemType.Show)
            {
                Type = TraktItemType.Show;
                Show = historyItem.Show;
            }
            else if (historyItem.Type == TraktSyncItemType.Season)
            {
                Type = TraktItemType.Season;
                Show = historyItem.Show;
                Season = historyItem.Season;
            }
            else if (historyItem.Type == TraktSyncItemType.Episode)
            {
                Type = TraktItemType.Episode;
                Show = historyItem.Show;
                Episode = historyItem.Episode;
            }
        }

        public TraktItem(TraktSearchResult searchResult)
        {
            if (searchResult.Type == TraktSearchResultType.Movie)
            {
                Type = TraktItemType.Movie;
                Movie = searchResult.Movie;
            }
            else if (searchResult.Type == TraktSearchResultType.Show)
            {
                Type = TraktItemType.Show;
                Show = searchResult.Show;
            }
            else if (searchResult.Type == TraktSearchResultType.Episode)
            {
                Type = TraktItemType.Episode;
                Show = searchResult.Show;
                Episode = searchResult.Episode;
            }
            else if(searchResult.Type == TraktSearchResultType.List)
            {
                Type = TraktItemType.List;
                Show = searchResult.Show;
                List = searchResult.List;
            }
        }
    }
}