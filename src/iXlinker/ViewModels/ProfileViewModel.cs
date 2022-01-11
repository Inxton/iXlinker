namespace ViewModels
{
    public class ProfileViewModel : BaseViewModel 
    {
        private int profileNo;
        public int ProfileNo
        {
            get { return this.profileNo; }
            set
            {
                this.profileNo = value;
                NotifyPropertyChanged(nameof(ProfileNo));
            }
        }

        private int addInfo;
        public int AddInfo
        {
            get { return this.addInfo; }
            set
            {

                this.addInfo = value;
                NotifyPropertyChanged(nameof(AddInfo));
            }
        }


        private string coeProfileNo;
        public string CoeProfileNo
        {
            get { return this.coeProfileNo; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.coeProfileNo = value;
                    NotifyPropertyChanged(nameof(CoeProfileNo));
                }
            }
        }

        private string displayName;
        public string DisplayName
        {
            get { return this.displayName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.displayName = value;
                    NotifyPropertyChanged(nameof(DisplayName));
                }
            }
        }
    }
}
