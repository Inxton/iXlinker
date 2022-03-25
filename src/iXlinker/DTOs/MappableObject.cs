using System.Collections.ObjectModel;

namespace iXlinkerDtos
{
    public class MappableObject : NotifiableBase
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

        private string type_Value;
        public string Type_Value
        {
            get { return this.type_Value; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.type_Value = value;
                    NotifyPropertyChanged(nameof(Type_Value));
                }
            }
        }

        private string typeNamespace;
        public string TypeNamespace
        {
            get { return this.typeNamespace; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.typeNamespace = value;
                    NotifyPropertyChanged(nameof(TypeNamespace));
                }
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

        private ObservableCollection<MappableItem> mapableItems;
        public ObservableCollection<MappableItem> MapableItems 
        {
            get { return this.mapableItems ?? (this.mapableItems = new ObservableCollection<MappableItem>()); }
            set
            {
                if (value != null)
                {
                    this.mapableItems = value;
                    NotifyPropertyChanged(nameof(MapableItems));
                }
            }
        }
    }
}
