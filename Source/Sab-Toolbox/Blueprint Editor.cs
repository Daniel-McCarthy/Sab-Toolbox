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


    public partial class Blueprint_Editor : Form
    {
        public Stream fileInput; //data file
        public int fileCount; //amount of files extracted
        public Hashtable fileNames; //hashtable to keep the files in order, name and file #
        public List<string> fileNamesList; //names of each file as they are extracted
        public List<byte[]> listOfFileArrays; //data of each file as they are extracted
        public List<Int32> fileSizes; //amount of data for each file to read
        /// 
        /// </summary>



        public Blueprint_Editor()
        {
            InitializeComponent();
            fileNames = new Hashtable();
            fileNamesList = new List<string>();
            listOfFileArrays = new List<byte[]>();
            fileSizes = new List<Int32>();

        }

        private void generateNewToolStripMenuItem_Click(object sender, EventArgs e)  //New File
        {
            
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) //About page
        {
            Sabtool_About about1 = new Sabtool_About();
            about1.Show();
        }

        private void mainSaboteurBlueprintsToolStripMenuItem_Click(object sender, EventArgs e) //Open/Extract Loosefiles_BiPC.pack
        {
            string path = returnGamePath();
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
                            fileInput = File.Open(Path.Combine(path + "\\France\\", "Loosefiles_BinPC.pack"), FileMode.Open);;
                            extractGameTemplatesFromLooseFiles();
                            loadBlueprintsWSD();
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



        private void dLCBlueprintsToolStripMenuItem_Click(object sender, EventArgs e) //Open/ GameTemplates.wsd
        {
            string path = returnGamePath();
            switch (path)
            {
                case "PathInvalid":
                    MessageBox.Show("Has your game been moved? Your Saboteur Game Path is not valid, please re-select your Saboteur Game Path.");
                    break;
                case "NoSettingsFile":
                    MessageBox.Show("Please set the location of your Game Folder in Settings. Otherwise use Select Other and locate the file manually.");
                    break;
                default:
                    if (File.Exists(Path.Combine(path + "\\DLC\\01\\", "GameTemplates.wsd")))
                    {
                        //MessageBox.Show("GameTemplates.wsd found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\DLC\\01\\", "GameTemplates.wsd"), FileMode.Open);
                            loadBlueprintsWSD();
                            
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


        public void loadBlueprintsWSD() //Processes the extracted WSD file.
        {

       
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

                progressBar1.Maximum = fileCount + 4; //4 to accomodate bytes already read

                progressBar1.Visible = true;

                //for (int i = 0; i < fileCount; i++) //Because the filecount is not always correct.. we have to use a more dynamic way to iterate
                int i = 0;
                while(fileInput.Position != fileInput.Length -1 && fileInput.Position != fileInput.Length) //While we're not at the end..
                {

                    int size = binReader1.ReadInt32();
                    fileSizes.Add(size); //Add size of file to fileSizes list.
                    String midValues = "" + binReader1.ReadInt64();
                    int nameLength = binReader1.ReadInt32();


                    String name = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(nameLength-1));
                    binReader1.ReadByte();
                    fileNames.Add(name, i); //Add name and file number to names hash table
                    fileNamesList.Add(name);

                    int subtypeLength = binReader1.ReadInt32(); 
                    string subtype = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(subtypeLength-1)); //Read in the subtype
                    binReader1.ReadByte();

                    if (Blueprints.Nodes.ContainsKey(subtype)) //Check if the treeview has this subtype already.
                    {
                        Blueprints.Nodes[subtype].Nodes.Add(name); //Add the file to that subtype
                    } else
                    {
                        Blueprints.Nodes.Add(subtype, subtype); //Create the subtype first
                        Blueprints.Nodes[subtype].Nodes.Add(name); //Add the file to that subtype
                    }
                    

                    byte[] blueprint = null;
                    blueprint = binReader1.ReadBytes((size) - 12 - nameLength - 4 - subtypeLength); //Read in the header data.


                    listOfFileArrays.Add(blueprint); //Add extracted file's bytes to list of the data files



                    //MessageBox.Show("Position: " + fileInput.Position + " Size: " + fileInput.Length);
                    //Console.WriteLine("File #: " + i + " Size: " + size + " Int64: " + a + " Name: " + name + "\n");
                    //Console.Write("\n" + "File #: " + i);
                    //MessageBox.Show("");
                    //Console.Write(" Size: " + size);
                    //MessageBox.Show("");
                    //Console.Write(" Int64: " + midValues);
                    //MessageBox.Show("");
                    //Console.Write(" Subtype: " + subtype);
                    //MessageBox.Show("");
                    //Console.Write(" Name: " + name);

                    //if (i != fileCount)
                    if (fileInput.Position != fileInput.Length - 1 && fileInput.Position != fileInput.Length)
                    {
                        while (binReader1.ReadInt32() == 8)
                        {
                            binReader1.ReadInt64();
                            //Can add line to add current i to List to keep track of every file that has this feature.
                        }
                        fileInput.Seek(fileInput.Position - 4, 0);



                    }
                    extractBlueprintsButton.Visible = true;
                    printListButton.Visible = true;

                    //Export blueprint for testing
                    //MemoryStream blueprintStream = new MemoryStream(blueprint);
                    //FileStream banana = new FileStream("C:\\Users\\Dan\\Desktop\\blueprints\\" + i + ".blueprint", FileMode.Create, FileAccess.Write);
                    //blueprintStream.WriteTo(banana);

                    i++;
                    progressBar1.Value++;


                }

                progressBar1.Visible = false;
                progressBar1.Value = 0;

                //MessageBox.Show(fileNames[fileNames.Count]);
                Blueprints.Sort();
                binReader1.Dispose();
                binReader1.Close();
                //fileInput.SetLength(0);
                //fileInput.Dispose();
                //fileInput.Close();
                GC.Collect();
            }
            else
            {
                MessageBox.Show("This is not a blueprint archive, valid files are GameTemplates.wsd and Loosefiles_BinPC.pack.");
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
            } else {
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












        public static string returnGamePath()
        {
            string path = Application.ExecutablePath;
            path = path.Substring(0, path.Length - 15);

            //Does Settings File Exist?
            if (File.Exists(Path.Combine(path, "settings.txt")))
            {
                //Settings Exist, Now Check Path Setting
                string[] readText = File.ReadAllLines(Path.Combine(path, "settings.txt"));
                string pathInput = readText[0];
                if (pathInput.Length > 5)
                {
                    //Check if Path Setting Is Valid
                    pathInput = pathInput.Substring(5, pathInput.Length - 5);
                    if (File.Exists(Path.Combine(pathInput, "Saboteur.exe")))
                    {
                        //It is, now set up path and image to tell user their path is set.
                        return pathInput;
                    }
                    else
                    {
                        //The setting is no longer valid
                        return "PathInvalid";
                    }

                }
                else
                {
                    //The setting is no longer valid
                    return "NoSettingsFile";
                }
            }
            else
            {
                //The setting is no longer valid
                return "NoSettingsFile";
            }
        }

        private void selectOtherFileToolStripMenuItem_Click(object sender, EventArgs e) //Load User Selected file.
        {
            openFileDialog1.Filter = "WSD files (*.)|*.wsd|Blueprint files (*.)|*.blueprint";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInput = openFileDialog1.OpenFile();
                loadBlueprintsWSD();
            }
            
        }

        private void editToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Sabtool_Settings settings1 = new Sabtool_Settings();
            settings1.Show();
        } //Settings Window







        private void Blueprints_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Node.Level == 0)
            {
                e.Cancel = true;
            }

            //Correct place to write code to check if file needs to be saved before changing selection.
        } //If selection is parent node, cancel selection.





        private void Blueprints_AfterSelect(object sender, TreeViewEventArgs e) // Else, load file node is associated with.
        {
            string name = e.Node.Text;
            //MessageBox.Show(name);


            


            if (fileNames.ContainsKey(name))
            {
                int fileNumber = Convert.ToInt32(fileNames[name].ToString()); //Hash table. Grabs the fileNumber saved with the file name.
                MemoryStream blueprint = new MemoryStream(listOfFileArrays[fileNumber]);
                BinaryReader blueprintReader1 = new BinaryReader(blueprint);

                progressBar1.Visible = true;
                progressBar1.Maximum = Convert.ToInt32(blueprint.Length);

                blueprintReader1.ReadInt32();

                //for (int i = 0; i < (blueprint.Length - 8) / 4; i++)
                //for (int i = 0; i < 1; i++)
                int i = 0;
                int runOnce = 0;
                while (blueprint.Position < blueprint.Length && blueprint.Position < blueprint.Length - 1)
                {
                    if (runOnce == 0)
                    {
                        for (int index = 0; index < 2000; index++)
                        {
                            dataGridView1.Rows.Add();
                        }
                        runOnce++;
                    }

                    string hash = "";
                    for (int i2 = 0; i2 < 4; i2++)
                    {
                        hash += convertIntToHexByte(blueprintReader1.ReadByte());
                    }
                    //MessageBox.Show(hash);

                    int dataSize = blueprintReader1.ReadInt32();
                    //dataGridView1.AllowUserToAddRows = false;
                    string value = "";

                    progressBar1.Value += 8 + dataSize;

                    switch (dataSize)
                    {
                        case 1:
                            dataGridView1.Rows[i].Cells[0].Value = hash;
                            //dataGridView1.Rows[i].Cells[1].Value = blueprintReader1.ReadByte();
                            dataGridView1.Rows[i].Cells[1].Value = convertIntToHexByte(blueprintReader1.ReadByte());
                            blueprint.Seek(blueprint.Position - 1, 0);
                            dataGridView1.Rows[i].Cells[2].Value = blueprintReader1.ReadByte();
                            dataGridView1.Rows[i].Cells[3].Value = dataSize;
                            break;

                        case 4:
                            dataGridView1.Rows[i].Cells[0].Value = hash;
                            dataGridView1.Rows[i].Cells[3].Value = dataSize;
                            for(int i2 = 0; i2 < 4; i2++)
                            {
                                value += convertIntToHexByte(blueprintReader1.ReadByte());
                            }
                            blueprint.Seek(blueprint.Position - 4, 0);
                            int integer = blueprintReader1.ReadInt32();
                            blueprint.Seek(blueprint.Position - 4, 0);
                            dataGridView1.Rows[i].Cells[2].Value = integer + "/" + blueprintReader1.ReadSingle();
                            dataGridView1.Rows[i].Cells[1].Value = value;
                            break;

                        case 8:
                            dataGridView1.Rows[i].Cells[0].Value = hash;
                            dataGridView1.Rows[i].Cells[3].Value = dataSize;
                            for (int i2 = 0; i2 < 8; i2++)
                            {
                                value += convertIntToHexByte(blueprintReader1.ReadByte());
                            }
                            dataGridView1.Rows[i].Cells[1].Value = value;
                            break;

                        case 12:
                            dataGridView1.Rows[i].Cells[0].Value = hash;
                            dataGridView1.Rows[i].Cells[3].Value = dataSize;
                            for (int i2 = 0; i2 < 12; i2++)
                            {
                                value += convertIntToHexByte(blueprintReader1.ReadByte());
                            }

                            blueprint.Seek(blueprint.Position - 12, 0);
                            dataGridView1.Rows[i].Cells[2].Value = blueprintReader1.ReadSingle() + " " + blueprintReader1.ReadSingle() + " " + blueprintReader1.ReadSingle();

                            dataGridView1.Rows[i].Cells[1].Value = value;
                            break;

                        case 16:
                            dataGridView1.Rows[i].Cells[0].Value = hash;
                            dataGridView1.Rows[i].Cells[3].Value = dataSize;
                            for (int i2 = 0; i2 < 16; i2++)
                            {
                                value += convertIntToHexByte(blueprintReader1.ReadByte());
                                if(i2 == 4 || i2 == 8 || i2 == 12)
                                {
                                    value += " ";
                                }
                            }

                            dataGridView1.Rows[i].Cells[1].Value = value;

                            blueprint.Seek(blueprint.Position - 16,0);
                            dataGridView1.Rows[i].Cells[2].Value = "";
                            for (int i3 = 0; i3 < 4; i3++)
                            {
                                dataGridView1.Rows[i].Cells[2].Value += blueprintReader1.ReadSingle() + "/";
                            }
                            break;
                        default:
                            dataGridView1.Rows[i].Cells[0].Value = hash;
                            for (int i3 = 0; i3 < dataSize; i3++)
                            {
                                value += blueprintReader1.ReadByte();
                            }
                            //MessageBox.Show("Data Size Unprecedented! Seriously though, the data size is " + dataSize + ", write some code to read it.");
                            break;
                    }



                    dataGridView1.Rows[i].Cells[4].Value = i; //Sets hidden column to index to keep track of row/file count/order.

                    if (blueprint.Position < blueprint.Length)
                    {
                        dataGridView1.Update();
                    }

                    
                    //Console.WriteLine("#:" + i + " Hash: " + hash + " Data Size: " + dataSize + " success.");
                    //if(i == 217) break;
                    i++;

                }
                int saveValue = 0;
                for(int i2 = 1; i2 < 2000; i2++)
                {

                    if(dataGridView1.Rows[i2].Cells[0].Value == null)
                    {

                        saveValue = i2;
                        while (dataGridView1.Rows[saveValue] != null)
                        {
                            if (!dataGridView1.Rows[saveValue].IsNewRow)
                            {
                                dataGridView1.Rows.RemoveAt(saveValue);
                            }
                            else
                            {
                                break;
                            }
                        }

                        i2 = 2001;


                    }
                }

                //MessageBox.Show(fileNames[name].ToString());
            }
            //listOfFileArrays()




            for(int i = 0; i < 50; i++)
            {
                dataGridView2.Rows.Add();
            }

            //Console.WriteLine("\"" + dataGridView1.Rows[2].Cells[0].Value + "\"");

            int currentRow = 0;
            for(int i2 = 0; i2 < dataGridView1.RowCount; i2++)
            {
                if (dataGridView1.Rows[i2].Cells[0].Value != null)
                {
                    string hash = dataGridView1.Rows[i2].Cells[0].Value.ToString();

                    if (hash.Equals("5042725B")) //Model
                    {
                        //MessageBox.Show(dataGridView1.Rows[i2].Cells[0].Value.ToString());
                        dataGridView2.Rows[currentRow].Cells[0].Value = "5042725B";
                        dataGridView2.Rows[currentRow].Cells[1].Value = "PBr[";
                        dataGridView2.Rows[currentRow].Cells[2].Value = "Model";
                        dataGridView2.Rows[currentRow].Cells[3].Value = dataGridView1.Rows[i2].Cells[1].Value;
                        dataGridView2.Rows[currentRow].Cells[4].Value = "Hash";
                        currentRow++;
                    }

                    if (hash.Equals("D29F907A")) //Firerate
                    {
                        //MessageBox.Show(dataGridView1.Rows[i2].Cells[0].Value.ToString());
                        //int parsedHash = int.Parse(hash, System.Globalization.NumberStyles.AllowHexSpecifier);
                        //byte[] floatConversionHash = BitConverter.GetBytes(parsedHash);
                        //float finalFloat = BitConverter.ToSingle(floatConversionHash, 0);

                        dataGridView2.Rows[currentRow].Cells[0].Value = "D29F907A";
                        dataGridView2.Rows[currentRow].Cells[1].Value = "...z";
                        dataGridView2.Rows[currentRow].Cells[2].Value = "Fire rate";
                        dataGridView2.Rows[currentRow].Cells[3].Value = dataGridView1.Rows[i2].Cells[1].Value;
                        //dataGridView2.Rows[currentRow].Cells[3].Value = finalFloat.ToString();
                        dataGridView2.Rows[currentRow].Cells[4].Value = "Float";
                        currentRow++;
                    }

                    if (hash.Equals("2A7235F5")) //HUD
                    {
                        //MessageBox.Show(dataGridView1.Rows[i2].Cells[0].Value.ToString());
                        dataGridView2.Rows[currentRow].Cells[0].Value = "2A7235F5";
                        dataGridView2.Rows[currentRow].Cells[1].Value = "*r5.";
                        dataGridView2.Rows[currentRow].Cells[2].Value = "HUD Image";
                        dataGridView2.Rows[currentRow].Cells[3].Value = dataGridView1.Rows[i2].Cells[1].Value;
                        dataGridView2.Rows[currentRow].Cells[4].Value = "Hash";
                        currentRow++;
                    }

                    if (hash.Equals("82D2C709")) //Clip Size
                    {
                        //MessageBox.Show(dataGridView1.Rows[i2].Cells[0].Value.ToString());
                        //int parsedHash = int.Parse(hash, System.Globalization.NumberStyles.AllowHexSpecifier);
                        //byte[] floatConversionHash = BitConverter.GetBytes(parsedHash);
                        dataGridView2.Rows[currentRow].Cells[0].Value = "82D2C709";
                        dataGridView2.Rows[currentRow].Cells[1].Value = "....";
                        dataGridView2.Rows[currentRow].Cells[2].Value = "Clip Size";
                        dataGridView2.Rows[currentRow].Cells[3].Value = dataGridView1.Rows[i2].Cells[1].Value;
                        //dataGridView2.Rows[currentRow].Cells[3].Value = parsedHash;
                        dataGridView2.Rows[currentRow].Cells[4].Value = "Int32";
                        currentRow++;
                    }

                }
            }

            progressBar1.Visible = false;
            progressBar1.Value = 0;
            
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
                //int inputDiv2 = inputDiv1 / 16;

                //int hundrethPlace = inputDiv2 % 16 * 16;

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



        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            panel1.SendToBack();
            panel2.BringToFront();
            label3.Text = "Full Data List";

        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            panel2.SendToBack();
            panel1.BringToFront();
            label3.Text = "Known Data List";

        }

        private void printListButton_Click(object sender, EventArgs e) //Print Text List of all Blueprints loaded.
        {
            saveFileDialog1.Filter = "text files (*.)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Maximum = listOfFileArrays.Count;
                progressBar1.Visible = true;

                int fileCount = 0;
                string headerInfo = "";
                string fileToWrite = "";
                foreach (TreeNode node in Blueprints.Nodes)
                {
                    fileToWrite += "\n" + node.Text + "\n";

                    foreach (TreeNode subnode in node.Nodes)
                    {
                        fileToWrite += "\t" + subnode.Text + "\n";
                        fileCount++;
                        progressBar1.Value++;
                    }
                }

                headerInfo = "Blueprint List from GameTemplates on " + DateTime.Now + " containing " + fileCount + " files." + "\n";

                File.WriteAllText(saveFileDialog1.FileName, headerInfo + fileToWrite);

                progressBar1.Visible = false;
                progressBar1.Value = 0;

            }
        }

        private void extractBlueprintsButton_Click(object sender, EventArgs e) //Extract loaded Blueprints to selected folder.
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Maximum = listOfFileArrays.Count;
                progressBar1.Visible = true;

                int i = 0;
                foreach (byte[] array in listOfFileArrays)
                {

                    File.WriteAllBytes((Path.Combine(folderBrowserDialog1.SelectedPath, fileNamesList[i]) + ".blueprint"), array);
                    i++;
                    progressBar1.Value++;
                }

                progressBar1.Visible = false;
                progressBar1.Value = 0;
            }
        }

    }
    
}
