using Parse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MountainGuideBG.Models
{
    [ParseClassName("Mountains")]
    class Mountain : ParseObject
    {
        public Mountain()
        {

        }

        [ParseFieldName("name")]
        public string Name
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

        [ParseFieldName("cabins")]
        public ObservableCollection<Cabin> cabins
        {
            get { return GetProperty<ObservableCollection<Cabin>>(); }
            set { SetProperty<ObservableCollection<Cabin>>(value); }
        }

    }
}
