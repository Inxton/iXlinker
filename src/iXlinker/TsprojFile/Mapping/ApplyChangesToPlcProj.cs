using System.IO;
using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using System.Xml.Serialization;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ApplyChangesToPlcProj(Solution vs , Project plcProj)
        {
            EventLogger.Instance.Logger.Information("Writing changes of the PLC project {0} into the file {1}!!!", vs.PlcProject.Name,vs.PlcProject.FileNameInFileSystem);
            Project plcProject = plcProj;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            StreamWriter  writer = new StreamWriter(vs.PlcProject.CompletePathInFileSystem);
            serializer.Serialize(writer, plcProject);
            writer.Close();
            EventLogger.Instance.Logger.Information("Changes of the PLC project {0} written into the file {1}!!!", vs.PlcProject.Name, vs.PlcProject.FileNameInFileSystem);
        }
    }
}
