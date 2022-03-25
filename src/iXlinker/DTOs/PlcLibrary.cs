namespace iXlinkerDtos
{
    public class PlcLibrary : NotifiableBase
    {
        private string title;
        public string Title
        {
            get { return this.title; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.title = value;
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }

        private string version;
        public string Version
        {
            get { return this.version; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.version = value;
                    NotifyPropertyChanged(nameof(Version));
                }
            }
        }

        private string companyName;
        public string CompanyName
        {
            get { return this.companyName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.companyName = value;
                    NotifyPropertyChanged(nameof(CompanyName));
                }
            }
        }

        private string effectiveVersion;
        public string EffectiveVersion
        {
            get { return this.effectiveVersion; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.effectiveVersion = value;
                    NotifyPropertyChanged(nameof(EffectiveVersion));
                }
            }
        }

        private string path;
        public string Path
        {
            get { return this.path; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.path = value;
                    NotifyPropertyChanged(nameof(Path));
                }
            }
        }

        private string @namespace;
        public string Namespace
        {
            get { return this.@namespace; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.@namespace = value;
                    NotifyPropertyChanged(nameof(Namespace));
                }
            }
        }

        private string placeHolder;
        public string PlaceHolder
        {
            get { return this.placeHolder; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.placeHolder = value;
                    NotifyPropertyChanged(nameof(PlaceHolder));
                }
            }
        }
    }
}
