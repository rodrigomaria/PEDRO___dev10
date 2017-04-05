using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelasProjetoPedro
{
    public partial class Home : Form
    {
        public Home()
        {
           InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
        
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            new SelecionarArquivoCriptografar().Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            new SelecionarArquivoDesincriptar().Show();
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            new TelaDivisaoUpload().Show();
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            //new SelecionarArquivoCriptografar().Show();
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            new TelaUpload().Show();
        }
    }
}
