
using System.Collections.ObjectModel;
using TwincatXmlSchemas.TcSmProject;

namespace iXlinkerDtos
{
    public class TcModel : NotifiableBase
    {
        public const string tmpLevelSeparator = "\x1";
        public const string tmpSlotSeparator = "\x2";
        public const string tmpStructSeparator = "\x3";
        public const string ioLevelSeparator = "^";
        public const string plcStructSeparator = ".";
        public const string hwStructSeparator = "^";
        public const string attributeHide = "{attribute 'hide'}";
        public const string emptyStructure = "EMPTY_STRUCTURE";
        public const string plcLibContainingHwBaseName = "TcoIo";
        public const string etcSlaveBaseStructName = "EtcSlaveTerminalBase";



        private TcSmProject tc;
        public TcSmProject Tc { get => tc; set => tc = value; }

        private string ownerAPlcName;
        public string OwnerAPlcName
        {
            get { return this.ownerAPlcName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.ownerAPlcName = value;
                    NotifyPropertyChanged(nameof(OwnerAPlcName));
                }
            }
        }

        private uint totalNumberOfDevices;
        public uint TotalNumberOfDevices
        {
            get { return this.totalNumberOfDevices; }
            set
            {
                this.totalNumberOfDevices = value;
                NotifyPropertyChanged(nameof(TotalNumberOfDevices));
            }
        }

        private uint totalNumberOfBoxes;
        public uint TotalNumberOfBoxes
        {
            get { return this.totalNumberOfBoxes; }
            set
            {
                this.totalNumberOfBoxes = value;
                NotifyPropertyChanged(nameof(TotalNumberOfBoxes));
            }
        }

        private uint totalNumberOfFixedBoxes;
        public uint TotalNumberOfFixedBoxes
        {
            get { return this.totalNumberOfFixedBoxes; }
            set
            {
                this.totalNumberOfFixedBoxes = value;
                NotifyPropertyChanged(nameof(TotalNumberOfFixedBoxes));
            }
        }

        private uint totalNumberOfVariableBoxes;
        public uint TotalNumberOfVariableBoxes
        {
            get { return this.totalNumberOfVariableBoxes; }
            set
            {
                this.totalNumberOfVariableBoxes = value;
                NotifyPropertyChanged(nameof(TotalNumberOfVariableBoxes));
            }
        }

        private uint totalNumberOfPdos;
        public uint TotalNumberOfPdos
        {
            get { return this.totalNumberOfPdos; }
            set
            {
                this.totalNumberOfPdos = value;
                NotifyPropertyChanged(nameof(TotalNumberOfPdos));
            }
        }

        private uint totalNumberOfPdoEntries;
        public uint TotalNumberOfPdoEntries
        {
            get { return this.totalNumberOfPdoEntries; }
            set
            {
                this.totalNumberOfPdoEntries = value;
                NotifyPropertyChanged(nameof(TotalNumberOfPdoEntries));
            }
        }

        private uint totalNumberOftasks;
        public uint TotalNumberOfTasks
        {
            get { return this.totalNumberOftasks; }
            set
            {
                this.totalNumberOftasks = value;
                NotifyPropertyChanged(nameof(TotalNumberOfTasks));
            }
        }

        private uint totalNumberOfSyncUnits;
        public uint TotalNumberOfSyncUnits
        {
            get { return this.totalNumberOfSyncUnits; }
            set
            {
                this.totalNumberOfSyncUnits = value;
                NotifyPropertyChanged(nameof(TotalNumberOfSyncUnits));
            }
        }

        private string context;
        public string Context
        {

            get { return this.context; }
            set
            {
                this.context = value;
                NotifyPropertyChanged(nameof(Context));
            }
        }

        private static string nameOfTheExportedGVL;
        public static string NameOfTheExportedGVL
        {

            get { return nameOfTheExportedGVL; }
            set
            {
                nameOfTheExportedGVL = value;
            }
        }

        private string exportSubDirName;
        public string ExportSubDirName
        {

            get { return this.exportSubDirName; }
            set
            {
                this.exportSubDirName = value;
                NotifyPropertyChanged(nameof(ExportSubDirName));
            }
        }

