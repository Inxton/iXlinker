using System.Diagnostics;

namespace ViewModels
{
    public class SolutionViewModel : BaseViewModel
    {
        private ProjectItemViewModel sln;
        public ProjectItemViewModel Sln
        {
            get { return this.sln; }
            set
            {
                this.sln = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(Sln));
            }
        }

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

        private TsProjectViewModel tsProject;
        public TsProjectViewModel TsProject
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

        private PlcProjectViewModel plcProject;
        public PlcProjectViewModel PlcProject
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

        private ProjectItemViewModel gvlExported;
        public ProjectItemViewModel GvlExported
        {
            get { return this.gvlExported; }
            set
            {
                this.gvlExported = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(GvlExported));
            }
        }

        private ProjectItemViewModel dutsIo;
        public ProjectItemViewModel DutsIo
        {
            get { return this.dutsIo; }
            set
            {
                this.dutsIo = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(DutsIo));
            }
        }

        private ProjectItemViewModel dutsIoPdoEntry;
        public ProjectItemViewModel DutsIoPdoEntry
        {
            get { return this.dutsIoPdoEntry; }
            set
            {
                this.dutsIoPdoEntry = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(DutsIoPdoEntry));
            }
        }

        private ProjectItemViewModel dutsIoPdo;
        public ProjectItemViewModel DutsIoPdo
        {
            get { return this.dutsIoPdo; }
            set
            {
                this.dutsIoPdo = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(DutsIoPdo));
            }
        }

        private ProjectItemViewModel dutsIoBox;
        public ProjectItemViewModel DutsIoBox
        {
            get { return this.dutsIoBox; }
            set
            {
                this.dutsIoBox = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(DutsIoBox));
            }
        }

        private ProjectItemViewModel dutsIoDevice;
        public ProjectItemViewModel DutsIoDevice
        {
            get { return this.dutsIoDevice; }
            set
            {
                this.dutsIoDevice = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(DutsIoDevice));
            }
        }

        private ProjectItemViewModel dutsIoTopology;
        public ProjectItemViewModel DutsIoTopology
        {
            get { return this.dutsIoTopology; }
            set
            {
                this.dutsIoTopology = value ?? new ProjectItemViewModel();
                NotifyPropertyChanged(nameof(DutsIoTopology));
            }
        }
    }
}
