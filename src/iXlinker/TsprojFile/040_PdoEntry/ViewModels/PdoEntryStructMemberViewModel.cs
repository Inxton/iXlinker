using System.Collections.Generic;

namespace iXlinkerDtos
{
    public class PdoEntryStructMemberViewModel : NotifiableBase
    {
        private List<string> attributes;
        public List<string> Attributes
        {
            get { return this.attributes ?? (this.attributes = new List<string>()); }
            set
            {
                if (value != null)
                {
                    this.attributes = value;
                    NotifyPropertyChanged(nameof(Attributes));
                }
            }
        }

        private string nameA;
        public string NameA
        {
            get { return this.nameA; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.nameA = value;
                    NotifyPropertyChanged(nameof(NameA));
                }
            }
        }

        private string nameB;
        public string NameB
        {
            get { return this.nameB; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.nameB = value;
                    NotifyPropertyChanged(nameof(NameB));
                }
            }
        }

        private string type_value;
        public string Type_Value
        {
            get { return this.type_value; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.type_value = value;
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

        private double size;
        public double Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                NotifyPropertyChanged(nameof(Size));
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

        private string inOut = "";
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

    }
}
