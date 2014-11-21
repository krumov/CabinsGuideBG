using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MountainGuideBG.DataModel
{
    public class MountainModel
    {
        public MountainModel(String uniqueId, String title, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Name = title;
            this.Description = description;
            this.ImagePath = imagePath;
            this.cabins = new ObservableCollection<CabinModel>();
        }

        public string UniqueId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<CabinModel> cabins { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
