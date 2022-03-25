using System;

namespace iXlinker.Utils
{
    public static class Conversion
    {
        public static Version StringToVersion(string input)
        {
            Version version = new Version();
            try
            {
                version = Version.Parse(input);
            }
            catch (ArgumentNullException)
            {
                EventLogger.Instance.Logger.Error("Error: String to be parsed is null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                EventLogger.Instance.Logger.Error("Error: Negative value in '{0}'.", input);
            }
            catch (ArgumentException)
            {
                EventLogger.Instance.Logger.Error("Error: Bad number of components in '{0}'.",input);
            }
            catch (FormatException)
            {
                EventLogger.Instance.Logger.Error("Error: Non-integer value in '{0}'.", input);
            }
            catch (OverflowException)
            {
                EventLogger.Instance.Logger.Error("Error: Number out of range in '{0}'.", input);
            }
            return version;
        }
    }
}
