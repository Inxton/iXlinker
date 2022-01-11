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

    }
}
