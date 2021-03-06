﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lib
{

    public static class FileIO
    {
        public static Encoding FEncoding = Encoding.Default;
        public static string StartupPath { get { return Application.StartupPath; } }
        public static void saveFile(string fileName, string path, string content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path + "\\" + fileName, false, FEncoding);
                sw.Write(content);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка записи файла: " + ex.Message);
            }
        }
        public static string[] readFiles(IEnumerable<string> filePaths)
        {
            int i = 0;
            int fCount = filePaths.Count();
            string[] files = new string[fCount];
            foreach (string file in filePaths)
            {
                try
                {
                    StreamReader sr = new StreamReader(file, FEncoding);
                    files[i++] = sr.ReadToEnd();
                    sr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения файла: " + ex.Message);
                }
            }
            return files;
        }
        // читает список файлов, разбивает каждый на строки (seperator разделитель)
        public static string[] readFiles(IEnumerable<string> filePaths, char[] separator)
        {
            string[] FilesContent = readFiles(filePaths);
            List<string> result = new List<string>();
            foreach (string str in FilesContent)
                result.AddRange(str.Split(separator, StringSplitOptions.RemoveEmptyEntries));
            return result.ToArray();
        }
        public static void SerializeBin<T>(T obj, string path)
        {
            Stream stream = File.Create(path);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            stream.Close();
        }
        public static T DeserializeBin<T>(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                T rez = (T)bf.Deserialize(fs);
                return rez;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            fs.Close();
            return default(T);
        }
        public static Exception SerializeXml<T>(T obj, string path)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(sw, obj);
                sw.Close();
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }
        public static T DeserializeXML<T>(string path)
        {
            T rez = default(T);
            StreamReader sr = null;
            try
            {
                DeserializeEx = null;
                sr = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                rez = (T)serializer.Deserialize(sr);
            }
            catch (Exception ex)
            {
                DeserializeEx = ex;
                return default(T);
            }
            finally
            {
                if (sr != null) sr.Close();
            }
            return rez;

        }
        public static Exception DeserializeEx = null;
        public static T DeserializeDataContract<T>(string path)
        {
            DeserializeEx = null;
            T rez;
            FileStream fs = null;
            XmlDictionaryReader reader = null;
            try
            {
                fs = new FileStream(path, FileMode.Open);
                reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                var serializer = new DataContractSerializer(typeof(T));
                rez = (T)serializer.ReadObject(reader);
            }
            catch (Exception ex)
            {
                DeserializeEx = ex;
                return default(T);
            }
            finally
            {
                if (reader != null) reader.Close();
                if (fs != null) fs.Close();
            }
            return rez;
        }
        public static string SerializeJson(object obj)
        {
            try
            {
                ITraceWriter traceWriter = new NLogTraceWriter();
                var sett = new JsonSerializerSettings();
                sett.TypeNameHandling = TypeNameHandling.All;
                sett.TraceWriter = traceWriter;
                var res = JsonConvert.SerializeObject(obj, sett);
                Debug.WriteLine(traceWriter);
                return res;
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public class NLogTraceWriter : ITraceWriter
        {

            public TraceLevel LevelFilter
            {
                // trace all messages. nlog can handle filtering
                get { return TraceLevel.Verbose; }
            }

            public void Trace(TraceLevel level, string message, Exception ex)
            {
                Debug.WriteLine(message);
            }

        }
        public static T DeserializeJsonFromString<T>(string jsonStr) where T : class
        {
            DeserializeEx = null;
            T rez;
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                var res = JsonConvert.DeserializeObject<T>(jsonStr);
                return res;
            }
            catch (Exception e)
            {
                DeserializeEx = e;
            }
            return default(T);
        }
        public static Exception SerializeJson<T>(T obj, string path)
        {
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, obj);
                    return null;
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public static T DeserializeJson<T>(string path)
        {
            DeserializeEx = null;
            T rez;
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using (var sr = new StreamReader(path))
                using (JsonReader jr = new JsonTextReader(sr))
                {
                    rez = serializer.Deserialize<T>(jr);
                    return rez;
                }
            }
            catch (Exception e)
            {
                DeserializeEx = e;
            }
            return default(T);
        }
        public static Exception SerializeDataContract<T>(T obj, string path)
        {
            FileStream fs;
            try
            {
                fs = new FileStream(path, FileMode.Create);
            }
            catch (Exception ex)
            {
                return ex;
            }

            XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(fs);
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(writer, obj);
            }
            catch (Exception ex) { return ex; }
            finally { writer.Close(); }
            return null;
        }
        public static string SerializeDataContract<T>(T obj)
        {
            MemoryStream ms = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(ms, obj);
            return FEncoding.GetString(ms.ToArray());
        }
        public static T DeserializeXMLFromString<T>(string str)
        {
            T rez;
            byte[] bytes = FEncoding.GetBytes(str.ToCharArray());
            MemoryStream ms = new MemoryStream(bytes);
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            try
            {
                rez = (T) serializer.ReadObject(ms);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default(T);
            }
            finally { ms.Close(); }
            return rez;
        }

        public static T Clone<T>(T obj) where T : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            var clone = bf.Deserialize(ms) as T;
            return clone;
        }
    }
}
