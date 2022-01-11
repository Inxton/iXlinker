namespace ViewModels
{
    public class DataTypeViewModel : BaseViewModel
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

        private string guid;
        public string GUID
        {
            get { return this.guid; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.guid = value;
                    NotifyPropertyChanged(nameof(GUID));
                }
            }
        }

        private string baseType;
        public string BaseType
        {
            get { return this.baseType; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.baseType = value;
                    NotifyPropertyChanged(nameof(BaseType));
                }
            }
        }

        private bool iecbaseType;
        public bool IecbaseType
        {
            get { return this.iecbaseType; }
            set
            {
                this.iecbaseType = value;
                NotifyPropertyChanged(nameof(IecbaseType));
            }
        }

        private bool autoDelete;
        public bool AutoDelete
        {
            get { return this.autoDelete; }
            set
            {
                this.autoDelete = value;
                NotifyPropertyChanged(nameof(AutoDelete));
            }
        }

        private bool hideSubItems;
        public bool HideSubItems
        {
            get { return this.hideSubItems; }
            set
            {
                this.hideSubItems = value;
                NotifyPropertyChanged(nameof(HideSubItems));
            }
        }
        
        private uint bitSize;
        public uint BitSize
        {
            get { return this.bitSize; }
            set
            {
                this.bitSize = value;
                NotifyPropertyChanged(nameof(BitSize));
            }
        }

        private uint lbound;
        public uint Lbound
        {
            get { return this.lbound; }
            set
            {
                this.lbound = value;
                NotifyPropertyChanged(nameof(Lbound));
            }
        }

        private uint elements;
        public uint Elements
        {
            get { return this.elements; }
            set
            {
                this.elements = value;
                NotifyPropertyChanged(nameof(Elements));
            }
        }
    }
}
