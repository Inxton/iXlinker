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

        private double size;
        public double Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                NotifyPropertyChanged(nameof(Size));
            }
        }

        private string previousPort;
        public string PreviousPort
        {
            get { return this.previousPort; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.previousPort = value;
                    NotifyPropertyChanged(nameof(PreviousPort));
                }
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
