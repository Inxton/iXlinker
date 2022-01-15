using iXlinkerDtos;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckIfTopologyStructureDoesNotExist(TopologyStructViewModel actDevStruct)
        {
            bool ret = true;
            foreach(TopologyStructViewModel structVM in TopologyStructures)
            {
                if (actDevStruct.Name.Equals(structVM.Name) && actDevStruct.Crc32.Equals(structVM.Crc32) && actDevStruct.Id.Equals(structVM.Id))
                {
                    ret = false;
                    break;
                }

            }
            return ret;
        }
    }
}
