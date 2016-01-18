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
        //todo:��׼ֽ��֧��
        //todo:��ӡ�������һ��PrintDoucument����
        //todo:ĳЩ���ýڵ�û����ʱ��Ĭ��ֵ
        //todo:�ʵ������쳣
        //todo:��ѡ���ӡ��
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
        /// ��ӡ����
        /// </summary>
        public void PrintBarCode()
        {
            //InitPrintInfo();
            printDocument.Print();
            printDocument.Dispose();
        }

        /// <summary>
        /// ��ӡ��ά��
        /// </summary>
        public void PrintQRCode()
        {
            //InitPrintInfo();
            printDocument.Print();
            printDocument.Dispose();
        }

        /// <summary>
        /// ��ʼ����ӡ��Ϣ
        /// </summary>
        private void InitPrintInfo()
        {
            #region �����ȡ���롢��ά��ʵ���Ӧ��Ϣ
            Type entityType = codeEntity.GetType();
            entityFields = entityType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            #endregion

            #region ��XML������Ϣ�����л���ʵ����
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//����xmlע�ͣ���ע�͵�xml�����л���������

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

            #region ��ʼ��printDocument
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
        /// PrintDocument�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PrintEvent(object sender, PrintPageEventArgs e)
        {
            #region ����
            string strText = string.Empty;
            string elementType = string.Empty;//Ԫ������
            string codeType = string.Empty;//��������
            string strCode = string.Empty;//Ҫ���ɵ����롢��ά������
            int width = 0;//Ԫ�ؿ��
            int height = 0;//Ԫ�ظ߶�
            int x = 0;//Ԫ��xλ��
            int y = 0;//Ԫ��yλ��
            string fontFamilyStr = string.Empty;//����
            int fontSize = 0;//����Size
            int fontStyle = 0;//�ο�ö��FontStyle
            string imgPath = string.Empty;//ͼƬ·��
            int linePointStartX = 0;//������ʼ��x
            int linePointStartY = 0;//������ʼ��y
            int linePointEndX = 0;//����������x
            int linePointEndY = 0;//����������y
            int lineWidth = 0;//�������
            string xmlEntityFields = string.Empty;//xml�����õ��ֶ�
            Font font;

            #endregion

            #region ����XML������Ϣ�ڴ�ӡֽ�ϻ�ͼ

            using (Graphics gr = e.Graphics)
            {
                #region �������롢��ά��
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

                #region ��������Ԫ��
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
                            gr.DrawString(strText, font, new SolidBrush(Color.Black), x, y);//��ɫ����չ����������ɫ
                            break;
                        case "entityfield":
                            font = new Font(fontFamilyStr, fontSize, (FontStyle)fontStyle);
                            gr.DrawString(strText, font, new SolidBrush(Color.Black), x, y);//��ɫ����չ����������ɫ
                            break;
                        default:
                            break;
                    }

                }
                #endregion

                gr.Save();
            }
            #endregion

            #region ���ƴ�ӡ����
            if (printCount > 1)
            {
                e.HasMorePages = true;
                printCount--;
            }
            #endregion

        }

        /// <summary>
        /// ����Xml�����ļ��е�EntityFildes��㣬��@��ͷ��Ϊ������������ȡ��ӡʵ����ֶ�ֵ
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
        /// �����ֶ�����ȡ�ֶ�ֵ
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
        /// ���ر����ʽ
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
        /// ��������ͼƬ
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
        /// ��������ͼƬ
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
        /// ������λ��ͼƬ
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
        /// ������ǰʵ�����롢��ά��ͼƬ
        /// </summary>
        /// <returns></returns>
        public Bitmap GetCodeBitmap()
        {
            #region ����
            string strText = string.Empty;
            string elementType = string.Empty;//Ԫ������
            string codeType = string.Empty;//��������
            string strCode = string.Empty;//Ҫ���ɵ����롢��ά������
            int width = 0;//Ԫ�ؿ��
            int height = 0;//Ԫ�ظ߶�
            int x = 0;//Ԫ��xλ��
            int y = 0;//Ԫ��yλ��
            string fontFamilyStr = string.Empty;//����

            string imgPath = string.Empty;//ͼƬ·��
            string xmlEntityFields = string.Empty;//xml�����õ��ֶ�

            Bitmap bitMapCode = null;

            #endregion
            try
            {
                #region �������롢��ά��
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

        #region ͼ������
        /// <summary>
        /// ��ȡͼ����ɫ����
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

            #region �ҵ����Ͻǵ�һ����ɫ���ص�
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

            #region �ҵ����½����һ����ɫ���ص�
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
        /// ���� -- ��GDI+    
        /// </summary>   
        /// <param name="b">ԭʼBitmap</param>   
        /// <param name="StartX">��ʼ����X</param>   
        /// <param name="StartY">��ʼ����Y</param>   
        /// <param name="iWidth">���</param>   
        /// <param name="iHeight">�߶�</param>   
        /// <returns>���ú��Bitmap</returns>   
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
