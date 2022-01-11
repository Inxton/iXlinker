using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfBoxStructureDoesNotExist(BoxStructViewModel actBoxStruct)
        {
            bool ret = true;
            foreach(BoxStructViewModel structVM in BoxStructures)
            {
                if (actBoxStruct.Name.Equals(structVM.Name) && actBoxStruct.Crc32.Equals(structVM.Crc32) && actBoxStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
    }
}
