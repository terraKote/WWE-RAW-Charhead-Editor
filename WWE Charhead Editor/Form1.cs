using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WWE_Charhead_Editor
{
    public partial class Form1 : Form
    {
        const string appName = "WWE RAW Charhead Editor";
        string currentFile;
        WrestlerData[] wrestlerData;
        WrestlerData currentWrestler;
        bool isDirty = false;

        public Form1()
        {
            InitializeComponent();

            applyButton.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
        }

        private void OpenFile(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                currentFile = openFileDialog1.FileName;
                wrestlerData = CharheadStream.GetWrestlerData(currentFile);

                if (wrestlerData == null)
                    return;

                RefreshWrestlerList();
            }

            openFileDialog1.Dispose();

            if (isDirty)
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }

        private void SelectWrestler(object sender, EventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            currentWrestler = wrestlerData[listBox.SelectedIndex];
            nameBox.Text = currentWrestler.Name;
            applyButton.Enabled = true;
        }

        private void ApplyName(object sender, EventArgs e)
        {
            currentWrestler.Name = nameBox.Text;
            isDirty = true;
            RefreshWrestlerList();
        }

        private void RefreshWrestlerList()
        {
            listBox1.Items.Clear();

            foreach (WrestlerData wrestler in wrestlerData)
            {
                listBox1.Items.Add(string.Format("{0}: {1}", wrestler.Index, wrestler.Name));
            }

            string file = currentFile.Split('\\').Last();

            if (isDirty)
                file += "*";
            else
                file.Remove(file.Length - 1);

            saveToolStripMenuItem.Enabled = isDirty;
            saveAsToolStripMenuItem.Enabled = isDirty;

            ActiveForm.Text = string.Format("{0} - {1}", appName, file);
        }

        private void Save(object sender, EventArgs e)
        {
            CharheadStream.SaveWrestlerData(wrestlerData, currentFile);
            isDirty = false;
            RefreshWrestlerList();
        }

        private void ChangeText(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            switch (radioButton.Name)
            {
                case "upperRadio":
                    nameBox.Text = nameBox.Text.ToUpper();
                    break;
                case "lowerRadio":
                    nameBox.Text = nameBox.Text.ToLower();
                    break;
            }
        }

        private void ShowInfo(object sender, EventArgs e)
        {
            MessageBox.Show("This tool was created by Denis 'terraKote' Kushnirenko for education purposes.\nYou can edit wrestler header files from WWE RAW (2002) game or WWE Ultimate Impact.\n(c)2017", "Info");
        }

        private void SaveAs(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                CharheadStream.SaveWrestlerData(wrestlerData, saveFileDialog1.FileName);
            }

            currentFile = saveFileDialog1.FileName;
            wrestlerData = CharheadStream.GetWrestlerData(currentFile);

            if (wrestlerData == null)
                return;

            isDirty = false;
            RefreshWrestlerList();

            saveFileDialog1.Dispose();
        }
    }
}
