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
using TraktApiSharp;
using TraktApiSharp.Authentication;

namespace Dspeckmann.QuickCheckIn
{
    public class TraktApiHelper
    {
        private static TraktClient client;

        public static TraktClient Client
        {
            get
            {
                if(client == null)
                {
                    client = new TraktClient(Application.Context.GetString(Resource.String.ClientId),
                        Application.Context.GetString(Resource.String.ClientSecret));

                    client.Configuration.UseSandboxEnvironment = true; // TODO: Remove in prod
                    client.Configuration.ForceAuthorization = true; // TODO: Necessary?

                    var preferences = Application.Context.GetSharedPreferences("Authorization", FileCreationMode.Private);
                    var accessToken = preferences.GetString("AccessToken", null);
                    var refreshToken = preferences.GetString("RefreshToken", null);
                    if(accessToken != null)
                    {
                        if(refreshToken != null)
                        {
                            client.Authorization = TraktAuthorization.CreateWith(accessToken, refreshToken);
                        }
                        else
                        {
                            client.Authorization = TraktAuthorization.CreateWith(accessToken);
                        }
                    }
                }

                return client;
            }
        }

        public static void SaveAuthorization(TraktAuthorization authorization)
        {
            var preferences = Application.Context.GetSharedPreferences("Authorization", FileCreationMode.Private);
            var editor = preferences.Edit();
            editor.PutString("AccessToken", authorization.AccessToken); // TODO: Use constants or Android strings
            editor.PutString("RefreshToken", authorization.RefreshToken);
            editor.Apply();
        }

        public static void DeleteAuthorization()
        {
            var preferences = Application.Context.GetSharedPreferences("Authorization", FileCreationMode.Private);
            var editor = preferences.Edit();
            editor.Remove("AccessToken");
            editor.Remove("RefreshToken");
            editor.Apply();
        }
    }
}