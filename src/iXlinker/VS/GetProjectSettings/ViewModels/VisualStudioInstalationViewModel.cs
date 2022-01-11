namespace iXlinkerDtos
{
    using System;
    using System.Collections.ObjectModel;

    public class VisualStudioInstalationViewModel : PouStructBase
    {

        private Version version;
        public Version Version
        {
            get { return this.version; }
            set
            {
                if (value != null)
                {
                    this.version = value;
                    NotifyPropertyChanged(nameof(Version));
                }
            }
        }

        private string instalationPath;
        public string InstalationPath
        {
            get { return this.instalationPath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.instalationPath = value;
                    NotifyPropertyChanged(nameof(InstalationPath));
                }
            }
        }
        public VisualStudioInstalationViewModel(Version version, string instalationPath)
        {
            Version = version;
            InstalationPath = instalationPath;
        }
    }
}
