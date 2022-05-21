using System;

namespace iXlinkerExt.WPF.ViewModels
{
    public class ProjectItemViewModel : BaseViewModel
    {
        private Guid gUID;
        public Guid GUID
        {
            get { return this.gUID; }
            set
            {
                if (value != null)
                {
                    this.gUID = value;
                    NotifyPropertyChanged(nameof(GUID));
                }
            }
        }

        private string completePathInFileSystem;
        public string CompletePathInFileSystem
        {
            get { return this.completePathInFileSystem; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.completePathInFileSystem = value;
                    NotifyPropertyChanged(nameof(CompletePathInFileSystem));
                }
            }
        }

        private string folderPathInFileSystem;
        public string FolderPathInFileSystem
        {
            get { return this.folderPathInFileSystem; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.folderPathInFileSystem = value;
                    NotifyPropertyChanged(nameof(FolderPathInFileSystem));
                }
            }
        }

        private string fileNameInFileSystem;
        public string FileNameInFileSystem
        {
            get { return this.fileNameInFileSystem; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.fileNameInFileSystem = value;
                    NotifyPropertyChanged(nameof(FileNameInFileSystem));
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

        private string uniqueName;
        public string UniqueName
        {
            get { return this.uniqueName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.uniqueName = value;
                    NotifyPropertyChanged(nameof(UniqueName));
                }
            }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                NotifyPropertyChanged(nameof(IsChecked));
            }
        }

    }
}

