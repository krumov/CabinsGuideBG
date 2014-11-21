﻿using MountainGuideBG.DataModel;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace MountainGuideBG.Data
{
    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// 
    /// SampleDataSource initializes with data read from a static json file included in the 
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class AppViewModel
    {
        private static AppViewModel appData = new AppViewModel();

        private ObservableCollection<MountainModel> mountains = new ObservableCollection<MountainModel>();
        public ObservableCollection<MountainModel> Mountains
        {
            get { return this.mountains; }
        }

        public static async Task<IEnumerable<MountainModel>> GetMountainsAsync()
        {
            await appData.GetParseDataAsync();

            return appData.Mountains;
        }

        public static async Task<MountainModel> GetMountainAsync(string uniqueId)
        {
            await appData.GetParseDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = appData.Mountains.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<CabinModel> GetCabinAsync(string uniqueId)
        {
            await appData.GetParseDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = appData.Mountains.SelectMany(group => group.cabins).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetParseDataAsync()
        {
            if (this.mountains.Count != 0)
                return;

            var testObject = await ParseObject.GetQuery("TestObject").FindAsync();

            var b = 5;
        }
    }
}