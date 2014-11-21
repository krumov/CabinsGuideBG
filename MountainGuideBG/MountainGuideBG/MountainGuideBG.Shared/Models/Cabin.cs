using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace MountainGuideBG.Models
{
    [ParseClassName("Cabins")]
    public class Cabin: ParseObject
    {
        public Cabin()
        {

        }

        [ParseFieldName("name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("mountain")]
        public string Mountain
{ 
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
}

        [ParseFieldName("description")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty<string>(value); }
        }

        [ParseFieldName("image")]
        public ParseFile Image
        {
            get { return GetProperty<ParseFile>(); }
            set { SetProperty<ParseFile>(value); }
        }

        [ParseFieldName("coordinates")]
        public ParseGeoPoint Coordinates
        {
            get { return GetProperty<ParseGeoPoint>(); }
            set { SetProperty<ParseGeoPoint>(value); }
        }
    }
}
