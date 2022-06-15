using System.IO;
using TwincatXmlSchemas.TcPlcProj;
using iXlinkerDtos;
using System.Xml.Serialization;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private Project ReadoutPlcProj(Solution vs)
        {
            string plcProjPath = vs.PlcProject.Plcproj.CompletePathInFileSystem;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            StreamReader reader = new StreamReader(plcProjPath);
            Project plcProj = (Project)serializer.Deserialize(reader);
            reader.Close();
            return plcProj;
        }
    }
}
