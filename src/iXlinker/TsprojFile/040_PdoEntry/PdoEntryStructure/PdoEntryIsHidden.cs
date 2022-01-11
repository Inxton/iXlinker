using iXlinkerDtos;
using System.Collections.Generic;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool PdoEntryIsHidden(List<string> pdoEntryAttributes)
        {
            bool isHidden = false;
            foreach(string attribute in pdoEntryAttributes)
            {
                if (attribute.Contains(attributeHide))
                {
                    isHidden = true;
                    break;
                }
            }
            return isHidden;
        }
    }
}
