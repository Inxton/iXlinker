namespace iXlinkerDtos
{
    using System.Collections.ObjectModel;

    public enum BoxTypes
    {
        EtherCAT = 1,
        Profinet = 2,

    }

    public class BoxViewModel : NotifiableBase
    {
        private BoxTypes boxType;
        public BoxTypes BoxType
        {

            get { return this.boxType; }
            set
            {
                this.boxType = value;
                NotifyPropertyChanged(nameof(BoxType));
            }
        }

        private DeviceTypes masterDeviceType;
        public DeviceTypes MasterDeviceType
        {

            get { return this.masterDeviceType; }
            set
            {
                this.masterDeviceType = value;
                NotifyPropertyChanged(nameof(MasterDeviceType));
            }
        }

        private string masterDeviceName;
        public string MasterDeviceName
        {

            get { return this.masterDeviceName; }
            set
            {
                this.masterDeviceName = value;
                NotifyPropertyChanged(nameof(MasterDeviceName));
            }
        }

        private string masterDeviceId;
        public string MasterDeviceId
        {

            get { return this.masterDeviceId; }
            set
            {
                this.masterDeviceId = value;
                NotifyPropertyChanged(nameof(MasterDeviceId));
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

        private int id = 0;
        public int Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        private string productCode;
        public string ProductCode
        {
            get { return this.productCode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.productCode = value;
                    NotifyPropertyChanged(nameof(ProductCode));
                }
            }
        }

        private string revisionNo;
        public string RevisionNo
        {
            get { return this.revisionNo; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.revisionNo = value;
                    NotifyPropertyChanged(nameof(RevisionNo));
                }
            }
        }

        private string portABoxInfo;
        public string PortABoxInfo
        {
            get { return this.portABoxInfo; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.portABoxInfo = value;
                    NotifyPropertyChanged(nameof(PortABoxInfo));
                }
            }
        }

        private string connectedToPort;
        public string ConnectedToPort
        {
            get { return this.connectedToPort; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.connectedToPort = value;
                    NotifyPropertyChanged(nameof(ConnectedToPort));
                }
            }
        }

        private int connectedToBoxId;
        public int ConnectedToBoxId
        {
            get { return this.connectedToBoxId; }
            set
            {
                    this.connectedToBoxId = value;
                    NotifyPropertyChanged(nameof(ConnectedToBoxId));
            }
        }

        private string connectedToBox;
        public string ConnectedToBox
        {
            get { return this.connectedToBox; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.connectedToBox = value;
                    NotifyPropertyChanged(nameof(ConnectedToBox));
                }
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

        private bool containsSubBoxes;
        public bool ContainsSubBoxes
        {
            get { return this.containsSubBoxes; }
            set
            {
                this.containsSubBoxes = value;
                NotifyPropertyChanged(nameof(ContainsSubBoxes));
            }
        }

        private int portPhys = 0;
        public int PortPhys
        {
            get { return this.portPhys;}
            set
            {
                this.portPhys = value;
                NotifyPropertyChanged(nameof(PortPhys));
            }
        }

        private string physics;
        public string Physics
        {
            get { return this.physics; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.physics = value;
                    NotifyPropertyChanged(nameof(Physics));
                }
            }
        }

        private string description;
        public string Description
        {
            get { return this.description; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        private int totalNumberOfBoxes;
        public int TotalNumberOfBoxes
        {
            get { return this.totalNumberOfBoxes; }
            set
            {
                this.totalNumberOfBoxes = value;
                NotifyPropertyChanged(nameof(TotalNumberOfBoxes));
            }
        }

        private int numberOfSubBoxes;
        public int NumberOfSubBoxes
        {
            get { return this.numberOfSubBoxes; }
            set
            {
                this.numberOfSubBoxes = value;
                NotifyPropertyChanged(nameof(NumberOfSubBoxes));
            }
        }

        private ObservableCollection<BoxViewModel> boxes;
        public ObservableCollection<BoxViewModel> Boxes
        {
            get
            {
                return this.boxes ?? (this.boxes = new ObservableCollection<BoxViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.boxes = value;
                    NotifyPropertyChanged(nameof(Boxes));
                }
            }
        }

        private int totalNumberOfPdos;
        public int TotalNumberOfPdos
        {
            get { return this.totalNumberOfPdos; }
            set
            {
                this.totalNumberOfPdos = value;
                NotifyPropertyChanged(nameof(TotalNumberOfPdos));
            }
        }
        private int numberOfMapablePdos;
        public int NumberOfMapablePdos
        {
            get { return this.numberOfMapablePdos; }
            set
            {
                this.numberOfMapablePdos = value;
                NotifyPropertyChanged(nameof(NumberOfMapablePdos));
            }
        }

        private ObservableCollection<PdoViewModel> pdos;
        public ObservableCollection<PdoViewModel> Pdos
        {
            get
            {
                return this.pdos ?? (this.pdos = new ObservableCollection<PdoViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.pdos = value;
                    NotifyPropertyChanged(nameof(Pdos));
                }
            }
        }

        private string treeViewDesc;
        public string TreeViewDesc
        {
            get
            {
                this.treeViewDesc = this.Name + @" (Id: " + this.Id + @", Type: " + this.BoxOrderCode + ")";
                return this.treeViewDesc;
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

        private ObservableCollection<MappableObject> mapableObjects;
        public ObservableCollection<MappableObject> MapableObjects
        {
            get
            {
                return this.mapableObjects ?? (this.mapableObjects = new ObservableCollection<MappableObject>());
            }
            set
            {
                if (value != null)
                {
                    this.mapableObjects = value;
                    NotifyPropertyChanged(nameof(MapableObjects));
                }
            }
        }

        private MappableObject mapableObjectGrouped;
        public MappableObject MapableObjectGrouped
        {
            get
            {
                return this.mapableObjectGrouped ?? (this.mapableObjectGrouped = new MappableObject());
            }
            set
            {
                if (value != null)
                {
                    this.mapableObjectGrouped = value;
                    NotifyPropertyChanged(nameof(MapableObjectGrouped));
                }
            }
        }

        private bool wcStateWcState;
        public bool WcStateWcState
        {
            get { return this.wcStateWcState; }
            set
            {
                this.wcStateWcState = value;
                NotifyPropertyChanged(nameof(WcStateWcState));
            }
        }

        private bool wcStateInputToggle;
        public bool WcStateInputToggle
        {
            get { return this.wcStateInputToggle; }
            set
            {
                this.wcStateInputToggle = value;
                NotifyPropertyChanged(nameof(WcStateInputToggle));
            }
        }

        private bool infoDataState;
        public bool InfoDataState
        {
            get { return this.infoDataState; }
            set
            {
                this.infoDataState = value;
                NotifyPropertyChanged(nameof(InfoDataState));
            }
        }

        private bool infoDataAddr;
        public bool InfoDataAddr
        {
            get { return this.infoDataAddr; }
            set
            {
                this.infoDataAddr = value;
                NotifyPropertyChanged(nameof(InfoDataAddr));
            }
        }

        private bool infoDataNetId;
        public bool InfoDataNetId
        {
            get { return this.infoDataNetId; }
            set
            {
                this.infoDataNetId = value;
                NotifyPropertyChanged(nameof(InfoDataNetId));
            }
        }

        private bool infoDataDcTimes;
        public bool InfoDataDcTimes
        {
            get { return this.infoDataDcTimes; }
            set
            {
                this.infoDataDcTimes = value;
                NotifyPropertyChanged(nameof(InfoDataDcTimes));
            }
        }

        private bool infoDataSoeDS401;
        public bool InfoDataSoeDS401
        {
            get { return this.infoDataSoeDS401; }
            set
            {
                this.infoDataSoeDS401 = value;
                NotifyPropertyChanged(nameof(InfoDataSoeDS401));
            }
        }

        private bool infoDataObjectId;
        public bool InfoDataObjectId
        {
            get { return this.infoDataObjectId; }
            set
            {
                this.infoDataObjectId = value;
                NotifyPropertyChanged(nameof(InfoDataObjectId));
            }
        }

        private bool hasFixedPdoStructure;
        public bool HasFixedPdoStructure
        {
            get { return this.hasFixedPdoStructure; }
            set
            {
                this.hasFixedPdoStructure = value;
                NotifyPropertyChanged(nameof(HasFixedPdoStructure));
            }
        }

        private bool syncUnitDefinedOnAtLeastOnePdo;
        public bool SyncUnitDefinedOnAtLeastOnePdo
        {
            get { return this.syncUnitDefinedOnAtLeastOnePdo; }
            set
            {
                this.syncUnitDefinedOnAtLeastOnePdo = value;
                NotifyPropertyChanged(nameof(SyncUnitDefinedOnAtLeastOnePdo));
            }
        }

        private ObservableCollection<SlotViewModel> slots;
        public ObservableCollection<SlotViewModel> Slots
        {
            get
            {
                return this.slots ?? (this.slots = new ObservableCollection<SlotViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.slots = value;
                    NotifyPropertyChanged(nameof(Slots));
                }
            }
        }

        private ObservableCollection<Profile> profiles;
        public ObservableCollection<Profile> Profiles
        {
            get
            {
                return this.profiles ?? (this.profiles = new ObservableCollection<Profile>());
            }
            set
            {
                if (value != null)
                {
                    this.profiles = value;
                    NotifyPropertyChanged(nameof(Profiles));
                }
            }
        }

        private string suName;
        public string SuName
        {
            get { return this.suName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.suName = value;
                    NotifyPropertyChanged(nameof(SuName));
                }
            }
        }

        private string vendorId;
        public string VendorId
        {
            get { return this.vendorId; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.vendorId = value;
                    NotifyPropertyChanged(nameof(VendorId));
                }
            }
        }
    }
}
