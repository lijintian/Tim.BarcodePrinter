using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tim.BarcodePrinter
{
    /// <summary>
    /// 条码
    /// </summary>
    public class BarcodeDemo : AbsCode
    {

        /// <summary>
        /// 字段1
        /// </summary>
        public string Field1;
        /// <summary>
        /// 字段2
        /// </summary>
        public string Field2;
        /// <summary>
        /// 字段3
        /// </summary>
        public string Field3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardType">板箱类型</param>
        /// <param name="carrier">承运人</param>
        /// <param name="serialNo">五位阿拉伯数字自编码</param>
        /// <param name="printCount">打印张数</param>
        public BarcodeDemo(string field1,string field2, string field3,int printCount)
        {
            this.CodeType = "CODE_128";
            this.Field1 = field1;
            this.Field2 = field2;
            this.Field3 = field3;
            this.PrintCount = printCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeString">完整的要生成条码的字符串</param>
        /// <param name="printCount">打印张数</param>
        public BarcodeDemo(string codeString, int printCount)
        {
            this.CodeType = "CODE_128";
            this.CodeString = codeString;
            this.PrintCount = printCount;
        }

        ~BarcodeDemo()
        {

        }

        /// <summary>
        /// 校验条码所包含信息的合法性
        /// </summary>
        public override bool IsValid(out string validMsg)
        {
            validMsg = "valid success";
            return true;
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        public override void Print()
        {
            PrintCodeHelper<BarcodeDemo> PH = new PrintCodeHelper<BarcodeDemo>(this);
            PH.PrintBarCode();
        }

        /// <summary>
        /// 获取条码、二维码图片
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetCodeBitmap()
        {
            PrintCodeHelper<BarcodeDemo> PH = new PrintCodeHelper<BarcodeDemo>(this);
            return PH.GetCodeBitmap();
        }

    }//end BarcodeDemo
}//end Tim.BarcodePrinter
