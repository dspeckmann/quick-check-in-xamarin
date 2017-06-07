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

namespace Dspeckmann.QuickCheckIn.Adapters
{
    public class ListAdapter : BaseAdapter<TraktList>
    {
        Context context;
        TraktList[] items;

        public ListAdapter(Context context, TraktList[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        public override TraktList this[int position] => items[position];

        public override int Count => items.Count();

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView ?? LayoutInflater.From(context).Inflate(Android.Resource.Layout.SimpleListItem2, null);
            var list = items[position];
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = list.Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = $"{list.Description} ({list.ItemCount.Value} items)"; // TODO: Check for value
            return view;
        }
    }
}