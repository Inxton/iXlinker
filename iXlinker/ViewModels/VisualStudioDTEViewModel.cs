using System.Diagnostics;

namespace ViewModels
{
    public class VisualStudioDTEViewModel : BaseViewModel
    {

        private EnvDTE80.DTE2 dte2;
        public EnvDTE80.DTE2 Dte2
        {
            get { return this.dte2; }
            set
            {
                if (value != null)
                {
                    this.dte2 = value;
                    NotifyPropertyChanged(nameof(Dte2));
                }
            }
        }

        private int processID;
        public int ProcessID
        {
            get { return this.processID; }
            set
            {
                this.processID = value;
                NotifyPropertyChanged(nameof(ProcessID));
            }
        }

        private string devenv;
        public string Devenv
        {
            get { return this.devenv; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.devenv = value;
                    NotifyPropertyChanged(nameof(Devenv));
                }
            }
        }

        private EnvDteTsProjectViewModel tsProject;
        public EnvDteTsProjectViewModel TsProject
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

        private EnvDtePlcProjectViewModel plcProject;
        public EnvDtePlcProjectViewModel PlcProject
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
