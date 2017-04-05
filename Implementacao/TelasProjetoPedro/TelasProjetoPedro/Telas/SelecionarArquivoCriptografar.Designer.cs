namespace TelasProjetoPedro
{
    partial class SelecionarArquivoCriptografar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelecionarArquivoCriptografar));
            this.BotaoProcurarArquivo = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.BotaoEncriptar = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BotaoProcurarArquivo
            // 
            this.BotaoProcurarArquivo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotaoProcurarArquivo.Location = new System.Drawing.Point(117, 36);
            this.BotaoProcurarArquivo.Name = "BotaoProcurarArquivo";
            this.BotaoProcurarArquivo.Size = new System.Drawing.Size(150, 26);
            this.BotaoProcurarArquivo.TabIndex = 0;
            this.BotaoProcurarArquivo.Text = "Procurar Arquivo";
            this.BotaoProcurarArquivo.UseVisualStyleBackColor = true;
            this.BotaoProcurarArquivo.Click += new System.EventHandler(this.ProcurarArquivo_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // BotaoEncriptar
            // 
            this.BotaoEncriptar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotaoEncriptar.Location = new System.Drawing.Point(160, 184);
            this.BotaoEncriptar.Name = "BotaoEncriptar";
            this.BotaoEncriptar.Size = new System.Drawing.Size(75, 29);
            this.BotaoEncriptar.TabIndex = 1;
            this.BotaoEncriptar.Text = "Encriptar";
            this.BotaoEncriptar.UseVisualStyleBackColor = true;
            this.BotaoEncriptar.Click += new System.EventHandler(this.BotaoEncriptar_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(45, 147);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(295, 20);
            this.textBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(88, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 51);
            this.label1.TabIndex = 3;
            this.label1.Text = "Chave de segurança do arquivo:\r\nESSA CHAVE É IMPORTANTE.\r\nANOTE-A EM UM LOCAL SEG" +
    "URO.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // SelecionarArquivoCriptografar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.BotaoEncriptar);
            this.Controls.Add(this.BotaoProcurarArquivo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelecionarArquivoCriptografar";
            this.Text = "Criptografar";
            this.Load += new System.EventHandler(this.SelecionarArquivoCriptografar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BotaoProcurarArquivo;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button BotaoEncriptar;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}