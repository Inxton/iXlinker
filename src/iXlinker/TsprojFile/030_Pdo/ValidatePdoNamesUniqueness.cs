using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using System.IO;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
         private bool ValidatePdoNamesUniqueness(ref IBox box)
         {
            List<string> pdoNames = new List<string>();
            List<string> pdoNamesDuplicities = new List<string>();

            int sameNameIndex = 1;
            bool ret = true;

            TcSmBoxDefEtherCAT boxItem = (TcSmBoxDefEtherCAT)box.Item;

            if (boxItem.Pdo != null)
            {
                foreach (EtherCATSlavePdo pdo in boxItem.Pdo)
                {
                    if (pdo.SyncMan != null)
                    {
                        if (pdoNames.Contains(pdo.Name))
                        {
                            if (!pdoNamesDuplicities.Contains(pdo.Name))
                            {
                                sameNameIndex = 1;
                                pdoNamesDuplicities.Add(pdo.Name);
                            }
                            EventLogger.Instance.Logger.Information("Not unique pdo name {0} found in box name {1}, box type {2}!!!", pdo.Name, box.Name, boxItem.Desc);
                            if (exportDuplicities)
                            {
                                using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\PdoNamesDuplicities.txt", true))
                                {
                                    sw.WriteLine("{0} ; {1} ; {2}", box.Name, boxItem.Desc, pdo.Name);
                                }
                            }
                            pdo.Name = pdo.Name + "_" + sameNameIndex.ToString();
                            sameNameIndex++;
                            EventLogger.Instance.Logger.Information("\t Renamed to {0}!!!", pdo.Name);
                            pdoNames.Add(pdo.Name);
                        }
                        else
                        {
                            pdoNames.Add(pdo.Name);
                        }
                    }
                }
            }
            box.Item = boxItem;
            return ret;
        }
     }
}
