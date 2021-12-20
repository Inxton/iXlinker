using System;

namespace Utils
{
    public static class PlcBaseTypes
    {
        public static UInt32 GetSizeInBites(string type)
        {
            switch (type)
            {
                case "BIT":
                    return 1;
                    break;
                case "BOOL":
                    return 8;
                    break;
                case "BYTE":
                    return 8;
                    break;
                case "SINT":
                    return 8;
                    break;
                case "USINT":
                    return 8;
                    break;
                case "WORD":
                    return 16;
                    break;
                case "INT":
                    return 16;
                    break;
                case "UINT":
                    return 16;
                    break;
                case "DWORD":
                    return 32;
                    break;
                case "DINT":
                    return 32;
                    break;
                case "UDINT":
                    return 32;
                    break;
                case "REAL":
                    return 32;
                    break;
                case "LWORD":
                    return 64;
                    break;
                case "LINT":
                    return 64;
                    break;
                case "ULINT":
                    return 64;
                    break;
                case "LREAL":
                    return 64;
                    break;
                case "AMSNETID":
                    return 48;
                    break;
                case "AMSADDR":
                    return 64;
                    break;
                case "OTCID":
                    return 32;
                    break;
                default:
                    return 0;
                    break;

            }
        }

        public static double GetSizeInBytes(string type)
        {
            switch (type)
            {
                case "BIT":
                    return 0.125;
                    break;
                case "BOOL":
                    return 1;
                    break;
                case "BYTE":
                    return 1;
                    break;
                case "SINT":
                    return 1;
                    break;
                case "USINT":
                    return 1;
                    break;
                case "WORD":
                    return 2;
                    break;
                case "INT":
                    return 2;
                    break;
                case "UINT":
                    return 2;
                    break;
                case "DWORD":
                    return 4;
                    break;
                case "DINT":
                    return 4;
                    break;
                case "UDINT":
                    return 4;
                    break;
                case "REAL":
                    return 4;
                    break;
                case "LWORD":
                    return 8;
                    break;
                case "LINT":
                    return 8;
                    break;
                case "ULINT":
                    return 8;
                    break;
                case "LREAL":
                    return 8;
                    break;
                case "AMSNETID":
                    return 6;
                    break;
                case "AMSADDR":
                    return 8;
                    break;
                case "OTCID":
                    return 4;
                    break;
                default:
                    return 0;
                    break;

            }
        }
    }
}
