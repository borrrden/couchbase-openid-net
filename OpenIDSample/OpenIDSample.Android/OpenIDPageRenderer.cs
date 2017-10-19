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
using System;
using System.Collections.Generic;
using Android.App;
using Android.Support.CustomTabs;
using OpenIDSample;
using OpenIDSample.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(OpenIDPage), typeof(OpenIDPageRenderer))]

namespace OpenIDSample
{
    public class OpenIDPageRenderer : PageRenderer
    {
        #region Variables

        private Uri _loginUrl;

        #endregion

        #region Overrides

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            var mgr = new CustomTabsActivityManager(Context as Activity);
            mgr.CustomTabsServiceConnected += (name, client) =>
            {
                mgr.LaunchUrl(_loginUrl.AbsoluteUri);
            };

            mgr.BindService();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var parent = e.NewElement as OpenIDPage;
            var data = parent.BindingContext as Dictionary<string, object>;
            _loginUrl = new Uri(data[OpenIDPage.LoginUrlKey] as string);
            (Context as MainActivity).Continuation = data[OpenIDPage.ContinuationKeyKey] as string;
        }

        #endregion
    }
}