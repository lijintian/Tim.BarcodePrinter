using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tim.BarcodePrinter
{
    public interface IPrintCode
    {
        bool IsValid(out string validMsg);
        void Print();
        Bitmap GetCodeBitmap();
       
    }
}
