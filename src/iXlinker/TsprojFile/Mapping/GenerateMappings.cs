using System;
using System.IO;
using System.Xml.Serialization;
using iXlinkerDtos;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;
using PlcprojFile;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        internal void GenerateMappings(Solution vs)
        {
            if (vs.PlcProject.IsIndependent)
            {
                GenerateMappingsToXti(vs);
            }
            else
            {
                GenerateMappingsToTsProj(vs);
            }

        }
        internal void GenerateMappingsToTsProj(Solution vs)
        {
            EventLogger.Instance.Logger.Information("Generating mappings into the project file {0}!!!", vs.TsProject.CompletePathInFileSystem);

            TcSmProject tsProj = new TcSmProject();
            XmlSerializer deserializer = new XmlSerializer(typeof(TcSmProject));
            StreamReader reader = new StreamReader(vs.TsProject.CompletePathInFileSystem);
            tsProj = (TcSmProject)deserializer.Deserialize(reader);
            reader.Close();

            MappingsTypeOwnerA OwnerA = new MappingsTypeOwnerA() { Name = OwnerAPlcName.Replace(tmpLevelSeparator,ioLevelSeparator) };
            MappingsTypeOwnerAOwnerB OwnerB = new MappingsTypeOwnerAOwnerB();
            string prevOwnerBname = "";

            List<MappingsTypeOwnerAOwnerB> ownerBlist = new List<MappingsTypeOwnerAOwnerB>();
            List<MappingsTypeOwnerAOwnerBLink> linkList = new List<MappingsTypeOwnerAOwnerBLink>();

            foreach (DeviceViewModel deviceViewModel in Devices)
            {
                foreach (MappableItem mapableItem in deviceViewModel.MapableObjectGrouped.MapableItems)
                {
                    string ownerBname = mapableItem.OwnerBname.Replace(tmpLevelSeparator,ioLevelSeparator);
                    if (!ownerBname.Equals(prevOwnerBname))
                    {
                        if (prevOwnerBname != "")
                        {
                            AddOwnerBToOwnerBlist(ref OwnerB, ref ownerBlist, ref linkList, prevOwnerBname);
                        }
                    }
                    prevOwnerBname = ownerBname;

                    string varA = mapableItem.VarAprefix.Replace(tmpLevelSeparator, ioLevelSeparator) + plcStructSeparator + ValidatePlcItem.LinkA(mapableItem.VarA);
                    string varB = ValidatePlcItem.LinkB(mapableItem.VarB);

                    MappingsTypeOwnerAOwnerBLink link = new MappingsTypeOwnerAOwnerBLink() { VarA = varA, VarB = varB };
                    linkList.Add(link);
                }
            }
            AddOwnerBToOwnerBlist(ref OwnerB, ref ownerBlist, ref linkList, prevOwnerBname);

            AddOwnerBListToOwnerA(ref OwnerA, ownerBlist);

            tsProj.Mappings = new MappingsType();
            tsProj.Mappings.OwnerA = new MappingsTypeOwnerA[1] {OwnerA};

            XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
            StreamWriter writer = new StreamWriter(vs.TsProject.CompletePathInFileSystem);
            serializer.Serialize(writer, tsProj);
            writer.Close();

            EventLogger.Instance.Logger.Information("Mappings generated into the project file {0}!!!", vs.TsProject.CompletePathInFileSystem);
        }

        internal void GenerateMappingsToXti(Solution vs)
        {
            EventLogger.Instance.Logger.Information("Generating mappings into the project file {0}!!!", vs.PlcProject.Xti.CompletePathInFileSystem);

            TcSmItem xti = new TcSmItem();
            XmlSerializer deserializer = new XmlSerializer(typeof(TcSmItem));
            StreamReader reader = new StreamReader(vs.PlcProject.Xti.CompletePathInFileSystem);
            xti = (TcSmItem)deserializer.Deserialize(reader);
            reader.Close();

            MappingsTypeOwnerA OwnerA = new MappingsTypeOwnerA() { Name = OwnerAPlcName.Replace(tmpLevelSeparator, ioLevelSeparator) };
            MappingsTypeOwnerAOwnerB OwnerB = new MappingsTypeOwnerAOwnerB();
            string prevOwnerBname = "";

            List<MappingsTypeOwnerAOwnerB> ownerBlist = new List<MappingsTypeOwnerAOwnerB>();
            List<MappingsTypeOwnerAOwnerBLink> linkList = new List<MappingsTypeOwnerAOwnerBLink>();

            foreach (DeviceViewModel deviceViewModel in Devices)
            {
                foreach (MappableItem mapableItem in deviceViewModel.MapableObjectGrouped.MapableItems)
                {
                    string ownerBname = mapableItem.OwnerBname.Replace(tmpLevelSeparator, ioLevelSeparator);
                    if (!ownerBname.Equals(prevOwnerBname))
                    {
                        if (prevOwnerBname != "")
                        {
                            AddOwnerBToOwnerBlist(ref OwnerB, ref ownerBlist, ref linkList, prevOwnerBname);
                        }
                    }
                    prevOwnerBname = ownerBname;

                    string varA = mapableItem.VarAprefix.Replace(tmpLevelSeparator, ioLevelSeparator) + plcStructSeparator + ValidatePlcItem.LinkA(mapableItem.VarA);
                    string varB = ValidatePlcItem.LinkB(mapableItem.VarB);

                    MappingsTypeOwnerAOwnerBLink link = new MappingsTypeOwnerAOwnerBLink() { VarA = varA, VarB = varB };
                    linkList.Add(link);
                }
            }
            AddOwnerBToOwnerBlist(ref OwnerB, ref ownerBlist, ref linkList, prevOwnerBname);

            AddOwnerBListToOwnerA(ref OwnerA, ownerBlist);

            xti.Mappings = new MappingsType();
            xti.Mappings.OwnerA = new MappingsTypeOwnerA[1] { OwnerA };

            XmlSerializer serializer = new XmlSerializer(typeof(TcSmItem));
            StreamWriter writer = new StreamWriter(vs.PlcProject.Xti.CompletePathInFileSystem);
            serializer.Serialize(writer, xti);
            writer.Close();

            EventLogger.Instance.Logger.Information("Mappings generated into the project file {0}!!!", vs.PlcProject.Xti.CompletePathInFileSystem);
        }
        private void AddOwnerBToOwnerBlist(ref MappingsTypeOwnerAOwnerB OwnerB, ref List<MappingsTypeOwnerAOwnerB> ownerBlist, ref List<MappingsTypeOwnerAOwnerBLink> linkList, string prevOwnerBname)
        {
            int linksCount = linkList.Count;
            OwnerB.Link = new MappingsTypeOwnerAOwnerBLink[linksCount];
            OwnerB.Name = prevOwnerBname;
            int linkIndex = 0;
            foreach (MappingsTypeOwnerAOwnerBLink linkListItem in linkList)
            {
                OwnerB.Link[linkIndex] = linkListItem;
                linkIndex++;
            }
            linkList = new List<MappingsTypeOwnerAOwnerBLink>();

            ownerBlist.Add(OwnerB);
            OwnerB = new MappingsTypeOwnerAOwnerB();
        }
        private void AddOwnerBListToOwnerA(ref MappingsTypeOwnerA OwnerA, List<MappingsTypeOwnerAOwnerB> ownerBlist)
        {
            int ownerBlistCount = ownerBlist.Count;
            OwnerA.OwnerB = new MappingsTypeOwnerAOwnerB[ownerBlistCount];

            int ownerBindex = 0;
            foreach (MappingsTypeOwnerAOwnerB ownerBlistItem in ownerBlist)
            {
                OwnerA.OwnerB[ownerBindex] = ownerBlistItem;
                ownerBindex++;
            }
        }
    }
}
