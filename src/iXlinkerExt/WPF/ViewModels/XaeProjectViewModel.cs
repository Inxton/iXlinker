using System.Collections.Generic;

namespace iXlinkerExt.WPF.ViewModels
{
    public class XaeProjectViewModel : ProjectItemViewModel
    {
        private string activeTargetConfigurationPlatform;
        public string ActiveTargetConfigurationPlatform
        {
            get { return this.activeTargetConfigurationPlatform; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.activeTargetConfigurationPlatform = value;
                    NotifyPropertyChanged(nameof(ActiveTargetConfigurationPlatform));
                }
            }
        }

        private List<PlcProjectViewModel> plcProjects;
        public List<PlcProjectViewModel> PlcProjects
        {
            get
            {
                return this.plcProjects ?? (this.plcProjects = new List<PlcProjectViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.plcProjects = value;
                    NotifyPropertyChanged(nameof(PlcProjects));
                }
            }
        }
    }
}
