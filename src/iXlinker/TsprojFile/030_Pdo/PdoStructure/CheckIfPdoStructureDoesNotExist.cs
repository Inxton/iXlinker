using ViewModels;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfPdoStructureDoesNotExist(PdoStructViewModel actPdoStruct)
        {
            bool ret = true;
            foreach(PdoStructViewModel structVM in PdoStructures)
            {
                if (actPdoStruct.Name.Equals(structVM.Name) && actPdoStruct.Crc32.Equals(structVM.Crc32) && actPdoStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    break;
                }

            }
            return ret;
        }
    }
}
