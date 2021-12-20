using System.Text;

namespace Utils
{
    class CRC32
    {
        private static uint[] crc32table = new uint[256];
        private static bool tableInitiated = false;

        private static void CalculateCrcTable_CRC32()
        {
            const uint polynomial = 0x04C11DB7;


            for (int divident = 0; divident < 256; divident++) /* iterate over all possible input byte values 0 - 255 */
            {
                uint curByte = (uint)(divident << 24); /* move divident byte into MSB of 32Bit CRC */
                for (byte bit = 0; bit < 8; bit++)
                {
                    if ((curByte & 0x80000000) != 0)
                    {
                        curByte <<= 1;
                        curByte ^= polynomial;
                    }
                    else
                    {
                        curByte <<= 1;
                    }
                }

                crc32table[divident] = curByte;
            }

        }
        public static uint Calculate_CRC32(string str)
        {
            if (!tableInitiated)
            {
                CalculateCrcTable_CRC32();
                tableInitiated = true;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(str);

            uint crc = 0xFFFFFFFF;
            foreach (byte b in bytes)
            {
                /* XOR-in next input byte into MSB of crc and get this MSB, that's our new intermediate divident */
                byte pos = (byte)((crc >> 24) ^ b);
                /* Shift out the MSB used for division per lookuptable and XOR with the remainder */
                crc = (uint)((crc << 8) ^ (uint)(crc32table[pos]));
            }

            return crc;
        }

    }
}
