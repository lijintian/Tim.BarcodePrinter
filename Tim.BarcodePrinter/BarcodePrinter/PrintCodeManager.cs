using System;
using System.Collections.Generic;
using System.Text;

namespace Tim.BarcodePrinter
{
    /// <summary>
    /// 打印条码、二维码管理类
    /// </summary>
    public class PrintCodeManager
    {
        private List<AbsCode> absCodes=new List<AbsCode> ();

        /// <summary>
        /// 加载要打印的条码、二维码
        /// </summary>
        /// <param name="absCode"></param>
        public void LoadCode(AbsCode absCode)
        {
            absCodes.Add(absCode);
        }

        /// <summary>
        /// 利用多态调用实际的条码、二维码打印方法打印
        /// </summary>
        public void PrintAbsCodes()
        {
            foreach (AbsCode absCode in absCodes)
            {
                absCode.Print();
            }
        }
    }
}
