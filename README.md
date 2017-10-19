# OpenID Connect Barebones Example
### Xamarin Forms for iOS / Android

This repo has a functional (but not terribly pretty) example of implementing OpenID.  Note that because Nuget is weird, you will need to either open the Couchbase.Lite.iOS and Couchbase.Lite.Android solutions once (inside of vendor/couchbase-lite-net/src) or run `nuget restore` on them so that the packages get downloaded into the proper directories.