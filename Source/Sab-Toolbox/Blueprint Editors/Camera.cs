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
    public partial class Camera : Form
    {
        public Stream fileInput; //data file
        public int fileCount; //amount of files extracted
        public string[][] originalStats;
        public List<string> listOfNames;
        public Hashtable fileNames; //hashtable to keep the files in order, name and file #
        public List<byte[]> listOfFileArrays; //data of each file as they are extracted
        public Hashtable statHashReference;
        public Hashtable hashDataNames;

        public Camera()
        {
            InitializeComponent();
            listOfFileArrays = new List<byte[]>();
            listOfNames = new List<string>();
            fileNames = new Hashtable();
            statHashReference = new Hashtable();
            hashDataNames = new Hashtable();

            statHashReference.Add("E6E4528D", "12 bytes");
            statHashReference.Add("694B8014", "12 bytes");

            hashDataNames.Add("E6E4528D", "XYZ Position");
            hashDataNames.Add("694B8014", "XYZ Rotation");
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
            originalStats = null;
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


                    if (subtype == "CameraSettings")
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

                if (wtfCount == 0)
                {
                    MessageBox.Show("No Will To Fight blueprints were found in this file.");
                }
            }
            else
            {
                MessageBox.Show("This is not a blueprint archive, valid files are GameTemplates.wsd and Loosefiles_BinPC.pack.");
            }

        }

        private void Camera_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileInput != null)
            {
                fileInput.Dispose();
                fileInput.Close();
                fileInput = null;
            }
        }

        private void Blueprints_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                int index = e.Node.Index;
                string name = Blueprints.Nodes[0].Nodes[index].Text;
                int finalIndex = returnHashTableKeyIndex(Blueprints.SelectedNode.Text, fileNames);
                displayFileToDataGrid(finalIndex);
                //MessageBox.Show(index.ToString());

                if (originalStats == null)
                {
                    originalStats = new string[Blueprints.Nodes[0].Nodes.Count][];
                }


                string allStats = returnAllModifiableStats(new MemoryStream(listOfFileArrays[finalIndex]));
                if (allStats != null)
                {
                    string[] allStatsArray = allStats.Split(';');

                    if (originalStats != null)
                    {
                        if (originalStats[index] == null)
                        {
                            //originalColors[index] = new string[allColorsArray.Length];
                            originalStats[index] = allStatsArray;
                        }

                    }
                }

                label3.Text = e.Node.Text;
            }
        }

        private void displayFileToDataGrid(int index)
        {
            if (listOfFileArrays[index] != null)
            {
                MemoryStream file = new MemoryStream(listOfFileArrays[index]);
                List<string> list = findModifiableStats(file);

                int i = 0;
                dataGridView1.Rows.Clear();
                foreach (string line in list)
                {
                    //add code to check if hash is familiar and tell what it is
                    string[] data = line.Split(';');
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells[0].Value = i;
                    dataGridView1.Rows[i].Cells[1].Value = data[1];
                    dataGridView1.Rows[i].Cells[2].Value = data[2];
                    dataGridView1.Rows[i].Cells[3].Value = data[3];
                    dataGridView1.Rows[i].Cells[4].Value = data[4];
                    dataGridView1.Rows[i].Cells[5].Value = data[5];
                    i++;
                }

            }
        }


        private List<string> findModifiableStats(MemoryStream file)
        {
            List<string> returnList = new List<string>();

            using (BinaryReader reader1 = new BinaryReader(file))
            {
                int count = reader1.ReadInt32();
                //MessageBox.Show(count.ToString());

                for (int i = 0; i < count; i++)
                {
                    string hash = Encoding.Default.GetString(reader1.ReadBytes(4));
                    file.Seek(-4, SeekOrigin.Current);
                    byte number1 = reader1.ReadByte();
                    byte number2 = reader1.ReadByte();
                    byte number3 = reader1.ReadByte();
                    byte number4 = reader1.ReadByte();
                    string hexString = convertIntToHexByte(number1) + convertIntToHexByte(number2) + convertIntToHexByte(number3) + convertIntToHexByte(number4);
                    int dataSize = reader1.ReadInt32();
                    //string data = "";

                    //if(statHashReference.Contains(hash))
                    if (statHashReference.Contains(hexString))
                    {
                        //switch((string)statHashReference[hash])
                        switch ((string)statHashReference[hexString])
                        {
                            case "bool":
                                {
                                    bool val = reader1.ReadBoolean();
                                    string hex = val ? "01" : "00";
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + val);
                                    break;
                                }
                            case "int":
                                {
                                    int numb = reader1.ReadInt32();
                                    file.Seek(-4, SeekOrigin.Current);
                                    byte num1 = reader1.ReadByte();
                                    byte num2 = reader1.ReadByte();
                                    byte num3 = reader1.ReadByte();
                                    byte num4 = reader1.ReadByte();
                                    string hex = convertIntToHexByte(num1) + convertIntToHexByte(num2) + convertIntToHexByte(num3) + convertIntToHexByte(num4);
                                    //returnList.Add(hash + ';' + dataSize + ';' + num);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + numb);
                                    break;
                                }
                            case "float":
                                {
                                    float numb = reader1.ReadSingle();
                                    file.Seek(-4, SeekOrigin.Current);
                                    byte num1 = reader1.ReadByte();
                                    byte num2 = reader1.ReadByte();
                                    byte num3 = reader1.ReadByte();
                                    byte num4 = reader1.ReadByte();
                                    string hex = convertIntToHexByte(num1) + convertIntToHexByte(num2) + convertIntToHexByte(num3) + convertIntToHexByte(num4);
                                    //returnList.Add(hash + ';' + dataSize + ';' + num);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + numb);
                                    break;
                                }
                            case "double":
                                {
                                    double numb = reader1.ReadDouble();
                                    file.Seek(-8, SeekOrigin.Current);
                                    byte num1 = reader1.ReadByte();
                                    byte num2 = reader1.ReadByte();
                                    byte num3 = reader1.ReadByte();
                                    byte num4 = reader1.ReadByte();
                                    byte num5 = reader1.ReadByte();
                                    byte num6 = reader1.ReadByte();
                                    byte num7 = reader1.ReadByte();
                                    byte num8 = reader1.ReadByte();
                                    string hex = convertIntToHexByte(num1) + convertIntToHexByte(num2) + convertIntToHexByte(num3) + convertIntToHexByte(num4) + convertIntToHexByte(num5) + convertIntToHexByte(num6) + convertIntToHexByte(num7) + convertIntToHexByte(num8);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + numb);
                                    break;
                                }
                            case "8 bytes":
                                {
                                    float numb1 = reader1.ReadSingle();
                                    float numb2 = reader1.ReadSingle();
                                    file.Seek(-12, SeekOrigin.Current);
                                    byte num1 = reader1.ReadByte();
                                    byte num2 = reader1.ReadByte();
                                    byte num3 = reader1.ReadByte();
                                    byte num4 = reader1.ReadByte();
                                    byte num5 = reader1.ReadByte();
                                    byte num6 = reader1.ReadByte();
                                    byte num7 = reader1.ReadByte();
                                    byte num8 = reader1.ReadByte();
                                    byte num9 = reader1.ReadByte();
                                    byte num10 = reader1.ReadByte();
                                    byte num11 = reader1.ReadByte();
                                    byte num12 = reader1.ReadByte();
                                    string hex = convertIntToHexByte(num1) + convertIntToHexByte(num2) + convertIntToHexByte(num3) + convertIntToHexByte(num4) + convertIntToHexByte(num5) + convertIntToHexByte(num6) + convertIntToHexByte(num7) + convertIntToHexByte(num8);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + numb1 + ';' + numb2);
                                    break;
                                }
                            case "12 bytes":
                                {
                                    float numb1 = reader1.ReadSingle();
                                    float numb2 = reader1.ReadSingle();
                                    float numb3 = reader1.ReadSingle();
                                    file.Seek(-12, SeekOrigin.Current);
                                    byte num1 = reader1.ReadByte();
                                    byte num2 = reader1.ReadByte();
                                    byte num3 = reader1.ReadByte();
                                    byte num4 = reader1.ReadByte();
                                    byte num5 = reader1.ReadByte();
                                    byte num6 = reader1.ReadByte();
                                    byte num7 = reader1.ReadByte();
                                    byte num8 = reader1.ReadByte();
                                    byte num9 = reader1.ReadByte();
                                    byte num10 = reader1.ReadByte();
                                    byte num11 = reader1.ReadByte();
                                    byte num12 = reader1.ReadByte();
                                    string hex = convertIntToHexByte(num1) + convertIntToHexByte(num2) + convertIntToHexByte(num3) + convertIntToHexByte(num4) + convertIntToHexByte(num5) + convertIntToHexByte(num6) + convertIntToHexByte(num7) + convertIntToHexByte(num8) + convertIntToHexByte(num9) + convertIntToHexByte(num10) + convertIntToHexByte(num11) + convertIntToHexByte(num12);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + numb1 + ';' + numb2 + ';' + numb3);
                                    break;
                                }
                            case "hash":
                                {
                                    if (dataSize != 4) {; }
                                    byte numb1 = reader1.ReadByte();
                                    byte numb2 = reader1.ReadByte();
                                    byte numb3 = reader1.ReadByte();
                                    byte numb4 = reader1.ReadByte();
                                    string hex1 = convertIntToHexByte(numb1) + convertIntToHexByte(numb2) + convertIntToHexByte(numb3) + convertIntToHexByte(numb4);
                                    string hex = "";
                                    //returnList.Add(hash + ';' + dataSize + ';' + hash1);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex1 + ';' + hex);
                                    break;
                                }
                            case "string":
                                {
                                    string hash1 = Encoding.ASCII.GetString(reader1.ReadBytes(dataSize));
                                    file.Seek(-dataSize, SeekOrigin.Current);
                                    string hex = "";
                                    for (int j = 0; j < dataSize; j++)
                                    {
                                        hex += convertIntToHexByte(reader1.ReadByte());
                                    }
                                    hash1 = "";
                                    //returnList.Add(hash + ';' + dataSize + ';' + hash1);
                                    returnList.Add(hash + ';' + hashDataNames[hexString] + ';' + hex + ';' + hash1);
                                    break;
                                }
                            default:
                                {
                                    reader1.ReadBytes(dataSize);
                                    break;
                                }
                        }
                        //returnList.Add(hash + ';' + dataSize + ';' + data);
                    }
                    else
                    {
                        reader1.ReadBytes(dataSize);
                    }
                }
                return returnList;
            }
        }


        private string returnAllModifiableStats(MemoryStream file)
        {
            string allStats = "";

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
                            allStats += data + ';';
                        }
                    }
                    else
                    {
                        reader1.ReadBytes(dataSize);
                    }
                }
                if (allStats.Length > 0)
                {
                    allStats = allStats.Substring(0, allStats.Length - 1);
                    return allStats;
                }
                else
                {
                    return null;
                }
            }
        }



        private int returnHashTableKeyIndex(string value, Hashtable table)
        {
            int key = (int)table[value];
            //string name = listOfNames[key];
            return key;
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


        //has out of bounds error when saving?
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

                                if (subtype == "CameraSettings")
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

          /* ===================================== */
         /*   Apply Grid Changes to Memory Stream */
        /* ===================================== */
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e) 
        {
            //MessageBox.Show("hellokitty");
            DataGridViewCell editedCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            decimal value;

            if (Decimal.TryParse(editedCell.Value.ToString(), out value))
            {
                int fileIndex = returnHashTableKeyIndex(Blueprints.SelectedNode.Text, fileNames); //get index of currently open blueprint file

                List<string> names = listOfNames;
                Hashtable namesTable = fileNames;
                List<byte[]> files = listOfFileArrays;

                int nodeIndex = Blueprints.SelectedNode.Index;

                int originalFileLength = listOfFileArrays[fileIndex].Length;

                
                byte[] returnFile = setNewDecimalsToStream(listOfFileArrays[fileIndex], readDecimalsFromDataGrid());

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
            else
            {
                MessageBox.Show("Invalid Decimal! You have entered: " + editedCell.Value.ToString() + ". Reverting value to 0.");
                editedCell.Value = "0";
            }
        }
        //above needs to have setNewDecimalsToStream completed
        //so that when the cell is edited it is updated to the
        //copy of the blueprint in memory which can later be
        //saved directly to the file.


        private string readDecimalsFromDataGrid()
        {
            string data = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                data += row.Cells[3].Value.ToString() + ';'; //x
                data += row.Cells[4].Value.ToString() + ';'; //y
                data += row.Cells[5].Value.ToString() + ';'; //z
            }
            data = data.Substring(0, data.Length - 1);

            return data;
        }

        //stream never gets edited?
        //says edited in memroy but when reloading blueprint the value is reverted

        //problem might be that WTF colors could be ignored until a 4byte value ending in FF appeared.
        //with decimals I now have to accept every 4 byte data I run into because there's no knowing
        //which are the values I want. Look above at how function reads this data in the first time
        //because I had to have found the way to do it already
        private byte[] setNewDecimalsToStream(byte[] file, string allDecimals)
        {
            MemoryStream stream = new MemoryStream(file);

            string[] allDecimalsArray = allDecimals.Split(';'); //decimals from grid
            int decimalCount = 0;

            using (BinaryReader reader1 = new BinaryReader(stream))
            {
                int count = reader1.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    string hash = Encoding.Default.GetString(reader1.ReadBytes(4)); //reads file's hash
                    stream.Seek(-4, SeekOrigin.Current);
                    byte number1 = reader1.ReadByte();
                    byte number2 = reader1.ReadByte();
                    byte number3 = reader1.ReadByte();
                    byte number4 = reader1.ReadByte();
                    string hexString = convertIntToHexByte(number1) + convertIntToHexByte(number2) + convertIntToHexByte(number3) + convertIntToHexByte(number4);

                    int dataSize = reader1.ReadInt32(); //reads file's data size
                    int offset = Convert.ToInt32(stream.Position); //reads file's offset
                    string data = "";

                    //if (dataSize == 4 && statHashReference.Contains(hexString)) 
                    if (dataSize == 12 && statHashReference.Contains(hexString))
                    {
                        byte byte1 = reader1.ReadByte();
                        byte byte2 = reader1.ReadByte();
                        byte byte3 = reader1.ReadByte();
                        byte byte4 = reader1.ReadByte();
                        data += convertIntToHexByte(Convert.ToInt32(byte1));
                        data += convertIntToHexByte(Convert.ToInt32(byte2));
                        data += convertIntToHexByte(Convert.ToInt32(byte3));
                        data += convertIntToHexByte(Convert.ToInt32(byte4));
                        byte byte5 = reader1.ReadByte();
                        byte byte6 = reader1.ReadByte();
                        byte byte7 = reader1.ReadByte();
                        byte byte8 = reader1.ReadByte();
                        data += convertIntToHexByte(Convert.ToInt32(byte5));
                        data += convertIntToHexByte(Convert.ToInt32(byte6));
                        data += convertIntToHexByte(Convert.ToInt32(byte7));
                        data += convertIntToHexByte(Convert.ToInt32(byte8));
                        byte byte9 = reader1.ReadByte();
                        byte byte10 = reader1.ReadByte();
                        byte byte11 = reader1.ReadByte();
                        byte byte12 = reader1.ReadByte();
                        data += convertIntToHexByte(Convert.ToInt32(byte9));
                        data += convertIntToHexByte(Convert.ToInt32(byte10));
                        data += convertIntToHexByte(Convert.ToInt32(byte11));
                        data += convertIntToHexByte(Convert.ToInt32(byte12));

                        //if hash == existing known data
                        if (decimalCount < allDecimalsArray.Length) //&& allDecimalsArray[decimalCount] != data)
                        {

                            //set all three 4 byte hex representations of floats to blueprint memory stream
                            string valueString1 = allDecimalsArray[decimalCount];
                            string valueString2 = allDecimalsArray[decimalCount+1];
                            string valueString3 = allDecimalsArray[decimalCount+2];
                            decimal valueDecimal1;
                            decimal valueDecimal2;
                            decimal valueDecimal3;
                            if (Decimal.TryParse(valueString1, out valueDecimal1)
                                && Decimal.TryParse(valueString2, out valueDecimal2)
                                && Decimal.TryParse(valueString3, out valueDecimal3))
                            {

                                byte[] bytes1 = BitConverter.GetBytes((float)valueDecimal1); //convert decimal value to hex
                                byte[] bytes2 = BitConverter.GetBytes((float)valueDecimal2);
                                byte[] bytes3 = BitConverter.GetBytes((float)valueDecimal3);
                                file[offset] = bytes1[0];
                                file[offset + 1] = bytes1[1];
                                file[offset + 2] = bytes1[2];
                                file[offset + 3] = bytes1[3];
                                file[offset + 4] = bytes2[0];
                                file[offset + 5] = bytes2[1];
                                file[offset + 6] = bytes2[2];
                                file[offset + 7] = bytes2[3];
                                file[offset + 8] = bytes3[0];
                                file[offset + 9] = bytes3[1];
                                file[offset + 10] = bytes3[2];
                                file[offset + 11] = bytes3[3];

                            }
                            else
                            {
                                MessageBox.Show("setNewDecimalsToMemoryStream failed to convert decimal array string to decimal.");
                            }
                            decimalCount++;//first 4 bytes
                            decimalCount++;//second 4 bytes
                            decimalCount++;//third 4 bytes
                        }



                    }
                    else
                    {
                        reader1.ReadBytes(dataSize);
                    }

                }
            }

            MessageBox.Show("Decimal Changed in Memory");
            return file;
        }


        /*
          /* ============== */
         /* Revert Decimal */
        /* ============== */
        /*
        private void revertDecimal(int row, int column)
        {
            if (label3.Text != "")
            {
                int index = (int)fileNames[label3.Text];
                string[][] files = originalStats;

                if (!originalStats[Blueprints.SelectedNode.Index][row].Equals(dataGridView1.Rows[row].Cells[column].Value))
                {
                    string originalDecimal = originalStats[Blueprints.SelectedNode.Index][row];

                    dataGridView1.Rows[row].Cells[column].Value = originalDecimal;

                }
                else
                {
                    MessageBox.Show("Already the original color.");
                }
            }
        }
        */

          /* =========== */
         /* Right Click */
        /* =========== */
        /*
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add(new MenuItem("Revert to original color"));

                int rowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                int columnIndex = dataGridView1.HitTest(e.X, e.Y).ColumnIndex;

                menu.MenuItems[0].Click += (sender1, e1) => revertDecimal(rowIndex, columnIndex);


                if (rowIndex > -1 && columnIndex > 1)
                {
                    menu.Show(dataGridView1, new Point(e.X, e.Y));
                }

            }
        }
        */
  



    }
}

