// 
// App.xaml.cs
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

using Xamarin.Forms;

namespace OpenIDSample
{
    public partial class App : Application
	{
	    #region Constructors

	    public App ()
		{
			InitializeComponent();

#if __ANDROID__
            Couchbase.Lite.Storage.CustomSQLite.Plugin.Register();
#else
		    Couchbase.Lite.Storage.SystemSQLite.Plugin.Register();
#endif

            MainPage = new NavigationPage(new MainPage());
		}

	    #endregion
	}
}
