namespace iXlinkerDtos
{
    public class BoxDetails : NotifiableBase
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

        private string ioTreePath;
        public string IoTreePath
        {
            get { return this.ioTreePath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.ioTreePath = value;
                    NotifyPropertyChanged(nameof(IoTreePath));
                }
            }
        }

        private bool isIndependentProjectFile;

        public bool IsIndependentProjectFile
        {
            get { return isIndependentProjectFile; }
            set
            {
                isIndependentProjectFile = value;
                NotifyPropertyChanged(nameof(IsIndependentProjectFile));
            }
        }


        private string fileName;
        public string FileName
        {
            get { return this.fileName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.fileName = value;
                    NotifyPropertyChanged(nameof(FileName));
                }
            }
        }

        private bool fileExists;

        public bool FileExists
        {
            get { return fileExists; }
            set
            {
                fileExists = value;
                NotifyPropertyChanged(nameof(FileExists));
            }
        }

        private bool isDisabled;

        public bool IsDisabled
        {
            get { return isDisabled; }
            set
            {
                isDisabled = value;
                NotifyPropertyChanged(nameof(IsDisabled));
            }
        }

    }
}
