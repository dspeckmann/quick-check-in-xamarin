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

namespace Dspeckmann.QuickCheckIn.Adapters
{
    public class SearchResultAdapter : BaseAdapter<TraktSearchResult>
    {
        Context context;
        TraktSearchResult[] items;

        public SearchResultAdapter(Context context, TraktSearchResult[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktSearchResult this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem2, null);
            var searchResult = items[position];
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = searchResult.Movie?.Title ?? searchResult.Episode?.Title ?? searchResult.Show?.Title ?? "Something Else";
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = searchResult.Type.ToString();
            return view;
        }
    }
}