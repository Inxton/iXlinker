using System;
using System.Windows;
using iXlinkerExt.VisualTreeHelperEx;

namespace iXlinkerExt
{
    public static class SlnfFinder
    {
        private const string Slnf = ".slnf";
        public static string DiscoverSlnfFilePath()
        {
            string slnfFilePath = string.Empty;
            try
            {
                var mw = Application.Current.MainWindow;

                var textBlock = mw.GetRecursiveByName("PART_SolutionNameTextBlock");
                if (textBlock != null)
                {
                    string filePath = (textBlock.TemplatedParent as dynamic).ToolTip.ToString();
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        if (filePath.Contains(Slnf))
                        {
                            slnfFilePath = filePath;
                        }
                        else
                        {
                            slnfFilePath = string.Empty;
                        }
                    }
                    else
                    {
                        slnfFilePath = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "iXlinker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return slnfFilePath;
        }
    }
}
