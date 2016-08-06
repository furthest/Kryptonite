using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace Kryptonite
{   //Codice scritto da Gianmatteo Palmieri e Michele Bevilacqua
    public partial class Form2 : Form
    {

        string decryptText = null;
        string cryptFile = null;

        string folderPath = null;

        private const string salt = "ph2DoYmgABfXlyD8JExHjdgfcUBaspNooUoCwtMolIi9jS8eWFedWTW77Sr3WIV";

        public Form2()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    DialogResult dialog = saveFileDialog1.ShowDialog();
                    string file = saveFileDialog1.FileName;
                    KryptoByteToFile(file, KryptoToByte(textBox3.Text, textBox4.Text));
                }
                else
                {
                    MessageBox.Show("Non hai inserito la chiave di decrittazione, puoi generarne una con il pulsante o usarne una a tua scelta.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Non hai inserito un testo da criptare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tabControl1.SelectTab("tabPage1");
            panel1.Visible = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox4.Text))
            {
                button6.Visible = true;
                label17.Visible = true;
                textBox4.Size = new Size(517, 25);
            }
            else
            {
                button6.Visible = false;
                label17.Visible = false;
                textBox4.Size = new Size(316, 25);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(decryptText))
            {
                MessageBox.Show("Devi prima scegliere il file .krypt da decriptare!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!decryptText.Contains(".krypt"))
                {
                    MessageBox.Show("Per piacere scegli un file .krypt!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(textBox7.Text))
                    {
                        textBox8.Text = KryptoToString(System.IO.File.ReadAllBytes(decryptText), textBox7.Text);
                        if (Kryptons.isDecrypted == true)
                        {
                            textBox8.Visible = true;
                        }                      
                    }
                    else
                    {
                        MessageBox.Show("Non hai inserito la chiave di decrittazione!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage8");
        }

        private void textBox8_VisibleChanged(object sender, EventArgs e)
        {
            if (textBox8.Visible == true )
            {
                label21.Text = "Testo decriptato";
            }
            else
            {
                label21.Text = "Inserisci chiave";
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox6.Text))
            {
                button10.Visible = true;
            }
            else
            {
                button10.Visible = false;
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox7.Text))
            {
                button9.Visible = true;
            }
            else
            {
                button9.Visible = false;
            }
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                textBox7.UseSystemPasswordChar = false;
            }
            else
            {
                textBox7.UseSystemPasswordChar = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                textBox6.UseSystemPasswordChar = false;
            }
            else
            {
                textBox6.UseSystemPasswordChar = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            DialogResult dialog = openFileDialog2.ShowDialog();
            cryptFile = openFileDialog2.FileName;
            textBox1.Text = cryptFile;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = Random256Key();
        }


        public bool KryptoByteToFile(string file, byte[] byteToFile)
        {
            System.IO.FileStream fileStream = new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            fileStream.Write(byteToFile, 0, byteToFile.Length);
            fileStream.Close();
            return true;
        }

        private byte[] KryptoToByte(string text, string key)
        {
            byte[] encryptedByte = null;

            using (var algorithm = new RijndaelManaged())
            {
                algorithm.KeySize = 256;
                algorithm.BlockSize = 128;

                var hashedKey = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt));
                algorithm.Key = hashedKey.GetBytes(algorithm.KeySize / 8);
                algorithm.IV = hashedKey.GetBytes(algorithm.BlockSize / 8);

                encryptedByte = Kryptonite.Kryptons.EncryptStringToBytes(text, algorithm.Key, algorithm.IV);
            }

            return encryptedByte;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dialog = openFileDialog1.ShowDialog();
            decryptText = openFileDialog1.FileName;
            textBox5.Text = decryptText;
         
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(decryptText))
            {
                if (!string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    string newFile = null;
                    newFile = decryptText.Replace(".krypt", "");
                    Kryptonite.Kryptons.DecryptFile(decryptText, newFile, textBox6.Text);
                }
                else
                {
                    MessageBox.Show("Non hai inserito la chiave di decrittazione!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Non hai scelto nessun file da decriptare!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private string KryptoToString(byte[] encryptedByte, string key)
        {
            string decryptedString = null;

            using (var algorithm = new RijndaelManaged())
            {
                algorithm.KeySize = 256;
                algorithm.BlockSize = 128;

                var hashedKey = new Rfc2898DeriveBytes(key, Encoding.ASCII.GetBytes(salt));
                algorithm.Key = hashedKey.GetBytes(algorithm.KeySize / 8);
                algorithm.IV = hashedKey.GetBytes(algorithm.BlockSize / 8);

                decryptedString = Kryptonite.Kryptons.DecryptStringFromBytes(encryptedByte, algorithm.Key, algorithm.IV);
            }
            return decryptedString;
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cryptFile))

            {
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    Kryptonite.Kryptons.EncryptFile(cryptFile, cryptFile + ".krypt", textBox2.Text);
                }
                else       
                {
                    MessageBox.Show("Non hai inserito la chiave di decrittazione, puoi generarne una con il pulsante o usarne una a tua scelta.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Non hai scelto nessun file da criptare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tabControl1.SelectTab("tabPage2");
        }


        public string Random256Key()
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            int length = 64;
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("AES (Advanced Encryption Standard) è l'implementazione dell'algoritmo Rijndael progettato da Vincent Rijmen e Joan Daemen.E' fino ad adesso il più sicuro algoritmo mai creato, talmente sicuro che viene usato anche da istituti governativi.Utilizzando una chiave lunga 256 bit ci vorrebbe un infinità di tempo per decriptare un file con la tecnica del brute force, si parla di bilioni e bilioni di anni!", "Info su AES", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage1");
        }


        private void label29_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage10");
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage2");
            textBox9.Text = "";
            textBox10.Text = "";
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox10.Text))
            {
                textBox9.Enabled = true;
            }
            else
            {
                textBox9.Enabled = false;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox9.Text))
            {
                button11.Visible = true;
                label31.Visible = true;
                textBox9.Size = new Size(542, 25);
            }
            else
            {
                button11.Visible = false;
                label31.Visible = false;
                textBox9.Size = new Size(341, 25);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox5.Checked == true)
            {
                textBox9.UseSystemPasswordChar = false;
            }
            else
            {
                textBox9.UseSystemPasswordChar = true;
            }
        }

        private void label28_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage9");
        }


        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                textBox11.UseSystemPasswordChar = false;
            }
            else
            {
                textBox11.UseSystemPasswordChar = true;
            }
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            DialogResult dialog = folderBrowserDialog1.ShowDialog();
            folderPath = folderBrowserDialog1.SelectedPath.ToString();
            textBox10.Text = folderPath;
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            string zippedPath = null;

            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                if (!string.IsNullOrWhiteSpace(textBox9.Text))
                {
                    zippedPath = folderPath + ".zip";
                    ZipFile.CreateFromDirectory(folderPath, folderPath + ".zip");
                    Kryptonite.Kryptons.EncryptFile(zippedPath, zippedPath + ".krypt", textBox9.Text);
                    Directory.Delete(folderPath, true);
                    zippedPath = folderPath + ".krypt";

                }
                else
                {
                    MessageBox.Show("Non hai inserito la chiave di decrittazione, puoi generarne una con il pulsante o usarne una a tua scelta.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Non hai scelto nessuna cartella da criptare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            tabControl1.SelectTab("tabPage2");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox9.Text = Random256Key();
        }

        private void label31_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox9.Text))
            {
                System.Windows.Forms.Clipboard.SetText(textBox9.Text);
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox11.Text))
            {
                button14.Visible = true;
            }
            else
            {
                button14.Visible = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(decryptText))
            {
                if (!string.IsNullOrWhiteSpace(textBox11.Text))
                {
                    string newFile = null;
                    newFile = decryptText.Replace(".krypt", "");
                    Kryptonite.Kryptons.DecryptFolder(decryptText, newFile, textBox11.Text);
                }
                else
                {
                    MessageBox.Show("Non hai inserito la chiave di decrittazione!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Non hai scelto nessun file da decriptare!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Shown_1(object sender, EventArgs e)
        {
            label24.Text = "Versione: " + Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage5");
            textBox11.Text = "";
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab("tabPage2");
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab("tabPage5");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage1");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage2");
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage3");
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                button4.Visible = true;

            }
            else
            {
                button4.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage4");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage2");
            textBox3.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
            }

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                button3.Visible = true;
                label16.Visible = true;
                textBox2.Size = new Size(542, 25);
            }
            else
            {
                button3.Visible = false;
                label16.Visible = false;
                textBox2.Size = new Size(341, 25);
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox2.Text = Random256Key();


        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                System.Windows.Forms.Clipboard.SetText(textBox2.Text);
            }
        }

        private void textBox2_EnabledChanged(object sender, EventArgs e)
        {
            if (textBox2.Enabled == false)
            {
                button3.Visible = false;
            }
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox4.UseSystemPasswordChar = false;
            }
            else
            {
                textBox4.UseSystemPasswordChar = true;
            }
        }

        private void label17_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox4.Text))
            {
                System.Windows.Forms.Clipboard.SetText(textBox4.Text);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox5.Text))
            {
                pictureBox9.Visible = true;
                pictureBox11.Visible = true;
                pictureBox19.Visible = true;
                label18.Visible = true;
                label14.Visible = true;
                label29.Visible = true;

            }
            else
            {
                pictureBox9.Visible = false;
                pictureBox11.Visible = false;
                pictureBox19.Visible = false;
                label18.Visible = false;
                label14.Visible = false;
                label29.Visible = false;
            }


        }

        private void label14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage7");
        }

        private void label18_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage6");

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox6.Text = "";
            tabControl1.SelectTab("tabPage1");
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage5");
            textBox6.Text = "";
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage5");
            textBox8.Text = "";
            textBox8.Visible = false;
            textBox7.Text = "";
            label21.Text = "Inserisci chiave";
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
