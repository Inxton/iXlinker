namespace iXlinkerDtos
{
    public class SlotViewModel : NotifiableBase
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
                this.id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        private string pdoIndex;
        public string PdoIndex
        {

            get { return this.pdoIndex; }
            set
            {
                this.pdoIndex = value;
                NotifyPropertyChanged(nameof(PdoIndex));
            }
        }

    }
}
