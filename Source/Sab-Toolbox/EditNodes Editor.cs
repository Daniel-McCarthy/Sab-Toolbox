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
    public partial class EditNodes_Editor : Form
    {
        private Stream fileInput = null;
        private List<int> fileSizes;
        private List<int> fileOffsets;
        private List<byte[]> listOfFileArrays;

        public EditNodes_Editor()
        {
            InitializeComponent();
            fileSizes = new List<int>();
            fileOffsets = new List<int>();
            listOfFileArrays = new List<byte[]>();
        }

        private void openLoosefilesBinPCpackToolStripMenuItem_Click(object sender, EventArgs e)
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
                            fileInput = File.Open(Path.Combine(path + "\\France\\", "Loosefiles_BinPC.pack"), FileMode.Open); ;
                            extractEditNodesPackFromLooseFiles();
                            loadEditNodes();
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_Settings settings = new Sabtool_Settings();
            settings.Show();
        }

        private void extractEditNodesPackFromLooseFiles()
        {
            BinaryReader binReader1 = new BinaryReader(fileInput);

            fileInput.Seek(38500424, 0); //Skip all the data prior to GameTemplates.wsd
            String versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(31)); //Check if we found the right file
            if (versionNumber.Equals("France\\EditNodes\\EditNodes.pack"))
            {
                fileInput.Seek(38500424, 0); //if yes, go back to the start of the file

                byte[] gameTemplatesArray = binReader1.ReadBytes(41107032 - 38500424);

                MemoryStream gameTemplatesStream = new MemoryStream(gameTemplatesArray);

                binReader1.Dispose();
                binReader1.Close();
                fileInput.Close();

                fileInput = gameTemplatesStream;
            }
            else
            {
                binReader1.Dispose();
                binReader1.Close();
                MessageBox.Show("Oh well");
                //extractGameTemplatesFromModifiedLooseFiles();
            }
        }

        private void loadEditNodes()
        {
            BinaryReader binReader1 = new BinaryReader(fileInput);

            String versionNumber = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(31)); //Check if we found the right file
            //MessageBox.Show(versionNumber);
            if (versionNumber.Equals("France\\EditNodes\\EditNodes.pack"))
            {
                fileInput.Seek(89, SeekOrigin.Current);
            }
            else
            {
                fileInput.Seek(-31, SeekOrigin.Current);
            }

            if (System.Text.Encoding.Default.GetString(binReader1.ReadBytes(4)) == "00ED")
            {
                int fileCount = binReader1.ReadInt32();
                //MessageBox.Show(fileCount.ToString());

                for (int i = 0; i < fileCount; i++)
                {
                    string hash = System.Text.Encoding.Default.GetString(binReader1.ReadBytes(4));
                    int size = binReader1.ReadInt32();
                    int offset = binReader1.ReadInt32(); //offset is not including the 120 bytes at the start of the file
                    fileSizes.Add(size);
                    fileOffsets.Add(offset);
                }
                long offset1 = fileInput.Position;

                for (int i = 0; i < fileCount; i++)
                {
                    if (fileInput.Position - 120 == fileOffsets[i])
                    {
                        byte[] file = binReader1.ReadBytes(fileSizes[i]);
                        listOfFileArrays.Add(file);

                        //File.WriteAllBytes("C:\\Users\\Dan\\Desktop\\editnodes\\" + i + ".node", file);
                    }
                    else
                    {
                        MessageBox.Show("File Reading has been derailed. I: " + i + " Position-120: " + (fileInput.Position - 120) + " File offset: " + fileOffsets[i]);
                    }
                }

                long offset2 = fileInput.Position;
                ;
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
        } //Returns current path of the program, minus the application's exe.

    }
}
