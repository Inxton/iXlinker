using iXlinker.Utils;
using iXlinkerDtos;
using System;
using System.IO;
using System.Xml.Serialization;
using TwincatXmlSchemas.TcSmProject;


namespace iXlinker.TsprojFile.Mapping
{
    internal partial class VS
    {
        private static void GetInpedendentProjectFileIO(Solution vs)
        {
            if (vs.TsProject.CompletePathInFileSystem != null) 
            {
                if (File.Exists(vs.TsProject.CompletePathInFileSystem))
                {

                    XmlSerializer serializer = new XmlSerializer(typeof(TcSmProject));
                    StreamReader reader = new StreamReader(vs.TsProject.CompletePathInFileSystem);
                    TcSmProject TsProj = new TcSmProject();
                    try
                    {
                        TsProj = (TcSmProject)serializer.Deserialize(reader);
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Instance.Logger.Error(@"Unable to deserialize project file: " + vs.TsProject.CompletePathInFileSystem + " !!!"
                                                           + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                    }
                    finally
                    {
                        reader.Close();
                    }


                    if (TsProj.Project.Io != null && TsProj.Project.Io.Items != null)
                    {
                        try
                        {
                            foreach (TcSmProjectProjectIODevice device in TsProj.Project.Io.Items)
                            {
                                if (!vs.DoNotGenerateDisabled || !device.DisabledSpecified || !device.Disabled)
                                {
                                    bool isIndependentProjectFile = device.Name == null && device.File != null;
                                    if (isIndependentProjectFile)
                                    {
                                        IoDevice ioDevice = new IoDevice();

                                        ioDevice.FolderPathInFileSystem = Path.Combine(vs.TsProject.FolderPathInFileSystem, @"_Config\IO");
                                        ioDevice.FileNameInFileSystem = device.File;
                                        ioDevice.CompletePathInFileSystem = Path.Combine(ioDevice.FolderPathInFileSystem, ioDevice.FileNameInFileSystem);
                                        ioDevice.Name = ioDevice.FileNameInFileSystem.Replace(".xti", "");
                                        vs.IndependentIoDevices.Add(ioDevice);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            EventLogger.Instance.Logger.Error("No devices found!!!"
                                                            + Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.Message);
                        }
                    }


                    if (vs.IndependentIoDevices.Count > 0)
                    {
                        string IndependentDeviceList = "";
                        foreach (ProjectItem ioDevice in vs.IndependentIoDevices)
                        {
                            IndependentDeviceList = IndependentDeviceList + "\t" + ioDevice.Name + Environment.NewLine;
                        }
                        EventLogger.Instance.Logger.Information(@"Project contains " + vs.IndependentIoDevices.Count  + " IoDevices in individual project files" +
                            Environment.NewLine + IndependentDeviceList);
                    }
                }
                else
                {
                    EventLogger.Instance.Logger.Error(Environment.NewLine + @"Unable to find Xae project file specified: " + vs.TsProject.CompletePathInFileSystem + " !!!"
                                                      + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
                }
            }
            else
            {
                EventLogger.Instance.Logger.Error(Environment.NewLine + @"Variable: " + EventLoggerDetails.NameOf<Solution>(x=>x.TsProject.CompletePathInFileSystem) + " has an invalid reference!!!"
                                                  + Environment.NewLine + "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
