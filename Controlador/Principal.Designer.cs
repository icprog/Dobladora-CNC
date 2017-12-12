namespace Controlador
{
    partial class Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.txt_info = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_conectar = new System.Windows.Forms.Button();
            this.cb_menu = new System.Windows.Forms.ComboBox();
            this.lb_proceso = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txt_dirgcode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_cargar = new System.Windows.Forms.Button();
            this.lb_gcode = new System.Windows.Forms.ListBox();
            this.lb_transformado = new System.Windows.Forms.ListBox();
            this.lb_angulos = new System.Windows.Forms.ListBox();
            this.lb_diferencia = new System.Windows.Forms.ListBox();
            this.lb_segmentos = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tx_dpp = new System.Windows.Forms.TextBox();
            this.btn_dpp = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_dist = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_menu_lectura = new System.Windows.Forms.ComboBox();
            this.btn_conectar_lectura = new System.Windows.Forms.Button();
            this.btn_iniciar_captura = new System.Windows.Forms.Button();
            this.bnt_detener_lectura = new System.Windows.Forms.Button();
            this.lbl_cant_vectores = new System.Windows.Forms.Label();
            this.lbl_cant_angulos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txt_info
            // 
            this.txt_info.Enabled = false;
            this.txt_info.Location = new System.Drawing.Point(633, 332);
            this.txt_info.Margin = new System.Windows.Forms.Padding(2);
            this.txt_info.Name = "txt_info";
            this.txt_info.Size = new System.Drawing.Size(140, 20);
            this.txt_info.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Yellow;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(634, 74);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "Procesar Codigo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "Puerto Control:";
            // 
            // btn_conectar
            // 
            this.btn_conectar.Location = new System.Drawing.Point(275, 12);
            this.btn_conectar.Margin = new System.Windows.Forms.Padding(2);
            this.btn_conectar.Name = "btn_conectar";
            this.btn_conectar.Size = new System.Drawing.Size(67, 19);
            this.btn_conectar.TabIndex = 6;
            this.btn_conectar.Text = "Conectar";
            this.btn_conectar.UseVisualStyleBackColor = true;
            this.btn_conectar.Click += new System.EventHandler(this.btn_conectar_Click);
            // 
            // cb_menu
            // 
            this.cb_menu.FormattingEnabled = true;
            this.cb_menu.Location = new System.Drawing.Point(140, 12);
            this.cb_menu.Margin = new System.Windows.Forms.Padding(2);
            this.cb_menu.Name = "cb_menu";
            this.cb_menu.Size = new System.Drawing.Size(121, 21);
            this.cb_menu.TabIndex = 7;
            this.cb_menu.SelectedIndexChanged += new System.EventHandler(this.cb_menu_SelectedIndexChanged);
            // 
            // lb_proceso
            // 
            this.lb_proceso.FormattingEnabled = true;
            this.lb_proceso.Location = new System.Drawing.Point(633, 129);
            this.lb_proceso.Margin = new System.Windows.Forms.Padding(2);
            this.lb_proceso.Name = "lb_proceso";
            this.lb_proceso.Size = new System.Drawing.Size(140, 173);
            this.lb_proceso.TabIndex = 8;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txt_dirgcode
            // 
            this.txt_dirgcode.Location = new System.Drawing.Point(126, 85);
            this.txt_dirgcode.Margin = new System.Windows.Forms.Padding(2);
            this.txt_dirgcode.Name = "txt_dirgcode";
            this.txt_dirgcode.Size = new System.Drawing.Size(174, 20);
            this.txt_dirgcode.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Archivio GCode:";
            // 
            // btn_cargar
            // 
            this.btn_cargar.Location = new System.Drawing.Point(304, 85);
            this.btn_cargar.Margin = new System.Windows.Forms.Padding(2);
            this.btn_cargar.Name = "btn_cargar";
            this.btn_cargar.Size = new System.Drawing.Size(56, 19);
            this.btn_cargar.TabIndex = 11;
            this.btn_cargar.Text = "Cargar";
            this.btn_cargar.UseVisualStyleBackColor = true;
            this.btn_cargar.Click += new System.EventHandler(this.btn_cargar_Click);
            // 
            // lb_gcode
            // 
            this.lb_gcode.FormattingEnabled = true;
            this.lb_gcode.HorizontalScrollbar = true;
            this.lb_gcode.Location = new System.Drawing.Point(7, 152);
            this.lb_gcode.Margin = new System.Windows.Forms.Padding(2);
            this.lb_gcode.Name = "lb_gcode";
            this.lb_gcode.Size = new System.Drawing.Size(154, 160);
            this.lb_gcode.TabIndex = 12;
            // 
            // lb_transformado
            // 
            this.lb_transformado.FormattingEnabled = true;
            this.lb_transformado.HorizontalScrollbar = true;
            this.lb_transformado.Location = new System.Drawing.Point(7, 507);
            this.lb_transformado.Margin = new System.Windows.Forms.Padding(2);
            this.lb_transformado.Name = "lb_transformado";
            this.lb_transformado.Size = new System.Drawing.Size(163, 121);
            this.lb_transformado.TabIndex = 13;
            this.lb_transformado.SelectedIndexChanged += new System.EventHandler(this.lb_transformado_SelectedIndexChanged);
            // 
            // lb_angulos
            // 
            this.lb_angulos.FormattingEnabled = true;
            this.lb_angulos.HorizontalScrollbar = true;
            this.lb_angulos.Location = new System.Drawing.Point(573, 507);
            this.lb_angulos.Margin = new System.Windows.Forms.Padding(2);
            this.lb_angulos.Name = "lb_angulos";
            this.lb_angulos.Size = new System.Drawing.Size(215, 121);
            this.lb_angulos.TabIndex = 14;
            // 
            // lb_diferencia
            // 
            this.lb_diferencia.FormattingEnabled = true;
            this.lb_diferencia.HorizontalScrollbar = true;
            this.lb_diferencia.Location = new System.Drawing.Point(195, 507);
            this.lb_diferencia.Name = "lb_diferencia";
            this.lb_diferencia.Size = new System.Drawing.Size(373, 121);
            this.lb_diferencia.TabIndex = 15;
            // 
            // lb_segmentos
            // 
            this.lb_segmentos.FormattingEnabled = true;
            this.lb_segmentos.Location = new System.Drawing.Point(7, 354);
            this.lb_segmentos.Margin = new System.Windows.Forms.Padding(2);
            this.lb_segmentos.Name = "lb_segmentos";
            this.lb_segmentos.Size = new System.Drawing.Size(154, 147);
            this.lb_segmentos.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 17;
            this.label3.Text = "Codigo G";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 329);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Codigo Maquina";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(631, 354);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Distancia total del trabajo";
            // 
            // tx_dpp
            // 
            this.tx_dpp.Location = new System.Drawing.Point(602, 13);
            this.tx_dpp.Margin = new System.Windows.Forms.Padding(2);
            this.tx_dpp.Name = "tx_dpp";
            this.tx_dpp.Size = new System.Drawing.Size(104, 20);
            this.tx_dpp.TabIndex = 20;
            // 
            // btn_dpp
            // 
            this.btn_dpp.Location = new System.Drawing.Point(711, 12);
            this.btn_dpp.Margin = new System.Windows.Forms.Padding(2);
            this.btn_dpp.Name = "btn_dpp";
            this.btn_dpp.Size = new System.Drawing.Size(62, 21);
            this.btn_dpp.TabIndex = 21;
            this.btn_dpp.Text = "Enviar";
            this.btn_dpp.UseVisualStyleBackColor = true;
            this.btn_dpp.Click += new System.EventHandler(this.btn_dpp_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(631, 311);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Estado:";
            // 
            // txt_dist
            // 
            this.txt_dist.Enabled = false;
            this.txt_dist.Location = new System.Drawing.Point(633, 370);
            this.txt_dist.Margin = new System.Windows.Forms.Padding(2);
            this.txt_dist.Name = "txt_dist";
            this.txt_dist.Size = new System.Drawing.Size(140, 20);
            this.txt_dist.TabIndex = 23;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(170, 148);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(443, 354);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Narrow", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(528, 14);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 17);
            this.label7.TabIndex = 25;
            this.label7.Text = "Comando:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(357, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 26;
            this.button2.Text = "recargar COM\'s";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 19);
            this.label8.TabIndex = 27;
            this.label8.Text = "Puerto Lectura:";
            // 
            // cb_menu_lectura
            // 
            this.cb_menu_lectura.FormattingEnabled = true;
            this.cb_menu_lectura.Location = new System.Drawing.Point(140, 44);
            this.cb_menu_lectura.Name = "cb_menu_lectura";
            this.cb_menu_lectura.Size = new System.Drawing.Size(121, 21);
            this.cb_menu_lectura.TabIndex = 28;
            // 
            // btn_conectar_lectura
            // 
            this.btn_conectar_lectura.Location = new System.Drawing.Point(275, 44);
            this.btn_conectar_lectura.Name = "btn_conectar_lectura";
            this.btn_conectar_lectura.Size = new System.Drawing.Size(67, 23);
            this.btn_conectar_lectura.TabIndex = 29;
            this.btn_conectar_lectura.Text = "Conectar";
            this.btn_conectar_lectura.UseVisualStyleBackColor = true;
            this.btn_conectar_lectura.Click += new System.EventHandler(this.btn_conectar_lectura_Click);
            // 
            // btn_iniciar_captura
            // 
            this.btn_iniciar_captura.Location = new System.Drawing.Point(107, 115);
            this.btn_iniciar_captura.Name = "btn_iniciar_captura";
            this.btn_iniciar_captura.Size = new System.Drawing.Size(136, 23);
            this.btn_iniciar_captura.TabIndex = 30;
            this.btn_iniciar_captura.Text = "Iniciar mediciones";
            this.btn_iniciar_captura.UseVisualStyleBackColor = true;
            this.btn_iniciar_captura.Click += new System.EventHandler(this.btn_iniciar_captura_Click);
            // 
            // bnt_detener_lectura
            // 
            this.bnt_detener_lectura.Location = new System.Drawing.Point(275, 115);
            this.bnt_detener_lectura.Name = "bnt_detener_lectura";
            this.bnt_detener_lectura.Size = new System.Drawing.Size(122, 23);
            this.bnt_detener_lectura.TabIndex = 31;
            this.bnt_detener_lectura.Text = "Detener Lectura";
            this.bnt_detener_lectura.UseVisualStyleBackColor = true;
            this.bnt_detener_lectura.Click += new System.EventHandler(this.bnt_detener_lectura_Click);
            // 
            // lbl_cant_vectores
            // 
            this.lbl_cant_vectores.AutoSize = true;
            this.lbl_cant_vectores.Location = new System.Drawing.Point(207, 638);
            this.lbl_cant_vectores.Name = "lbl_cant_vectores";
            this.lbl_cant_vectores.Size = new System.Drawing.Size(93, 13);
            this.lbl_cant_vectores.TabIndex = 32;
            this.lbl_cant_vectores.Text = "Cantidad vectores";
            // 
            // lbl_cant_angulos
            // 
            this.lbl_cant_angulos.AutoSize = true;
            this.lbl_cant_angulos.Location = new System.Drawing.Point(577, 638);
            this.lbl_cant_angulos.Name = "lbl_cant_angulos";
            this.lbl_cant_angulos.Size = new System.Drawing.Size(89, 13);
            this.lbl_cant_angulos.TabIndex = 33;
            this.lbl_cant_angulos.Text = "Cantidad angulos";
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(799, 663);
            this.Controls.Add(this.lbl_cant_angulos);
            this.Controls.Add(this.lbl_cant_vectores);
            this.Controls.Add(this.bnt_detener_lectura);
            this.Controls.Add(this.btn_iniciar_captura);
            this.Controls.Add(this.btn_conectar_lectura);
            this.Controls.Add(this.cb_menu_lectura);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txt_dist);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_dpp);
            this.Controls.Add(this.tx_dpp);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lb_segmentos);
            this.Controls.Add(this.lb_diferencia);
            this.Controls.Add(this.lb_angulos);
            this.Controls.Add(this.lb_transformado);
            this.Controls.Add(this.lb_gcode);
            this.Controls.Add(this.btn_cargar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_dirgcode);
            this.Controls.Add(this.lb_proceso);
            this.Controls.Add(this.cb_menu);
            this.Controls.Add(this.btn_conectar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_info);
            this.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Principal";
            this.Text = "Control Dobladora";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_info;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_conectar;
        private System.Windows.Forms.ComboBox cb_menu;
        private System.Windows.Forms.ListBox lb_proceso;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txt_dirgcode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_cargar;
        private System.Windows.Forms.ListBox lb_gcode;
        private System.Windows.Forms.ListBox lb_transformado;
        private System.Windows.Forms.ListBox lb_angulos;
        private System.Windows.Forms.ListBox lb_diferencia;
        private System.Windows.Forms.ListBox lb_segmentos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tx_dpp;
        private System.Windows.Forms.Button btn_dpp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_dist;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_menu_lectura;
        private System.Windows.Forms.Button btn_conectar_lectura;
        private System.Windows.Forms.Button btn_iniciar_captura;
        private System.Windows.Forms.Button bnt_detener_lectura;
        private System.Windows.Forms.Label lbl_cant_vectores;
        private System.Windows.Forms.Label lbl_cant_angulos;
    }
}

