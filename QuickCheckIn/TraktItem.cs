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
using TraktApiSharp.Objects.Get.Movies;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Users;
using TraktApiSharp.Objects.Get.Users.Lists;

namespace Dspeckmann.QuickCheckIn
{
    public class TraktItem
    {
        public TraktItemType Type { get; private set; }
        public TraktMovie Movie { get; private set; }
        public TraktShow Show { get; private set; }
        public TraktSeason Season { get; private set; }
        public TraktEpisode Episode { get; private set; }
        public TraktList List { get; private set; }

        // TODO: Constructors for all different types (+ search results, etc)
    }
}