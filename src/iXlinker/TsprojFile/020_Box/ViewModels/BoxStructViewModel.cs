namespace iXlinkerDtos
{
    using System.Collections.ObjectModel;

    public class BoxStructViewModel : PouStructBase
    {

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

    }
}
