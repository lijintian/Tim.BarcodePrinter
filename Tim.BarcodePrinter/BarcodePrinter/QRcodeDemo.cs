using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
namespace Tim.BarcodePrinter
{
	/// <summary>
	/// ��ά��
	/// </summary>
	public class QRcodeDemo : AbsCode {
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
        /// <param name="account">�˺�</param>
        /// <param name="currentCity">��ǰ����</param>
        /// <param name="department">����</param>
        /// <param name="employeeNo">Ա����</param>
        /// <param name="name">����</param>
        /// <param name="permissionStr">Ȩ���ַ���</param>
        /// <param name="printCount">��ӡ����</param>
        public QRcodeDemo(string field1, string field2, string field3, int printCount)
        {
            this.CodeType = "QR_CODE";
            this.Field1 = field1;
            this.Field2 = field2;
            this.Field3 = field3;
            this.PrintCount = printCount;
		}

        public QRcodeDemo(string codeString, int printCount)
        {
            this.CodeType = "QR_CODE";
            this.CodeString = codeString;
            this.PrintCount = printCount;
        }

		~QRcodeDemo(){

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
		public override void Print(){
            PrintCodeHelper<QRcodeDemo> PH = new PrintCodeHelper<QRcodeDemo>(this);
            PH.PrintQRCode();
		}

        /// <summary>
        /// ��ȡ���롢��ά��ͼƬ
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetCodeBitmap()
        {
            PrintCodeHelper<QRcodeDemo> PH = new PrintCodeHelper<QRcodeDemo>(this);
            return PH.GetCodeBitmap();
        }

    }//end QRcodeDemo

}//end Tim.BarcodePrinter