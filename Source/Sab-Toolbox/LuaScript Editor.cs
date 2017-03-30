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
using System.Diagnostics;

namespace Sab_Toolbox
{
    public partial class LuaScript_Editor: Form
    {
        public Stream fileInput = null;
        public int fileCount;
        public int[] fileSizes;
        public List<String> fileNames;
        public List<String> filePaths;
        public int initialOffset;

        public LuaScript_Editor()
        {
            InitializeComponent();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) //Menu Button, opens Settings window.
        {
            Sabtool_Settings settings1 = new Sabtool_Settings();
            settings1.Show();
        }

        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e) //Loads user selected file from custom location.
        {
            openFileDialog1.Filter = "Luap files (*.luap)|*.luap";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInput = openFileDialog1.OpenFile();
                loadLuapFile();
            }
            progressBar1.Value = 0;
        }

        private void luaScriptsluapToolStripMenuItem_Click(object sender, EventArgs e) //Directly loads the LuaScripts.luap file from game location.
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
                    if (File.Exists(Path.Combine(path, "LuaScripts.luap")))
                    {
                        //MessageBox.Show("LuaScripts.luap found.");
                        try
                        {
                            //MessageBox.Show(path);
                            fileInput = File.Open(Path.Combine(path, "LuaScripts.luap"), FileMode.Open);
                            loadLuapFile();
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
            progressBar1.Value = 0;
        }


        private void loadLuapFile() //Loads the Lua Scripts archive and identifies the file count as well as adds all the file sizes to a list, then executes generateTreeList function.
        {
            BinaryReader binaryReader1 = new BinaryReader(fileInput);
            fileCount = binaryReader1.ReadInt32();
            textBox1.Text = fileCount.ToString();

            fileSizes = new int[fileCount];
            progressBar1.Value = 0;
            int runOnce = 0;
            for(int i = 0; i < fileCount; i++)
            {
                long hash = binaryReader1.ReadInt64();
                int offset = binaryReader1.ReadInt32();
                int size1 = binaryReader1.ReadInt32();
                int size2 = binaryReader1.ReadInt32();
                byte watisdis = binaryReader1.ReadByte();

                progressBar1.Value++;

                fileSizes[i] = size1;
                //MessageBox.Show(offset.ToString());

                if (runOnce == 0)
                {
                    initialOffset = offset;
                }
            }

            generateTreeList();
        }

        private void loadLuapFileBigEndian() //Same as above function, except runs a Big Endian variant of Binary Reader for the Big Endian console files.
        {
            BinaryReaderBigEndian binaryReader1 = new BinaryReaderBigEndian(fileInput);
            fileCount = binaryReader1.ReadInt32();
            textBox1.Text = fileCount.ToString();

            fileSizes = new int[fileCount];
            progressBar1.Value = 0;
            int runOnce = 0;
            for (int i = 0; i < fileCount; i++)
            {
                long hash = binaryReader1.ReadInt64();
                int offset = binaryReader1.ReadInt32();
                int size1 = binaryReader1.ReadInt32();
                int size2 = binaryReader1.ReadInt32();
                byte watisdis = binaryReader1.ReadByte();

                progressBar1.Value++;

                fileSizes[i] = size1;
                //MessageBox.Show(offset.ToString());

                if (runOnce == 0)
                {
                    initialOffset = offset;
                }
            }

            generateTreeList();
        }

        private void generateTreeList() //Generates list of all scripts and adds them to tree view.
        {
            

            fileInput.Seek(0, 0);
            BinaryReader binaryReader1 = new BinaryReader(fileInput);
            fileNames = new List<String>();
            filePaths = new List<String>();
            binaryReader1.ReadBytes(fileCount * 21 + 1);
            Byte[] stringBytes;
            //string text = binaryReader1.ReadString();
            //MessageBox.Show(text);


            for (int i = 0; i < fileCount; i++)
            {
                string fileName = "";
                stringBytes = binaryReader1.ReadBytes(130);
                foreach(byte imabyteyo in stringBytes)
                {
                    //MessageBox.Show(imabyteyo.ToString());
                    String letter = Convert.ToChar(imabyteyo).ToString();
                    //MessageBox.Show(letter);
                    if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890\\.".Contains(letter)) //Grabs 130 characters, sifts through and builds a raw string to work with.
                    {
                        fileName += letter;
                    }
                }

                //Console.WriteLine(fileName);
                fileName = fileName.Substring(0, fileName.IndexOf(".lua")); //removes file extension

                //Grab Folder Path
                String exportFolderPath = Path.GetFullPath(fileName).Substring(Path.GetFullPath(fileName).IndexOf("\\projects\\")); //Cuts the user folders out, leaving the file path.

                if(exportFolderPath[0].Equals('W'))
                {
                    exportFolderPath = "\\" + exportFolderPath;
                }
                exportFolderPath = exportFolderPath.Substring(0, exportFolderPath.LastIndexOf('\\'));
                //MessageBox.Show(exportFolderPath);
                filePaths.Add(exportFolderPath);

                //Grab File Name
                fileName = Path.GetFileName(fileName);

                ///////////////////////////// Code responsible for listing files by directory ////////////////////////////////////
                if (treeView1.Nodes.ContainsKey(exportFolderPath)) //Check if the treeview has this subtype already.
                {
                    treeView1.Nodes[exportFolderPath].Nodes.Add(fileName); //Add the file to that subtype
                } else
                {
                    treeView1.Nodes.Add(exportFolderPath, exportFolderPath); //Create the subtype first
                    treeView1.Nodes[exportFolderPath].Nodes.Add(fileName); //Add the file to that subtype
                }

                fileNames.Add(fileName + ".lua");
                //treeView1.Nodes.Add(fileName + ".lua"); //Just adds all scripts on same level with just the name and extension.
                fileInput.Seek(fileInput.Position - 130,0);
                binaryReader1.ReadBytes(fileSizes[i]);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            }
            //MessageBox.Show(fileNames.Count.ToString());
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            extractFiles();
        } //Menu Button to extracted all scripts from loaded LuaScripts file.



        private void extractFiles() //Reads the file input and extracts it to a directory then sends the path of the file to be decompiled by another function.
        {
            if (fileInput != null)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Value = 0;

                    fileInput.Seek(3, 0);
                    BinaryReader binaryReader1 = new BinaryReader(fileInput);
                    binaryReader1.ReadBytes(fileCount * 21 +1);// + 1);

                    for (int i = 0; i < fileCount; i++)
                    {
                        //Instantiate byteArray to contain file data.
                        byte[] wereBytesYo = new byte[fileSizes[i]];

                        progressBar1.Value++;

                        int index = 0;
                        //Read Data and fill byteArray with it.
                        foreach (byte imabyteyo in binaryReader1.ReadBytes(fileSizes[i]))
                        {
                            wereBytesYo[index] = imabyteyo;
                            index++;
                        }


                        //Generate required folders if they do not exist.
                        if (!Directory.Exists(folderBrowserDialog1.SelectedPath + filePaths[i]))
                        {
                            Directory.CreateDirectory(Path.Combine(folderBrowserDialog1.SelectedPath + filePaths[i]));
                        }
                        //Write File to folders.
                        File.WriteAllBytes(Path.Combine(folderBrowserDialog1.SelectedPath + filePaths[i], fileNames[i]), wereBytesYo);
                        decompileFile(Path.Combine(folderBrowserDialog1.SelectedPath + filePaths[i], fileNames[i]), folderBrowserDialog1.SelectedPath, i);
                    }
                    fileInput.Dispose();
                    fileInput.Close();
                }
            }
            else
            {
                MessageBox.Show("You must open the archive first.");
            }

            progressBar1.Value = 0;
        }




        private void decompileFile(String fileDirectory, String selectedDirectory, int index) //Takes extracted file and sends it to be decompiled via unluac.jar.
        {


            String newFileDirectory = fileDirectory.Substring(selectedDirectory.Length);
            String selectedDirectoryWithDecompileFolder = selectedDirectory + "\\Decompiled\\" + newFileDirectory;
            String finalOutputDirectory = selectedDirectoryWithDecompileFolder.Substring(0,selectedDirectoryWithDecompileFolder.LastIndexOf('\\'));

            String fileName = Path.GetFileName(selectedDirectoryWithDecompileFolder);


            if (!Directory.Exists(finalOutputDirectory))
            {
                Directory.CreateDirectory(finalOutputDirectory);
            }
            
            Process doctorProc = new Process();
            doctorProc.StartInfo.FileName = "java";
            doctorProc.StartInfo.Arguments = "-jar unluac.jar " + fileDirectory;
            doctorProc.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            doctorProc.StartInfo.RedirectStandardOutput = true;
            doctorProc.StartInfo.UseShellExecute = false;
            doctorProc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            doctorProc.StartInfo.CreateNoWindow = true;
            doctorProc.Start();
            String luaScript = doctorProc.StandardOutput.ReadToEnd();
            File.WriteAllText(Path.Combine(finalOutputDirectory,fileName), luaScript);
            doctorProc.WaitForExit();
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
        } //Returns current path of the program, minus the application's exe.

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_About about1 = new Sabtool_About();
            about1.Show();
        } //Menu Button, opens the About window.

        private void select360PS3FIleToolStripMenuItem_Click(object sender, EventArgs e) //Allows user to select an Xbox 360/PS3 file from a custom location.
        {
            openFileDialog1.Filter = "Luap files (*.luap)|*.luap";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileInput = openFileDialog1.OpenFile();
                loadLuapFileBigEndian();
            }
            progressBar1.Value = 0;
        }
    }




    class BinaryReaderBigEndian : BinaryReader
    {
        private byte[] a16 = new byte[2];
        private byte[] a32 = new byte[4];
        private byte[] a64 = new byte[8];
        public BinaryReaderBigEndian(System.IO.Stream stream) : base(stream) { }
        public override int ReadInt32()
        {
            a32 = base.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToInt32(a32, 0);
        }
        public override Int16 ReadInt16()
        {
            a16 = base.ReadBytes(2);
            Array.Reverse(a16);
            return BitConverter.ToInt16(a16, 0);
        }
        public override Int64 ReadInt64()
        {
            a64 = base.ReadBytes(8);
            Array.Reverse(a64);
            return BitConverter.ToInt64(a64, 0);
        }
        public override UInt32 ReadUInt32()
        {
            a32 = base.ReadBytes(4);
            Array.Reverse(a32);
            return BitConverter.ToUInt32(a32, 0);
        }

    } //Reads Big Endian files, like for the console versions of the files.
}
