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
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Users.Lists;
using TraktApiSharp.Objects.Get.History;

namespace Dspeckmann.QuickCheckIn.Adapters
{
    // TODO: Rename all adapters TraktHistoryItemAdapter etc?
    public class HistoryAdapter : BaseAdapter<TraktHistoryItem>
    {
        Context context;
        TraktHistoryItem[] items;

        public HistoryAdapter(Context context, TraktHistoryItem[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktHistoryItem this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem2, null);
            var historyItem = items[position];
            var text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            if(historyItem.Type == TraktSyncItemType.Movie)
            {
                text1.Text = historyItem.Movie.Title;
            }
            else if(historyItem.Type == TraktSyncItemType.Episode)
            {
                text1.Text = $"{historyItem.Show.Title} S{historyItem.Episode.SeasonNumber:00}E{historyItem.Episode.Number:00}: {historyItem.Episode.Title}";
            }
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = historyItem.WatchedAt.Value.ToString();
            return view;
        }
    }
}