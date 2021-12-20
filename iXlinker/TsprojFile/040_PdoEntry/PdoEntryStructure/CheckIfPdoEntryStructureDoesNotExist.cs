using ViewModels;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfPdoEntryStructureDoesNotExist(PdoEntryStructViewModel actPdoEntryStruct)
        {
            bool ret = true;
            foreach(PdoEntryStructViewModel structVM in PdoEntryStructures)
            {
                if (actPdoEntryStruct.Name.Equals(structVM.Name) && actPdoEntryStruct.Crc32.Equals(structVM.Crc32) && actPdoEntryStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    break;
                }

            }
            return ret;
        }
    }
}
