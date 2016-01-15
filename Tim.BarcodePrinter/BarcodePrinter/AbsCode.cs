using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tim.BarcodePrinter
{
    public abstract class AbsCode:IPrintCode
    {
        public string CodeString;
        public string CodeType;
        public int PrintCount;
        public string XmlConfigName;
        public abstract bool IsValid(out string validMsg);
        public abstract void Print();
        public abstract Bitmap GetCodeBitmap();
    }
}
