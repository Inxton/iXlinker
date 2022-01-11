namespace ViewModels
{
    public class TaskViewModel : BaseViewModel
    {

        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }

        private string id;
        public string Id
        {
            get { return this.id; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.id = value;
                    NotifyPropertyChanged(nameof(Id));
                }
            }
        }

        private int priority;
        public int Priority
        {
            get { return this.priority; }
            set
            {
                this.priority = value;
                NotifyPropertyChanged(nameof(Priority));
            }
        }

        private int cycleTime;
        public int CycleTime
        {
            get { return this.cycleTime; }
            set
            {
                this.cycleTime = value;
                NotifyPropertyChanged(nameof(CycleTime));
            }
        }

        private int amsPort;
        public int AmsPort
        {
            get { return this.amsPort; }
            set
            {
                this.amsPort = value;
                NotifyPropertyChanged(nameof(AmsPort));
            }
        }
    }
}
