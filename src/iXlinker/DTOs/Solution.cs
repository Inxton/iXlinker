using System.Collections.ObjectModel;
using System.Diagnostics;

namespace iXlinkerDtos
{
    public class Solution : NotifiableBase
    {
        private string devenvPath;
        public string DevenvPath
        {
            get { return this.devenvPath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.devenvPath = value;
                    NotifyPropertyChanged(nameof(DevenvPath));
                }
            }
        }

        private TsProject tsProject;
        public TsProject TsProject
        {
            get { return this.tsProject; }
            set
            {
                if (value != null)
                {
                    this.tsProject = value;
                    NotifyPropertyChanged(nameof(TsProject));
                }
            }
        }

        private ObservableCollection<IoDevice> independentIoDevices;
        public ObservableCollection<IoDevice> IndependentIoDevices
        {
            get
            {
                return this.independentIoDevices ?? (this.independentIoDevices = new ObservableCollection<IoDevice>());
            }
            set
            {
                if (value != null)
                {
                    this.independentIoDevices = value;
                    NotifyPropertyChanged(nameof(IndependentIoDevices));
                }
            }
        }

        private PlcProject plcProject;
        public PlcProject PlcProject
        {
            get { return this.plcProject; }
            set
            {
                if (value != null)
                {
                    this.plcProject = value;
                    NotifyPropertyChanged(nameof(PlcProject));
                }
            }
        }

        private string activeTargetPlatform;
        public string ActiveTargetPlatform
        {
            get { return this.activeTargetPlatform; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.activeTargetPlatform = value;
                    NotifyPropertyChanged(nameof(ActiveTargetPlatform));
                }
            }
        }

        private ushort maxEthercatFrameIndex;
        public ushort MaxEthercatFrameIndex
        {
            get
            {
                return this.maxEthercatFrameIndex;
            }
            set
            {
                if (value != null)
                {
                    this.maxEthercatFrameIndex = value;
                    NotifyPropertyChanged(nameof(MaxEthercatFrameIndex));
                }
            }
        }

        private bool doNotGenerateDisabled;
        public bool DoNotGenerateDisabled
        {
            get
            {
                return this.doNotGenerateDisabled;
            }
            set
            {
                if (value != null)
                {
                    this.doNotGenerateDisabled = value;
                    NotifyPropertyChanged(nameof(DoNotGenerateDisabled));
                }
            }
        }

        private ProjectItem gvlExported;
        public ProjectItem GvlExported
        {
            get { return this.gvlExported; }
            set
            {
                this.gvlExported = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(GvlExported));
            }
        }

        private ProjectItem dutsIo;
        public ProjectItem DutsIo
        {
            get { return this.dutsIo; }
            set
            {
                this.dutsIo = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIo));
            }
        }

        private ProjectItem dutsIoPdoEntry;
        public ProjectItem DutsIoPdoEntry
        {
            get { return this.dutsIoPdoEntry; }
            set
            {
                this.dutsIoPdoEntry = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIoPdoEntry));
            }
        }

        private ProjectItem dutsIoPdo;
        public ProjectItem DutsIoPdo
        {
            get { return this.dutsIoPdo; }
            set
            {
                this.dutsIoPdo = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIoPdo));
            }
        }

        private ProjectItem dutsIoBox;
        public ProjectItem DutsIoBox
        {
            get { return this.dutsIoBox; }
            set
            {
                this.dutsIoBox = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIoBox));
            }
        }

        private ProjectItem dutsIoDevice;
        public ProjectItem DutsIoDevice
        {
            get { return this.dutsIoDevice; }
            set
            {
                this.dutsIoDevice = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIoDevice));
            }
        }

        private ProjectItem dutsIoTopology;
        public ProjectItem DutsIoTopology
        {
            get { return this.dutsIoTopology; }
            set
            {
                this.dutsIoTopology = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIoTopology));
            }
        }

        private ProjectItem dutsIoBase;
        public ProjectItem DutsIoBase
        {
            get { return this.dutsIoBase; }
            set
            {
                this.dutsIoBase = value ?? new ProjectItem();
                NotifyPropertyChanged(nameof(DutsIoBase));
            }
        }

        private ObservableCollection<PlcLibRepository> plcLibRepositories;
        public ObservableCollection<PlcLibRepository> PlcLibRepositories
        {
            get
            {
                return this.plcLibRepositories ?? (this.plcLibRepositories = new ObservableCollection<PlcLibRepository>());
            }
            set
            {
                if (value != null)
                {
                    this.plcLibRepositories = value;
                    NotifyPropertyChanged(nameof(PlcLibRepositories));
                }
            }
        }



    }
}
