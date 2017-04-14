using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;


namespace Sab_Toolbox
{
    public partial class Megapack_Editor : Form
    {

        private Stream fileInput = null; //Initial file to load in, selected by user.
        private List<byte[]> parsedSBLAFiles = new List<byte[]>(); //Store all SBLA files from fileInput archive.


        public Megapack_Editor()
        {
            InitializeComponent();
        }


        private DialogResult identifyPackFileFormat() //Checkis if version number matches Megapack format (MP00)
        {
            if (fileInput != null)
            {
                BinaryReader anonymousFileReader1 = new BinaryReader(fileInput);
                String textMagicNumber = System.Text.Encoding.Default.GetString(anonymousFileReader1.ReadBytes(4));
                if (Utilities.reverseString(textMagicNumber) == "MP00") return DialogResult.OK;
                else return DialogResult.No;

            }
            else return DialogResult.No;
        }

        private void loadAndParseMegapack() //Reads Megapack file and copies the internal SBLA files to memory. Also adds them to tree view.
        {

            //MP00 header: 8 bytes - hash / 4 bytes - size / 4 bytes - offset / 4 bytes - empty

            int fileCount = 0;

            BinaryReader fileInputReader1 = new BinaryReader(fileInput);
            fileCount = fileInputReader1.ReadInt32();

            int[] fileSizes = new int[fileCount];

            //MP00 Headers
            for (int i = 0; i < fileCount; i++)
            {
                Int64 hash = fileInputReader1.ReadInt64();
                int size = fileInputReader1.ReadInt32();

                int offset = 0;
                int zero = 0;
                if (i != fileCount) //Keeps from reading after last file
                {
                    offset = fileInputReader1.ReadInt32();
                    zero = fileInputReader1.ReadInt32(); //This is supposed to be zero. It is read anyways to debug if it is non-zero.
                }
                fileSizes[i] = size;
                //MessageBox.Show("Hash: " + hash + " Size: " + size + " Offset: " + offset + " Zero: " + zero);
            }

            //Some of these megapacks have extra header data for some other sort of archive.
            //You'll need to figure out the format. Good luck and god speed.
            //In the mean time I'm just going to skip them.


            string character = System.Text.Encoding.Default.GetString(fileInputReader1.ReadBytes(1));
            Console.WriteLine(character);

            if (character == "Ë") //This forms the basis for reading the rest of the headers. (This should be the character for the 00 hex .)
            {
                //MessageBox.Show("End of standard SBLA file.");
            }
            else
            {
                //MessageBox.Show("There's still more headers!");
            }

            fileInput.Seek(0, 0);

            while (true) //This skips all data until the first ALBS archive.
            {
                if (Encoding.Default.GetString(fileInputReader1.ReadBytes(1)) == "A")
                {
                    if (Encoding.Default.GetString(fileInputReader1.ReadBytes(3)) == "LBS")
                    {
                        fileInput.Seek(fileInput.Position - 4, 0); //To start reading in front of version number.
                        break;
                    }

                }
            }

            //Save SBLA data to List.
            for (int i = 0; i < fileCount; i++)
            {
                if (Encoding.Default.GetString(fileInputReader1.ReadBytes(1)) != "A") //if the next character is not 'A'
                {
                    while (Encoding.Default.GetString(fileInputReader1.ReadBytes(1)) != "A") //keep reading until you find 'A'
                    {

                    }
                    fileInput.Position = fileInput.Position - 1; //seek back one character to be in front of the 'A'
                }
                else
                {
                    fileInput.Position = fileInput.Position - 1;
                }
                parsedSBLAFiles.Add(fileInputReader1.ReadBytes(fileSizes[i])); //The file size has been saved to a list previously, now we read that many bytes to get the file.
                treeView1.Nodes.Add(i + ".sbla"); //Add the number + file extension to treeview. (SBLA files have no names, so number is best option.)

            }
            extractButton.Enabled = true;
            parseSBLAFiles();
        }

