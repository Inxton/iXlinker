using System.Diagnostics;

namespace ViewModels
{
    public class DTEViewModel : BaseViewModel
    {

        private EnvDTE80.DTE2 dte2;
        public EnvDTE80.DTE2 Dte2
        {
            get { return this.dte2; }
            set
            {
                if (value != null)
                {
                    this.dte2 = value;
                    NotifyPropertyChanged(nameof(Dte2));
                }
            }
        }

        private int processID;
        public int ProcessID
        {
            get { return this.processID; }
            set
            {
                this.processID = value;
                NotifyPropertyChanged(nameof(ProcessID));
            }
        }
    }
}
