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


    }
}
