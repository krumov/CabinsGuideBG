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
        public static Expression<Func<ParseObject, CabinModel>> FromParseObject
        {
            get
            {
                return parseObj => new CabinModel()
                {
                    UniqueId = parseObj["objectId"].ToString(),
                    Name = parseObj["name"].ToString(),
                    Mountain = parseObj["mountain"].ToString(),
                    Description = parseObj["description"].ToString(),
                    Image = new BitmapImage(parseObj.Get<ParseFile>(parseObj["name"].ToString().ToLower()).Url)
                };
            }
        }

        public CabinModel()
        {

        }

        public CabinModel(String uniqueId, String title, String subtitle, BitmapImage imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Name = title;
            this.Mountain = subtitle;
            this.Description = description;
            this.Image = imagePath;
        }

        public string UniqueId { get; private set; }
        public string Name { get; private set; }
        public string Mountain { get; private set; }
        public string Description { get; private set; }
        public BitmapImage Image { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
