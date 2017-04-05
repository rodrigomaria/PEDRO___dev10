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

namespace TelasProjetoPedro
{
    public partial class TelaUpload : Form
    {
        public TelaUpload()
        {
            {
                int partes = 5;
                InitializeComponent();
                comboBox1.Items.Add("Nenhum");
                for (int i = 1; i < partes; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }
                comboBox1.Items.Add("Todos");

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int destinos = 9;
            List<string> caminho = new List<string>();
            string caminhoLocal = @"C:\Users\rodri\Desktop\destinoLocal\";
            if (!Directory.Exists(caminhoLocal))
            {
                Directory.CreateDirectory(caminhoLocal);
            }

            for (int i = 1; i <= destinos; i++)
            {
                caminho.Add(@"C:\Users\rodri\Desktop\destino" + i + @"\");
                if (!Directory.Exists(@"C:\Users\rodri\Desktop\destino" + i + @"\"))
                {
                    Directory.CreateDirectory(@"C:\Users\rodri\Desktop\destino" + i + @"\");
                }
            }

            List<string> arquivos = new List<string>();
            Random random = new Random();
            int num;
            for (int i = 0; i < (comboBox1.Items.Count - 1); i++)
            {
                if (comboBox1.SelectedIndex > i)
                {
                    num = random.Next(1, destinos);
                    FileStream fs = File.Create(caminho[num] + "parte" + (i + 1) + ".txt");
                    fs.Close();
                    File.WriteAllText(caminho[num] + "parte" + (i + 1) + ".txt", "texto criptografado" + (i + 1));
                }
                else
                {
                    num = random.Next(1, destinos);
                    FileStream fs = File.Create(caminhoLocal + "parte" + (i + 1) + ".txt");
                    fs.Close();
                    File.WriteAllText(caminhoLocal + "parte" + (i + 1) + ".txt", "texto criptografado" + (i + 1));
                }

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }
    }
}
