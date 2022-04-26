using System;

namespace Utils
{
    public static class PlcBaseTypes
    {
         public static double GetSize(string type)
        {
            switch (type)
            {
                case "BIT":
                    return 0.125;
                case "BOOL":
                    return 1;
                case "BYTE":
                    return 1;
                case "SINT":
                    return 1;
                case "USINT":
                    return 1;
                case "WORD":
                    return 2;
                case "INT":
                    return 2;
                case "UINT":
                    return 2;
                case "DWORD":
                    return 4;
                case "DINT":
                    return 4;
                case "UDINT":
                    return 4;
                case "REAL":
                    return 4;
                case "LWORD":
                    return 8;
                case "LINT":
                    return 8;
                case "ULINT":
                    return 8;
                case "LREAL":
                    return 8;
                case "AMSNETID":
                    return 6;
                case "AMSADDR":
                    return 8;
                case "OTCID":
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
