namespace iXlinkerDtos
{
    public class PlcStruct : NotifiableBase
    {
        private string nameSpace;
        public string NameSpace
        {
            get { return this.nameSpace; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.nameSpace = value;
                    NotifyPropertyChanged(nameof(NameSpace));
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
    }
}
