using System;
using ViewModels;
using TwincatXmlSchemas.TcSmProject;
using System.Collections.Generic;

namespace TsprojFile.Scan
{
    public partial class ScanTcProjFile : TcModel
    {
        private bool CheckDeviceAndBoxNamesUniqueness(TcSmProjectProjectIO Io)
        {
            List<string> devNames = new List<string>();

            bool ret = true;
            if (Io!=null && Io.Items != null)
            {
                foreach (TcSmProjectProjectIODevice d in Io.Items)
                {
                    List<string> boxNames = new List<string>();

                    if (!d.DisabledSpecified || !d.DisabledSpecified)
                    {


                        if (devNames.Contains(d.Name))
                        {
                            Console.WriteLine("Not unique device name {0} found!!!", d.Name);
                            ret = false;
                        }
                        else
                        {
                            devNames.Add(d.Name);
                        }
                        if (d.Box != null)
                        {
                            foreach (TcSmDevDefBox box in d.Box)
                            {
                                if (!CheckDevDefNameUniqueness(box, ref boxNames))
                                {
                                    ret = false;
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
        private bool CheckDevDefNameUniqueness(TcSmDevDefBox box, ref List<string> boxNames)
        {
            bool ret = true;
            if (!box.DisabledSpecified || !box.Disabled)
            {

                if (boxNames.Contains(box.Name))
                {
                    Console.WriteLine("Not unique box name {0} found!!!", box.Name);
                    ret = false;
                }
                else
                {
                    boxNames.Add(box.Name);
                }
                if (box.Box != null)
                {
                    foreach (TcSmBoxDefBox subbox in box.Box)
                    {
                        if (!CheckBoxDefNameUniqueness(subbox, ref boxNames))
                        {
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }

        private bool CheckBoxDefNameUniqueness(TcSmBoxDefBox box, ref List<string> boxNames)
        {
            bool ret = true;
            if (!box.DisabledSpecified || !box.Disabled)
            {

                if (boxNames.Contains(box.Name))
                {
                    Console.WriteLine("Not unique box name {0} found!!!", box.Name);
                    ret = false;
                }
                else
                {
                    boxNames.Add(box.Name);
                }
                if (box.Box != null)
                {
                    foreach (TcSmBoxDefBox subbox in box.Box)
                    {
                        if (!CheckBoxDefNameUniqueness(subbox, ref boxNames))
                        {
                            Console.WriteLine("Not unique box name {0} found!!!", box.Name);
                            ret = false;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
