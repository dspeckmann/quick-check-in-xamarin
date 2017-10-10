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
using TraktApiSharp.Exceptions;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using Dspeckmann.QuickCheckIn;
using TraktApiSharp.Enums;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn.Fragments
{
    public class HistoryFragment : TraktItemListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // TODO: Funktioniert nur beim ersten Mal.
            // Entweder muss der Titel anders gesetzt werden oder die Daten anders gecached werden (und dann transaction.replace statt .add)
            (Activity as AppCompatActivity).SupportActionBar.Title = "History";
            return inflater.Inflate(Resource.Layout.History, container, false);
        }

        public override async void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var client = TraktApiHelper.Client;
            var history = await client.Users.GetWatchedHistoryAsync("me");
            var historyListView = View.FindViewById<ListView>(Resource.Id.HistoryListView);
            SetUpListView(historyListView, history.Select(historyItem => new TraktItem(historyItem)).ToArray());
        }
    }
}