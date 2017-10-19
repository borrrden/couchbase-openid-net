// 
// MainActivity.cs
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

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace OpenIDSample.Droid
{
    [Activity (Label = "OpenIDSample", 
        Icon = "@drawable/icon",
        Theme="@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
	[IntentFilter(new [] { Intent.ActionView }, 
	    DataScheme="cbl",
	    Categories=new [] { Intent.CategoryDefault, Intent.CategoryBrowsable })]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
	    #region Properties

	    public string Continuation { get; set; }

	    #endregion

	    #region Overrides

	    protected override void OnCreate (Bundle bundle)
		{
		    TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);
            LoadApplication (new App ());
		}

	    protected override void OnNewIntent(Intent intent)
        {
            // Called when the activity receives the URL from the custom tabs
            base.OnNewIntent(intent);

            var callback = OpenIDAuthenticator.GetLoginContinuation(Continuation);
            var url = new Uri(intent.Data.ToString());
            callback(url, null);
            OpenIDAuthenticator.UnregisterLoginContinuation(Continuation);
        }

	    #endregion
	}
}

