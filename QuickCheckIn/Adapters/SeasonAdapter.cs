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

namespace Dspeckmann.QuickCheckIn.Adapters
{
    public class SeasonAdapter : BaseAdapter<TraktSeason>
    {
        Context context;
        TraktSeason[] items;

        public SeasonAdapter(Context context, TraktSeason[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktSeason this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem1, null);
            var season = items[position];
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = season.Number.HasValue ? (season.Number.Value == 0 ? "Specials" : $"Season {season.Number.Value}") : "Unknown Season";
            return view;
        }
    }
}