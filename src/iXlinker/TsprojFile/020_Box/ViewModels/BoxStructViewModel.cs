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

        private string physics;
        public string Physics
        {
            get { return this.physics; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.physics = value;
                    NotifyPropertyChanged(nameof(Physics));
                }
            }
        }

        private string description;
        public string Description
        {
            get { return this.description; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        private string nameOrigin;
        public string NameOrigin
        {
            get { return this.nameOrigin; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.nameOrigin = value;
                    NotifyPropertyChanged(nameof(NameOrigin));
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
