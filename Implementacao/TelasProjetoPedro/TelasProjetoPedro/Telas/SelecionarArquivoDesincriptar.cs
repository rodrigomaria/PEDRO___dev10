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
    public partial class SelecionarArquivoDesincriptar : Form
    {
        public SelecionarArquivoDesincriptar()
        {
            InitializeComponent();
        }

        private string skey;
        private string inputFile;
        private string outputFile;

        private void BotaoSelecionarArquivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Selecione arquivo para decriptar";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                this.inputFile = @openFileDialog1.FileName;
            }
        }

        private void BotaoDecriptar_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Selecione diretório de saída do arquivo decriptado";
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result != DialogResult.Cancel)
            {
                this.outputFile = @folderBrowserDialog1.SelectedPath + "\\Decrypted";
                this.skey = textBox1.Text;
            }

            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(skey);

                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(skey);

                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                        fsOut.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Arquivo decriptado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro.\nInfo para desenvolvedores: " + ex.HelpLink +
                    "\n" + ex.Message + "\n" + ex.Data + "\n" + ex.StackTrace);
            }
        }
    }
}
