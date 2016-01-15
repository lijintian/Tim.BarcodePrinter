namespace Demo
{
    partial class FormDemo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPrintBarcode = new System.Windows.Forms.Button();
            this.btnPrintQRcode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrintBarcode
            // 
            this.btnPrintBarcode.Location = new System.Drawing.Point(46, 30);
            this.btnPrintBarcode.Name = "btnPrintBarcode";
            this.btnPrintBarcode.Size = new System.Drawing.Size(75, 42);
            this.btnPrintBarcode.TabIndex = 0;
            this.btnPrintBarcode.Text = "Print Barcode";
            this.btnPrintBarcode.UseVisualStyleBackColor = true;
            this.btnPrintBarcode.Click += new System.EventHandler(this.btnPrintBarcode_Click);
            // 
            // btnPrintQRcode
            // 
            this.btnPrintQRcode.Location = new System.Drawing.Point(149, 30);
            this.btnPrintQRcode.Name = "btnPrintQRcode";
            this.btnPrintQRcode.Size = new System.Drawing.Size(75, 42);
            this.btnPrintQRcode.TabIndex = 0;
            this.btnPrintQRcode.Text = "Print QRcode";
            this.btnPrintQRcode.UseVisualStyleBackColor = true;
            this.btnPrintQRcode.Click += new System.EventHandler(this.btnPrintQRcode_Click);
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 105);
            this.Controls.Add(this.btnPrintQRcode);
            this.Controls.Add(this.btnPrintBarcode);
            this.Name = "FormDemo";
            this.Text = "BarcodePrintDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrintBarcode;
        private System.Windows.Forms.Button btnPrintQRcode;
    }
}

