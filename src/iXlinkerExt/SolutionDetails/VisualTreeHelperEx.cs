using System;
using System.Windows;
using System.Windows.Media;


namespace iXlinkerExt.VisualTreeHelperEx
{
    public static class VisualTreeHelperEx
    {
        public static FrameworkElement GetRecursiveByName(this DependencyObject root, string name)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var childrenCount = VisualTreeHelper.GetChildrenCount(root);

            for (var cc = 0; cc < childrenCount; cc++)
            {
                var control = VisualTreeHelper.GetChild(root, cc);

                if (control is FrameworkElement fe)
                {
                    if (fe.Name == name)
                    {
                        return fe;
                    }
                }

                var result = control.GetRecursiveByName(name);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
