namespace iXlinkerDtos
{
    using System.Collections.ObjectModel;

    public class DeviceStructViewModel : PouStructBase
    {

        private ObservableCollection<DeviceStructMemberViewModel> structMembers;
        public ObservableCollection<DeviceStructMemberViewModel> StructMembers
        {
            get
            {
                return this.structMembers ?? (this.structMembers = new ObservableCollection<DeviceStructMemberViewModel>());
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

        public void AddMemberAndUpdateIdAndSize(DeviceStructMemberViewModel member)
        {
            StructMembers.Add(member);
            Id = Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
            Size = Size + member.Size;
            //Id = Id + member.Name + member.InOutPlcProj + member.Type_Value.Substring(member.Type_Value.LastIndexOf(".") + 1) + member.Size;
        }
    }
}
