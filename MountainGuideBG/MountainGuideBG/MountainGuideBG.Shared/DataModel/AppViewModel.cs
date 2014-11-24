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

namespace MountainGuideBG.Data
{

    public sealed class AppViewModel
    {
        private static AppViewModel appData = new AppViewModel();

        private ObservableCollection<CabinModel> cabins = new ObservableCollection<CabinModel>();
        public ObservableCollection<CabinModel> Cabins
        {
            get
            {
                if (this.cabins == null)
                {
                    this.Cabins = new ObservableCollection<CabinModel>();
                }
                return this.cabins;
            }
            set
            {
                if (this.cabins == null)
                {
                    this.cabins = new ObservableCollection<CabinModel>();
                }
                this.cabins.Clear();
                foreach (var item in value)
                {
                    this.cabins.Add(item);
                }
            }
        }
       
        private ObservableCollection<CabinModel> visitedCabins = new ObservableCollection<CabinModel>();
        public ObservableCollection<CabinModel> VisitedCabins
        {
            get
            {
                if (this.visitedCabins == null)
                {
                    this.VisitedCabins = new ObservableCollection<CabinModel>();
                }
                return this.visitedCabins;
            }
            set
            {
                if (this.visitedCabins == null)
                {
                    this.visitedCabins = new ObservableCollection<CabinModel>();
                }
                //this.visitedCabins.Clear();
                foreach (var item in value)
                {
                    this.visitedCabins.Add(item);
                }
            }
        }

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

        public static  IEnumerable<CabinModel> GetCabins()
        {
         //   await appData.GetParseDataAsync();

            return appData.Cabins;
        }
        public static IEnumerable<CabinModel> GetVisitedCabins()
        {
            //   await appData.GetParseDataAsync();

            return appData.VisitedCabins;
        }

        public static async Task<IEnumerable<MountainModel>> GetMountainsAsync()
        {
            await appData.GetParseDataAsync();

            return appData.Mountains;
        }

        public static  MountainModel GetMountainAsync(string uniqueId)
        {
            //await appData.GetParseDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = appData.Mountains.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static void RemoveFromCabins(string cabinId)
        {
            var cabin = GetCabin(cabinId);
            appData.Cabins.Remove(cabin);
        }

        public static void AddToVisitedCabins(CabinModel cabin)
        {
            appData.VisitedCabins.Add(cabin);
        }

        public static  CabinModel GetCabin(string uniqueId)
        {
            //await appData.GetParseDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = appData.Mountains.SelectMany(group => group.cabins).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static void SetCabins(IEnumerable<Cabin> cabins)
        {
            appData.Cabins.Clear();

            foreach (var cabin in cabins)
            {
                var newCabin = new CabinModel() { };

                newCabin.UniqueId = cabin.ObjectId;
                newCabin.Name = cabin.Name;
                newCabin.Phone = cabin.Phone;
                newCabin.Mountain = cabin.Mountain;
                newCabin.Description = cabin.Description;
                newCabin.Latitude = cabin.Coordinates.Latitude;
                newCabin.Longtitude = cabin.Coordinates.Longitude;
                newCabin.Image = new BitmapImage(cabin.Get<ParseFile>("image").Url);

                appData.Cabins.Add(newCabin);
            }
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


            SetCabins(cabinsFromParse);


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
                foreach (var cabin in this.Cabins)
                {
                    if (cabin.Mountain == mountain.Name)
                    {
                        mountain.cabins.Add(cabin);
                    }
                }
            }
        }

        internal static void SortCabinsByName()
        {
            throw new NotImplementedException();
        }

        internal static void SortCabinsByMountain()
        {
            throw new NotImplementedException();
        }
    }
}