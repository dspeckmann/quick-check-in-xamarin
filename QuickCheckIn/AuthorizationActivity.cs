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
using Android.Webkit;
using Android.Support.V7.App;

namespace Dspeckmann.QuickCheckIn
{
    [Activity(Label = "AuthorizationActivity")]
    public class AuthorizationActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var client = TraktApiHelper.Client;
            string authUrl = client.OAuth.CreateAuthorizationUrl();

            // TODO: Does this make sense at all?
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookies(null);

            var webView = new WebView(this);
            var webViewClient = new AuthorizationWebViewClient();
            webViewClient.CodeReceived += async (sender, e) =>
            {
                var traktAuthorization = await client.OAuth.GetAuthorizationAsync(e.Code);
                TraktApiHelper.SaveAuthorization(traktAuthorization);
                Finish();
            };
            webView.SetWebViewClient(webViewClient);
            SetContentView(webView);
            webView.LoadUrl(authUrl);
        }
    }
}