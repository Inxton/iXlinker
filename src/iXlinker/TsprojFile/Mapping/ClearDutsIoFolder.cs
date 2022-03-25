using System;
using System.IO;
using iXlinkerDtos;
using TwincatXmlSchemas.TcPlcProj;
using System.Collections.Generic;
using iXlinker.Utils;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private void ClearDutsIoFolder(Solution vs , ref Project plcProj)
        {
            EventLogger.Instance.Logger.Information("Cleaning existing DUTs!");
            List<ProjectItem> FoldersToClear= new List<ProjectItem>();

            FoldersToClear.Add(vs.DutsIo);
            FoldersToClear.Add(vs.DutsIoPdoEntry);
            FoldersToClear.Add(vs.DutsIoPdo);
            FoldersToClear.Add(vs.DutsIoBox);
            FoldersToClear.Add(vs.DutsIoDevice);
            FoldersToClear.Add(vs.DutsIoTopology);

            //Delete all items in the PLC project that are inside "FoldersToClear"
            //As the "item.Compile" is an array, "deleting" is made such a way that in the first foreach loop, there is just counted the number of the items to be removed.
            //Subsequently the new array of the new dimension is creted and in the next foreach loop only that items that should not be deleted are copied.
            //In the last step previous "item.Compile" is overwritten by the "newItemCompile".
            foreach (ProjectItemGroup item in plcProj.ItemGroup)
            {
                if(item.Compile != null)
                {
                    int oldCompileItemsCount = item.Compile.Length;
                    int compileItemToRemove = 0;
                    int compileIndex = 0;

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile )
                    {
                        bool removeItem = false;
                        foreach(ProjectItem folderToClear in FoldersToClear)
                        {
                            removeItem = false;
                            if (itemCompile.Include.Contains(folderToClear.Path))
                            {
                                removeItem = true;
                                break;
                            }
                        }
                        removeItem = removeItem && itemCompile.SubType.Contains("Code");

                        if (removeItem)
                        {
                            compileItemToRemove++;
                        }
                    }
                    
                    int newCompileItemsCount = oldCompileItemsCount - compileItemToRemove;
                    ProjectItemGroupCompile[] newItemCompile = new ProjectItemGroupCompile[newCompileItemsCount];

                    foreach (ProjectItemGroupCompile itemCompile in item.Compile)
                    {
                        bool removeItem = false;
                        foreach (ProjectItem folderToClear in FoldersToClear)
                        {
                            removeItem = false;
                            if (itemCompile.Include.Contains(folderToClear.Path))
                            {
                                removeItem = true;
                                break;
                            }
                        }
                        removeItem = removeItem && itemCompile.SubType.Contains("Code");

                        if (!removeItem)
                        {
                            newItemCompile[compileIndex] = itemCompile;
                            compileIndex++;
                        }
                    }
                    item.Compile = newItemCompile;
                }
            }

            //Check if each folder in the "FoldersToClear" list exist, if not create it.
            foreach (ProjectItemGroup item in plcProj.ItemGroup)
            {
                bool folderExistsInThePlcProj = false;
                foreach (ProjectItem folderToClear in FoldersToClear)
                {
                    folderExistsInThePlcProj = false;

                    if (item.Folder != null)
                    {
                        int oldFoldersCount = item.Folder.Length;
                        int folderIndex = 0;
                        foreach (ProjectItemGroupFolder itemFolder in item.Folder)
                        {
                            if ((itemFolder.Include.Contains(folderToClear.Path)))
                            {
                                folderExistsInThePlcProj = true;
                                break;
                            }
                        }

                        if (!folderExistsInThePlcProj)
                        {
                            int newFoldersCount = oldFoldersCount + 1;
                            ProjectItemGroupFolder[] newItemFolder = new ProjectItemGroupFolder[newFoldersCount];

                            foreach (ProjectItemGroupFolder itemFolder in item.Folder)
                            {
                                newItemFolder[folderIndex] = itemFolder;
                                folderIndex++;
                            }
                            newItemFolder[folderIndex] = new ProjectItemGroupFolder() { Include = folderToClear.Path};
                            item.Folder = newItemFolder;
                        }
                    }
                }
            }

            //Delete all files and folders in each of the folder in the "FoldersToClear" list in the file system, such a folder exists.
            //If not create it.
            foreach (ProjectItem folderToClear in FoldersToClear)
            {
                if (Directory.Exists(folderToClear.FolderPathInFileSystem))
                {
                    string[] files = Directory.GetFiles(folderToClear.FolderPathInFileSystem);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                    string[] dirs = Directory.GetDirectories(folderToClear.FolderPathInFileSystem);
                    foreach (string dir in dirs)
                    {
                        Directory.Delete(dir, true);
                    }
                }
                else
                {
                    Directory.CreateDirectory(folderToClear.FolderPathInFileSystem);
                }
            }
            EventLogger.Instance.Logger.Information("Existing DUTs cleared!");
        }
    }
}
