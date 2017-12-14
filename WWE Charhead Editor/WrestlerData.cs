using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWE_Charhead_Editor
{
    struct WrestlerData
    {
        List<byte> space;
        int index;
        byte[] bytes;

        const int maxCharNameLength = 25;
        static readonly int[] nameIndex = { 0, 30, 90, 120 };

        public byte[] Bytes { get => bytes; set => bytes = value; }
        public string Name { get => Encoding.UTF8.GetString(bytes); set => SetName(value); }
        public int Index { get => index; set => index = value; }

        private void SetName(string name)
        {
            string editedName = string.Empty;
            space = new List<byte>();

            if (name.Length > maxCharNameLength)
            {
                editedName = name.Remove(maxCharNameLength);
            }
            else
            {
                editedName = name;
                int requiredSpacers = maxCharNameLength - editedName.Length;

                for (int i = 0; i < requiredSpacers; i++)
                {
                    space.Add(0x00);
                }
            }

            byte[] nameInBytes = Encoding.UTF8.GetBytes(editedName);
            nameInBytes = nameInBytes.Concat(space).ToArray();

            foreach (int index in nameIndex)
            {
                for (int i = index; i < index + maxCharNameLength; i++)
                {
                    bytes[i] = nameInBytes[i - index];
                }
            }
        }
    }
}
