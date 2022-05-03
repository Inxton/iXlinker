namespace iXlinkerDtos
{
    public class PlcStruct : NotifiableBase
    {
        private string @namespace;
        public string Namespace
        {
            get { return this.@namespace; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.@namespace = value;
                    NotifyPropertyChanged(nameof(Namespace));
                }
            }
        }

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

        private uint numberOfUses;
        public uint NumberOfUses
        {
            get { return this.numberOfUses; }
            set
            {
                this.numberOfUses = value;
                NotifyPropertyChanged(nameof(NumberOfUses));
            }
        }
    }
}
