using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private string GetBoxNameFromId(int id)
        {
            string name = "";
            foreach (BoxIdentification box in BoxIdentificationList)
            {
                if (box.Id.Equals(id))
                {
                    name = box.Name;
                    break;
                }
            }
            return name;
        }
    }
}
