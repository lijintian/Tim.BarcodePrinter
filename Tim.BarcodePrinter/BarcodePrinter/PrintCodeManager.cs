using System;
using System.Collections.Generic;
using System.Text;

namespace Tim.BarcodePrinter
{
    /// <summary>
    /// ��ӡ���롢��ά�������
    /// </summary>
    public class PrintCodeManager
    {
        private List<AbsCode> absCodes=new List<AbsCode> ();

        /// <summary>
        /// ����Ҫ��ӡ�����롢��ά��
        /// </summary>
        /// <param name="absCode"></param>
        public void LoadCode(AbsCode absCode)
        {
            absCodes.Add(absCode);
        }

        /// <summary>
        /// ���ö�̬����ʵ�ʵ����롢��ά���ӡ������ӡ
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
