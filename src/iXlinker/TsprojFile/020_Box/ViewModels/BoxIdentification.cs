namespace iXlinkerDtos
{
    public class BoxIdentification : NotifiableBase
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

        private int id = 0;
        public int Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }
    }
}
