using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
namespace Tim.BarcodePrinter
{
	/// <summary>
	/// 二维码
	/// </summary>
	public class QRcodeDemo : AbsCode {
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
        /// <param name="account">账号</param>
        /// <param name="currentCity">当前城市</param>
        /// <param name="department">部门</param>
        /// <param name="employeeNo">员工号</param>
        /// <param name="name">姓名</param>
        /// <param name="permissionStr">权限字符串</param>
        /// <param name="printCount">打印张数</param>
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
		public override void Print(){
            PrintCodeHelper<QRcodeDemo> PH = new PrintCodeHelper<QRcodeDemo>(this);
            PH.PrintQRCode();
		}

        /// <summary>
        /// 获取条码、二维码图片
        /// </summary>
        /// <returns></returns>
        public override Bitmap GetCodeBitmap()
        {
            PrintCodeHelper<QRcodeDemo> PH = new PrintCodeHelper<QRcodeDemo>(this);
            return PH.GetCodeBitmap();
        }

    }//end QRcodeDemo

}//end Tim.BarcodePrinter