using System.IO;
using TwincatXmlSchemas.TcPlcProj;
using ViewModels;
using System.Xml.Serialization;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ApplyChangesToPlcProj(SolutionViewModel vs , Project plcProj)
        {
            System.Console.WriteLine("Writing changes of the PLC project {0} into the file {1}!!!", vs.PlcProject.Name,vs.PlcProject.FileNameInFileSystem);
            Project plcProject = plcProj;
            XmlSerializer serializer = new XmlSerializer(typeof(Project));
            StreamWriter  writer = new StreamWriter(vs.PlcProject.CompletePathInFileSystem);
            serializer.Serialize(writer, plcProject);
            writer.Close();
            System.Console.WriteLine("Changes of the PLC project {0} written into the file {1}!!!", vs.PlcProject.Name, vs.PlcProject.FileNameInFileSystem);
        }
    }
}
