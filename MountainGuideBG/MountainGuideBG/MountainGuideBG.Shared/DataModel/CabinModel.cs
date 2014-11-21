using MountainGuideBG.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace MountainGuideBG.DataModel
{
    public class CabinModel
    {
        public static Expression<Func<Cabin, CabinModel>> FromParseObject
        {
            get
            {
                return parseObj => new CabinModel()
                {
                    UniqueId = parseObj.ObjectId,
                    Name = parseObj.Name,
                    Mountain = parseObj.Mountain,
                    Description = parseObj.Description,
                    Image = new BitmapImage(parseObj.Get<ParseFile>(parseObj.Name.ToLower()).Url)
                };
            }
        }


        public string UniqueId { get;  set; }
        public string Name { get;  set; }
        public string Mountain { get;  set; }
        public string Description { get;  set; }
        public BitmapImage Image { get;  set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
