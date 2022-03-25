using System;
using System.IO;
using iXlinker.Utils;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ClearGvlGenerated(Solution vs , ref Project tcPlcProj)
        {
            EventLogger.Instance.Logger.Information("Cleaning existing GVL!");

            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if(item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToRemove = 0;
                    int compileIndex = 0;

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile )
                    {
                        if ((itemCompile.SubType.Contains("Code") && itemCompile.Include.Contains(vs.GvlExported.Name)))
                        {
                            compileItemToRemove++;
                        }
                    }
                    
                    int newCompileItemsCount = oldCompileItemsCount - compileItemToRemove;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        if (!(itemCompile.SubType.Contains("Code") && itemCompile.Include.Contains(vs.GvlExported.Name)))
                        {
                            newItemCompile[compileIndex] = itemCompile;
                            compileIndex++;
                        }
                    }
                    item.Compile = newItemCompile;
                }
            }

            bool gvlPathExistsInPlcProj = false;
            foreach (ProjectItemGroup item in tcPlcProj.ItemGroup)
            {
                if (item.Folder != null)
                {
                    int oldFoldersCount = item.Folder.Length;
                    int folderIndex = 0;
                    foreach (ProjectItemGroupFolder itemFolder in item.Folder)
                    {
                        if ((itemFolder.Include.Contains(vs.GvlExported.Path)))
                        {
                            gvlPathExistsInPlcProj = true;
                            break;
                        }
                    }

                    if(!gvlPathExistsInPlcProj)
                    {
                        int newFoldersCount = oldFoldersCount + 1;
                        ProjectItemGroupFolder[] newItemFolder = new ProjectItemGroupFolder[newFoldersCount];

                        foreach (ProjectItemGroupFolder itemFolder in item.Folder)
                        {
                            newItemFolder[folderIndex] = itemFolder;
                            folderIndex++;
                        }
                        newItemFolder[folderIndex] = new ProjectItemGroupFolder() { Include = vs.GvlExported.Path};
                        item.Folder = newItemFolder;
                    }
                }
            }


            if (Directory.Exists(vs.GvlExported.FolderPathInFileSystem))
            {
                string[] files = Directory.GetFiles(vs.GvlExported.FolderPathInFileSystem);
                foreach (string file in files)
                {
                    if(file.Contains(vs.GvlExported.Name))
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(vs.GvlExported.Path);
            }
            EventLogger.Instance.Logger.Information("Existing GVL cleared!");
        }
    }
}
