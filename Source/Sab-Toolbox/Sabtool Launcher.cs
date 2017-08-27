using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sab_Toolbox
{
    public partial class Sabtool_Launcher : Form
    {
        public Boolean rememberPath = false;
        public Boolean pathValidated = false;

        public Sabtool_Launcher()
        {
            InitializeComponent();

            string path = Application.ExecutablePath;
            path = path.Substring(0, path.Length - 15);

            //Does Settings File Exist?
            if (File.Exists(Path.Combine(path, "settings.txt")))
            {
                //Settings Exist, Now Check Path Setting
                //MessageBox.Show("Settings already exist");
                //MessageBox.Show(path);
                string[] readText = File.ReadAllLines(Path.Combine(path, "settings.txt"));
                string pathInput = readText[0];
                if (pathInput.Length > 5)
                {
                    //Check if Path Setting Is Valid
                    pathInput = pathInput.Substring(5, pathInput.Length - 5);
                    if (File.Exists(Path.Combine(pathInput, "Saboteur.exe")))
                    {
                        //It is, now set up path and image to tell user their path is set.
                        pictureBox5.Image = Properties.Resources.Box2;
                        textBox1.Text = pathInput;
                        pathValidated = true;
                    }
                    else
                    {
                        //The setting is no longer valid
                        MessageBox.Show("Has your game been moved? Your Saboteur Game Path is not valid, please re-select your Saboteur Game Path.");
                        textBox1.Text = "";
                        pictureBox5.Image = Properties.Resources.Box3;
                        pathValidated = false;
                    }
   
                }
            }
        }

        private void Sabtool_Launcher_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Blueprint_Editor blueprintEditor1 = new Blueprint_Editor();
            blueprintEditor1.Show();
            
        }

        private void textBox1_Click(object sender, EventArgs e)
        {

            //Open Folder Dialog and Select User Folder
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                String selectedPath = folderBrowserDialog1.SelectedPath;
                textBox1.Text = selectedPath;

                //Check if selected folder contains the game .exe
                if (File.Exists(Path.Combine(selectedPath, "Saboteur.exe")))
                {
                    //This is the correct folder.
                    pictureBox5.Image = Properties.Resources.Box2;
                    //MessageBox.Show("This is The Saboteur folder.");
                    pathValidated = true;



                    //If User Wants To Save Path, Check for settings file and save path to it. Or create one.
                    if (rememberPath == true)
                    {
                        string path = Application.ExecutablePath;
                        path = path.Substring(0, path.Length - 15);
                        //Does Settings File Exist?
                        if (!File.Exists(Path.Combine(path, "settings.txt")))
                        {
                            //Settings Do Not Exist, User Wants Path Saved, Create Settings File
                            File.WriteAllText(Path.Combine(path, "settings.txt"), Properties.Resources.config);
                        }

                        //Read Settings File and Write Path To First Line
                        string[] lines = System.IO.File.ReadAllLines(Path.Combine(path, "settings.txt"));
                        string output = "";

                        //Add Path To Settings File
                        if (lines[0] != null)
                        {
                            lines[0] = "Path:" + selectedPath;
                        }

                        //Overwrite Settings File with new version.
                        for (int i = 0; i < lines.Length; i++)
                        {
                            output += lines[i] + "\n";
                        }
                        //MessageBox.Show(path);
                        System.IO.File.WriteAllText(Path.Combine(path,"settings.txt"), output);
                    }





                }
                else
                {
                    //Incorrect folder selected.
                    pictureBox5.Image = Properties.Resources.Box3;
                    MessageBox.Show("This is not The Saboteur game folder. It must contain the executable.");
                    pathValidated = false;

                }



            }

        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                rememberPath = true;
                //MessageBox.Show(pictureBox5.Image.GetHashCode().ToString());
                //if(pictureBox5.Image.GetHashCode() == 36038289)
                if(pathValidated)
                {
                    string path = Application.ExecutablePath;
                    path = path.Substring(0, path.Length - 15);

                    if (!File.Exists(Path.Combine(path, "settings.txt")))
                    {
                        //Settings Do Not Exist, User Wants Path Saved, Create Settings File
                        File.WriteAllText(path, Properties.Resources.config);
                    }

                    //Read Settings File and Write Path To First Line
                    string[] lines = System.IO.File.ReadAllLines(Path.Combine(path, "settings.txt"));
                    string output = "";

                    //Add Path To Settings File
                    if (lines[0] != null)
                    {
                        lines[0] = "Path:" + textBox1.Text; 
                    }

                    //Overwrite Settings File with new version.
                    for (int i = 0; i < lines.Length; i++)
                    {
                        output += lines[i] + "\n";
                    }

                    System.IO.File.WriteAllText(Path.Combine(path, "settings.txt"), output);
                }
            } else {
                rememberPath = false;
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LuaScript_Editor luaScripts1 = new LuaScript_Editor();
            luaScripts1.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_About about1 = new Sabtool_About();
            about1.Show();
        }

        private void specialThanksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sabtool_Thanks thanks1 = new Sabtool_Thanks();
            thanks1.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Loosefiles_Editor loosefiles1 = new Loosefiles_Editor();
            loosefiles1.Show();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Megapack_Editor megapack1 = new Megapack_Editor();
            megapack1.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Pack_Editor pack1 = new Pack_Editor();
            pack1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EditNodes_Editor node1 = new EditNodes_Editor();
            node1.Show();
        }

        private void blueprintsEditor_Click(object sender, EventArgs e)
        {
            Blueprint_Selector selector1 = new Blueprint_Selector();
            selector1.Show();
        }
    }
}
