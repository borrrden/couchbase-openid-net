// 
// AppDelegate.cs
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

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace OpenIDSample.iOS
{
    [Register("AppDelegate")]
	public class AppDelegate : FormsApplicationDelegate
	{
	    #region Overrides

	    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			LoadApplication (new App());

            return base.FinishedLaunching (app, options);
		}

	    public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            if (sourceApplication == "com.apple.SafariViewService") {
                NSNotificationCenter.DefaultCenter.PostNotificationName(OpenIDPageRenderer.SafariCloseNotification,
                    url);
                return true;
            }

            return true;
        }

	    #endregion
	}
}
