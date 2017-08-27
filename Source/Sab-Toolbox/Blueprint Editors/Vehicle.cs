using System;
using System.Collections.Generic;
using System.Collections;
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
    public partial class Vehicle : Form
    {
        public Stream fileInput; //data file
        public int fileCount; //amount of files extracted
        public string[][] originalColors;
        public List<string> listOfNames;
        public Hashtable fileNames; //hashtable to keep the files in order, name and file #
        public List<byte[]> listOfFileArrays; //data of each file as they are extracted

        public Vehicle()
        {
            InitializeComponent();
            listOfFileArrays = new List<byte[]>();
            listOfNames = new List<string>();
            fileNames = new Hashtable();
        }

        private void openLoosefilesBinPCpackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "Loosefiles_BinPC.pack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\France\\", "Loosefiles_BinPC.pack"), FileMode.Open); ;
                            extractGameTemplatesFromLooseFiles();
                            loadBlueprintsWSD();
                        }
                        catch (IOException error)
                        {
                            MessageBox.Show("Loosefiles_BinPC.pack is open in another program. Please close it and try again.");
                            Console.WriteLine("An IO error occurred: '{0}'", error);
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine("An error occurred: '{0}'", error);
                        }

                    }
                    else
                    {
                        MessageBox.Show("File not found. Please double check your Game Path in settings. This system isn't perfect, you know.");
                    }

                    break;
            }
        }

        public void extractGameTemplatesFromLooseFiles() //Extracts data for processing by another function.
        {

            BinaryReader binReader1 = new BinaryReader(fileInput);

            fileInput.Seek(41107032, 0); //Skip all the data prior to GameTemplates.wsd
            String versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(17)); //Check if we found the right file
            //MessageBox.Show(versionNumber);
            if (versionNumber.Equals("GameTemplates.wsd"))
            {
                fileInput.Seek(41107032, 0); //if yes, go back to the start of the file

                byte[] gameTemplatesArray = binReader1.ReadBytes(49311584 - 41107032);


                MemoryStream gameTemplatesStream = new MemoryStream(gameTemplatesArray);

                //Can write MemoryStream to a file. //Writes GameTemplates.wsd as extracted from Loosefiles.
                //FileStream banana = new FileStream("C:\\Users\\Dan\\Desktop\\balls.txt", FileMode.Create, FileAccess.Write);
                //gameTemplatesStream.WriteTo(banana);

                binReader1.Dispose();
                binReader1.Close();
                //fileInput.Flush();
                fileInput.Close();

                fileInput = gameTemplatesStream;
            }
            else
            {
                binReader1.Dispose();
                binReader1.Close();

                extractGameTemplatesFromModifiedLooseFiles();
            }

        }

        public void extractGameTemplatesFromModifiedLooseFiles()  //Extracts data for processing by another function.
        {

            BinaryReader binReader1 = new BinaryReader(fileInput);


            byte[] ByteBuffer = binReader1.ReadBytes(Convert.ToInt32(fileInput.Length));
            byte[] StringBytes = Encoding.UTF8.GetBytes("GameTemplates.wsd");
            int offset = 0;
            int i = 0;
            int j;
            Boolean found = false;
            for (i = 0; i <= (ByteBuffer.Length - StringBytes.Length); i++)
            {
                if (ByteBuffer[i] == StringBytes[0])
                {
                    for (j = 1; j < StringBytes.Length && ByteBuffer[i + j] == StringBytes[j]; j++) ;
                    if (j == StringBytes.Length)
                    {
                        //Console.WriteLine("String was found at offset {0}", i);
                        //Console.WriteLine(i);
                        found = true;
                        offset = i;
                    }
                }
            }

            if (found == true)
            {
                fileInput.Seek(offset, 0);
                byte[] gameTemplatesArray = binReader1.ReadBytes(Convert.ToInt32(fileInput.Length) - offset);
                MemoryStream gameTemplatesStream = new MemoryStream(gameTemplatesArray);


                //FileStream banana = new FileStream("C:\\Users\\Dan\\Desktop\\balls2.txt", FileMode.Create, FileAccess.Write);
                //gameTemplatesStream.WriteTo(banana);

                //fileInput.Flush();
                fileInput.Close();
                fileInput = gameTemplatesStream;
                binReader1.Dispose();
                binReader1.Close();

            }
            else
            {
                MessageBox.Show("GameTemplates.wsd not found in Loosefiles_BinPC.Pack. Are you sure this is the correct file?");
                binReader1.Dispose();
                binReader1.Close();
            }


        }

        public void loadBlueprintsWSD() //Processes the extracted WSD file.
        {
            listOfFileArrays.Clear();
            listOfNames.Clear();
            fileNames.Clear();
            originalColors = null;
            if (Blueprints.Nodes.Count > 0) { Blueprints.Nodes[0].Remove(); }
            while (dataGridView1.Rows.Count > 0) { dataGridView1.Rows.RemoveAt(0); }

            BinaryReader binReader1 = new BinaryReader(fileInput);

            String versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(4));

            if (!versionNumber.Equals("AULB")) //Check position in the file, if it's not at the start, skip and check if we have skipped to the correct start position
            {
                binReader1.ReadBytes(116);
                versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(4));
            }
            if (versionNumber.Equals("AULB"))
            {
                fileCount = binReader1.ReadInt32(); //Read how many files will need to be extracted

                //for (int i = 0; i < fileCount; i++) //Because the filecount is not always correct.. we have to use a more dynamic way to iterate
                int i = 0;
                int carCount = 0;
                while (fileInput.Position != fileInput.Length - 1 && fileInput.Position != fileInput.Length) //While we're not at the end..
                {

                    int size = binReader1.ReadInt32();
                    String midValues = "" + binReader1.ReadInt64();
                    int nameLength = binReader1.ReadInt32();

                    String name = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(nameLength - 1));
                    listOfNames.Add(name);

                    binReader1.ReadByte();

                    int subtypeLength = binReader1.ReadInt32();
                    string subtype = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(subtypeLength - 1)); //Read in the subtype
                    binReader1.ReadByte();


                    if (subtype == "CAR" || subtype == "APC" || subtype == "TANK" || subtype == "TRUCK")
                    {
                        fileNames.Add(name, carCount); //Add name and file number to names hash table
                        carCount++;
                        if (Blueprints.Nodes.ContainsKey(subtype)) //Check if the treeview has this subtype already.
                        {
                            Blueprints.Nodes[subtype].Nodes.Add(name); //Add the file to that subtype
                        }
                        else
                        {
                            Blueprints.Nodes.Add(subtype, subtype); //Create the subtype first
                            Blueprints.Nodes[subtype].Nodes.Add(name); //Add the file to that subtype
                        }


                        byte[] blueprint = null;
                        blueprint = binReader1.ReadBytes((size) - 12 - nameLength - 4 - subtypeLength); //Read in the header data.


                        listOfFileArrays.Add(blueprint); //Add extracted file's bytes to list of the data files

                    }
                    else
                    {
                        byte[] blueprint = null;
                        blueprint = binReader1.ReadBytes((size) - 12 - nameLength - 4 - subtypeLength); //Read in the header data.
                    }

                    if (fileInput.Position != fileInput.Length - 1 && fileInput.Position != fileInput.Length)
                    {
                        while (binReader1.ReadInt32() == 8)
                        {
                            binReader1.ReadInt64();
                            //Can add line to add current i to List to keep track of every file that has this feature.
                        }
                        fileInput.Seek(fileInput.Position - 4, 0);
                    }
                    i++;
                }

                Blueprints.Sort();
                binReader1.Dispose();
                binReader1.Close();
                GC.Collect();

                if (carCount == 0)
                {
                    MessageBox.Show("No Will To Fight blueprints were found in this file.");
                }
            }
            else
            {
                MessageBox.Show("This is not a blueprint archive, valid files are GameTemplates.wsd and Loosefiles_BinPC.pack.");
            }

        }

        private void openSelectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Pack files (*.)|*.pack|WSD files (*.)|*.wsd";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInput = openFileDialog1.OpenFile();
                string extension = openFileDialog1.FileName;
                extension = extension.Substring(extension.IndexOf('.'), extension.Length - extension.IndexOf('.'));
                if (extension == ".wsd")
                {
                    loadBlueprintsWSD();
                }
                else
                {
                    extractGameTemplatesFromLooseFiles();
                    loadBlueprintsWSD();
                }
            }
        }
        private void Vehicle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileInput != null)
            {
                fileInput.Dispose();
                fileInput.Close();
                fileInput = null;
            }
        }


    }
}
