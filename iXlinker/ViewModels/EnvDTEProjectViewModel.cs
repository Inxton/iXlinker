namespace ViewModels
{
    public class EnvDTEProjectViewModel : BaseViewModel
    {

        private ProjectItemViewModel details;
        public ProjectItemViewModel Details
        {
            get { return this.details; }
            set
            {
                if (value != null)
                {
                    this.details = value;
                    NotifyPropertyChanged(nameof(Details));
                }
            }
        }


        private EnvDTE.Project project;
        public EnvDTE.Project Project
        {
            get { return this.project; }
            set
            {
                if (value != null)
                {
                    this.project = value;
                    NotifyPropertyChanged(nameof(Project));
                }
            }
        }

 
    }
}
