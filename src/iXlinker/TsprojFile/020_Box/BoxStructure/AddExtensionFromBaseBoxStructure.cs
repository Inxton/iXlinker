using iXlinker.Resources;
using iXlinkerDtos;
using Utils;
using PlcprojFile;
using System;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void AddExtensionFromBaseBoxStructure(BoxStructViewModel actBoxStruct)
        {
            foreach (StructureBase structureBase in StructureBasesResourceDictionary)
            {
                if (TypeEqual(actBoxStruct, structureBase) && NameFilterFit(actBoxStruct, structureBase) && PropertyFilterFit(actBoxStruct, structureBase))
                {
                    actBoxStruct.Extends = ValidatePlcItem.NameIncludingNamespace(structureBase.BaseStructureNamespace, structureBase.BaseStructureName);
                    break;
                }
            }
        }

        private bool TypeEqual(BoxStructViewModel actBoxStruct, StructureBase structureBase)
        {
            return structureBase.StructureType.Equals(actBoxStruct.GetType().ToString());
        }

        private bool NameFilterFit(BoxStructViewModel actBoxStruct, StructureBase structureBase)
        {
            bool ret = false;
            if (structureBase.StructureName.Equals(actBoxStruct.Name) || structureBase.StructureName.Equals("*"))
            {
                ret = true;
            }
            else if(structureBase.StructureName.Contains("*"))
            {
                int baseLength = structureBase.StructureName.LastIndexOf("*");
                string baseName = structureBase.StructureName.Substring(0, baseLength);
                string actName = actBoxStruct.Name;
                if(actName.Length >= baseLength)
                {
                    actName = actBoxStruct.Name.Substring(0, baseLength);
                }
                ret = baseName == actName;
            }
            return ret;
        }

        private bool PropertyFilterFit(BoxStructViewModel actBoxStruct, StructureBase structureBase)
        {
            bool ret = false;
            string PortAPhysicsPropperty = nameof(BoxViewModel.Physics);

            if (structureBase.PropertyFilter.Equals("*"))
            {
                ret = true;
            }
            else
            {
                string propertyName = structureBase.PropertyFilter.Split("=", StringSplitOptions.RemoveEmptyEntries)[0];
                string propertyValue = structureBase.PropertyFilter.Split("=", StringSplitOptions.RemoveEmptyEntries)[1];

                if (propertyName.Equals(PortAPhysicsPropperty))
                {
                    if (propertyValue.Equals(actBoxStruct.Physics))
                    {
                        ret = true;
                    }
                    else if(propertyValue.Contains("*"))
                    {
                        int length = propertyValue.LastIndexOf("*");
                        string baseValue = propertyValue.Substring(0, length);
                        string actValue = actBoxStruct.Physics;
                        if (actValue.Length >= length)
                        {
                            actValue = actValue.Substring(0, length);
                        }
                        ret = baseValue == actValue;
                    }
                }
            }
            return ret;
        }
    }
}

