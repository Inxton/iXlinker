using System;
using System.IO;
using iXlinker.Resources;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;

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
                    ret = specialPlcType.ReplacementTypeNamespace + "." + specialPlcType.ReplacementType;
                    break;
                }
            }
            return ret;
        }
    }
}
