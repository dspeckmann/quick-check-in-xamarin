using System;
using System.Linq;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using TraktApiSharp;
using TraktApiSharp.Enums;
using Dspeckmann.QuickCheckIn.Fragments;

namespace Dspeckmann.QuickCheckIn
{
    [Activity(Label = "Quick Check In", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        private TraktClient client;
        private ActionBarDrawerToggle drawerToggle;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // TODO: Remove on prod? Or put in Application class?
            AndroidEnvironment.UnhandledExceptionRaiser += (sender, e) =>
            {
                Console.WriteLine(e.Exception.Message);
                Console.WriteLine(e.Exception.StackTrace);
            };
            
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.MainToolbar);
            var drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.MainDrawerLayout);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Quick Check In";
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, Resource.String.OpenDrawer, Resource.String.CloseDrawer);
            drawerToggle.DrawerIndicatorEnabled = true;
            drawerLayout.AddDrawerListener(drawerToggle);
            drawerToggle.SyncState();
            
            var drawerListView = FindViewById<ListView>(Resource.Id.DrawerListView);
            string[] items = { "On Deck", "Trending", "Lists", "History" };
            drawerListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
            drawerListView.ItemClick += (sender, e) =>
            {
                Android.Support.V4.App.Fragment fragment = null;

                // TODO: Improve, don't rely on indices
                switch(e.Position)
                {
                    case 0: // On Deck
                        fragment = new OnDeckFragment();
                        break;
                    case 1: // Trending
                        // TODO: Add TrendingFragment
                        break;
                    case 2: // Lists
                        fragment = new ListOverviewFragment();
                        break;
                    case 3: // History
                        fragment = new HistoryFragment();
                        break;
                }

                if (fragment != null)
                {
                    var drawerTransaction = SupportFragmentManager.BeginTransaction();
                    drawerTransaction.Replace(Resource.Id.MainFrameLayout, fragment);
                    drawerTransaction.AddToBackStack(null); // Name?
                    drawerTransaction.Commit();
                    drawerLayout.CloseDrawer(FindViewById<LinearLayout>(Resource.Id.DrawerChildLayout)); // Parameter richtig?
                }
            };

            client = TraktApiHelper.Client;
            if(!client.Authorization.IsValid || (client.Authorization.IsValid && client.Authorization.IsExpired))
            {
                if (client.Authorization.IsRefreshPossible)
                {
                    var traktAuthorization = await client.OAuth.RefreshAuthorizationAsync();
                    TraktApiHelper.SaveAuthorization(traktAuthorization);
                }
                else
                {
                    Intent authorizationIntent = new Intent(this, typeof(AuthorizationActivity));
                    StartActivity(authorizationIntent);
                }
            }

            var logoutTextView = FindViewById<TextView>(Resource.Id.LogoutTextView);
            logoutTextView.Click += (sender, e) =>
            {
                if (client.Authorization == null || !client.Authorization.IsValid)
                {
                    Intent authorizationIntent = new Intent(this, typeof(AuthorizationActivity));
                    StartActivity(authorizationIntent);
                }
                else
                {
                    client.Authorization = null;
                    TraktApiHelper.DeleteAuthorization();
                    logoutTextView.Text = "Login";
                    var usernameTextView = FindViewById<TextView>(Resource.Id.UsernameTextView);
                    usernameTextView.Visibility = ViewStates.Gone;
                    var currentlyWatchingLayout = FindViewById<LinearLayout>(Resource.Id.CurrentlyWatchingLayout);
                    currentlyWatchingLayout.Visibility = ViewStates.Gone;
                }
            };
            
            var transaction = SupportFragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.MainFrameLayout, new OnDeckFragment());
            transaction.SetTransition((int)FragmentTransit.FragmentOpen);
            // Do not add this transaction to the back stack because then the app would be empty when the user presses back
            transaction.Commit();
        }

        protected override async void OnResume()
        {
            base.OnResume();

            var usernameTextView = FindViewById<TextView>(Resource.Id.UsernameTextView);
            var logoutTextView = FindViewById<TextView>(Resource.Id.LogoutTextView);

            try
            {    
                var traktUser = await client.Users.GetUserProfileAsync("me");
                usernameTextView.Text = traktUser.Username;
                usernameTextView.Visibility = ViewStates.Visible;
                logoutTextView.Text = "Logout";
            }
            catch(TraktApiSharp.Exceptions.TraktAuthorizationException)
            {
                usernameTextView.Visibility = ViewStates.Gone;
                logoutTextView.Text = "Login";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var currentlyWatchingLayout = FindViewById<LinearLayout>(Resource.Id.CurrentlyWatchingLayout);
                var currentlyWatchingTextView = FindViewById<TextView>(Resource.Id.CurrentlyWatchingTextView);

                var watching = await client.Users.GetWatchingAsync("me");

                if (watching == null)
                {
                    currentlyWatchingLayout.Visibility = ViewStates.Gone;
                }
                else
                {
                    if (watching.Type == TraktSyncType.Movie)
                    {
                        currentlyWatchingTextView.Text = watching.Movie.Title;
                        // TODO: Show notification of what is being watched right now? Allow user to toggle! Use cool Oreo style notifications?
                    }
                    else if (watching.Type == TraktSyncType.Episode)
                    {
                        currentlyWatchingTextView.Text = $"{watching.Show.Title} S{watching.Episode.SeasonNumber.Value:00}E{watching.Episode.Number.Value:00}: {watching.Episode.Title}";
                    }
                    
                    currentlyWatchingLayout.Visibility = ViewStates.Visible;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.OptionsMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Resource.Id.SearchMenuItem)
            {
                var searchFragment = new SearchFragment();
                var transaction = SupportFragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.MainFrameLayout, searchFragment);
                transaction.AddToBackStack(null); // Name?
                transaction.Commit();
                return true;
            }
            else if(drawerToggle.OnOptionsItemSelected(item))
            { 
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}

