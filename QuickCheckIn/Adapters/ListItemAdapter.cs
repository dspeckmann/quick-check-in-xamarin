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

namespace Dspeckmann.QuickCheckIn.Adapters
{
    public class ListItemAdapter : BaseAdapter<TraktListItem>
    {
        Context context;
        TraktListItem[] items;

        public ListItemAdapter(Context context, TraktListItem[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktListItem this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem2, null);
            var text1 = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            var text2 = view.FindViewById<TextView>(Android.Resource.Id.Text2);

            var listItem = items[position];
            if (listItem.Type == TraktListItemType.Movie)
            {
                text1.Text = listItem.Movie.Title;
                text2.Text = listItem.Movie.Year.ToString();
            }
            else if(listItem.Type == TraktListItemType.Show)
            {
                text1.Text = listItem.Show.Title;
                text2.Text = listItem.Show.Year.ToString();
            }
            // TODO: Season, Episode, Person?
            
            return view;
        }
    }
}