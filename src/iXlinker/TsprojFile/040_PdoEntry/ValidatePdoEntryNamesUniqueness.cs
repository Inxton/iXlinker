using System;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using System.IO;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ValidatePdoEntryNamesUniqueness(ref EtherCATSlavePdo pdo, PdoViewModel pdoViewModel)
        {
            List<string> pdoEntryNames = new List<string>();
            List<string> pdoEntryNamesDuplicities = new List<string>();
            int sameNameIndex = 1;

            if (pdo.Entry != null)
            {
                foreach (EtherCATSlavePdoEntry pdoEntry in pdo.Entry)
                {
                    if (pdoEntry.Name != null && !pdoEntry.Name.Contains("__") && pdoEntryNames.Contains(pdoEntry.Name))
                    {
                        if (!pdoEntryNamesDuplicities.Contains(pdoEntry.Name))
                        {
                            sameNameIndex = 1;
                            pdoEntryNamesDuplicities.Add(pdoEntry.Name);
                        }
                        if (string.IsNullOrEmpty(pdoEntry.Type.GUID))
                        {
                            Console.WriteLine("Not unique pdo entry name {0} found in the pdo name {1}, in the box type {2}!!!", pdoEntry.Name, pdo.Name, pdoViewModel.BoxOrderCode);
                            if (exportDuplicities)
                            {
                                using (StreamWriter sw = new StreamWriter(@"D:\Inxton\iXlinker\PdoEntryNamesDuplicities.txt", true))
                                {
                                    sw.WriteLine("{0} ; {1} ; {2}", pdoViewModel.BoxOrderCode, pdo.Name, pdoEntry.Name);
                                }
                            }
                        }
                        pdoEntry.Name = pdoEntry.Name + "_" + sameNameIndex.ToString();
                        sameNameIndex++;
                        Console.WriteLine("\t Renamed to {0}!!!", pdoEntry.Name);
                        pdoEntryNames.Add(pdoEntry.Name);
                    }
                    else
                    {
                        pdoEntryNames.Add(pdoEntry.Name);
                    }
                }
            }
        }
    }
}
