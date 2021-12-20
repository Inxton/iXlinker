using System.Collections.ObjectModel;

namespace ViewModels
{
    public class PdoStructViewModel : BaseStructViewModel
    {

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

    }
}
