using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;

namespace Tim.BarcodePrinter
{
    [Serializable]
    [XmlRoot("CodePrintConfig")]
    public class CodePrintConfig
    {
        [XmlElement("PaperInfo")]
        public C_PaperInfo PaperInfo;
        [XmlElement("PrintElements")]
        public C_PrintElements PrintElements;
    }

    [Serializable]
    public class C_PaperInfo
    {
        [XmlElement("Width")]
        public int Width;
        [XmlElement("Height")]
        public int Height;
        /// <summary>
        /// ��ƫ����
        /// </summary>
        [XmlElement("OffsetLeft")]
        public int OffsetLeft=0;
        /// <summary>
        /// ��ƫ����
        /// </summary>
        [XmlElement("OffsetTop")]
        public int OffsetTop=0;
        /// <summary>
        /// ֽ�ŷ��� trueΪ���� falseΪ����
        /// </summary>
        [XmlElement("LandScape")]
        public bool LandScape;
    }

    [Serializable]
    public class C_PrintElements
    {
        [XmlElement("CodeInfo")]
        public C_CodeInfo CodeInfo;

        public List<PrintElement> OtherInfos;
    }

    [Serializable]
    public class C_CodeInfo
    {
        [XmlElement("CodeType")]
        public string CodeType;
        [XmlElement("IsEncrip")]
        public bool IsEncrip;
        [XmlElement("EntityFields")]
        public string EntityFields;
        [XmlElement("ElementPosition")]
        public C_ElementPosition ElementPosition;
        [XmlElement("ElementStyle")]
        public C_ElementStyle ElementStyle;
    }

    [Serializable]
    public class PrintElement
    {
        [XmlElement("ElementType")]
        public string ElementType;
        [XmlElement("EntityFields")]
        public string EntityFields;
        [XmlElement("ElementPosition")]
        public C_ElementPosition ElementPosition;
        [XmlElement("ElementStyle")]
        public C_ElementStyle ElementStyle;
    }

    [Serializable]
    public class C_ElementPosition
    {
        [XmlElement("X")]
        public int X;
        [XmlElement("Y")]
        public int Y;
        /// <summary>
        /// �߽�����X
        /// </summary>
        [XmlElement("EndX")]
        public int EndX;
        /// <summary>
        /// �߽�����Y
        /// </summary>
        [XmlElement("EndY")]
        public int EndY;
    }

    public class C_ElementStyle
    {
        /// <summary>
        /// ���
        /// </summary>
        [XmlElement("Width")]
        public int Width;
        /// <summary>
        /// �߶�
        /// </summary>
        [XmlElement("Height")]
        public int Height;
        /// <summary>
        /// ��������  Times New Roman
        /// </summary>
        [XmlElement("FontFamilyStr")]
        public string FontFamilyStr;
        /// <summary>
        /// �����С
        /// </summary>
        [XmlElement("FontSize")]
        public int FontSize;
        /// <summary>
        /// ������ʽ
        /// </summary>
        [XmlElement("FontStyle")]
        public int FontStyle;
        /// <summary>
        /// ͼƬ·��
        /// </summary>
        [XmlElement("ImgPath")]
        public string ImgPath;
        /// <summary>
        /// �������
        /// </summary>
        [XmlElement("LineWidth")]
        public int LineWidth;
    }
}
