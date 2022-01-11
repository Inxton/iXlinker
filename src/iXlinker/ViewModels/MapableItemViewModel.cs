namespace ViewModels
{
    public class MapableItemViewModel : BaseViewModel
    {

        private string ownerBname;
        public string OwnerBname
        {
            get { return this.ownerBname; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.ownerBname = value;
                    NotifyPropertyChanged(nameof(OwnerBname));
                }
            }
        }

        private string varA;
        public string VarA
        {
            get { return this.varA; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.varA = value;
                    NotifyPropertyChanged(nameof(VarA));
                }
            }
        }

        private string varB;
        public string VarB
        {
            get { return this.varB; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.varB = value;
                    NotifyPropertyChanged(nameof(VarB));
                }
            }
        }

        private string varA_PLC;
        public string VarAPLC
        {
            get { return this.varA_PLC; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.varA_PLC = value;
                    NotifyPropertyChanged(nameof(VarAPLC));
                }
            }
        }

        private string inOut;
        public string InOut
        {

            get { return this.inOut; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.inOut = value;
                    NotifyPropertyChanged(nameof(InOut));
                }
            }
        }

        private string inOutPlcProj = "";
        public string InOutPlcProj
        {
            get { return this.inOutPlcProj; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.inOutPlcProj = value;
                    NotifyPropertyChanged(nameof(InOutPlcProj));
                }
            }
        }

        private string inOutMappings = "";
        public string InOutMappings
        {
            get { return this.inOutMappings; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.inOutMappings = value;
                    NotifyPropertyChanged(nameof(InOutMappings));
                }
            }
        }

        private string type_Value;
        public string Type_Value
        {
            get { return this.type_Value; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.type_Value = value;
                    NotifyPropertyChanged(nameof(Type_Value));
                }
            }
        }
    }
}
