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
using Android.Graphics;

namespace Dspeckmann.QuickCheckIn
{
    public class CodeReceivedEventArgs : EventArgs
    {
        public string Code { get; private set; }

        public CodeReceivedEventArgs(string code)
            : base()
        {
            Code = code;
        }
    }

    class AuthorizationWebViewClient : WebViewClient
    {
        public delegate void CodeReceivedEventHandler(object sender, CodeReceivedEventArgs e);
        public event CodeReceivedEventHandler CodeReceived;

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            if(url.StartsWith("https://trakt.tv/oauth/authorize/") || url.StartsWith("https://staging.trakt.tv/oauth/authorize/"))
            {
                string code = url.Split(new[] { '/' }).Last();
                OnCodeReceived(new CodeReceivedEventArgs(code));
            }

            base.OnPageStarted(view, url, favicon); // TODO: Necessary?
        }

        private void OnCodeReceived(CodeReceivedEventArgs e)
        {
            CodeReceived?.Invoke(this, e);
        }
    }
}