        private void parseSBLAFiles()
        {
            int sblaNumber = 1;
            foreach (byte[] sbla in parsedSBLAFiles)
            {
                if (sblaNumber == 12) {; }

                MemoryStream sblaStream = new MemoryStream(sbla);
                BinaryReader fileInputReader1 = new BinaryReader(sblaStream);

                string magicNumber = Utilities.reverseString(Encoding.Default.GetString(fileInputReader1.ReadBytes(4)));

                fileInput.Seek(4, SeekOrigin.Current); //Skipping 4 0's to get to second magic number
                string secondMagicNumber = Utilities.reverseString(Encoding.Default.GetString(fileInputReader1.ReadBytes(4)));


                //if (magicNumber == "SBLA" && secondMagicNumber != "HEI1" && secondMagicNumber != "\0\0\0\0" && sbla.Length > 8) //HEI1 is the second magic number, a variant of the SBLA I don't know how to read. //\0\0\0\0 is in SBLA 13
                if (magicNumber == "SBLA" && secondMagicNumber != "\0\0\0\0" && sbla.Length > 8)
                {
                    //MessageBox.Show("Position: " + sblaStream.Position + " Length: " + sblaStream.Length);


                    List<int> fileSizes = new List<int>();
                    List<int> offsets = new List<int>();

                    int fileCount = 0;
                    int fileSizeTotal = 0;
                    int headerSizeTotal = 0;

                    sblaStream.Seek(4, 0);

                    while (headerSizeTotal + fileSizeTotal + 4 + 4 != sblaStream.Length) //Check to see if our headers+file sizes have accounted for every byte of file. +4 for version number, +4 for bool before first MSHA
                    {
                        try
                        {

                            fileInputReader1.ReadBytes(8); //Skips 4 bytes of 0's and hash
                            int offset = fileInputReader1.ReadInt32();
                            int compressedSize = fileInputReader1.ReadInt32();
                            int uncompressedSize = fileInputReader1.ReadInt32();
                            fileInputReader1.ReadBytes(4); //Skips 4 bytes of 0's

                            fileSizes.Add(compressedSize);
                            offsets.Add(offset);

                            //MessageBox.Show("SBLA #" + sblaNumber + " " + " Offset: " + offset + " of " + sblaStream.Length + " comp'd Size: " + compressedSize + " uncomp'd Size: " + uncompressedSize);

                            if (fileCount > 1) //if the previously stored size doesn't reach the current offset, recalculate the size by comparing the current offset with the prior.
                            {
                                if (offsets[fileCount - 1] + fileSizes[fileCount-1] != offset)
                                {
                                    fileSizes[fileCount - 1] = offset - offsets[fileCount - 1];
                                }
                            }

                            fileCount++;
                            headerSizeTotal += 24;
                            fileSizeTotal += compressedSize;

                            //MessageBox.Show((headerSizeTotal + fileSizeTotal + 4 + 4 + " of " + sblaStream.Length));
                        }
                        catch
                        {
                            Console.WriteLine("Exception: Position: " + sblaStream.Position + "of " + sblaStream.Length + " SBLA #" + sblaNumber + " File #" + fileCount + "Data total: " + (headerSizeTotal + fileSizeTotal + 4 + 4));
                            break;
                        }

                    }
                    if (sblaNumber == 1) {; }
                    //{
                    List<byte[]> mshaFiles = new List<byte[]>();
                    List<byte[]> zlibFiles = new List<byte[]>();
                    List<byte[]> dxtFiles = new List<byte[]>();
                    List<byte[]> cfxFiles = new List<byte[]>();

                    int mshaI = 0; //These are necessary as even though i will work for indexing the fileSizes
                    int zlibI = 0; //Once the msha's are read, zlibs will be added to a new list, which will start
                    int dxtI = 0; //from 0, so they can not be indexed with the typical i index. This way, i is a
                    int cfxI = 0; //count for all files, while these count the amount of specific types of files.

                    fileInputReader1.ReadBytes(4); //Read Bool before first MSHA
                    for (int i = 0; i < fileCount; i++)
                    {
                        if (sblaNumber == 1 && i == 705) {; }
                        if (sblaNumber == 9 && i == 113) { ; }
                        if (sblaNumber == 7 && i == 18) {; }

                        byte[] file = fileInputReader1.ReadBytes(fileSizes[i]);
                        MemoryStream fileStream = new MemoryStream(file);
                        BinaryReader fileStreamReader1 = new BinaryReader(fileStream);

                        //MessageBox.Show(convertIntToHexByte(fileInputReader1.ReadByte())); //FOR TESTING, NOT LEGITIMATE FUNCTIONALITY
                        //MessageBox.Show(convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte())+ convertIntToHexByte(fileInputReader1.ReadByte()));
                        //MessageBox.Show(fileStream.Position.ToString());
                        //fileStream.Seek(fileStream.Position - 1, 0);

                        

                        string magic = fileStreamReader1.ReadChar().ToString();
                        //MessageBox.Show(magic);

                        /*
                        while (magic == "\0") //FOR TESTING ONLY, NOT LEGITIMATE FUNCTIONALITY
                        {
                            magic = fileStreamReader1.ReadChar().ToString();
                        }
                        */
                        

                        
                        if (sblaNumber == 11 && i == 26) {; } //fileCount-1) {; }
                        if (sblaNumber == 33 && i == 1) {; } //fileCount-1) {; }
                        switch (magic)
                        {
                            case "A": //MSHA
                                {
                                    mshaFiles.Add(file);

                                    MemoryStream MSHAStream = new MemoryStream(mshaFiles[mshaI]); //mshaI, counts amount of mshas so that indexing list of them works.
                                    BinaryReader mshaReader1 = new BinaryReader(MSHAStream);

                                    mshaReader1.ReadBytes(20);
                                    string fileName = Encoding.Default.GetString(mshaReader1.ReadBytes(100));
                                    fileName = Utilities.stringContainsOnly(fileName, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_");


                                    if (treeView1.Nodes[sblaNumber - 1] != null)
                                    {
                                        treeView1.Nodes[sblaNumber - 1].Nodes.Add(fileName + ".msha");
                                    }

                                    mshaI++;

                                    break;
                                }
                            case "x": //Stray Zlib Data
                                {
                                    zlibFiles.Add(file);
                                    MemoryStream ZLIBStream = new MemoryStream(zlibFiles[zlibI]);
                                    BinaryReader zlibStreamReader1 = new BinaryReader(ZLIBStream);

                                    if (treeView1.Nodes[sblaNumber - 1] != null)
                                    {
                                        treeView1.Nodes[sblaNumber - 1].Nodes.Add(zlibI + ".zlib");
                                    }

                                    zlibI++;

                                    break;
                                }
                            case "C": //CFX
                                {
                                    cfxFiles.Add(file);

                                    MemoryStream cfxStream = new MemoryStream(cfxFiles[cfxI]);
                                    BinaryReader cfxStreamReader1 = new BinaryReader(cfxStream);


                                    if (treeView1.Nodes[sblaNumber - 1] != null)
                                    {
                                        treeView1.Nodes[sblaNumber - 1].Nodes.Add(i + ".cfx");
                                    }

                                    break;
                                }
                            default: //DXT
                                {
                                    dxtFiles.Add(file);

                                    MemoryStream dxtStream = new MemoryStream(dxtFiles[dxtI]);
                                    BinaryReader dxtStreamReader1 = new BinaryReader(dxtStream);

                                    int nameLength = dxtStreamReader1.ReadInt32();
                                    string name = Encoding.Default.GetString(dxtStreamReader1.ReadBytes(nameLength));
                                    string extension = Encoding.Default.GetString(dxtStreamReader1.ReadBytes(4));

                                    if (extension != "DXT1" && extension != "DXT5")
                                    {
                                        extension = "MysteryDXT";
                                    }

                                    if (treeView1.Nodes[sblaNumber - 1] != null)
                                    {
                                        treeView1.Nodes[sblaNumber - 1].Nodes.Add(name + ".dds");
                                    }

                                    dxtI++;

                                    break;
                                }
                        }


                    }

                    //MessageBox.Show("SBLA #" + sblaNumber);
                    

                }
                else
                {
                    //MessageBox.Show("SBLA file not valid. The SBLA file was not properly extracted by the shitty programmer who made this.");
                }
                sblaNumber++;
            }
        } //Reads each SBLA file and copies the internal MSHA files to memory. Also adds them to tree view.



        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) //Menu Button to open up Settings page.
        {
            Sabtool_Settings settings1 = new Sabtool_Settings();
            settings1.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) //Menu Button to open up About page.
        {
            Sabtool_About about1 = new Sabtool_About();
            about1.Show();
        }

