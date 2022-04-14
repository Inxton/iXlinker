using System.Collections.ObjectModel;

namespace iXlinkerDtos
{
    public class PdoViewModel : NotifiableBase
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

        private string index;
        public string Index
        {

            get { return this.index; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.index = value;
                    NotifyPropertyChanged(nameof(Index));
                }
            }
        }

        private uint indexNumber = 0;
        public uint IndexNumber
        {
            get { return this.indexNumber; }
            set
            {
                this.indexNumber = value;
                NotifyPropertyChanged(nameof(IndexNumber));
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

        private string inOut;
        public string InOut
        {

            get { return this.inOut; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.inOut = value;
                    NotifyPropertyChanged(nameof(InOut));
                }
            }
        }

        private string inOutPlcProj = "";
        public string InOutPlcProj
        {
            get { return this.inOutPlcProj; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.inOutPlcProj = value;
                    NotifyPropertyChanged(nameof(InOutPlcProj));
                }
            }
        }

        private string inOutMappings = "";
        public string InOutMappings
        {
            get { return this.inOutMappings; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.inOutMappings = value;
                    NotifyPropertyChanged(nameof(InOutMappings));
                }
            }
        }

        private string syncMan;
        public string SyncMan
        {
            get { return this.syncMan; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.syncMan = value;
                    NotifyPropertyChanged(nameof(SyncMan));
                }
            }
        }

        private string syncUnit;
        public string SyncUnit
        {
            get { return this.syncUnit; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.syncUnit = value;
                    NotifyPropertyChanged(nameof(SyncUnit));
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

        private string ownerBname;
        public string OwnerBname
        {
            get { return this.ownerBname; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.ownerBname = value;
                    NotifyPropertyChanged(nameof(OwnerBname));
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

        private ObservableCollection<PdoEntryViewModel> pdoEntriesUnstructured;
        public ObservableCollection<PdoEntryViewModel> PdoEntriesUnstructured
        {
            get
            {
                return this.pdoEntriesUnstructured ?? (this.pdoEntriesUnstructured = new ObservableCollection<PdoEntryViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.pdoEntriesUnstructured = value;
                    NotifyPropertyChanged(nameof(PdoEntriesUnstructured));
                }
            }
        }

        private ObservableCollection<PdoEntryViewModel> pdoEntriesStructured;
        public ObservableCollection<PdoEntryViewModel> PdoEntriesStructured
        {
            get
            {
                return this.pdoEntriesStructured ?? (this.pdoEntriesStructured = new ObservableCollection<PdoEntryViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.pdoEntriesStructured = value;
                    NotifyPropertyChanged(nameof(PdoEntriesStructured));
                }
            }
        }

        private MappableObject mapableObject;
        public MappableObject MapableObject
        {
            get
            {
                return this.mapableObject ?? (this.mapableObject = new MappableObject());
            }
            set
            {
                if (value != null)
                {
                    this.mapableObject = value;
                    NotifyPropertyChanged(nameof(MapableObject));
                }
            }
        }

    }
}
