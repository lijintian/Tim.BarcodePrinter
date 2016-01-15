using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tim.BarcodePrinter
{
    /// <summary>
    /// ����
    /// </summary>
    public class BarcodeDemo : AbsCode
    {

        /// <summary>
        /// �ֶ�1
        /// </summary>
        public string Field1;
        /// <summary>
        /// �ֶ�2
        /// </summary>
        public string Field2;
        /// <summary>
        /// �ֶ�3
        /// </summary>
        public string Field3;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boardType">��������</param>
        /// <param name="carrier">������</param>
        /// <param name="serialNo">��λ�����������Ա���</param>
        /// <param name="printCount">��ӡ����</param>
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
        /// <param name="codeString">������Ҫ����������ַ���</param>
        /// <param name="printCount">��ӡ����</param>
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
        /// У��������������Ϣ�ĺϷ���
        /// </summary>
        public override bool IsValid(out string validMsg)
        {
            validMsg = "valid success";
            return true;
        }

        /// <summary>
        /// ��ӡ����
        /// </summary>
        public override void Print()
        {
            PrintCodeHelper<BarcodeDemo> PH = new PrintCodeHelper<BarcodeDemo>(this);
            PH.PrintBarCode();
        }

        /// <summary>
        /// ��ȡ���롢��ά��ͼƬ
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetCodeBitmap()
        {
            PrintCodeHelper<BarcodeDemo> PH = new PrintCodeHelper<BarcodeDemo>(this);
            return PH.GetCodeBitmap();
        }

    }//end BarcodeDemo
}//end Tim.BarcodePrinter