        private void selectFileToOpenToolStripMenuItem_Click(object sender, EventArgs e) //User selects specific file from custom location.
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInput = openFileDialog1.OpenFile();
                if (identifyPackFileFormat() == DialogResult.OK)
                {
                    loadAndParseMegapack();
                }
                else
                {
                    MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                    fileInput.Dispose();
                    fileInput.Close();
                }

            }
            FileInfo fileName = new FileInfo(openFileDialog1.FileName);
            this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - " + fileName.Name;
        }





        private void mega0megapackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "Mega0.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\France\\", "Mega0.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }
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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Mega0.megapack";

                    break;
            }
        } //Directly open mega 0 megapack file.

        private void mega1megapackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "Mega1.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {

                            fileInput = fileInput = File.Open(Path.Combine(path + "\\France\\", "Mega1.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }


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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Mega1.megapack";

                    break;
            }
        } // Directly open mega 1 megapack file.

        private void mega2megapackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "Mega2.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = fileInput = File.Open(Path.Combine(path + "\\France\\", "Mega2.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }
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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Mega2.megapack";

                    break;
            }
        } //Directly open mega 2 megapack file.

        private void start0kiloPackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "Start0.kiloPack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {

                            fileInput = fileInput = File.Open(Path.Combine(path + "\\France\\", "Start0.kiloPack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }

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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Start0.kiloPack";

                    break;
            }
        } //Directly open start 0 kilopack file.

        private void startGerman0kiloPackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "Start_German0.kiloPack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\France\\", "Start_German0.kiloPack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }
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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Start_German0.kiloPack";

                    break;
            }
        } //Directly open start German 0 kilopack file.

        private void belleStart0kiloPackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\France\\", "BelleStart0.kiloPack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\France\\", "BelleStart0.kiloPack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }
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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - BelleStart0.kiloPack";

                    break;
            }
        } //Directly open belle Start 0 kilopack file.

        private void dynamic0megapackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\Global\\", "Dynamic0.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\Global\\", "Dynamic0.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }
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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Dynamic0.megapack";

                    break;
            }
        } //Directly open Dynamic 0 megapack file.

        private void palettes0megapackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\Global\\", "Palettes0.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {
                            fileInput = File.Open(Path.Combine(path + "\\Global\\", "Palettes0.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }
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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - Palettes0.megapack";

                    break;
            }
        } //Directly open Palettes 0 megapack file.

        private void dlc01mega0megapackToolStripMenuItem_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\DLC\\01\\", "dlc01mega0.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {

                            fileInput = fileInput = File.Open(Path.Combine(path + "\\DLC\\01\\", "dlc01mega0.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }


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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - DLC Mega0.megapack";

                    break;
            }
        } //Directly open DLC mega0 megapack file.

        private void dynamic0megapackToolStripMenuItem1_Click(object sender, EventArgs e)
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
                    if (File.Exists(Path.Combine(path + "\\DLC\\01\\", "Dynamic0.megapack")))
                    {
                        //MessageBox.Show("Loosefiles_BinPC.pack found.");
                        try
                        {

                            fileInput = fileInput = File.Open(Path.Combine(path + "\\DLC\\01\\", "Dynamic0.megapack"), FileMode.Open);
                            if (identifyPackFileFormat() == DialogResult.OK)
                            {
                                loadAndParseMegapack();
                            }
                            else
                            {
                                MessageBox.Show("This is not a proper Megapack/Kilopack. The version number for the correct format is 00PM.");
                                fileInput.Dispose();
                                fileInput.Close();
                            }


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

                    this.Text = "Sab - Toolbox v 0.0 - Megapack Editor - DLC Dynamic0.megapack";

                    break;
            }
        } //Directly open DLC Dynamic 0 file.



        private void extractButton_Click(object sender, EventArgs e) //GUI Button, Extract SBLA Files
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                int i = 0;
                foreach (byte[] sbla in parsedSBLAFiles)
                {
                    File.WriteAllBytes(Path.Combine(folderBrowserDialog1.SelectedPath, i + ".sbla"), sbla);
                    i++;
                }

            }
        }









    }
}
