// 
// OpenIDPageRenderer.cs
// 
// Author:
//     Jim Borden  <jim.borden@couchbase.com>
// 
// Copyright (c) 2017 Couchbase, Inc All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
using System.Collections.Generic;

using Foundation;
using OpenIDSample;
using SafariServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(OpenIDPage), typeof(OpenIDPageRenderer))]

namespace OpenIDSample
{
    public class OpenIDPageRenderer : PageRenderer
    {
        #region Constants

        // Will be used to fire an event to resume login after getting the token from 
        // the login procedure
        public const string SafariCloseNotification = "SafariCloseNotification";

        #endregion

        #region Variables

        private string _continuation;

        private string _loginUrl;
        private SFSafariViewController _safariController;

        #endregion

        #region Private Methods

        private void SafariLogin(NSNotification obj)
        {
            var url = obj.Object as NSUrl;
            var callback = OpenIDAuthenticator.GetLoginContinuation(_continuation);
            callback(url, null);
            OpenIDAuthenticator.UnregisterLoginContinuation(_continuation);
            _safariController.DismissViewController(true, null);
        }

        #endregion

        #region Overrides

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            NSNotificationCenter.DefaultCenter.AddObserver(new NSString(SafariCloseNotification), SafariLogin);

            var page = e.NewElement as OpenIDPage;
            var data = page?.BindingContext as Dictionary<string, object>;

            _loginUrl = data[OpenIDPage.LoginUrlKey] as string;
            _continuation = data[OpenIDPage.ContinuationKeyKey] as string;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _safariController = new SFSafariViewController(new NSUrl(_loginUrl.Replace("localhost", "192.168.1.2")))
            {
                Delegate = new SafariDelegate()

            };

            PresentViewController(_safariController, true, null);
        }

        #endregion

        #region Nested

        private sealed class SafariDelegate : SFSafariViewControllerDelegate
        {
            #region Overrides

            public override void DidFinish(SFSafariViewController controller)
            {
                var nav = controller.ParentViewController.NavigationController;
                controller.DismissViewController(true, null);
                nav.PopViewController(false);
            }

            #endregion
        }

        #endregion
    }
}