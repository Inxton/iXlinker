namespace iXlinkerDtos
{
    //public class PlcProject : ProjectItem
    public class PlcProject 
    {
        public ProjectItem Plcproj;
        public bool IsIndependent;
        public ProjectItem Xti;

        public PlcProject()
        {
            Plcproj = new ProjectItem();
            IsIndependent = false;
            Xti = new ProjectItem();
        }
    }
}
