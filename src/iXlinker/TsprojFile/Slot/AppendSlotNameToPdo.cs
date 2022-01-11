using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {

        private void AppendSlotNameToPdo(ref IBox box)
        {
            TcSmBoxDefEtherCAT boxItem = (TcSmBoxDefEtherCAT)box.Item;
            if (boxItem.Slot != null && boxItem.Pdo != null)
            {
                foreach (EtherCATSlaveSlot slot in boxItem.Slot)
                {
                    if (slot.Module != null && slot.Module[0] != null)
                    {
                        string slotName = slot.Module[0].Name ?? "";
                        string slotId = slot.Module[0].Id ?? "";
                        if (slot.Module[0].PdoIndex != null && slot.Module[0].PdoIndex[0] != null)
                        {
                            foreach (string slotPI in slot.Module[0].PdoIndex)
                            {
                                int slotPdoIndex = Convert.ToInt32(slotPI);
                                foreach (EtherCATSlavePdo pdo in boxItem.Pdo)
                                {
                                    string pdoName = pdo.Name;
                                    int pdoIndex = Convert.ToInt32(pdo.Index.Replace("#x", ""), 16);
                                    if (!slotName.Equals("") && !slotId.Equals("") && slotPdoIndex != 0 && pdoIndex != 0 && slotPdoIndex == pdoIndex)
                                    {
                                        pdo.Name = slotName + ioSlotSeparator + pdoName;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            box.Item = boxItem;
        }
    }
}
