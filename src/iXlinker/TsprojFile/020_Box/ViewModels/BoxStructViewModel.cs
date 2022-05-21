namespace iXlinkerDtos
{
    using System.Collections.ObjectModel;

    public class BoxStructViewModel : PouStructBase
    {
        private string extends;
        public string Extends
        {
            get { return this.extends; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.extends = value;
                    NotifyPropertyChanged(nameof(Extends));
                }
            }
        }

        private string portAPhysics;
        public string PortAPhysics
        {
            get { return this.portAPhysics; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.portAPhysics = value;
                    NotifyPropertyChanged(nameof(PortAPhysics));
                }
            }
        }

        private ObservableCollection<BoxStructMemberViewModel> structMembers;
        public ObservableCollection<BoxStructMemberViewModel> StructMembers
        {
            get
            {
                return this.structMembers ?? (this.structMembers = new ObservableCollection<BoxStructMemberViewModel>());
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

        public void AddMemberAndUpdateIdAndSize(BoxStructMemberViewModel member)
        {
            StructMembers.Add(member);
            Id = Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
            Size = Size + member.Size;
            //Id = Id + member.Name + member.InOutPlcProj + member.Type_Value.Substring(member.Type_Value.LastIndexOf(".") + 1) + member.Size;
        }
    }


}
