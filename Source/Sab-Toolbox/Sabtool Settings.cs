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
    public partial class Sabtool_Settings : Form
    {
         
        public Boolean rememberPath = false;
        public Boolean pathValidated = false;

        public Sabtool_Settings()
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
                            MessageBox.Show(path);
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
                        System.IO.File.WriteAllText(Path.Combine(path, "settings.txt"), output);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                rememberPath = true;
                //MessageBox.Show(pictureBox5.Image.GetHashCode().ToString());
                //MessageBox.Show(pictureBox5.ImageLocation);
                //if (pictureBox5.Image.GetHashCode() == 30631159 || pictureBox5.Image.GetHashCode() == 47096010)
                if(pathValidated)
                {
                    //MessageBox.Show("Test");
                    string path = Application.ExecutablePath;
                    path = path.Substring(0, path.Length - 15);

                    if (!File.Exists(Path.Combine(path, "settings.txt")))
                    {
                        //Settings Do Not Exist, User Wants Path Saved, Create Settings File
                        File.WriteAllText(Path.Combine(path, "settings.txt"), Properties.Resources.config);
                    }

                    //Read Settings File and Write Path To First Line
                    string[] lines = File.ReadAllLines(Path.Combine(path, "settings.txt"));
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

                    File.WriteAllText(Path.Combine(path, "settings.txt"), output);
                }
            } else {
                rememberPath = false;
            }
        }


          //////////////////////
         // Return Game Path //
        //////////////////////
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





    }
}
