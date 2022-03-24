namespace iXlinkerDtos
{
    public class PlcLibRepository : NotifiableBase 
    {
        private string repositoryName;
        public string RepositoryName
        {
            get { return this.repositoryName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.repositoryName = value;
                    NotifyPropertyChanged(nameof(RepositoryName));
                }
            }
        }

        private string repositoryPath;
        public string RepositoryPath
        {
            get { return this.repositoryPath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.repositoryPath = value;
                    NotifyPropertyChanged(nameof(RepositoryPath));
                }
            }
        }
    }
}

