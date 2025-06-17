namespace CrucigramaApp
{
    partial class MainForm
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
            panelCrucigrama = new Panel();
            btnGenerar = new Button();
            numLongitudMinimaPalabraRaiz = new NumericUpDown();
            label1 = new Label();
            lstPalabras = new ListBox();
            label2 = new Label();
            txtNuevaPalabra = new TextBox();
            btnAgregarPalabra = new Button();
            lblLongitudMinima = new Label();
            txtNuevaPista = new TextBox();
            label4 = new Label();
            btnValidar = new Button();
            numLongitudMaximaPalabraRaiz = new NumericUpDown();
            lblLongitudMaxima = new Label();
            ((System.ComponentModel.ISupportInitialize)numLongitudMinimaPalabraRaiz).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLongitudMaximaPalabraRaiz).BeginInit();
            SuspendLayout();
            // 
            // panelCrucigrama
            // 
            panelCrucigrama.AutoScroll = true;
            panelCrucigrama.BackColor = SystemColors.ControlDark;
            panelCrucigrama.BorderStyle = BorderStyle.FixedSingle;
            panelCrucigrama.Location = new Point(16, 18);
            panelCrucigrama.Margin = new Padding(4, 5, 4, 5);
            panelCrucigrama.Name = "panelCrucigrama";
            panelCrucigrama.Size = new Size(666, 768);
            panelCrucigrama.TabIndex = 0;
            // 
            // btnGenerar
            // 
            btnGenerar.Location = new Point(1060, 133);
            btnGenerar.Margin = new Padding(4, 5, 4, 5);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(267, 62);
            btnGenerar.TabIndex = 1;
            btnGenerar.Text = "Generar Crucigrama";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += btnGenerar_Click;
            // 
            // numLongitudMinimaPalabraRaiz
            // 
            numLongitudMinimaPalabraRaiz.Location = new Point(1210, 16);
            numLongitudMinimaPalabraRaiz.Margin = new Padding(4, 5, 4, 5);
            numLongitudMinimaPalabraRaiz.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numLongitudMinimaPalabraRaiz.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            numLongitudMinimaPalabraRaiz.Name = "numLongitudMinimaPalabraRaiz";
            numLongitudMinimaPalabraRaiz.Size = new Size(117, 27);
            numLongitudMinimaPalabraRaiz.TabIndex = 2;
            numLongitudMinimaPalabraRaiz.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(776, 343);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(49, 20);
            label1.TabIndex = 3;
            label1.Text = "Pistas:";
            // 
            // lstPalabras
            // 
            lstPalabras.FormattingEnabled = true;
            lstPalabras.Location = new Point(833, 343);
            lstPalabras.Margin = new Padding(4, 5, 4, 5);
            lstPalabras.Name = "lstPalabras";
            lstPalabras.Size = new Size(494, 304);
            lstPalabras.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(922, 666);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(121, 20);
            label2.TabIndex = 5;
            label2.Text = "Agregar palabra:";
            // 
            // txtNuevaPalabra
            // 
            txtNuevaPalabra.CharacterCasing = CharacterCasing.Upper;
            txtNuevaPalabra.Location = new Point(1051, 666);
            txtNuevaPalabra.Margin = new Padding(4, 5, 4, 5);
            txtNuevaPalabra.Name = "txtNuevaPalabra";
            txtNuevaPalabra.Size = new Size(265, 27);
            txtNuevaPalabra.TabIndex = 6;
            // 
            // btnAgregarPalabra
            // 
            btnAgregarPalabra.Location = new Point(1049, 740);
            btnAgregarPalabra.Margin = new Padding(4, 5, 4, 5);
            btnAgregarPalabra.Name = "btnAgregarPalabra";
            btnAgregarPalabra.Size = new Size(267, 35);
            btnAgregarPalabra.TabIndex = 7;
            btnAgregarPalabra.Text = "Agregar";
            btnAgregarPalabra.UseVisualStyleBackColor = true;
            btnAgregarPalabra.Click += btnAgregarPalabra_Click;
            // 
            // lblLongitudMinima
            // 
            lblLongitudMinima.AutoSize = true;
            lblLongitudMinima.Location = new Point(1049, 16);
            lblLongitudMinima.Margin = new Padding(4, 0, 4, 0);
            lblLongitudMinima.Name = "lblLongitudMinima";
            lblLongitudMinima.Size = new Size(153, 20);
            lblLongitudMinima.TabIndex = 8;
            lblLongitudMinima.Text = "Longitud mínima raíz:";
            // 
            // txtNuevaPista
            // 
            txtNuevaPista.Location = new Point(1051, 703);
            txtNuevaPista.Margin = new Padding(4, 5, 4, 5);
            txtNuevaPista.Name = "txtNuevaPista";
            txtNuevaPista.Size = new Size(265, 27);
            txtNuevaPista.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1000, 703);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(43, 20);
            label4.TabIndex = 9;
            label4.Text = "Pista:";
            // 
            // btnValidar
            // 
            btnValidar.Location = new Point(1060, 205);
            btnValidar.Margin = new Padding(4, 5, 4, 5);
            btnValidar.Name = "btnValidar";
            btnValidar.Size = new Size(267, 62);
            btnValidar.TabIndex = 10;
            btnValidar.Text = "Validar Crucigrama";
            btnValidar.UseVisualStyleBackColor = true;
            btnValidar.Visible = false;
            btnValidar.Click += btnValidar_Click;
            // 
            // numLongitudMaximaPalabraRaiz
            // 
            numLongitudMaximaPalabraRaiz.Location = new Point(1210, 53);
            numLongitudMaximaPalabraRaiz.Margin = new Padding(4, 5, 4, 5);
            numLongitudMaximaPalabraRaiz.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            numLongitudMaximaPalabraRaiz.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            numLongitudMaximaPalabraRaiz.Name = "numLongitudMaximaPalabraRaiz";
            numLongitudMaximaPalabraRaiz.Size = new Size(117, 27);
            numLongitudMaximaPalabraRaiz.TabIndex = 11;
            numLongitudMaximaPalabraRaiz.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // lblLongitudMaxima
            // 
            lblLongitudMaxima.AutoSize = true;
            lblLongitudMaxima.Location = new Point(1046, 60);
            lblLongitudMaxima.Margin = new Padding(4, 0, 4, 0);
            lblLongitudMaxima.Name = "lblLongitudMaxima";
            lblLongitudMaxima.Size = new Size(156, 20);
            lblLongitudMaxima.TabIndex = 12;
            lblLongitudMaxima.Text = "Longitud máxima raíz:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1340, 815);
            Controls.Add(lblLongitudMaxima);
            Controls.Add(numLongitudMaximaPalabraRaiz);
            Controls.Add(btnValidar);
            Controls.Add(label4);
            Controls.Add(txtNuevaPista);
            Controls.Add(lblLongitudMinima);
            Controls.Add(btnAgregarPalabra);
            Controls.Add(txtNuevaPalabra);
            Controls.Add(label2);
            Controls.Add(lstPalabras);
            Controls.Add(label1);
            Controls.Add(numLongitudMinimaPalabraRaiz);
            Controls.Add(btnGenerar);
            Controls.Add(panelCrucigrama);
            Margin = new Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "Generador de Crucigramas";
            ((System.ComponentModel.ISupportInitialize)numLongitudMinimaPalabraRaiz).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLongitudMaximaPalabraRaiz).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelCrucigrama;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.NumericUpDown numLongitudMinimaPalabraRaiz; // Renombrado
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstPalabras;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNuevaPalabra;
        private System.Windows.Forms.Button btnAgregarPalabra;
        private System.Windows.Forms.Label lblLongitudMinima; // Nueva etiqueta
        private System.Windows.Forms.TextBox txtNuevaPista;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnValidar;
        private System.Windows.Forms.NumericUpDown numLongitudMaximaPalabraRaiz; // Nuevo control
        private System.Windows.Forms.Label lblLongitudMaxima; // Nueva etiqueta
    }
}