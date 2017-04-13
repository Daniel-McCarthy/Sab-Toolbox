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
    public partial class Pack_Editor : Form
    {
        public Pack_Editor()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_About about1 = new Sabtool_About();
            about1.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_Settings settings1 = new Sabtool_Settings();
            settings1.Show();
        }

        private void Pack_Editor_Load(object sender, EventArgs e)
        {
            string path = Sabtool_Settings.returnGamePath();
            if (path != "PathInvalid" && path != "NoSettingsFile")
            {
                String[] allPackFiles = System.IO.Directory.GetFiles(path, "*.pack*", System.IO.SearchOption.AllDirectories);
                String[] allDynPackFiles = System.IO.Directory.GetFiles(path, "*.dynpack*", System.IO.SearchOption.AllDirectories);
                String[] allCinPackFiles = System.IO.Directory.GetFiles(path, "*.cinpack*", System.IO.SearchOption.AllDirectories);
                String[] allCnvPackFiles = System.IO.Directory.GetFiles(path, "*.cnvpack*", System.IO.SearchOption.AllDirectories);


                List<String[]> allFiles = new List<String[]>();
                allFiles.Add(allPackFiles);
                allFiles.Add(allDynPackFiles);
                allFiles.Add(allCinPackFiles);
                allFiles.Add(allCnvPackFiles);

                //MessageBox.Show(path);
                //MessageBox.Show(allPackFiles[0]);

                foreach(String[] arr in allFiles)
                {
                    foreach(string filePath in arr)
                    {
                        string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                        string newFilePath = filePath.Substring(path.Length+1);

                        string subType = "";
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            using (BinaryReader binaryReader1 = new BinaryReader(fileStream))
                            {
                                subType = reverseString(Encoding.Default.GetString(binaryReader1.ReadBytes(4)));
                            }
                        }

                        if (newFilePath.Contains('\\')) //If it isn't in the main directory, remove the file name from the folder path.
                        {
                            newFilePath = newFilePath.Substring(0, newFilePath.LastIndexOf('\\'));
                        }
                       
                        if (fileName == newFilePath) //If it's just a file name (main directory) just add it.
                        {
                            if (!fileName.Contains("loosefiles") && !fileName.Contains(".html")) //unless it's an .html or a loosefile.pack
                            {
                                packFileTreeView.Nodes.Add(fileName + " (" + subType + ")");
                            }
                        }
                        else
                        {
                            if (packFileTreeView.Nodes.ContainsKey(newFilePath))
                            {
                                packFileTreeView.Nodes[newFilePath].Nodes.Add(fileName + " (" + subType + ")");
                            }
                            else
                            {
                                packFileTreeView.Nodes.Add(newFilePath, newFilePath);

                                if (!fileName.Contains("loosefiles") && !fileName.Contains(".html"))
                                {
                                    packFileTreeView.Nodes[newFilePath].Nodes.Add(fileName + " (" + subType + ")");
                                }
                            }
                        }



                    }
                }



            }
        }


        String[] removeAt(int index, String[] arr)
        {
            String[] newArr = new String[arr.Length - 1];
            int usedElements = 0;

            for(int i = 0; i < arr.Length; i++)
            {
                if(i != index)
                {
                    newArr[usedElements] = arr[i];
                    usedElements++;
                }
            }
            return newArr;
        }

        private string reverseString(string textInput)
        {
            if (textInput == null) return null;

            char[] array = textInput.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }



    }

}


