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
                    cabins = new ObservableCollection<CabinModel>(),
                    Image = new BitmapImage(parseObj.Get<ParseFile>(parseObj.Name.ToLower()).Url)
                };
            }
        }

        public MountainModel()
        {
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
