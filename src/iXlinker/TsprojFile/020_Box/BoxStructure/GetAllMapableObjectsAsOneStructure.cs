using iXlinkerDtos;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using PlcprojFile;
using System;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private MappableObject GetAllMapableObjectsAsOneStructure(DeviceViewModel deviceViewModel, ObservableCollection<MappableObject> mapableObjects)
        {
            {
                MappableObject mapableObjectVM = new MappableObject();

                if (mapableObjects.Count == 1)
                {
                    MappableObject mapableObject = mapableObjects.ElementAt(0);

                    foreach (MappableItem mapableItem in mapableObject.MapableItems)
                    {
                        MappableItem item = new MappableItem();
                        item = mapableItem;
                        string varAprefix = item.VarAprefix;
                        varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator , StringComparison.Ordinal));
                        string varA = item.VarAprefix;
                        varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal) + 1)) + tmpLevelSeparator + item.VarA;
                        item.VarAprefix = varAprefix;
                        item.VarA = varA;
                        mapableObjectVM.MapableItems.Add(item);
                    }
                    mapableObjectVM.Name = ValidatePlcItem.Name(mapableObject.Name);
                    mapableObjectVM.Type_Value = mapableObject.Type_Value;
                    mapableObjectVM.TypeNamespace= mapableObject.TypeNamespace;
                    mapableObjectVM.Size = mapableObject.Size;
                }
                else if (mapableObjects.Count >= 1)
                {
                    TopologyStructViewModel actTopologyStruct = new TopologyStructViewModel() { Prefix = ValidatePlcItem.Name(deviceViewModel.Name), Id = "", BoxOrderCode = deviceViewModel.Type.ToString()};

                    foreach (MappableObject mapableObject in mapableObjects)
                    {
                        TopologyStructMemberViewModel member = new TopologyStructMemberViewModel();

                        member.Attributes.Add("{attribute addProperty Name \"" + mapableObject.Name + "\"}");
                        if (!string.IsNullOrEmpty(mapableObject.PreviousPort))
                        {
                            member.Attributes.Add("{attribute addProperty PreviousPort \"" + mapableObject.PreviousPort + "\"}");
                        }
                        member.Name = ValidatePlcItem.Name(mapableObject.Name);
                        member.Type_Value = mapableObject.Type_Value;
                        member.TypeNamespace= mapableObject.TypeNamespace;
                        member.Size = mapableObject.Size;
                        actTopologyStruct.StructMembers.Add(member);
                        actTopologyStruct.Id = actTopologyStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
                        actTopologyStruct.Size = actTopologyStruct.Size + member.Size;

                        foreach (MappableItem mapableItem in mapableObject.MapableItems)
                        {
                            MappableItem item = new MappableItem();
                            item = mapableItem;
                            string varAprefix = item.VarAprefix;
                            varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));
                            string varA = item.VarAprefix;
                            varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal) + 1)) + tmpLevelSeparator + item.VarA;
                            item.VarAprefix = varAprefix;
                            item.VarA = varA;
                            mapableObjectVM.MapableItems.Add(item);
                        }
                    }

                    ValidateTopologyStructMemberNamesUniqueness(ref actTopologyStruct);

                    if (actTopologyStruct.StructMembers.Count > 0)
                    {

                        //Calculate CRC of the actPdoStruct.Id
                        actTopologyStruct.Crc32 = CRC32.Calculate_CRC32(actTopologyStruct.Id);
                        actTopologyStruct.Name = ValidatePlcItem.Name(actTopologyStruct.Prefix + "_" + actTopologyStruct.Crc32.ToString("X8"));
                        //Check if such an structure exists
                        if (CheckIfTopologyStructureDoesNotExist(actTopologyStruct))
                        {
                            //if not add to the structure list
                            TopologyStructures.Add(actTopologyStruct);
                        }

                        mapableObjectVM.Name = ValidatePlcItem.Name(deviceViewModel.Name);
                        mapableObjectVM.Type_Value = ValidatePlcItem.Type(actTopologyStruct.Name);
                        mapableObjectVM.TypeNamespace = actTopologyStruct.TypeNamespace;
                        mapableObjectVM.Size = actTopologyStruct.Size;

                    }
                    else
                    {
                        mapableObjectVM = null;
                    }
                }
                return mapableObjectVM;
            }
        }
        private MappableObject GetAllMapableObjectsAsOneStructure(BoxViewModel boxViewModel, ObservableCollection<MappableObject> mapableObjects)
        {
            MappableObject mapableObjectVM = new MappableObject();

            if (mapableObjects.Count == 1)
            {
                MappableObject mapableObject = mapableObjects.ElementAt(0);

                foreach (MappableItem mapableItem in mapableObject.MapableItems)
                {
                    MappableItem item = new MappableItem();
                    item = mapableItem;
                    string varAprefix = item.VarAprefix;
                    varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));
                    string varA = item.VarAprefix;
                    varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal) + 1)) + tmpLevelSeparator + item.VarA;
                    item.VarAprefix = varAprefix;
                    item.VarA = varA;
                    mapableObjectVM.MapableItems.Add(item);
                }
                mapableObjectVM.Name = ValidatePlcItem.Name(mapableObject.Name);
                mapableObjectVM.Type_Value = ValidatePlcItem.Type(mapableObject.Type_Value);
                mapableObjectVM.TypeNamespace= mapableObject.TypeNamespace;
                mapableObjectVM.Size = mapableObject.Size;
            }
            else if (mapableObjects.Count >= 1)
            {
                TopologyStructViewModel actTopologyStruct = new TopologyStructViewModel() { Prefix = ValidatePlcItem.Name(boxViewModel.Name), Id = "", BoxOrderCode = boxViewModel.BoxOrderCode};

                foreach (MappableObject mapableObject in mapableObjects)
                {
                    TopologyStructMemberViewModel member = new TopologyStructMemberViewModel();
                    if (ValidatePlcItem.Name(boxViewModel.Name).Equals(mapableObject.Name) && mapableObjects.IndexOf(mapableObject) == 0)
                    {
                        actTopologyStruct.Extends = ValidatePlcItem.NameIncludingNamespace(mapableObject.TypeNamespace, mapableObject.Type_Value);
                    }
                    else
                    {
                        member.Attributes.Add("{attribute addProperty Name \"" + mapableObject.Name + "\"}");
                        if (!string.IsNullOrEmpty(mapableObject.PreviousPort))
                        {
                            member.Attributes.Add("{attribute addProperty PreviousPort \"" + mapableObject.PreviousPort + "\"}");
                        }
                        member.Name = ValidatePlcItem.Name(mapableObject.Name);
                        member.Type_Value = ValidatePlcItem.Type(mapableObject.Type_Value);
                        member.TypeNamespace =mapableObject.TypeNamespace;
                        member.Size = mapableObject.Size;
                        actTopologyStruct.StructMembers.Add(member);
                        actTopologyStruct.Id = actTopologyStruct.Id + member.Name + member.InOutPlcProj + member.Type_Value + member.Size;
                        actTopologyStruct.Size = actTopologyStruct.Size + member.Size;
                    }

                    foreach (MappableItem mapableItem in mapableObject.MapableItems)
                    {
                        MappableItem item = new MappableItem();
                        item = mapableItem;
                        string varAprefix = item.VarAprefix;
                        varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal));
                        string varA = item.VarAprefix;
                        varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator, StringComparison.Ordinal) + 1)) + tmpLevelSeparator + item.VarA;
                        item.VarAprefix = varAprefix;
                        item.VarA = varA;
                        mapableObjectVM.MapableItems.Add(item);
                    }
                }

                ValidateTopologyStructMemberNamesUniqueness(ref actTopologyStruct);

                if (actTopologyStruct.StructMembers.Count > 0)
                {

                    //Calculate CRC of the actPdoStruct.Id
                    actTopologyStruct.Crc32 = CRC32.Calculate_CRC32(actTopologyStruct.Id);
                    actTopologyStruct.Name = ValidatePlcItem.Name(actTopologyStruct.Prefix + "_" + actTopologyStruct.Crc32.ToString("X8"));
                    //Check if such an structure exists
                    if (CheckIfTopologyStructureDoesNotExist(actTopologyStruct))
                    {
                        //if not add to the structure list
                        TopologyStructures.Add(actTopologyStruct);
                    }

                    mapableObjectVM.Name = ValidatePlcItem.Name(boxViewModel.Name);
                    mapableObjectVM.Type_Value = ValidatePlcItem.Type(actTopologyStruct.Name);
                    mapableObjectVM.TypeNamespace = actTopologyStruct.TypeNamespace;
                    mapableObjectVM.Size = actTopologyStruct.Size;
                }
                else
                {
                    mapableObjectVM = null;
                }
            }
            return mapableObjectVM;
        }
    }
}
