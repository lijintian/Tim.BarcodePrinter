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
        /// 左偏移量
        /// </summary>
        [XmlElement("OffsetLeft")]
        public int OffsetLeft=0;
        /// <summary>
        /// 上偏移量
        /// </summary>
        [XmlElement("OffsetTop")]
        public int OffsetTop=0;
        /// <summary>
        /// 纸张方向 true为横向 false为纵向
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
        /// 线结束点X
        /// </summary>
        [XmlElement("EndX")]
        public int EndX;
        /// <summary>
        /// 线结束点Y
        /// </summary>
        [XmlElement("EndY")]
        public int EndY;
    }

    public class C_ElementStyle
    {
        /// <summary>
        /// 宽度
        /// </summary>
        [XmlElement("Width")]
        public int Width;
        /// <summary>
        /// 高度
        /// </summary>
        [XmlElement("Height")]
        public int Height;
        /// <summary>
        /// 字体类型  Times New Roman
        /// </summary>
        [XmlElement("FontFamilyStr")]
        public string FontFamilyStr;
        /// <summary>
        /// 字体大小
        /// </summary>
        [XmlElement("FontSize")]
        public int FontSize;
        /// <summary>
        /// 字体样式
        /// </summary>
        [XmlElement("FontStyle")]
        public int FontStyle;
        /// <summary>
        /// 图片路径
        /// </summary>
        [XmlElement("ImgPath")]
        public string ImgPath;
        /// <summary>
        /// 线条宽度
        /// </summary>
        [XmlElement("LineWidth")]
        public int LineWidth;
    }
}
