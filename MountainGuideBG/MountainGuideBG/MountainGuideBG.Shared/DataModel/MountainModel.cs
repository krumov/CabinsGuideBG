using MountainGuideBG.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;

namespace MountainGuideBG.DataModel
{
    public class MountainModel
    {

        public static Expression<Func<Mountain, MountainModel>> FromParseObject
        {
            get
            {
                return parseObj => new MountainModel()
                {
                    UniqueId = parseObj.ObjectId,
                    Name = parseObj.Name,
                    Description = parseObj.Description,
                    cabins = (ObservableCollection<CabinModel>) parseObj.cabins.AsQueryable().Select(CabinModel.FromParseObject),
                    Image = new BitmapImage(parseObj.Get<ParseFile>(parseObj["name"].ToString().ToLower()).Url)
                };
            }
        }

        public MountainModel()
        {
            this.cabins = new ObservableCollection<CabinModel>();
        }

        public MountainModel(String uniqueId, String title, BitmapImage image, String description)
        {
            this.UniqueId = uniqueId;
            this.Name = title;
            this.Description = description;
            this.Image = image;
            this.cabins = new ObservableCollection<CabinModel>();
        }

        public string UniqueId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
        public ObservableCollection<CabinModel> cabins { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
