using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WWE_Charhead_Editor
{
    class CharheadStream
    {
        private const int m_BlockLength = 862;
        private const int m_CharacterNumber = 47;
        private const int m_TrueFileSize = 40514;

        public static WrestlerData[] GetWrestlerData(string fileName)
        {
            List<WrestlerData> wrestlerData = new List<WrestlerData>();
            int lastByteIndex = 0;
            byte[] allBytes = File.ReadAllBytes(fileName);

            if (allBytes.Length != m_TrueFileSize)
            {
                MessageBox.Show("No suitable data detected!", "Error");
                return null;
            }

            for (int i = 0; i < m_CharacterNumber; i++)
            {
                WrestlerData wrestler = new WrestlerData();
                wrestler.Bytes = allBytes.Skip(lastByteIndex).Take(m_BlockLength).ToArray();
                wrestler.Index = i;
                lastByteIndex += m_BlockLength;
                wrestlerData.Add(wrestler);
            }

            return wrestlerData.ToArray();
        }

        public static void SaveWrestlerData(WrestlerData[] wrestlerData, string fileName)
        {
            List<byte> data = new List<byte>();

            foreach (WrestlerData wrestler in wrestlerData)
            {
                data.AddRange(wrestler.Bytes);
            }

            File.WriteAllBytes(fileName, data.ToArray());
        }
    }
}
