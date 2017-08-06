using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PeekSteamIP
{
    /// <summary>
    /// 表示一个INI配置文件及对文件进行的相关操作。
    /// </summary>
    /// <remarks>
    /// INI文件具有如下结构：
    ///     [section1]
    ///     key1=value1
    ///     key2=value2
    ///     [section2]
    ///     key3=value3
    ///     ...
    /// 其中section和key的名称都不是大小写敏感的。
    /// </remarks>
    public class IniFile
    {
        private const int InitialBufferSize = 256;
        private readonly string _file;

        /// <summary>
        /// 创建INI类型文件，并返回表示该文件的<see cref="IniFile"/>实例。
        /// </summary>
        /// <param name="fileName">INI文件的完整路径和名称。</param>
        /// <exception cref="ArgumentNullException"> 当<paramref name="fileName"/>为<c>null</c>。 </exception>
        /// <exception cref="ArgumentException"> 当<paramref name="fileName"/>为空或仅包含空白字符。 </exception>
        /// <returns>被创建的<see cref="IniFile"/>实例。</returns>
        public static IniFile Create(string fileName)
        {
            File.Create(fileName);
            return new IniFile(fileName);
        }

        /// <summary>
        /// 使用指定的文件完全限定名称初始化<see cref="IniFile"/>的新实例。
        /// </summary>
        /// <exception >当所操作文件不存在时抛出此异常。</exception>
        /// <param name="fileName">包含完整路径的INI文件全名。</param>
        /// <exception cref="ArgumentNullException"> 当<paramref name="fileName"/>为<c>null</c>。 </exception>
        /// <exception cref="ArgumentException"> 当<paramref name="fileName"/>为空或仅包含空白字符。 </exception>
        /// <exception cref="FileNotFoundException">当指定的文件不存在。</exception>
        public IniFile(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not fould.", fileName);

            _file = fileName;
        }

        /// <summary>
        /// 读取指定位置的值。
        /// </summary>
        /// <exception cref="FileNotFoundException">当所操作文件不存在时抛出此异常。</exception>
        /// <exception cref="System.ArgumentNullException">当节或键为空时抛出此异常。</exception>
        /// <param name="section">节。</param>
        /// <param name="key">键。</param>
        /// <param name="size">指定读取值的最大长度。</param>
        /// <returns>读取到的值。若未读取到对应值，返回<see cref="String.Empty"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// 当<paramref name="section"/>或<paramref name="key"/>为<c>null</c>。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 当<paramref name="section"/>或<paramref name="key"/>为空或仅包含空白字符。
        /// </exception>
        /// <exception cref="ArgumentException"> 当<paramref name="size"/>不大于0。 </exception>
        public string Read(string section, string key, int size)
        {
            var sb = new StringBuilder(size, size);
            GetPrivateProfileString(section, key, null, sb, size, _file);
            return sb.ToString();
        }

        /// <summary>
        /// 读取指定位置的值。
        /// </summary>
        /// <exception cref="FileNotFoundException">当所操作文件不存在时抛出此异常。</exception>
        /// <exception cref="System.ArgumentNullException">当节或键为空时抛出此异常。</exception>
        /// <param name="section">节。</param>
        /// <param name="key">键。</param>
        /// <returns>读取到的值。若未读取到对应值，返回<see cref="String.Empty"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// 当<paramref name="section"/>或<paramref name="key"/>为<c>null</c>。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// 当<paramref name="section"/>或<paramref name="key"/>为空或仅包含空白字符。
        /// </exception>
        public string Read(string section, string key)
        {
            return InternalRead(section, key);
        }

        /// <summary>
        /// 获取所有的节的名称。
        /// </summary>
        /// <returns>包含所有节的名称的数组。</returns>
        public string[] AllSections()
        {
            return InternalReadSectionOrKeys(null, null);
        }

        /// <summary>
        /// 获取指定的节下的所有的键的名称。
        /// </summary>
        /// <returns>包含指定的节下的所有的键的名称的数组。</returns>
        public string[] AllKeys(string section)
        {
            return InternalReadSectionOrKeys(section, null);
        }

        /// <summary>
        /// 获取指定的节下的所有的键值对。
        /// </summary>
        public Dictionary<string, string> AllKeyValues(string section)
        {
            var keys = AllKeys(section);
            var res = new Dictionary<string, string>();

            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var value = Read(section, key);
                res[key] = value;
            }

            return res;
        }

        private string InternalRead(string section, string key)
        {
            for (int bufSize = InitialBufferSize; ; bufSize *= 2)
            {
                var sb = new StringBuilder(bufSize, bufSize);
                var readSize = GetPrivateProfileString(section, key, null, sb, bufSize, _file);

                // according to the document (https://msdn.microsoft.com/en-us/library/windows/desktop/ms724353%28v=vs.85%29.aspx):
                // If neither (section) or lpKeyName (key) is NULL and the supplied destination 
                // buffer is too small to hold the requested string, the string is truncated 
                // and followed by a null character, and the return value is equal to nSize (size) minus one.
                if (readSize < bufSize - 1)
                    return sb.ToString();
            }
        }

        private string[] InternalReadSectionOrKeys(string section, string key)
        {
            for (int bufSize = InitialBufferSize; ; bufSize *= 2)
            {
                var buf = new byte[bufSize];
                var readSize = GetPrivateProfileString(section, key, null, buf, bufSize, _file);

                // according to the document (https://msdn.microsoft.com/en-us/library/windows/desktop/ms724353%28v=vs.85%29.aspx):
                // If either lpAppName (section) or lpKeyName (key) is NULL and the supplied destination 
                // buffer is too small to hold all the strings, the last string is truncated 
                // and followed by two null characters. In this case, the return value is equal to nSize (size) minus two.
                if (readSize < bufSize - 2)
                {
                    var count = readSize - (readSize > 0 ? 1 : 0); // truncate the trailing \0 
                    var ret = Encoding.Default.GetString(buf, 0, count);
                    return ret.Split('\0');
                }
            }
        }

        /// <summary>
        /// 从INI文件的指定位置读取字符串值。
        /// </summary>
        /// <param name="section">节名称。</param>
        /// <param name="key">键名称。</param>
        /// <param name="def">指定未找到结果时的默认返回值。为null时返回空字符串。</param>
        /// <param name="retVal">返回值。</param>
        /// <param name="bufferSize">读取时的缓冲区大小（字节数）。</param>
        /// <param name="fileName">文件名。若文件名不包含具体路径，系统将在全盘搜索。</param>
        /// <returns>读取到的值的大小（字节数）。</returns>
        /// <remarks>由Windows2000及之后的版本支持。</remarks>
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileString(
            string section, string key, string def, StringBuilder retVal, int bufferSize, string fileName);

        /// <summary>
        /// 从INI文件的指定位置读取字符串值。
        /// </summary>
        /// <param name="section">节名称。</param>
        /// <param name="key">键名称。</param>
        /// <param name="def">指定未找到结果时的默认返回值。为null时返回空字符串。</param>
        /// <param name="retVal">返回值。</param>
        /// <param name="bufferSize">读取时的缓冲区大小（字节数）。</param>
        /// <param name="fileName">文件名。若文件名不包含具体路径，系统将在全盘搜索。</param>
        /// <returns>读取到的值的大小（字节数）。</returns>
        /// <remarks>由Windows2000及之后的版本支持。</remarks>
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileString(string section, string key, string def,
            [MarshalAs(UnmanagedType.LPArray)] byte[] retVal, int bufferSize, string fileName);

        /// <summary>
        /// 从INI文件的指定位置读取整数值。
        /// </summary>
        /// <param name="section">节名称。</param>
        /// <param name="key">键名称。</param>
        /// <param name="def">指定未找到结果时的默认返回值。</param>
        /// <param name="fileName">文件名。若文件名不包含具体路径，系统将在全盘搜索。</param>
        /// <returns>读取到的值。</returns>
        /// <remarks>由Windows2000及之后的版本支持。</remarks>
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileInt(string section, string key, int def, string fileName);

        /// <summary>
        /// 将指定的值写入INI文件的指定节中。
        /// 若键与值都为<c>null</c>将删除整个节。
        /// 若键不为<c>null</c>，值设置为<c>null</c>将删除该键。
        /// </summary>
        /// <param name="section">节名称。</param>
        /// <param name="key">键名称。</param>
        /// <param name="val">值。</param>
        /// <param name="fileName">包含完整路径的文件全名。</param>
        /// <returns>是否写入成功。</returns>
        /// <remarks>由Windows2000及之后的版本支持。</remarks>
        [DllImport("kernel32")]
        internal static extern bool WritePrivateProfileString(string section, string key, string val, string fileName);
    }
}
