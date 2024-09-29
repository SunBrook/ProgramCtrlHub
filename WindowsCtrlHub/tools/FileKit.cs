/*
 * @author: S 2024/9/27 17:10:14
 */

using System;
using System.IO;
using System.Text;

namespace WindowsCtrlHub.tools
{
    /// <summary>
    /// 文件读写类
    /// </summary>
    public class FileKit
    {
        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool Exist(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文本内容</returns>
        public static string ReadText(string filePath)
        {
            string str = "";
            StreamReader sr = null;
            try
            {
                sr = new System.IO.StreamReader(filePath);
                str = sr.ReadToEnd();
            }
            catch (Exception e)
            {
                str = e.Message;
            }
            finally
            {
                if (sr != null) { sr.Close(); }
            }
            return str;
        }

        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="Text">文本内容</param>
        /// <param name="Append">是否追加，不追加则覆盖</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string Text, bool Append, Encoding encoding = null)
        {
            StreamWriter sr = null;
            try
            {
                if (encoding == null)
                {
                    sr = new StreamWriter(filePath, Append, System.Text.Encoding.UTF8);
                }
                else
                {
                    sr = new StreamWriter(filePath, Append, encoding);
                }
                sr.Write(Text);
            }
            catch
            {
            }
            finally
            {
                if (sr != null) { sr.Close(); }
            }
        }
    }
}
