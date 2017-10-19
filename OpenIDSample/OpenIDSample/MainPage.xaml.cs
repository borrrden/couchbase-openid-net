// 
// MainPage.xaml.cs
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
using Couchbase.Lite;
using Couchbase.Lite.Auth;
using Xamarin.Forms;

namespace OpenIDSample
{
    public partial class MainPage : ContentPage
	{
	    #region Constants

	    private const string DatabaseName = "openid";

	    #endregion

	    #region Variables

	    private readonly Uri GatewayURL = new Uri("INSERT_URL_HERE");
	    private Action<Replication> _changedHandler;

	    private Database _db;
	    private Replication _pull;
	    private Exception _syncError;
	    private string _username;

	    #endregion

	    #region Constructors

	    public MainPage()
		{
			InitializeComponent();
		    InitializeDatabase();
		}

	    #endregion

	    #region Private Methods

	    private void CheckAuthCodeLoginComplete(Replication repl)
	    {
	        if (repl != _pull) {
	            return;
	        }

	        // Check the pull replicator is done authenticating or not.
	        // If done, start the push replicator:
	        if (_username == null && repl.Username != null && IsReplicationStarted(repl)) {
	            if (_pull != null) {
	                _changedHandler = null;
	                CompleteLogin();
	            }
	        }
	    }

	    private void CompleteLogin()
	    {
	        Device.BeginInvokeOnMainThread(() =>
	        {
	            App.Current.MainPage.Navigation.PushAsync(new ReplicationPage());
	        });
	    }

	    private void InitializeDatabase()
	    {
	        if (_db == null) {
	            var opts = new DatabaseOptions
	            {
	                StorageType = StorageEngineTypes.SQLite,
	                Create = true
	            };

	            try {
	                _db = Manager.SharedInstance.OpenDatabase(DatabaseName, opts);
	            }
	            catch (Exception e) {
	                Console.WriteLine("Error, could not open database!");
	                Console.WriteLine(e);
	            }
	        }
	    }

	    private bool IsReplicationStarted(Replication repl)
	    {
	        return repl.Status == ReplicationStatus.Idle || repl.ChangesCount > 0;
	    }

	    private void OnChanged(object sender, ReplicationChangeEventArgs args)
        {
            Console.WriteLine($"Replication change status {args.Status} [{args.Source}]");

            _changedHandler?.Invoke(args.Source);

            var error = _pull?.LastError;
            if (error != null && error != _syncError) {
                _syncError = error;
                DisplayAlert("Error", _syncError.ToString(), "OK");
                _pull?.ClearAuthenticationStores();
            }
        }

	    private void PerformLogin(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            StartPull(r =>
            {
                var callback = OpenIDAuthenticator.GetOIDCCallback();
                r.Authenticator = AuthenticatorFactory.CreateOpenIDAuthenticator(Manager.SharedInstance, callback);
                _changedHandler = CheckAuthCodeLoginComplete;
            });
        }

	    private void StartPull(Action<Replication> callback)
	    {
	        _pull = _db.CreatePullReplication(GatewayURL);
            _pull.Continuous = true;
	        callback?.Invoke(_pull);
	        _pull.Changed += OnChanged;
	        _pull.Start();
	    }

	    #endregion
	}
}
