using System;
using System.IO;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void GenerateStructures(Solution vs)
        {
            //Reading out of the content of the associated PLC project to be able to modify it.
            Project tcPlcProj = ReadoutPlcProj(vs);
            //Delete the previously generated GVL, so as in the PLC project structure, so as in the file system.
            ClearGvlGenerated(vs, ref tcPlcProj);
            //Delete the previously generated DUTs, so as in the PLC project structure, so as in the file system.
            ClearDutsIoFolder(vs, ref tcPlcProj);
            //Write generated PLC structures to the file system and include them to the PLC project.
            AddStructuresToPlcproj(vs, ref tcPlcProj);
            //Write generated GVL to the file system and include it to the PLC project.
            AddGvlToPlcProj(vs, ref tcPlcProj);
            //Write changes on the PLC project to it's .plcproj file.
            ApplyChangesToPlcProj(vs, tcPlcProj);
        }
    }
}
