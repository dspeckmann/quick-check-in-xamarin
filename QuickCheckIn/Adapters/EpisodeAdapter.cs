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
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Get.Shows.Episodes;

namespace Dspeckmann.QuickCheckIn.Adapters
{
    public class EpisodeAdapter : BaseAdapter<TraktEpisode>
    {
        Context context;
        TraktEpisode[] items;

        public EpisodeAdapter(Context context, TraktEpisode[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktEpisode this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem1, null);
            var episode = items[position];
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = $"S{episode.SeasonNumber.Value:00}E{episode.Number.Value:00}: {episode.Title}"; // HasValue?
            return view;
        }
    }
}