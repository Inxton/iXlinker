namespace iXlinkerDtos
{
    using System.Collections.ObjectModel;

    public class PdoEntryStructViewModel : PouStructBase
    {
        private string pdoEntryVarA;
        public string PdoEntryVarA
        {
            get { return this.pdoEntryVarA; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.pdoEntryVarA = value;
                    NotifyPropertyChanged(nameof(PdoEntryVarA));
                }
            }
        }

        private string pdoEntryVarB;
        public string PdoEntryVarB
        {
            get { return this.pdoEntryVarB; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.pdoEntryVarB = value;
                    NotifyPropertyChanged(nameof(PdoEntryVarB));
                }
            }
        }

        private ObservableCollection<PdoEntryStructMemberViewModel> structMembers;
        public ObservableCollection<PdoEntryStructMemberViewModel> StructMembers
        {
            get
            {
                return this.structMembers ?? (this.structMembers = new ObservableCollection<PdoEntryStructMemberViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.structMembers = value;
                    NotifyPropertyChanged(nameof(StructMembers));
                }
            }
        }

    }
}
