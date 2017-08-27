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
    public partial class WillToFight : Form
    {
        public Stream fileInput; //data file
        public int fileCount; //amount of files extracted
        public string[][] originalColors;
        public List<string> listOfNames;
        public Hashtable fileNames; //hashtable to keep the files in order, name and file #
        public List<byte[]> listOfFileArrays; //data of each file as they are extracted


        public WillToFight()
        {
            //MessageBox.Show(convertHexByteToInt("FF").ToString());
            InitializeComponent();
            listOfFileArrays = new List<byte[]>();
            listOfNames = new List<string>();
            fileNames = new Hashtable();
        }


          /* ======================= */
         /* Find and Open Loosfiles */
        /* ======================= */
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
                        catch(IOException error)
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

          /* ================================== */
         /* Extract Blueprints from Loosefiles */
        /* ================================== */
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

          /* =============================================== */
         /* Extract Blueprints from Non-Standard Loosefiles */
        /* =============================================== */
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

          /* ======================================= */
         /* Load Will To Fight Data From Blueprints */
        /* ======================================= */
        public void loadBlueprintsWSD() //Processes the extracted WSD file.
        {
            listOfFileArrays.Clear();
            listOfNames.Clear();
            fileNames.Clear();
            originalColors = null;
            if(Blueprints.Nodes.Count > 0) { Blueprints.Nodes[0].Remove(); }
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
                int wtfCount = 0;
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


                    if (subtype == "WillToFight")
                    {
                        fileNames.Add(name, wtfCount); //Add name and file number to names hash table
                        wtfCount++;
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

                if(wtfCount == 0)
                {
                    MessageBox.Show("No Will To Fight blueprints were found in this file.");
                }
            }
            else
            {
                MessageBox.Show("This is not a blueprint archive, valid files are GameTemplates.wsd and Loosefiles_BinPC.pack.");
            }

        }

          /* ================================= */
         /* Retrieve Current Colors from Cells */
        /* ================================= */
        private string readColorsFromDataGrid()
        {
            string data = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                data += row.Cells[2].Value.ToString() + ';';
            }
            data = data.Substring(0, data.Length - 1);

            return data;
        }

        /* ============================================= */
        /* Apply Current Colors to Blueprints in Memory */
        /* =========================================== */
        private byte[] setNewColorsToStream(byte[] file, string allColors)
        {
            MemoryStream stream = new MemoryStream(file);
            //FileStream banana = new FileStream("C:\\Users\\Dan\\Desktop\\before.file", FileMode.Create, FileAccess.Write);
            //stream.WriteTo(banana);

            string[] allColorsArray = allColors.Split(';');
            int colorCount = 0;
            using (BinaryReader reader1 = new BinaryReader(stream))
            {
                int count = reader1.ReadInt32();

                for (int i = 0; i < count; i++)
                {

                    Encoding.Default.GetString(reader1.ReadBytes(4));
                    int dataSize = reader1.ReadInt32();
                    int offset = Convert.ToInt32(stream.Position);
                    string data = "";

                    if (dataSize == 4)
                    {
                        byte byte1 = reader1.ReadByte();
                        byte byte2 = reader1.ReadByte();
                        byte byte3 = reader1.ReadByte();
                        byte byte4 = reader1.ReadByte();
                        data += convertIntToHexByte(Convert.ToInt32(byte1));
                        data += convertIntToHexByte(Convert.ToInt32(byte2));
                        data += convertIntToHexByte(Convert.ToInt32(byte3));
                        data += convertIntToHexByte(Convert.ToInt32(byte4));

                        if (byte4 == 255)
                        {
                            if (colorCount < allColorsArray.Length && allColorsArray[colorCount] != data)
                            {

                                if (colorCount == 0)
                                {
                                    //MessageBox.Show(convertHexByteToInt(allColorsArray[colorCount].Substring(0, 2)).ToString());
                                    //MessageBox.Show(convertHexByteToInt(allColorsArray[colorCount].Substring(2, 2)).ToString());
                                    //MessageBox.Show(convertHexByteToInt(allColorsArray[colorCount].Substring(4, 2)).ToString());
                                    //MessageBox.Show(convertHexByteToInt(allColorsArray[colorCount].Substring(6, 2)).ToString());
                                }
                                file[offset] = Convert.ToByte(convertHexByteToInt(allColorsArray[colorCount].Substring(0, 2)));
                                file[offset + 1] = Convert.ToByte(convertHexByteToInt(allColorsArray[colorCount].Substring(2, 2)));
                                file[offset + 2] = Convert.ToByte(convertHexByteToInt(allColorsArray[colorCount].Substring(4, 2)));
                                file[offset + 3] = Convert.ToByte(convertHexByteToInt(allColorsArray[colorCount].Substring(6, 2)));

                                
                            }
                            colorCount++;
                        }
                    }
                    else
                    {
                        reader1.ReadBytes(dataSize);
                    }
                }
                //MemoryStream stream2 = new MemoryStream(file);
                //FileStream banana2 = new FileStream("C:\\Users\\Dan\\Desktop\\after.file", FileMode.Create, FileAccess.Write);
                //stream2.WriteTo(banana2);
                //stream.Close();
                //stream2.Close();
            }
            MessageBox.Show("Color Changed in Memory");
            return file;
        }

          /* ================================================== */
         /* Decide and Retrieve What Blueprint Data Are Colors */
        /* ================================================== */
        private string returnAllModifiableColors(MemoryStream file)
        {
            string allColors = "";

            using (BinaryReader reader1 = new BinaryReader(file))
            {
                int count = reader1.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    Encoding.Default.GetString(reader1.ReadBytes(4));
                    int dataSize = reader1.ReadInt32();
                    string data = "";

                    if (dataSize == 4)
                    {
                        byte byte1 = reader1.ReadByte();
                        byte byte2 = reader1.ReadByte();
                        byte byte3 = reader1.ReadByte();
                        byte byte4 = reader1.ReadByte();
                        data += convertIntToHexByte(Convert.ToInt32(byte1));
                        data += convertIntToHexByte(Convert.ToInt32(byte2));
                        data += convertIntToHexByte(Convert.ToInt32(byte3));
                        data += convertIntToHexByte(Convert.ToInt32(byte4));

                        if (byte4 == 255)
                        {
                            allColors += data + ';';
                        }
                    }
                    else
                    {
                        reader1.ReadBytes(dataSize);
                    }
                }
                allColors = allColors.Substring(0, allColors.Length - 1);
                return allColors;
            }
        }

          /* ================================================================ */
         /* Decides What Data Are Colors and Retrieves Hash, Size, and Color */
        /* ================================================================ */
        private List<string> findModifiableColors(MemoryStream file)
        {
            List<string> returnList = new List<string>();

            using (BinaryReader reader1 = new BinaryReader(file))
            {
                int count = reader1.ReadInt32();
                //MessageBox.Show(count.ToString());

                for (int i = 0; i < count; i++)
                {
                    string hash = Encoding.Default.GetString(reader1.ReadBytes(4));
                    int dataSize = reader1.ReadInt32();
                    string data = "";

                    if (dataSize == 4)
                    {
                        byte byte1 = reader1.ReadByte();
                        byte byte2 = reader1.ReadByte();
                        byte byte3 = reader1.ReadByte();
                        byte byte4 = reader1.ReadByte();


                        data += convertIntToHexByte(Convert.ToInt32(byte1));
                        data += convertIntToHexByte(Convert.ToInt32(byte2));
                        data += convertIntToHexByte(Convert.ToInt32(byte3));
                        data += convertIntToHexByte(Convert.ToInt32(byte4));
                        //MessageBox.Show(data);

                        if (byte4 == 255)
                        {
                            returnList.Add(hash + ';' + dataSize + ';' + data);
                        }
                    }
                    else
                    {
                        reader1.ReadBytes(dataSize);
                    }
                }
                return returnList;
            }
        }

          /* ======================================= */
         /* Sets Blueprint Data to Datagrid on Load */
        /* ======================================= */
        private void displayFileToDataGrid(int index)
        {
            if (listOfFileArrays[index] != null)
            {
                MemoryStream file = new MemoryStream(listOfFileArrays[index]);
                List<string> list = findModifiableColors(file);

                int i = 0;
                dataGridView1.Rows.Clear();
                foreach (string line in list)
                {
                    //add code to check if hash is familiar and tell what it is
                    string[] data = line.Split(';');
                    //foreach(string strng in data) { MessageBox.Show(strng); }
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = "Unknown " + i;
                    dataGridView1.Rows[i].Cells[2].Value = data[2];
                    int r = convertHexByteToInt(data[2].Substring(0, 2));
                    int g = convertHexByteToInt(data[2].Substring(2, 2));
                    int b = convertHexByteToInt(data[2].Substring(4, 2));
                    int a = convertHexByteToInt(data[2].Substring(6, 2));
                    Color color1 = Color.FromArgb(a, r, g, b);
                    //pictureBox2.BackColor = color1;
                    dataGridView1.Rows[i].Cells[3].Style.ForeColor = color1;
                    dataGridView1.Rows[i].Cells[3].Style.BackColor = color1;
                    dataGridView1.Rows[i].Cells[3].Style.SelectionBackColor = color1;
                    dataGridView1.Rows[i].Cells[3].Style.SelectionForeColor = color1;


                    i++;
                }

            }
        }

          /* ============================== */
         /* Open User Selected Loosefiles */
        /* ============================== */
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

          /* =============== */
         /* On Window Close */
        /* =============== */
        private void WillToFight_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileInput != null)
            {
                fileInput.Dispose();
                fileInput.Close();
                fileInput = null;
            }
        }


          /* ================================================================================ */
         /* Open File From Blueprint List and Display to Grid */
        /* ================================================================================ */
        private void Blueprints_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                int index = e.Node.Index;
                string name = Blueprints.Nodes[0].Nodes[index].Text;
                int finalIndex = returnHashTableKeyIndex(Blueprints.SelectedNode.Text, fileNames);
                displayFileToDataGrid(finalIndex);
                //MessageBox.Show(index.ToString());

                if (originalColors == null)
                {
                    originalColors = new string[Blueprints.Nodes[0].Nodes.Count][];
                }


                string allColors = returnAllModifiableColors(new MemoryStream(listOfFileArrays[finalIndex]));
                string[] allColorsArray = allColors.Split(';');

                if (originalColors != null)
                {
                    if (originalColors[index] == null)
                    {
                        //originalColors[index] = new string[allColorsArray.Length];
                        originalColors[index] = allColorsArray;
                    }

                }
                label3.Text = e.Node.Text;
            }
        }

        /*
        * @name Convert Int To Hex Byte
        * @return string Hex Byte converted from input integer.
        * @param int The integer to be converted to a Hex byte.
        */
        public string convertIntToHexByte(int inputValue)
        {
            string finalHexByte = "";
            if (inputValue > 255 || inputValue < 0)
            {
                Console.WriteLine("Input must be between 0 and 255. These are the values one byte can support.");
            }
            else
            {
                int zeroPlace = inputValue % 16;
                int inputDiv1 = inputValue / 16;

                int tenthPlace = inputDiv1 % 16;

                finalHexByte += convertIntToHexDigit(tenthPlace) + convertIntToHexDigit(zeroPlace);
            }

            return finalHexByte;
        }


        /*
        * @name Convert Int To Hex Digit
        * @return string Hex Digit converted from input Integer.
        * @param int The integer to be converted to Hex.
        */
        public string convertIntToHexDigit(int inputValue)
        {
            string outputString = "";
            if (inputValue > 15 || inputValue < 0)
            {
                Console.WriteLine("Input value must be between 0 and 15. These are the values a hex digit can represent.");
            }
            else
            {
                if (inputValue < 10)
                {
                    outputString += inputValue;
                }
                else
                {
                    if (inputValue == 10) { outputString += "A"; }
                    if (inputValue == 11) { outputString += "B"; }
                    if (inputValue == 12) { outputString += "C"; }
                    if (inputValue == 13) { outputString += "D"; }
                    if (inputValue == 14) { outputString += "E"; }
                    if (inputValue == 15) { outputString += "F"; }
                }
            }
            return outputString;
        }

          /* =============== */
         /* Hex Byte To Int */
        /* =============== */
        public int convertHexByteToInt(string hex)
        {
            int sum = 0;

            for (int i = 0; i < hex.Length; i++)
            {
                char letter = hex[hex.Length - 1 - i];
                int value = convertHexDigitToInt(letter);

                value *= (Convert.ToInt32(Math.Pow(16, i)));
                sum += value;
            }

            return sum;
        }

          /* ================ */
         /* Hex Digit To Int */
        /* ================ */
        public int convertHexDigitToInt(char inputValue)
        {
            int outputNumber = 0;

            if (inputValue == '1') { outputNumber = 1; }
            if (inputValue == '2') { outputNumber = 2; }
            if (inputValue == '3') { outputNumber = 3; }
            if (inputValue == '4') { outputNumber = 4; }
            if (inputValue == '5') { outputNumber = 5; }
            if (inputValue == '6') { outputNumber = 6; }
            if (inputValue == '7') { outputNumber = 7; }
            if (inputValue == '8') { outputNumber = 8; }
            if (inputValue == '9') { outputNumber = 9; }
            if (inputValue == 'A') { outputNumber = 10; }
            if (inputValue == 'B') { outputNumber = 11; }
            if (inputValue == 'C') { outputNumber = 12; }
            if (inputValue == 'D') { outputNumber = 13; }
            if (inputValue == 'E') { outputNumber = 14; }
            if (inputValue == 'F') { outputNumber = 15; }

            return outputNumber;
        }

          /* ================ */
         /* Set Color Dialog */
        /* ================ */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.ColumnIndex.ToString());
            if (e.ColumnIndex >= 2 && e.RowIndex >= 0)// && dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    Color selection = colorDialog1.Color;
                    int r = selection.R;
                    int g = selection.G;
                    int b = selection.B;
                    int a = selection.A;
                    string color = convertIntToHexByte(r) + convertIntToHexByte(g) + convertIntToHexByte(b) + convertIntToHexByte(a);
                    //pictureBox2.BackColor = Color.FromArgb(a, r, g, b);

                    int rowIndex;
                    int columnIndex;
                    if(e.ColumnIndex == 2)
                    {
                        rowIndex = e.RowIndex;
                        columnIndex = e.ColumnIndex;
                    }
                    else
                    {
                        rowIndex = e.RowIndex;
                        columnIndex = e.ColumnIndex -1;
                    }

                    dataGridView1.Rows[rowIndex].Cells[columnIndex + 1].Style.ForeColor = Color.FromArgb(a, r, g, b);
                    dataGridView1.Rows[rowIndex].Cells[columnIndex + 1].Style.BackColor = Color.FromArgb(a, r, g, b);
                    dataGridView1.Rows[rowIndex].Cells[columnIndex + 1].Style.SelectionForeColor = Color.FromArgb(a, r, g, b);
                    dataGridView1.Rows[rowIndex].Cells[columnIndex + 1].Style.SelectionBackColor = Color.FromArgb(a, r, g, b);

                    dataGridView1.Rows[rowIndex].Cells[columnIndex].Value = color;

                    //MessageBox.Show(Blueprints.SelectedNode.Index.ToString());
                    //listOfFileArrays[Blueprints.SelectedNode.Index] = setNewColorsToStream(listOfFileArrays[Blueprints.SelectedNode.Index], readColorsFromDataGrid());
                    int fileIndex = returnHashTableKeyIndex(Blueprints.SelectedNode.Text, fileNames);

                    List<string> names = listOfNames;
                    Hashtable namesTable = fileNames;
                    List<byte[]> files = listOfFileArrays;

                    int nodeIndex = Blueprints.SelectedNode.Index;

                    int originalFileLength = listOfFileArrays[fileIndex].Length;

                    byte[] returnFile = setNewColorsToStream(listOfFileArrays[fileIndex], readColorsFromDataGrid());

                    int newFileLength = returnFile.Length;

                   if (listOfFileArrays[fileIndex].Length == returnFile.Length)
                    {
                        listOfFileArrays[fileIndex] = returnFile;
                    }
                    else
                    {
                        MessageBox.Show("Function attempted to save color modification to wrong file.");
                    }
                }
            }
        }


        private int returnHashTableKeyIndex(string value, Hashtable table)
        {
            int key = (int)table[value];
            //string name = listOfNames[key];
            return key;
        }

          /* ========= */
         /* Save File */
        /* ========= */
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listOfFileArrays.Count > 0)
            {
                //MessageBox.Show(returnGamePath());
                string path = Sabtool_Settings.returnGamePath();
                if (path != "PathInvalid" && path != "NoSettingsFile") //if settings file exists, use it's path to get the loosefiles_BinPC.pack
                {
                    path += '\\' + "France" + '\\';
                    //MessageBox.Show(path);
                }

                string loosefilesPath = Path.Combine(path, "loosefiles_BinPC.pack");
                string backupLoosefilesPath = Path.Combine(path, "loosefiles_BinPC.copy.pack");

                if (!File.Exists(backupLoosefilesPath)) //if there's no backup of the file being saved, make a copy
                {
                    File.Copy(loosefilesPath, Path.Combine(path, "loosefiles_BinPC.copy.pack"));
                }

                if (File.Exists(loosefilesPath))
                {
                    FileStream file = File.Open(loosefilesPath, FileMode.Open, FileAccess.ReadWrite);

                    BinaryReader binReader1 = new BinaryReader(file);
                    BinaryWriter binWriter1 = new BinaryWriter(file);

                    binWriter1.Seek(41107032, SeekOrigin.Begin);

                    String versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(4));

                    if (!versionNumber.Equals("AULB")) //Check position in the file, if it's not at the start, skip and check if we have skipped to the correct start position
                    {
                        binReader1.ReadBytes(116);
                        versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(4));
                    }
                    if (versionNumber.Equals("AULB"))
                    {
                        fileCount = binReader1.ReadInt32(); //Read how many files will need to be extracted

                        int howManyEditsSoFar = 0;

                        
                        //int i = 0;
                        //while (file.Position != file.Length - 1 && file.Position != file.Length) //While we're not at the end..
                        for (int i = 0; i < listOfNames.Count; i++)
                        {
                            {
                                List<string> a = listOfNames;
                                List<byte[]> b = listOfFileArrays;


                                int size = binReader1.ReadInt32();
                                String midValues = "" + binReader1.ReadInt64();

                                int nameLength = binReader1.ReadInt32();
                                String name = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(nameLength - 1));
                                binReader1.ReadByte();

                                int subtypeLength = binReader1.ReadInt32();
                                string subtype = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(subtypeLength - 1)); //Read in the subtype
                                binReader1.ReadByte();

                                if (subtype == "WillToFight")
                                {
                                    int fileLength = listOfFileArrays[howManyEditsSoFar].Length;
                                    binWriter1.Write(listOfFileArrays[howManyEditsSoFar]);
                                    howManyEditsSoFar++;
                                    //byte[] blueprint = null;
                                    //blueprint = binReader1.ReadBytes((size) - 12 - nameLength - 4 - subtypeLength); //Read in the header data.

                                }
                                else
                                {
                                    byte[] blueprint = null;
                                    blueprint = binReader1.ReadBytes((size) - 12 - nameLength - 4 - subtypeLength); //Read in the header data.
                                }

                                if (file.Position != file.Length - 1 && file.Position != file.Length)
                                {
                                    while (binReader1.ReadInt32() == 8)
                                    {
                                        binReader1.ReadInt64();
                                        //Can add line to add current i to List to keep track of every file that has this feature.
                                    }
                                    file.Seek(file.Position - 4, 0);
                                }
                                //i++;
                            }
                        }
                        binReader1.Dispose();
                        binReader1.Close();
                        binWriter1.Close();
                        GC.Collect();
                    }
                    MessageBox.Show("Saving File Complete");
                }
            }
            else
            {
                MessageBox.Show("Please open the loosefiles_BinPC.pack file first.");
            }
        }


          /* ================= */
         /* Set Display Color */
        /* ================= */
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;

                if (columnIndex == 2 && rowIndex >= 0 && dataGridView1.Rows[rowIndex].Cells[columnIndex].Value != null)
                {
                    string color = dataGridView1.CurrentCell.Value.ToString();
                    int r = convertHexByteToInt(color.Substring(0, 2));
                    int g = convertHexByteToInt(color.Substring(2, 2));
                    int b = convertHexByteToInt(color.Substring(4, 2));
                    int a = convertHexByteToInt(color.Substring(6, 2));
                    Color color1 = Color.FromArgb(a, r, g, b);
                    dataGridView1.Rows[rowIndex].Cells[columnIndex + 1].Style.ForeColor = color1;

                }
            }
        }


          /* ================ */
         /* Cell Right Click */
        /* ================ */
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add(new MenuItem("Revert to original color"));

                int rowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                int columnIndex = dataGridView1.HitTest(e.X, e.Y).ColumnIndex;

                menu.MenuItems[0].Click += (sender1, e1) => revertColor(rowIndex, columnIndex);
                //menu.MenuItems.Add(new MenuItem("Revert all to original colors"));
                //menu.MenuItems.Add(new MenuItem("Revert to original color from backup file"));

                if (rowIndex > -1 && columnIndex > 1)
                {
                    menu.Show(dataGridView1, new Point(e.X, e.Y));
                }

            }
        }


          /* ============ */
         /* Revert Color */
        /* ============ */
        private void revertColor(int row, int column)
        {
            if (label3.Text != "")
            {
                int index = (int)fileNames[label3.Text];
                string[][] files = originalColors;

                if (!originalColors[Blueprints.SelectedNode.Index][row].Equals(dataGridView1.Rows[row].Cells[2].Value))
                {
                    string originalColor = originalColors[Blueprints.SelectedNode.Index][row];

                    dataGridView1.Rows[row].Cells[column].Value = originalColor;
                    Color newColor = retrieveColorFromString(originalColor);

                    if(column == 2) { column++; }
                    dataGridView1.Rows[row].Cells[column].Style.ForeColor = newColor;
                    dataGridView1.Rows[row].Cells[column].Style.BackColor = newColor;
                    dataGridView1.Rows[row].Cells[column].Style.SelectionForeColor = newColor;
                    dataGridView1.Rows[row].Cells[column].Style.SelectionBackColor = newColor;

                }
                else
                {
                    MessageBox.Show("Already the original color.");
                }
            }
        }

          /* ============================ */
         /* Return Color from Hex String */
        /* ============================ */
        private Color retrieveColorFromString(string hexColor)
        {
            int r = convertHexByteToInt(hexColor.Substring(0, 2));
            int g = convertHexByteToInt(hexColor.Substring(2, 2));
            int b = convertHexByteToInt(hexColor.Substring(4, 2));
            int a = convertHexByteToInt(hexColor.Substring(6, 2));
            return Color.FromArgb(a, r, g, b);
        }

        /*
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(returnGamePath());
            string path = returnGamePath();
            if (path != "PathInvalid" && path != "NoSettingsFile") //if settings file exists, use it's path to get the loosefiles_BinPC.pack
            {
                path += '\\' + "France" + '\\';
                MessageBox.Show(path);
            }

            string loosefilesPath = Path.Combine(path, "loosefiles_BinPC.pack");

            if (!File.Exists(loosefilesPath)) //if there's no backup of the file being saved, make a copy
            {
                File.Copy(loosefilesPath, Path.Combine(path, "loosefiles_BinPC.copy.pack"));
            }

            if(File.Exists(loosefilesPath))
            {
                FileStream file = File.Open(loosefilesPath, FileMode.Open, FileAccess.ReadWrite);
                BinaryWriter writer1 = new BinaryWriter(file);
                writer1.Seek(41107032, SeekOrigin.Begin);
                writer1.Seek(4, SeekOrigin.Current); //skips fake versionNumber
                writer1.Seek(116, SeekOrigin.Current); //skips to versionNumber
                writer1.Seek(4, SeekOrigin.Current); //skips versionNumber
                writer1.Seek(4, SeekOrigin.Current); //skips fileCount
                byte[] kitty = { Convert.ToByte('H'), Convert.ToByte('e'), Convert.ToByte('l'), Convert.ToByte('l'), Convert.ToByte('o'), Convert.ToByte('K'), Convert.ToByte('i'), Convert.ToByte('t'), Convert.ToByte('t'), Convert.ToByte('y') };
                byte[] eight = { Convert.ToByte('U'), Convert.ToByte('U'), Convert.ToByte('U'), Convert.ToByte('U'), Convert.ToByte('U'), Convert.ToByte('U'), Convert.ToByte('U'), Convert.ToByte('U') };
                //writer1.Write(kitty);

                int howManyEditsSoFar = 0;
                for(int i = 0; i < isWillToFight.Count; i++)
                {
                    writer1.Seek(4, SeekOrigin.Current); //skips size
                    //writer1.Seek(8, SeekOrigin.Current); //skips midValues
                    writer1.Write(eight);
                    writer1.Seek(4, SeekOrigin.Current); //skips nameLength
                    writer1.Seek(listOfNames[i].Length + 1, SeekOrigin.Current); //skips name and null character
                    writer1.Seek(4, SeekOrigin.Current); //skips subTypeLength
                    writer1.Seek(listOfSubTypes[i].Length + 1, SeekOrigin.Current); //skips subtype and null character


                    //MessageBox.Show("NameLength:" + listOfNames[i].Length);
                    //MessageBox.Show("SubTypeLength:" + listOfSubTypes[i].Length);


                    if (isWillToFight[i] == true)
                    {

                        //MessageBox.Show("File Has Been Edited");
                        //----writer1.Write(listOfFileArrays[howManyEditsSoFar]);
                        //if(fileSizes[i] != listOfFileArrays[howManyEditsSoFar].Length) { MessageBox.Show("FileSize:" + fileSizes[i] + " ArrayFileSize:" + listOfFileArrays[howManyEditsSoFar]); }
                        writer1.Seek(fileSizes[i], SeekOrigin.Current);
                        //MessageBox.Show("Modified FileLength:" + listOfFileArrays[howManyEditsSoFar].Length);
                        //if (howManyEditsSoFar == 0) { writer1.Write(kitty); }
                        howManyEditsSoFar++;
                    } else
                    {
                        writer1.Seek(fileSizes[i], SeekOrigin.Current);
                        //MessageBox.Show("Normal FileLength:" + fileSizes[i]);
                    }

                }

                writer1.Close();
                file.Close();

            }
            MessageBox.Show("Saving File Complete");
        }
        */

        /*
        //This should be moved to a new function that modifies the loosefiles memory stream and checks each piece for changes
        //and modifies the appropriate bytes to match the new  colors that have been selected
        if(originalColors != null) //if we have saved color lists
        {
            for(int i = 0; i < Blueprints.Nodes[0].Nodes.Count; i++)
            {
                if(originalColors[i] != null) //if there is a saved color list
                {

                    string newColorList = returnAllModifiableColors(new MemoryStream(listOfFileArrays[i]));
                    string[] newColorArray = newColorList.Split(';');

                    bool similarityCheck = newColorArray.SequenceEqual(originalColors[i]);


                }
            }
        }
        */

    }
}