        private ObservableCollection<DeviceViewModel> devices;
        public ObservableCollection<DeviceViewModel> Devices
        {
            get
            {
                return this.devices ?? (this.devices = new ObservableCollection<DeviceViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.devices = value;
                    NotifyPropertyChanged(nameof(Devices));
                }
            }
        }

        private ObservableCollection<TaskViewModel> tasks;
        public ObservableCollection<TaskViewModel> Tasks
        {
            get
            {
                return this.tasks ?? (this.tasks = new ObservableCollection<TaskViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.tasks = value;
                    NotifyPropertyChanged(nameof(Tasks));
                }
            }
        }

        private ObservableCollection<PdoEntryStructViewModel> pdoEntryStructures;
        public ObservableCollection<PdoEntryStructViewModel> PdoEntryStructures
        {
            get
            {
                return this.pdoEntryStructures ?? (this.pdoEntryStructures = new ObservableCollection<PdoEntryStructViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.pdoEntryStructures = value;
                    NotifyPropertyChanged(nameof(PdoEntryStructures));
                }
            }
        }

        private ObservableCollection<PdoStructViewModel> pdoStructures;
        public ObservableCollection<PdoStructViewModel> PdoStructures
        {
            get
            {
                return this.pdoStructures ?? (this.pdoStructures = new ObservableCollection<PdoStructViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.pdoStructures = value;
                    NotifyPropertyChanged(nameof(PdoStructures));
                }
            }
        }

        private ObservableCollection<BoxStructViewModel> boxStructures;
        public ObservableCollection<BoxStructViewModel> BoxStructures
        {
            get
            {
                return this.boxStructures ?? (this.boxStructures = new ObservableCollection<BoxStructViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.boxStructures = value;
                    NotifyPropertyChanged(nameof(BoxStructures));
                }
            }
        }

        private ObservableCollection<DeviceStructViewModel> deviceStructures;
        public ObservableCollection<DeviceStructViewModel> DeviceStructures
        {
            get
            {
                return this.deviceStructures ?? (this.deviceStructures = new ObservableCollection<DeviceStructViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.deviceStructures = value;
                    NotifyPropertyChanged(nameof(DeviceStructures));
                }
            }
        }

        private ObservableCollection<TopologyStructViewModel> topologyStructures;
        public ObservableCollection<TopologyStructViewModel> TopologyStructures
        {
            get
            {
                return this.topologyStructures ?? (this.topologyStructures = new ObservableCollection<TopologyStructViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.topologyStructures = value;
                    NotifyPropertyChanged(nameof(TopologyStructures));
                }
            }
        }

        private ObservableCollection<PlcLibrary> plcLibraries;
        public ObservableCollection<PlcLibrary> PlcLibraries
        {
            get
            {
                return this.plcLibraries ?? (this.plcLibraries = new ObservableCollection<PlcLibrary>());
            }
            set
            {
                if (value != null)
                {
                    this.plcLibraries = value;
                    NotifyPropertyChanged(nameof(PlcLibraries));
                }
            }
        }

        private ObservableCollection<PlcStruct> plcStructuresInPlcLibraries;
        public ObservableCollection<PlcStruct> PlcStructuresInPlcLibraries
        {
            get
            {
                return this.plcStructuresInPlcLibraries ?? (this.plcStructuresInPlcLibraries = new ObservableCollection<PlcStruct>());
            }
            set
            {
                if (value != null)
                {
                    this.plcStructuresInPlcLibraries = value;
                    NotifyPropertyChanged(nameof(PlcStructuresInPlcLibraries));
                }
            }
        }

        private ObservableCollection<BoxIdentification> boxIdentificationList;
        public ObservableCollection<BoxIdentification> BoxIdentificationList
        {
            get
            {
                return this.boxIdentificationList ?? (this.boxIdentificationList = new ObservableCollection<BoxIdentification>());
            }
            set
            {
                if (value != null)
                {
                    this.boxIdentificationList = value;
                    NotifyPropertyChanged(nameof(BoxIdentificationList));
                }
            }
        }

    }
}
