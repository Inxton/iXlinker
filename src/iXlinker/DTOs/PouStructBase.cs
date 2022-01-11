namespace iXlinkerDtos
{
    public class PouStructBase : NotifiableBase
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        private string boxOrderCode;
        public string BoxOrderCode
        {
            get { return this.boxOrderCode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.boxOrderCode = value;
                    NotifyPropertyChanged(nameof(BoxOrderCode));
                }
            }
        }

        private string prefix;
        public string Prefix
        {
            get { return this.prefix; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.prefix = value;
                    NotifyPropertyChanged(nameof(Prefix));
                }
            }
        }
        
        private string id;
        public string Id
        {
            get { return this.id; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }

        private uint crc32;
        public uint Crc32
        {
            get { return this.crc32; }
            set
            {
                this.crc32 = value;
                NotifyPropertyChanged(nameof(Crc32));
            }
        }

        private uint sizeInBites;
        public uint SizeInBites
        {
            get { return this.sizeInBites; }
            set
            {
                this.sizeInBites = value;
                NotifyPropertyChanged(nameof(SizeInBites));
            }
        }

        private double sizeInBytes;
        public double SizeInBytes
        {
            get { return this.sizeInBytes; }
            set
            {
                this.sizeInBytes = value;
                NotifyPropertyChanged(nameof(SizeInBytes));
            }
        }

    }
}
