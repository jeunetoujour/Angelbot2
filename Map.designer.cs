namespace AngelBot
{
    partial class Map
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.render1 = new AngelBot.Render();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(257, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Disable updates";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // render1
            // 
            this.render1.Location = new System.Drawing.Point(12, 34);
            this.render1.Name = "render1";
            this.render1.Size = new System.Drawing.Size(340, 277);
            this.render1.TabIndex = 0;
            this.render1.Paint += new System.Windows.Forms.PaintEventHandler(this.render1_Paint);
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 323);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.render1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Map";
            this.ShowIcon = false;
            this.Text = "Map";
            this.ResumeLayout(false);

        }

        #endregion

        private Render render1;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}

