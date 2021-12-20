using System.IO;
using TwincatXmlSchemas.TcPlcProj;
using ViewModels;
using System.Xml.Serialization;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private Project ReadoutPlcProj(VisualStudioDTEViewModel vs)
        {
            string plcProjPath = vs.PlcProject.Details.CompletePathInFileSystem;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            StreamReader reader = new StreamReader(plcProjPath);
            Project plcProj = (Project)serializer.Deserialize(reader);
            reader.Close();
            return plcProj;
        }
    }
}
