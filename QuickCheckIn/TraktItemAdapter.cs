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
using TraktApiSharp.Objects.Get.Watchlist;

namespace Dspeckmann.QuickCheckIn
{
    public class TraktItemAdapter : BaseAdapter<TraktItem>
    {
        Context context;
        TraktItem[] items;

        public TraktItemAdapter(Context context, TraktItem[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktItem this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem2, null);
            var text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            var text2 = view.FindViewById<TextView>(Android.Resource.Id.Text2);

            var item = items[position];
            switch(item.Type)
            {
                case TraktItemType.Movie:
                    text1.Text = item.Movie.Title;
                    text2.Text = item.Movie.Year.ToString();
                    break;
                case TraktItemType.Show:
                    text1.Text = item.Show.Title;
                    text2.Text = item.Show.Year.ToString();
                    break;
                case TraktItemType.Season:
                    text1.Text = item.Season.Number == 0 ? "Specials" : $"Season {item.Season.Number}";
                    // text2.Text = $"{item.Show.Title} / {item.Season.Episodes.Count()} Episodes"; // TODO: item.Season.Episodes is null
                    text2.Text = $"{item.Show.Title}";
                    break;
                case TraktItemType.Episode:
                    text1.Text = item.Episode.Title;
                    text2.Text = $"{item.Show.Title} S{item.Episode.SeasonNumber}E{item.Episode.Number}";
                    break;
                case TraktItemType.List:
                    text1.Text = item.List.Name;
                    text2.Text = item.List.ItemCount.Value.ToString() + " Items";
                    break;
            }
            
            return view;
        }
    }
}