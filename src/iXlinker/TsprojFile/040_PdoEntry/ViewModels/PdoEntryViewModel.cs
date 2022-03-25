namespace iXlinkerDtos
{
    public class PdoEntryViewModel : NotifiableBase
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

        private string index;
        public string Index
        {
            get { return this.index; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.index = value;
                    NotifyPropertyChanged(nameof(Index));
                }
            }
        }

        private string subIndex;
        public string SubIndex
        {
            get { return this.subIndex; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.subIndex = value;
                    NotifyPropertyChanged(nameof(SubIndex));
                }
            }
        }

        private uint indexNumber = 0;
        public uint IndexNumber
        {
            get { return this.indexNumber; }
            set
            {
                this.indexNumber = value;
                NotifyPropertyChanged(nameof(IndexNumber));
            }
        }

        private uint subIndexNumber = 0;
        public uint SubIndexNumber
        {
            get { return this.subIndexNumber; }
            set
            {
                this.subIndexNumber = value;
                NotifyPropertyChanged(nameof(SubIndexNumber));
            }
        }

        private uint sizeInBites;
        public uint SizeInBites
        {
            get { return this.sizeInBites; }
            set
            {
                this.sizeInBites = value;
                NotifyPropertyChanged(nameof(SizeInBites));
            }
        }

        private double sizeInBytes;
        public double SizeInBytes
        {
            get { return this.sizeInBytes; }
            set
            {
                this.sizeInBytes = value;
                NotifyPropertyChanged(nameof(SizeInBytes));
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

        private string type_GUID;
        public string Type_GUID
        {
            get { return this.type_GUID; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.type_GUID = value;
                    NotifyPropertyChanged(nameof(Type_GUID));
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

        private string typeNamespace;
        public string TypeNamespace
        {
            get { return this.typeNamespace; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.typeNamespace = value;
                    NotifyPropertyChanged(nameof(TypeNamespace));
                }
            }
        }

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

        private string boxOrderCode;
        public string BoxOrderCode
        {
            get { return this.boxOrderCode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.boxOrderCode = value;
                    NotifyPropertyChanged(nameof(BoxOrderCode));
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
    }
}
