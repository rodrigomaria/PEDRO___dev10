using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelasProjetoPedro
{
    public partial class SelecionarArquivoCriptografar : Form
    {
        public SelecionarArquivoCriptografar()
        {
            InitializeComponent();
        }

        private string skey;
        private string inputFile;
        private string outputFile;
        private Home home;

        //gerar chave
        private string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        private void ProcurarArquivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Selecione arquivo para encriptar";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                this.inputFile = @openFileDialog1.FileName;
                this.skey = GenerateKey() + GenerateKey();
                textBox1.Text = skey;
                this.outputFile = @"C:\Users\rodri\Documents\PEDRO\" + System.DateTime.Now.Ticks.ToString();
            }

            
        }

        private void BotaoEncriptar_Click(object sender, EventArgs e)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                        cs.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Arquivo encriptado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                    "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SelecionarArquivoCriptografar_Load(object sender, EventArgs e)
        {

        }
    }
}
