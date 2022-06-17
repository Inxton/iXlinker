namespace iXlinkerExt.WPF.ViewModels
{
    public class PlcProjectViewModel : ProjectItemViewModel
    {
        private bool isIndependent;
        public bool IsIndependent
        {
            get { return this.isIndependent; }
            set
            {
                this.isIndependent = value;
                NotifyPropertyChanged(nameof(IsIndependent));
            }
        }
        private string xtiPathInFileSystem;
        public string XtiPathInFileSystem
        {
            get { return this.xtiPathInFileSystem; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.xtiPathInFileSystem = value;
                    NotifyPropertyChanged(nameof(XtiPathInFileSystem));
                }
            }
        }

    }
}
