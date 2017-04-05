namespace TelasProjetoPedro
{
    partial class SelecionarArquivoDesincriptar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelecionarArquivoDesincriptar));
            this.BotaoSelecionarArquivo = new System.Windows.Forms.Button();
            this.BotaoDecriptar = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BotaoSelecionarArquivo
            // 
            this.BotaoSelecionarArquivo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotaoSelecionarArquivo.Location = new System.Drawing.Point(124, 34);
            this.BotaoSelecionarArquivo.Name = "BotaoSelecionarArquivo";
            this.BotaoSelecionarArquivo.Size = new System.Drawing.Size(132, 28);
            this.BotaoSelecionarArquivo.TabIndex = 0;
            this.BotaoSelecionarArquivo.Text = "Selecionar arquivo";
            this.BotaoSelecionarArquivo.UseVisualStyleBackColor = true;
            this.BotaoSelecionarArquivo.Click += new System.EventHandler(this.BotaoSelecionarArquivo_Click);
            // 
            // BotaoDecriptar
            // 
            this.BotaoDecriptar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotaoDecriptar.Location = new System.Drawing.Point(160, 187);
            this.BotaoDecriptar.Name = "BotaoDecriptar";
            this.BotaoDecriptar.Size = new System.Drawing.Size(75, 29);
            this.BotaoDecriptar.TabIndex = 1;
            this.BotaoDecriptar.Text = "Decriptar";
            this.BotaoDecriptar.UseVisualStyleBackColor = true;
            this.BotaoDecriptar.Click += new System.EventHandler(this.BotaoDecriptar_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(35, 136);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(318, 20);
            this.textBox1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Selecione o arquivo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 34);
            this.label1.TabIndex = 3;
            this.label1.Text = "Insira na caixa abaixo a chave\r\nde segurança do arquivo";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SelecionarArquivoDesincriptar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.BotaoDecriptar);
            this.Controls.Add(this.BotaoSelecionarArquivo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelecionarArquivoDesincriptar";
            this.Text = "Decriptar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BotaoSelecionarArquivo;
        private System.Windows.Forms.Button BotaoDecriptar;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
    }
}