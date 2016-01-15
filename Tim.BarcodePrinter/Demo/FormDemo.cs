using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tim.BarcodePrinter;

namespace Demo
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();
        }

        private void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            BarcodeDemo bc = new BarcodeDemo("0000","0001","0002", 1);
            bc.Print();
        }

        private void btnPrintQRcode_Click(object sender, EventArgs e)
        {
            QRcodeDemo qr = new QRcodeDemo("768615", "黎锦添", "商务研发部-程序编码-18022345819", 1);
            qr.Print();
        }
    }
}
