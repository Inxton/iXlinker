namespace ViewModels
{
    using System.Collections.ObjectModel;

    public class BoxStructViewModel : BaseStructViewModel
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
