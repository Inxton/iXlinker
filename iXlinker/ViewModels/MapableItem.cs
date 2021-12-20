namespace ViewModels
{
    public class MapableItem : BaseViewModel
    {
        private string varAprefix;
        public string VarAprefix
        {
            get { return this.varAprefix; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.varAprefix = value;
                    NotifyPropertyChanged(nameof(VarAprefix));
                }
            }
        }

        private string ownerBname;
        public string OwnerBname
        {
            get { return this.ownerBname; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.ownerBname = value;
                    NotifyPropertyChanged(nameof(OwnerBname));
                }
            }
        }

        private string varA;
        public string VarA
        {
            get { return this.varA; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.varA = value;
                    NotifyPropertyChanged(nameof(VarA));
                }
            }
        }

        private string varB;
        public string VarB
        {
            get { return this.varB; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.varB = value;
                    NotifyPropertyChanged(nameof(VarB));
                }
            }
        }
    }
}
