using ViewModels;
using System.Collections.ObjectModel;
using Utils;
using System.Linq;
using PlcprojFile;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private MapableObject GetAllMapableObjectsAsOneStructure(DeviceViewModel deviceViewModel, ObservableCollection<MapableObject> mapableObjects)
        {
            {
                MapableObject mapableObjectVM = new MapableObject();

                if (mapableObjects.Count == 1)
                {
                    MapableObject mapableObject = mapableObjects.ElementAt(0);

                    foreach (MapableItem mapableItem in mapableObject.MapableItems)
                    {
                        MapableItem item = new MapableItem();
                        item = mapableItem;
                        string varAprefix = item.VarAprefix;
                        varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator));
                        string varA = item.VarAprefix;
                        varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator) + 1)) + tmpLevelSeparator + item.VarA;
                        item.VarAprefix = varAprefix;
                        item.VarA = varA;
                        mapableObjectVM.MapableItems.Add(item);
                    }
                    mapableObjectVM.Name = ValidatePlcItem.Name(mapableObject.Name);
                    mapableObjectVM.Type_Value = mapableObject.Type_Value;
                    mapableObjectVM.SizeInBites = mapableObject.SizeInBites;
                    mapableObjectVM.SizeInBytes = mapableObject.SizeInBytes;
                }
                else if (mapableObjects.Count >= 1)
                {
                    TopologyStructViewModel actTopologyStruct = new TopologyStructViewModel() { Prefix = ValidatePlcItem.Name(deviceViewModel.Name), Id = "", BoxOrderCode = deviceViewModel.Type.ToString()};

                    foreach (MapableObject mapableObject in mapableObjects)
                    {
                        TopologyStructMemberViewModel member = new TopologyStructMemberViewModel();

                        member.Attributes.Add("{attribute addProperty Name \"" + mapableObject.Name + "\"}");
                        member.Name = ValidatePlcItem.Name(mapableObject.Name);
                        member.Type_Value = mapableObject.Type_Value;
                        member.SizeInBites = mapableObject.SizeInBites;
                        member.SizeInBytes = mapableObject.SizeInBytes;
                        actTopologyStruct.StructMembers.Add(member);
                        actTopologyStruct.Id = actTopologyStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes;
                        actTopologyStruct.SizeInBites = actTopologyStruct.SizeInBites + member.SizeInBites;
                        actTopologyStruct.SizeInBytes = actTopologyStruct.SizeInBytes + member.SizeInBytes;

                        foreach (MapableItem mapableItem in mapableObject.MapableItems)
                        {
                            MapableItem item = new MapableItem();
                            item = mapableItem;
                            string varAprefix = item.VarAprefix;
                            varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator));
                            string varA = item.VarAprefix;
                            varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator) + 1)) + tmpLevelSeparator + item.VarA;
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
                        mapableObjectVM.SizeInBites = actTopologyStruct.SizeInBites;
                        mapableObjectVM.SizeInBytes = actTopologyStruct.SizeInBytes;

                    }
                    else
                    {
                        mapableObjectVM = null;
                    }
                }
                return mapableObjectVM;
            }
        }
        private MapableObject GetAllMapableObjectsAsOneStructure(BoxViewModel boxViewModel, ObservableCollection<MapableObject> mapableObjects)
        {
            MapableObject mapableObjectVM = new MapableObject();

            if (mapableObjects.Count == 1)
            {
                MapableObject mapableObject = mapableObjects.ElementAt(0);

                foreach (MapableItem mapableItem in mapableObject.MapableItems)
                {
                    MapableItem item = new MapableItem();
                    item = mapableItem;
                    string varAprefix = item.VarAprefix;
                    varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator));
                    string varA = item.VarAprefix;
                    varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator) + 1)) + tmpLevelSeparator + item.VarA;
                    item.VarAprefix = varAprefix;
                    item.VarA = varA;
                    mapableObjectVM.MapableItems.Add(item);
                }
                mapableObjectVM.Name = ValidatePlcItem.Name(mapableObject.Name);
                mapableObjectVM.Type_Value = ValidatePlcItem.Type(mapableObject.Type_Value);
                mapableObjectVM.SizeInBites = mapableObject.SizeInBites;
                mapableObjectVM.SizeInBytes = mapableObject.SizeInBytes;
            }
            else if (mapableObjects.Count >= 1)
            {
                TopologyStructViewModel actTopologyStruct = new TopologyStructViewModel() { Prefix = ValidatePlcItem.Name(boxViewModel.Name), Id = "", BoxOrderCode = boxViewModel.BoxOrderCode};

                foreach (MapableObject mapableObject in mapableObjects)
                {
                    TopologyStructMemberViewModel member = new TopologyStructMemberViewModel();
                    if (ValidatePlcItem.Name(boxViewModel.Name).Equals(mapableObject.Name) && mapableObjects.IndexOf(mapableObject) == 0)
                    {
                        actTopologyStruct.Extends = mapableObject.Type_Value;
                    }
                    else
                    {
                        member.Attributes.Add("{attribute addProperty Name \"" + mapableObject.Name + "\"}");
                        member.Name = ValidatePlcItem.Name(mapableObject.Name);
                        member.Type_Value = ValidatePlcItem.Type(mapableObject.Type_Value);
                        member.SizeInBites = mapableObject.SizeInBites;
                        member.SizeInBytes = mapableObject.SizeInBytes;
                        actTopologyStruct.StructMembers.Add(member);
                        actTopologyStruct.Id = actTopologyStruct.Id + member.Name + member.Type_Value + member.SizeInBites + member.SizeInBytes;
                        actTopologyStruct.SizeInBites = actTopologyStruct.SizeInBites + member.SizeInBites;
                        actTopologyStruct.SizeInBytes = actTopologyStruct.SizeInBytes + member.SizeInBytes;
                    }

                    foreach (MapableItem mapableItem in mapableObject.MapableItems)
                    {
                        MapableItem item = new MapableItem();
                        item = mapableItem;
                        string varAprefix = item.VarAprefix;
                        varAprefix = varAprefix.Substring(0, varAprefix.LastIndexOf(tmpLevelSeparator));
                        string varA = item.VarAprefix;
                        varA = ValidatePlcItem.Link(varA.Substring(varA.LastIndexOf(tmpLevelSeparator) + 1)) + tmpLevelSeparator + item.VarA;
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
                    mapableObjectVM.SizeInBites = actTopologyStruct.SizeInBites;
                    mapableObjectVM.SizeInBytes = actTopologyStruct.SizeInBytes;
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
