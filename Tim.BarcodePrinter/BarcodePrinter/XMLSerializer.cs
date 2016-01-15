using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Tim.BarcodePrinter
{

    public class XMLSerializer
    {
        /// <summary>
        /// ���л�ָ������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="obj">�����л�����</param>
        /// <returns></returns>
        public static string SerializeInfo<T>(T obj)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                //lock (m_lockSerializer)
                //{
                serializer.Serialize(mStream, obj);
                //}
                mStream.Position = 0;
                byte[] buffer = new byte[mStream.Length];
                mStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// ���л�ָ������,�����ռ�
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="obj">�����л�����</param>
        /// <returns></returns>
        public static string SerializeInfoNameSpace<T>(T obj,XmlSerializerNamespaces ns)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(mStream, obj, ns);
                mStream.Position = 0;
                byte[] buffer = new byte[mStream.Length];
                mStream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// ���л�ָ������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="obj">�����л�����</param>
        /// <returns></returns>
        public static byte[] SerializeInfoTobytes<T>(T obj)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                //lock (m_lockSerializer)
                //{
                serializer.Serialize(mStream, obj);
                //}
                mStream.Position = 0;
                byte[] buffer = new byte[mStream.Length];
                mStream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// �����л�ָ������
        /// </summary>
        /// <typeparam name="T">�����ɵ�����</typeparam>
        /// <param name="xmlString">Դxml�ַ���</param>
        /// <returns></returns>
        public static T DeserializeInfo<T>(string xmlString)
        {
            T result;
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                //lock (m_lockSerializer)
                //{
                byte[] buffer = Encoding.UTF8.GetBytes(xmlString);
                mStream.Write(buffer, 0, buffer.Length);
                mStream.Position = 0;
                result = (T)serializer.Deserialize(mStream);
                //}
                return result;

            }

        }

        /// <summary>
        /// �����л�ָ������
        /// </summary>
        /// <typeparam name="T">�����ɵ�����</typeparam>
        /// <param name="data">Դxml����</param>
        /// <returns></returns>
        public static T DeserializeInfoFrombytes<T>(byte []data)
        {
            T result;
            using (MemoryStream mStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                mStream.Write(data, 0, data.Length);
                mStream.Position = 0;
                result = (T)serializer.Deserialize(mStream);
                //}
                return result;

            }

        }


        /// <summary>
        /// ��ʵ�����л���xml�ļ���
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="obj">Ҫ���л���ʵ��</param>
        /// <param name="filePath">�ļ�·��</param>
        public static void SerializeInfo<T>(T obj, string filePath)
        {

            using (FileStream mStream = new FileStream(filePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(mStream, obj);
            }
        }

        /// <summary>
        /// ��xml�����л�Ϊʵ��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="obj">Ҫ���л���ʵ��</param>
        /// <param name="filePath">�ļ�·��</param>
        public static T DeserializeInfo<T>(T obj, string filePath)
        {

            T result;
            using (FileStream mStream = new FileStream(filePath, FileMode.Open))
            {
                StreamReader reader = new StreamReader((Stream)mStream);
                string strContent = reader.ReadToEnd();
                result = DeserializeInfo<T>(strContent);
                //XmlSerializer serializer = new XmlSerializer(obj.GetType());
                //serializer.Serialize(mStream, obj);
                //result = (T)serializer.Deserialize(mStream);
                return result;
            }
        }


    }
}
