namespace ViewModels
{
    public class ProjectItemViewModel : BaseViewModel
    {

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

        private string completeName;
        public string CompleteName
        {
            get { return this.completeName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.completeName = value;
                    NotifyPropertyChanged(nameof(CompleteName));
                }
            }
        }

        private string path;
        public string Path
        {
            get { return this.path; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.path = value;
                    NotifyPropertyChanged(nameof(Path));
                }
            }
        }
  
    }
}
