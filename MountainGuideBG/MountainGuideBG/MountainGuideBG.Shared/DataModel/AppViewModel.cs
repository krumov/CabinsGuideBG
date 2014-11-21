using MountainGuideBG.DataModel;
using MountainGuideBG.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
            get
            {
                if (this.mountains == null)
                {
                    this.Mountains = new ObservableCollection<MountainModel>();
                }
                return this.mountains;
            }
            set
            {
                if (this.mountains == null)
                {
                    this.mountains = new ObservableCollection<MountainModel>();
                }
                this.mountains.Clear();
                foreach (var item in value)
                {
                    this.mountains.Add(item);
                }
            }
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

            var mountainsFromParse = await new ParseQuery<Mountain>()
                .FindAsync(
                CancellationToken.None);

            var cabinsFromParse = await new ParseQuery<Cabin>()
                .FindAsync(
                CancellationToken.None);

            var cabins = new List<CabinModel>();

            foreach (var cabin in cabinsFromParse)
            {
                var newCabin = new CabinModel(){};

                newCabin.UniqueId = cabin.ObjectId;
                newCabin.Name = cabin.Name;
                newCabin.Mountain = cabin.Mountain;
                newCabin.Description = cabin.Description;
                newCabin.Image = new BitmapImage(cabin.Get<ParseFile>("image").Url);
                

                cabins.Add(newCabin);
            }


            foreach (var mountain in mountainsFromParse)
            {
                var newMountain = new MountainModel() { };

                newMountain.UniqueId = mountain.ObjectId;
                newMountain.Name = mountain.Name;
                newMountain.Description = mountain.Description;
                newMountain.cabins = new ObservableCollection<CabinModel>();
                newMountain.Image = new BitmapImage(mountain.Get<ParseFile>("image").Url);


                this.Mountains.Add(newMountain);
            }

            foreach (var mountain in this.Mountains)
            {
                foreach (var cabin in cabins)
                {
                    if (cabin.Mountain == mountain.Name)
                    {
                        mountain.cabins.Add(cabin);
                    }
                }
            }

           var b = 5;
        }
    }
}