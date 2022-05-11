using System.Collections.ObjectModel;

namespace iXlinkerDtos
{
    public class PdoStructViewModel : PouStructBase
    {
        private string extends;
        public string Extends
        {
            get { return this.extends; }
            set
            {
                this.extends = value;
                NotifyPropertyChanged(nameof(Extends));
            }
        }

        private ObservableCollection<PdoStructMemberViewModel> structMembers;
        public ObservableCollection<PdoStructMemberViewModel> StructMembers
        {
            get
            {
                return this.structMembers ?? (this.structMembers = new ObservableCollection<PdoStructMemberViewModel>());
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

        public void AddMemberAndUpdateIdAndSize(PdoStructMemberViewModel member)
        {
            StructMembers.Add(member);
            Id = Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
            Size = Size + member.Size;
            //Id = Id + member.Name + member.InOutPlcProj + member.Type_Value.Substring(member.Type_Value.LastIndexOf(".") + 1) + member.Size;
        }
    }
}
