using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Sab_Toolbox
{
    public partial class Loosefiles_Editor : Form
    {

        private Stream fileInput = null;
        private Stream franceShaders = null;
        private Stream conversationsCNVPack = null;
        private Stream cinematicsCinPack = null;
        private Stream globalMap = null;
        private Stream franceMap = null;
        private Stream editNodesPack = null;
        private Stream gameTemplatesWSD = null;

        public Loosefiles_Editor()
        {
            InitializeComponent();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_Settings settings1 = new Sabtool_Settings();
            settings1.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_About about1 = new Sabtool_About();
            about1.Show();
        }



          //////////////////////
         // Load Loose Files //
        //////////////////////
        private void loadLoosefiles()
        {
            BinaryReader binReader1 = new BinaryReader(fileInput);
            if(binReader1.ReadInt64() == 99068874991756337)
            {
                //fileInput
                //franceShaders
                //conversationsCNVPack
                //cinematicsCinPack
                //globalMap
                //franceMap
                //editNodesPack
                //gameTemplatesWSD

                franceShaders = new MemoryStream();
                franceShaders.Write(binReader1.ReadBytes(23066400), 0, 23066400);
                BinaryReader franceShadersReader = new BinaryReader(franceShaders);
                franceShaders.Seek(0, 0);
                treeView1.Nodes.Add("File #1: " + System.Text.Encoding.Default.GetString(franceShadersReader.ReadBytes(14)) + " Size: " + franceShaders.Length);

                conversationsCNVPack = new MemoryStream();
                conversationsCNVPack.Write(binReader1.ReadBytes(509408), 0, 509408);
                BinaryReader conversationsPackReader = new BinaryReader(conversationsCNVPack);
                conversationsCNVPack.Seek(0, 0);
                treeView1.Nodes.Add("File #2: " + System.Text.Encoding.Default.GetString(conversationsPackReader.ReadBytes(46)) + " File Size: " + conversationsCNVPack.Length);

                cinematicsCinPack = new MemoryStream();
                cinematicsCinPack.Write(binReader1.ReadBytes(11822096), 0, 11822096);
                BinaryReader cinematicsPackReader = new BinaryReader(cinematicsCinPack);
                cinematicsCinPack.Seek(0, 0);
                treeView1.Nodes.Add("File #3: " + System.Text.Encoding.Default.GetString(cinematicsPackReader.ReadBytes(29)) + " File Size: " + cinematicsCinPack.Length);

                globalMap = new MemoryStream();
                globalMap.Write(binReader1.ReadBytes(284528), 0, 284528);
                BinaryReader globalMapReader = new BinaryReader(globalMap);
                globalMap.Seek(0, 0);
                treeView1.Nodes.Add("File #4: " + System.Text.Encoding.Default.GetString(globalMapReader.ReadBytes(10)) + " File Size: " + globalMap.Length);


                franceMap = new MemoryStream();
                franceMap.Write(binReader1.ReadBytes(2817991), 0, 2817991);
                BinaryReader franceMapReader = new BinaryReader(franceMap);
                franceMap.Seek(0, 0);
                treeView1.Nodes.Add("File #5: " + System.Text.Encoding.Default.GetString(franceMapReader.ReadBytes(10)) + " File Size: " + franceMap.Length);

                editNodesPack = new MemoryStream();
                editNodesPack.Write(binReader1.ReadBytes(2606601), 0, 2606601);
                BinaryReader editNodesReader = new BinaryReader(editNodesPack);
                editNodesPack.Seek(0, 0);
                treeView1.Nodes.Add("File #6: " + System.Text.Encoding.Default.GetString(editNodesReader.ReadBytes(24)) + " File Size: " + editNodesPack.Length);

                gameTemplatesWSD = new MemoryStream();
                gameTemplatesWSD.Write(binReader1.ReadBytes(8204552), 0, 8204552);
                BinaryReader gameTemplatesReader = new BinaryReader(gameTemplatesWSD);
                gameTemplatesWSD.Seek(0, 0);
                treeView1.Nodes.Add("File #7: " + System.Text.Encoding.Default.GetString(gameTemplatesReader.ReadBytes(17)) + " File Size: " + gameTemplatesWSD.Length);


            }
            else {
                MessageBox.Show("This is not the Loosefiles_BinPC.pack. But if it actually is, let me know, because there's a problem.");
                fileInput.Flush();
                fileInput.Close();
                binReader1.Dispose();
                binReader1.Close();

            }
        }

          //////////////////////
         // Open Loose Files //
        //////////////////////
        private void openLoosefilesBinPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Sabtool_Settings.returnGamePath();
            switch (path)
            {
                case "PathInvalid":
                    MessageBox.Show("Has your game been moved? Your Saboteur Game Path is not valid, please re-select your Saboteur Game Path.");
                    break;
                case "NoSettingsFile":
                    MessageBox.Show("Please set the location of your Game Folder in Settings. Otherwise use Select Other and locate the file manually.");
                    break;
                default:
                    if (File.Exists(Path.Combine(path, "LuaScripts.luap")))
                    {
                        try
                        {
                            fileInput = File.Open(path + "\\France\\Loosefiles_BinPC.pack", FileMode.Open);
                            loadLoosefiles();
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show("An error occurred: '{0}'" + error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("File not found. Please double check your Game Path in settings. This system isn't perfect, you know.");
                    }
                    break;
            }
        }

          ///////////////////////
         // Select Other File //
        ///////////////////////
        private void chooseOtherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Pack files (*.pack)|*.pack";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInput = openFileDialog1.OpenFile();
                loadLoosefiles();
            }
        }






        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //fileInput
            //franceShaders
            //conversationsCNVPack
            //cinematicsCinPack
            //globalMap
            //franceMap
            //editNodesPack
            //gameTemplatesWSD


            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if(franceShaders != null)
                {
                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\France.shaders", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    franceShaders.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                } else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }

                if (conversationsCNVPack != null)
                {
                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\Cinematics\\Conversations\\"))
                    {
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\Cinematics\\Conversations\\");
                    }

                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\Cinematics\\Conversations\\Conversations.cnvpack", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    conversationsCNVPack.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                }
                else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }

                if (cinematicsCinPack != null)
                {
                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\Cinematics\\"))
                    {
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\Cinematics\\");
                    }

                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\Cinematics\\Cinematics.cinpack", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    cinematicsCinPack.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                }
                else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }

                if (globalMap != null)
                {
                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\global.map", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    globalMap.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                }
                else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }

                if (franceMap != null)
                {
                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\France.map", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    franceMap.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                }
                else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }

                if (editNodesPack != null)
                {
                    if (!Directory.Exists(folderBrowserDialog1.SelectedPath + "\\France\\EditNodes\\"))
                    {
                        Directory.CreateDirectory(folderBrowserDialog1.SelectedPath + "\\France\\EditNodes\\");
                    }

                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\France\\EditNodes\\EditNodes.pack", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    editNodesPack.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                }
                else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }

                if (gameTemplatesWSD != null)
                {
                    FileStream exportFile = new FileStream(folderBrowserDialog1.SelectedPath + "\\GameTemplates.wsd", FileMode.Create, FileAccess.Write);
                    MemoryStream convertedStream = new MemoryStream();
                    gameTemplatesWSD.CopyTo(convertedStream);
                    convertedStream.WriteTo(exportFile);
                }
                else
                {
                    MessageBox.Show("You must open the archive before extracting.");
                }
            }
        }
    }
}



