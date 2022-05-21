using iXlinker.Resources;
using iXlinkerDtos;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private string ReplaceSpecialPlcTypeIfFoundInLibrary(string originalType)
        {
            string ret = originalType;

            foreach(SpecialPlcType specialPlcType in SpecialPlcTypes)
            {
                if (originalType.Equals(specialPlcType.OriginalType))
                {
                    if (!string.IsNullOrEmpty(specialPlcType.ReplacementTypeNamespace))
                    {
                        ret = ValidatePlcItem.NameIncludingNamespace(specialPlcType.ReplacementTypeNamespace, specialPlcType.ReplacementType);
                        break;
                    }
                }
            }
            return ret;
        }
    }
}
