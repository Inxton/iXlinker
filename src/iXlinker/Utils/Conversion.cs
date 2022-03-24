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
                Console.WriteLine("Error: String to be parsed is null.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Error: Negative value in '{0}'.", input);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Error: Bad number of components in '{0}'.",
                                  input);
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Non-integer value in '{0}'.", input);
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: Number out of range in '{0}'.", input);
            }
            return version;
        }
    }
}
