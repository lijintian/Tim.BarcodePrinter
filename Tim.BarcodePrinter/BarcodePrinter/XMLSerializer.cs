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
        /// 序列化指定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">待序列化对象</param>
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
        /// 序列化指定对象,命名空间
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">待序列化对象</param>
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
        /// 序列化指定对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">待序列化对象</param>
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
        /// 反序列化指定的类
        /// </summary>
        /// <typeparam name="T">待生成的类型</typeparam>
        /// <param name="xmlString">源xml字符串</param>
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
        /// 反序列化指定的类
        /// </summary>
        /// <typeparam name="T">待生成的类型</typeparam>
        /// <param name="data">源xml数据</param>
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
        /// 将实体序列化到xml文件中
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="obj">要序列化的实体</param>
        /// <param name="filePath">文件路径</param>
        public static void SerializeInfo<T>(T obj, string filePath)
        {

            using (FileStream mStream = new FileStream(filePath, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(mStream, obj);
            }
        }

        /// <summary>
        /// 将xml反序列化为实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="obj">要序列化的实体</param>
        /// <param name="filePath">文件路径</param>
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
