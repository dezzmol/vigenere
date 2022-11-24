using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Vigenere
{
    public partial class Form1 : Form
    {
        string[] text;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ReadFile()
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string name = openFileDialog1.FileName;
                    text = File.ReadAllLines(name, Encoding.GetEncoding(1251));
                    
                    textBox1.Text = text[0];
                    textBox2.Text = text[1];
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }

        private void FileCheck(string property)
        {
            if (File.Exists("Output.txt") == false)
            {
                File.Create("Output.txt");
                SaveFile(property);
            }
            else
            {
                SaveFile(property);
            }
        }

        private void SaveFile(string property)
        {
            StreamWriter sw = new StreamWriter("Output.txt", true);
            sw.WriteLine(property);
            sw.WriteLine(textBox1.Text);
            sw.WriteLine("Key: " + textBox2.Text);
            sw.WriteLine(textBox3.Text);
            sw.WriteLine("\n");
            sw.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReadFile();
        }

        private string InputPassword()
        {
            if (textBox2.Text == "" || textBox2.Text.Length < 5)
            {
                return "error";
            }
            else
            {
                string s = textBox2.Text;
                return s;
            }
        }

        private string InputMessage()
        {
            if (textBox1.Text == "" || textBox1.Text.Length < 5)
            {
                return "error";
            }
            else
            {
                string s = textBox1.Text;
                return s;
            }
        }

        private void ClearTextBox(TextBox textBox)
        {
            textBox.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearTextBox(textBox3);
            string m = InputMessage();
            string k = InputPassword();

            if (m == "error")
            {
                MessageBox.Show("Введите ключ");
                ClearTextBox(textBox1);
                return;
            }

            if (k == "error")
            {
                MessageBox.Show("Введите сообщение");
                ClearTextBox(textBox2);
                return;
            }

            Vigenere vigenere = new Vigenere();
            textBox3.Text = vigenere.Encrypt(m, k);
            FileCheck("\nЗашифровать");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearTextBox(textBox3);
            string m = InputMessage();
            string k = InputPassword();

            if (m == "error")
            {
                MessageBox.Show("Введите ключ");
                ClearTextBox(textBox1);
                return;
            }

            if (k == "error")
            {
                MessageBox.Show("Введите сообщение");
                ClearTextBox(textBox2);
                return;
            }

            Vigenere vigenere = new Vigenere();
            textBox3.Text = vigenere.Decrypt(m, k);
            FileCheck("\nРасшифровать");
        }
    }

    public class Vigenere
    {
        string A = "абвгдеёжзийклмнопрстуфхцчщщъыьэюя";
        string letters;
        
        private string GetRepeatKey(string s, int n)
        {
            var p = s;
            while (p.Length < n)
                p += p;

            return p.Substring(0, n);
        }

        private string VigenereCode(string text, string password, bool encrypting = true)
        {
            var repeatKey = GetRepeatKey(password, text.Length);
            var retValue = "";
            var q = A.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = A.IndexOf(text[i]);
                var codeIndex = A.IndexOf(repeatKey[i]);
                if (letterIndex < 0)
                {
                    retValue += text[i].ToString();
                }
                else
                {
                    retValue += A[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                }
            }

            return retValue;
        }

        public string Encrypt(string Message, string password) => VigenereCode(Message, password);

        public string Decrypt(string Message, string password) => VigenereCode(Message, password, false);


    }
}
