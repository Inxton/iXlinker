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
                case "BOOL":
                    return 8;                    
                case "BYTE":
                    return 8;                    
                case "SINT":
                    return 8;                    
                case "USINT":
                    return 8;                    
                case "WORD":
                    return 16;                    
                case "INT":
                    return 16;                    
                case "UINT":
                    return 16;                    
                case "DWORD":
                    return 32;                    
                case "DINT":
                    return 32;                    
                case "UDINT":
                    return 32;                    
                case "REAL":
                    return 32;                    
                case "LWORD":
                    return 64;                    
                case "LINT":
                    return 64;                    
                case "ULINT":
                    return 64;                    
                case "LREAL":
                    return 64;                    
                case "AMSNETID":
                    return 48;                    
                case "AMSADDR":
                    return 64;                    
                case "OTCID":
                    return 32;                    
                default:
                    return 0;                    

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
