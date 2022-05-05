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

        public void AddMemberAndUpdateIdAndSize(PdoEntryStructMemberViewModel member)
        {
            StructMembers.Add(member);
            Id = Id + member.NameA + member.InOutPlcProj + member.Type_Value + member.Size;
            Size = Size + member.Size;
            //Id = Id + member.NameA + member.InOutPlcProj + member.Type_Value.Substring(member.Type_Value.LastIndexOf(".") + 1) + member.Size;
        }
    }
}
