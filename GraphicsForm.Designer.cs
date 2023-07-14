namespace ECAD
{
    partial class GraphicsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pointBtn = new System.Windows.Forms.Button();
            this.lineBtn = new System.Windows.Forms.Button();
            this.popup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeBoundary = new System.Windows.Forms.ToolStripMenuItem();
            this.drawing = new System.Windows.Forms.PictureBox();
            this.popup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.drawing)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 560);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 592);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // pointBtn
            // 
            this.pointBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pointBtn.Location = new System.Drawing.Point(1099, 12);
            this.pointBtn.Name = "pointBtn";
            this.pointBtn.Size = new System.Drawing.Size(97, 39);
            this.pointBtn.TabIndex = 3;
            this.pointBtn.Text = "Point";
            this.pointBtn.UseVisualStyleBackColor = true;
            this.pointBtn.Click += new System.EventHandler(this.pointBtn_Click);
            // 
            // lineBtn
            // 
            this.lineBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lineBtn.Location = new System.Drawing.Point(1099, 57);
            this.lineBtn.Name = "lineBtn";
            this.lineBtn.Size = new System.Drawing.Size(97, 39);
            this.lineBtn.TabIndex = 4;
            this.lineBtn.Text = "Wire";
            this.lineBtn.UseVisualStyleBackColor = true;
            this.lineBtn.Click += new System.EventHandler(this.lineBtn_Click);
            // 
            // popup
            // 
            this.popup.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.popup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelToolStripMenuItem,
            this.closeBoundary});
            this.popup.Name = "popup";
            this.popup.Size = new System.Drawing.Size(123, 52);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(122, 24);
            this.cancelToolStripMenuItem.Text = "Cancel";
            // 
            // closeBoundary
            // 
            this.closeBoundary.Name = "closeBoundary";
            this.closeBoundary.Size = new System.Drawing.Size(122, 24);
            this.closeBoundary.Text = "Close";
            this.closeBoundary.Click += new System.EventHandler(this.closeBoundary_Click);
            // 
            // drawing
            // 
            this.drawing.Image = global::ECAD.Properties.Resources.istockphoto_1068663328_1024x1024;
            this.drawing.Location = new System.Drawing.Point(-1, -2);
            this.drawing.Name = "drawing";
            this.drawing.Size = new System.Drawing.Size(1025, 549);
            this.drawing.TabIndex = 0;
            this.drawing.TabStop = false;
            this.drawing.Paint += new System.Windows.Forms.PaintEventHandler(this.drawing_Paint);
            this.drawing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawing_MouseDown);
            this.drawing.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawing_MouseMove);
            // 
            // GraphicsForm
            // 
            this.ClientSize = new System.Drawing.Size(1229, 639);
            this.Controls.Add(this.lineBtn);
            this.Controls.Add(this.pointBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.drawing);
            this.Name = "GraphicsForm";
            this.popup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.drawing)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox drawing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button pointBtn;
        private System.Windows.Forms.Button lineBtn;
        private System.Windows.Forms.ContextMenuStrip popup;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeBoundary;
    }
}

