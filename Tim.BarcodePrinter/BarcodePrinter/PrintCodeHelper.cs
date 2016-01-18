using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ZXing;
using ZXing.Common;
using System.Drawing.Printing;
using System.Reflection;
using System.Xml;
using System.IO;
using ZXing.QrCode;
using System.Drawing.Imaging;

namespace Tim.BarcodePrinter
{
    public class PrintCodeHelper<T> where T : AbsCode
    {
        //todo:标准纸张支持
        //todo:打印多个是用一个PrintDoucument对象
        //todo:某些配置节点没配置时给默认值
        //todo:适当的抛异常
        //todo:可选择打印机
        private System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
        private System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
        private T codeEntity;
        private FieldInfo[] entityFields;
        private CodePrintConfig codePrintCodfig;
        private int printCount;
        public PrintCodeHelper(T entity)
        {
            codeEntity = entity;
            InitPrintInfo();
            this.printCount = entity.PrintCount;
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        public void PrintBarCode()
        {
            //InitPrintInfo();
            printDocument.Print();
            printDocument.Dispose();
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        public void PrintQRCode()
        {
            //InitPrintInfo();
            printDocument.Print();
            printDocument.Dispose();
        }

        /// <summary>
        /// 初始化打印信息
        /// </summary>
        private void InitPrintInfo()
        {
            #region 反射获取条码、二维码实体对应信息
            Type entityType = codeEntity.GetType();
            entityFields = entityType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            #endregion

            #region 将XML配置信息反序列化到实体上
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略xml注释，有注释的xml反序列化会有问题

            string xmlPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\CodePrintConfigXML\" + entityType.Name + ".xml";
            if (!string.IsNullOrEmpty(codeEntity.XmlConfigName))
            {
                xmlPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\CodePrintConfigXML\" + codeEntity.XmlConfigName + ".xml";
            }

            FileStream fs = new FileStream(xmlPath, FileMode.OpenOrCreate);

            //XmlReader reader = XmlReader.Create(xmlPath, settings);
            XmlReader reader = XmlReader.Create(fs, settings);
            xmlDoc.Load(reader);
            fs.Dispose();

            codePrintCodfig = XMLSerializer.DeserializeInfo<CodePrintConfig>(xmlDoc.InnerXml);
            #endregion

            #region 初始化printDocument
            int paperWidth = codePrintCodfig.PaperInfo.Width;
            int paperHeight = codePrintCodfig.PaperInfo.Height;
            bool landScape = codePrintCodfig.PaperInfo.LandScape;


            PaperSize pSize = new PaperSize(entityType.Name, paperWidth, paperHeight);

            printDocument.DefaultPageSettings.Landscape = landScape;
            printDocument.DefaultPageSettings.PaperSize = pSize;
            printDocument.OriginAtMargins = true;

            int offsetLeft = codePrintCodfig.PaperInfo.OffsetLeft;
            int offsetTop = codePrintCodfig.PaperInfo.OffsetTop;
            if (offsetLeft > 0)
            {
                printDocument.DefaultPageSettings.Margins.Left = offsetLeft;
                printDocument.DefaultPageSettings.Margins.Right = 0;
            }
            else
            {
                printDocument.DefaultPageSettings.Margins.Right = -offsetLeft;
                printDocument.DefaultPageSettings.Margins.Left = 0;
            }

            if (offsetTop > 0)
            {
                printDocument.DefaultPageSettings.Margins.Top = offsetTop;
                printDocument.DefaultPageSettings.Margins.Bottom = 0;
            }
            else
            {
                printDocument.DefaultPageSettings.Margins.Bottom = -offsetTop;
                printDocument.DefaultPageSettings.Margins.Top = 0;
            }
           
           
            printDocument.DefaultPageSettings.PaperSize.RawKind = 256;
            printDocument.PrintPage += new PrintPageEventHandler(PrintEvent);

            #endregion
        }

        /// <summary>
        /// PrintDocument事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PrintEvent(object sender, PrintPageEventArgs e)
        {
            #region 变量
            string strText = string.Empty;
            string elementType = string.Empty;//元素类型
            string codeType = string.Empty;//条码类型
            string strCode = string.Empty;//要生成的条码、二维码内容
            int width = 0;//元素宽度
            int height = 0;//元素高度
            int x = 0;//元素x位置
            int y = 0;//元素y位置
            string fontFamilyStr = string.Empty;//字体
            int fontSize = 0;//字体Size
            int fontStyle = 0;//参考枚举FontStyle
            string imgPath = string.Empty;//图片路径
            int linePointStartX = 0;//线条开始点x
            int linePointStartY = 0;//线条开始点y
            int linePointEndX = 0;//线条结束点x
            int linePointEndY = 0;//线条结束点y
            int lineWidth = 0;//线条宽度
            string xmlEntityFields = string.Empty;//xml中配置的字段
            Font font;

            #endregion

            #region 根据XML配置信息在打印纸上绘图

            using (Graphics gr = e.Graphics)
            {
                #region 绘制条码、二维码
                C_CodeInfo codeInfo = codePrintCodfig.PrintElements.CodeInfo;
                if (codeInfo != null)
                {
                    #region CodeType
                    codeType = codeInfo.CodeType;
                    #endregion

                    #region EntityFields
                    strCode = AnalyseXmlEntityFields(codeInfo.EntityFields.Trim());
                    if (codeInfo.IsEncrip)
                    {
                        strCode = AESHelper.Encrypt(strCode);
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(strCode))
                    {

                        #region ElementPosition
                        x = codeInfo.ElementPosition.X;
                        y = codeInfo.ElementPosition.Y;
                        #endregion

                        #region ElementStyle
                        width = codeInfo.ElementStyle.Width;
                        height = codeInfo.ElementStyle.Height;
                        #endregion

                        if (codeType.ToUpper().Trim() != "QR_CODE")
                        {
                            using (Image bitMapCode = CreateCode(strCode, width, height, GetBarcodeFormat(codeType)))
                            {
                                gr.DrawImage(bitMapCode, x, y, width, height);
                            }
                        }
                        else
                        {
                            using (Image bitMapCode = CreateQRCode(strCode, width, height))
                            {
                                gr.DrawImage(bitMapCode, x, y, width, height);
                            }
                        }
                    }
                }
                #endregion

                #region 绘制其他元素
                List<PrintElement> printElements = codePrintCodfig.PrintElements.OtherInfos;
                foreach (PrintElement pe in printElements)
                {

                    #region ElementType
                    elementType = pe.ElementType.Trim();
                    #endregion

                    #region EntityFields
                    strText = AnalyseXmlEntityFields(pe.EntityFields.Trim());
                    #endregion

                    #region ElementPosition
                    x = pe.ElementPosition.X;
                    y = pe.ElementPosition.Y;
                    #endregion

                    #region ElementStyle
                    width = pe.ElementStyle.Width;
                    height = pe.ElementStyle.Height;
                    fontFamilyStr = pe.ElementStyle.FontFamilyStr;
                    fontSize = pe.ElementStyle.FontSize;
                    fontStyle = pe.ElementStyle.FontStyle;
                    imgPath = pe.ElementStyle.ImgPath;


                    #endregion

                    switch (elementType.ToLower())
                    {
                        case "img":
                            FileStream fs = new FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\CodePrintImg\" + imgPath, FileMode.OpenOrCreate);
                            Bitmap img = new Bitmap(fs);
                            Rectangle imgRectangle = new Rectangle(x, y, width, height);
                            gr.DrawImage(img, imgRectangle);
                            fs.Dispose();
                            break;
                        case "line":
                            linePointStartX = pe.ElementPosition.X;
                            linePointStartY = pe.ElementPosition.Y;
                            linePointEndX = pe.ElementPosition.EndX;
                            linePointEndY = pe.ElementPosition.EndY;
                            lineWidth = pe.ElementStyle.LineWidth;
                            Point linePointStart = new Point(linePointStartX, linePointStartY);
                            Point linePointEnd = new Point(linePointEndX, linePointEndY);
                            Pen linePen = new Pen(Color.Black, lineWidth);
                            gr.DrawLine(linePen, linePointStart, linePointEnd);
                            linePen.Dispose();
                            break;
                        case "text":
                            strText = pe.EntityFields.Trim();
                            font = new Font(fontFamilyStr, fontSize, (FontStyle)fontStyle);
                            gr.DrawString(strText, font, new SolidBrush(Color.Black), x, y);//颜色可拓展配置其他颜色
                            break;
                        case "entityfield":
                            font = new Font(fontFamilyStr, fontSize, (FontStyle)fontStyle);
                            gr.DrawString(strText, font, new SolidBrush(Color.Black), x, y);//颜色可拓展配置其他颜色
                            break;
                        default:
                            break;
                    }

                }
                #endregion

                gr.Save();
            }
            #endregion

            #region 控制打印份数
            if (printCount > 1)
            {
                e.HasMorePages = true;
                printCount--;
            }
            #endregion

        }

        /// <summary>
        /// 分析Xml配置文件中的EntityFildes结点，以@开头视为常量，其他的取打印实体的字段值
        /// </summary>
        /// <param name="xmlEntityFields"></param>
        /// <returns></returns>
        private string AnalyseXmlEntityFields(string xmlEntityFields)
        {
            string analyseStr = string.Empty;
            string[] gXmlEntityField = xmlEntityFields.Split(',');
            foreach (string xf in gXmlEntityField)
            {
                if (xf.StartsWith("@"))
                {
                    analyseStr += xf.TrimStart('@').Trim();
                }
                else
                {
                    analyseStr += GetEntityFieldValue(xf.Trim());
                }
            }
            return analyseStr;
        }

        /// <summary>
        /// 根据字段名获取字段值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private string GetEntityFieldValue(string fieldName)
        {
            string fieldValue = string.Empty;
            Object obj = new object();
            foreach (FieldInfo field in entityFields)
            {
                if (field.Name.Trim().ToLower() == fieldName.Trim().ToLower())
                {
                    obj = field.GetValue(codeEntity);
                    if (obj != null)
                    {
                        fieldValue = obj.ToString().Trim();
                    }

                    break;
                }
            }
            return fieldValue;
        }

        /// <summary>
        /// 返回编码格式
        /// </summary>
        /// <param name="barcodeFormatStr"></param>
        /// <returns></returns>
        private BarcodeFormat GetBarcodeFormat(string barcodeFormatStr)
        {
            BarcodeFormat returnBF = BarcodeFormat.CODE_128;
            switch (barcodeFormatStr.ToUpper().Trim())
            {
                case "QR_CODE":
                    returnBF = BarcodeFormat.QR_CODE;
                    break;
                case "CODE_128":
                    returnBF = BarcodeFormat.CODE_128;
                    break;
                default:
                    returnBF = BarcodeFormat.CODE_128;
                    break;
            }
            return returnBF;
        }

        /// <summary>
        /// 产生条码图片
        /// </summary>
        /// <param name="barCode"></param>
        public Bitmap CreateCode(string codeStr, int width, int height, BarcodeFormat codeFormat)
        {
            // Writer writer = new QRCodeWriter();
            MultiFormatWriter mutiWriter = new MultiFormatWriter();
            BitMatrix bm = mutiWriter.encode(codeStr, codeFormat, width, height);
            Bitmap imageBarcode = new BarcodeWriter().Write(bm);
            imageBarcode = GetBlackRGB(imageBarcode);
            return imageBarcode;
        }

        /// <summary>
        /// 产生条码图片
        /// </summary>
        /// <param name="barCode"></param>
        public Bitmap CreateBarCode128(string barCode, int width, int height)
        {

            MultiFormatWriter mutiWriter = new MultiFormatWriter();
            BitMatrix bm = mutiWriter.encode(barCode, BarcodeFormat.CODE_128, width, height);
            Bitmap imageBarcode = new BarcodeWriter().Write(bm);
            return imageBarcode;
        }

        /// <summary>
        /// 产生二位码图片
        /// </summary>
        /// <param name="qrCode"></param>
        public Bitmap CreateQRCode(string qrCode, int width, int height)
        {
            QRCodeWriter writer = new QRCodeWriter();
            Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();
            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hints.Add(EncodeHintType.MARGIN, 0);
            BitMatrix bm = writer.encode(qrCode, BarcodeFormat.QR_CODE, width, height, hints);
            Bitmap imageRQCode = new BarcodeWriter().Write(bm);
            return imageRQCode;
        }

        /// <summary>
        /// 产生当前实体条码、二维码图片
        /// </summary>
        /// <returns></returns>
        public Bitmap GetCodeBitmap()
        {
            #region 变量
            string strText = string.Empty;
            string elementType = string.Empty;//元素类型
            string codeType = string.Empty;//条码类型
            string strCode = string.Empty;//要生成的条码、二维码内容
            int width = 0;//元素宽度
            int height = 0;//元素高度
            int x = 0;//元素x位置
            int y = 0;//元素y位置
            string fontFamilyStr = string.Empty;//字体

            string imgPath = string.Empty;//图片路径
            string xmlEntityFields = string.Empty;//xml中配置的字段

            Bitmap bitMapCode = null;

            #endregion
            try
            {
                #region 生成条码、二维码
                C_CodeInfo codeInfo = codePrintCodfig.PrintElements.CodeInfo;
                if (codeInfo != null)
                {
                    #region CodeType
                    codeType = codeInfo.CodeType;
                    #endregion

                    #region EntityFields
                    strCode = AnalyseXmlEntityFields(codeInfo.EntityFields.Trim());
                    if (codeInfo.IsEncrip)
                    {
                        strCode = AESHelper.Encrypt(strCode);
                    }
                    #endregion

                    #region ElementPosition
                    x = codeInfo.ElementPosition.X;
                    y = codeInfo.ElementPosition.Y;
                    #endregion

                    #region ElementStyle
                    width = codeInfo.ElementStyle.Width;
                    height = codeInfo.ElementStyle.Height;
                    #endregion

                    bitMapCode = new Bitmap(width, height);


                    if (codeType.ToUpper().Trim() != "QR_CODE")
                    {
                        bitMapCode = CreateCode(strCode, width, height, GetBarcodeFormat(codeType));

                    }
                    else
                    {
                        bitMapCode = CreateQRCode(strCode, width, height);

                    }

                }

                #endregion
                return bitMapCode;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                printDocument.Dispose();
                // bitMapCode.Dispose();
            }

        }

        #region 图像处理方法
        /// <summary>
        /// 获取图像有色部分
        /// </summary>
        /// <param name="srcBitmap"></param>
        /// <returns></returns>
        public static Bitmap GetBlackRGB(Bitmap srcBitmap)
        {

            int width = srcBitmap.Width;

            int height = srcBitmap.Height;

            Color pixelColor;

            int leftTopX = 0;
            int leftTopY = 0;

            int rightButtomX = width;
            int rightButtomY = height;

            bool isBreak = false;

            #region 找到左上角第一个有色像素点
            for (int i = 0; i < height - 1; i++)
            {

                for (int j = 0; j < width - 1; j++)
                {

                    pixelColor = srcBitmap.GetPixel(j, i);
                    if (pixelColor.ToArgb() != Color.White.ToArgb())
                    {
                        leftTopX = j;
                        leftTopY = i;
                        isBreak = true;
                        break;
                    }
                }

                if (isBreak)
                {
                    break;
                }
            }
            #endregion

            isBreak = false;

            #region 找到右下角最后一个有色像素点
            for (int i = height - 1; i >= 0; i--)
            {

                for (int j = width - 1; j >= 0; j--)
                {

                    pixelColor = srcBitmap.GetPixel(j, i);
                    if (pixelColor.ToArgb() != Color.White.ToArgb())
                    {
                        rightButtomX = j;
                        rightButtomY = i;
                        isBreak = true;
                        break;
                    }
                }

                if (isBreak)
                {
                    break;
                }
            }
            #endregion

            int newWidth = rightButtomX - leftTopX;
            int newHeight = rightButtomY - leftTopY;

            Bitmap dstBitmap = new Bitmap(newWidth, newHeight);

            dstBitmap = Cut(srcBitmap, leftTopX, leftTopY, newWidth, newHeight);


            return dstBitmap;

        }


        /// <summary>   
        /// 剪裁 -- 用GDI+    
        /// </summary>   
        /// <param name="b">原始Bitmap</param>   
        /// <param name="StartX">开始坐标X</param>   
        /// <param name="StartY">开始坐标Y</param>   
        /// <param name="iWidth">宽度</param>   
        /// <param name="iHeight">高度</param>   
        /// <returns>剪裁后的Bitmap</returns>   
        public static Bitmap Cut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }



        #endregion


    }



}
