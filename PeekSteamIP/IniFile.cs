using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PeekSteamIP
{
    /// <summary>
    /// ��ʾһ��INI�����ļ������ļ����е���ز�����
    /// </summary>
    /// <remarks>
    /// INI�ļ��������½ṹ��
    ///     [section1]
    ///     key1=value1
    ///     key2=value2
    ///     [section2]
    ///     key3=value3
    ///     ...
    /// ����section��key�����ƶ����Ǵ�Сд���еġ�
    /// </remarks>
    public class IniFile
    {
        private const int InitialBufferSize = 256;
        private readonly string _file;

        /// <summary>
        /// ����INI�����ļ��������ر�ʾ���ļ���<see cref="IniFile"/>ʵ����
        /// </summary>
        /// <param name="fileName">INI�ļ�������·�������ơ�</param>
        /// <exception cref="ArgumentNullException"> ��<paramref name="fileName"/>Ϊ<c>null</c>�� </exception>
        /// <exception cref="ArgumentException"> ��<paramref name="fileName"/>Ϊ�ջ�������հ��ַ��� </exception>
        /// <returns>��������<see cref="IniFile"/>ʵ����</returns>
        public static IniFile Create(string fileName)
        {
            File.Create(fileName);
            return new IniFile(fileName);
        }

        /// <summary>
        /// ʹ��ָ�����ļ���ȫ�޶����Ƴ�ʼ��<see cref="IniFile"/>����ʵ����
        /// </summary>
        /// <exception >���������ļ�������ʱ�׳����쳣��</exception>
        /// <param name="fileName">��������·����INI�ļ�ȫ����</param>
        /// <exception cref="ArgumentNullException"> ��<paramref name="fileName"/>Ϊ<c>null</c>�� </exception>
        /// <exception cref="ArgumentException"> ��<paramref name="fileName"/>Ϊ�ջ�������հ��ַ��� </exception>
        /// <exception cref="FileNotFoundException">��ָ�����ļ������ڡ�</exception>
        public IniFile(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File not fould.", fileName);

            _file = fileName;
        }

        /// <summary>
        /// ��ȡָ��λ�õ�ֵ��
        /// </summary>
        /// <exception cref="FileNotFoundException">���������ļ�������ʱ�׳����쳣��</exception>
        /// <exception cref="System.ArgumentNullException">���ڻ��Ϊ��ʱ�׳����쳣��</exception>
        /// <param name="section">�ڡ�</param>
        /// <param name="key">����</param>
        /// <param name="size">ָ����ȡֵ����󳤶ȡ�</param>
        /// <returns>��ȡ����ֵ����δ��ȡ����Ӧֵ������<see cref="String.Empty"/>��</returns>
        /// <exception cref="ArgumentNullException">
        /// ��<paramref name="section"/>��<paramref name="key"/>Ϊ<c>null</c>��
        /// </exception>
        /// <exception cref="ArgumentException">
        /// ��<paramref name="section"/>��<paramref name="key"/>Ϊ�ջ�������հ��ַ���
        /// </exception>
        /// <exception cref="ArgumentException"> ��<paramref name="size"/>������0�� </exception>
        public string Read(string section, string key, int size)
        {
            var sb = new StringBuilder(size, size);
            GetPrivateProfileString(section, key, null, sb, size, _file);
            return sb.ToString();
        }

        /// <summary>
        /// ��ȡָ��λ�õ�ֵ��
        /// </summary>
        /// <exception cref="FileNotFoundException">���������ļ�������ʱ�׳����쳣��</exception>
        /// <exception cref="System.ArgumentNullException">���ڻ��Ϊ��ʱ�׳����쳣��</exception>
        /// <param name="section">�ڡ�</param>
        /// <param name="key">����</param>
        /// <returns>��ȡ����ֵ����δ��ȡ����Ӧֵ������<see cref="String.Empty"/>��</returns>
        /// <exception cref="ArgumentNullException">
        /// ��<paramref name="section"/>��<paramref name="key"/>Ϊ<c>null</c>��
        /// </exception>
        /// <exception cref="ArgumentException">
        /// ��<paramref name="section"/>��<paramref name="key"/>Ϊ�ջ�������հ��ַ���
        /// </exception>
        public string Read(string section, string key)
        {
            return InternalRead(section, key);
        }

        /// <summary>
        /// ��ȡ���еĽڵ����ơ�
        /// </summary>
        /// <returns>�������нڵ����Ƶ����顣</returns>
        public string[] AllSections()
        {
            return InternalReadSectionOrKeys(null, null);
        }

        /// <summary>
        /// ��ȡָ���Ľ��µ����еļ������ơ�
        /// </summary>
        /// <returns>����ָ���Ľ��µ����еļ������Ƶ����顣</returns>
        public string[] AllKeys(string section)
        {
            return InternalReadSectionOrKeys(section, null);
        }

        /// <summary>
        /// ��ȡָ���Ľ��µ����еļ�ֵ�ԡ�
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
        /// ��INI�ļ���ָ��λ�ö�ȡ�ַ���ֵ��
        /// </summary>
        /// <param name="section">�����ơ�</param>
        /// <param name="key">�����ơ�</param>
        /// <param name="def">ָ��δ�ҵ����ʱ��Ĭ�Ϸ���ֵ��Ϊnullʱ���ؿ��ַ�����</param>
        /// <param name="retVal">����ֵ��</param>
        /// <param name="bufferSize">��ȡʱ�Ļ�������С���ֽ�������</param>
        /// <param name="fileName">�ļ��������ļ�������������·����ϵͳ����ȫ��������</param>
        /// <returns>��ȡ����ֵ�Ĵ�С���ֽ�������</returns>
        /// <remarks>��Windows2000��֮��İ汾֧�֡�</remarks>
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileString(
            string section, string key, string def, StringBuilder retVal, int bufferSize, string fileName);

        /// <summary>
        /// ��INI�ļ���ָ��λ�ö�ȡ�ַ���ֵ��
        /// </summary>
        /// <param name="section">�����ơ�</param>
        /// <param name="key">�����ơ�</param>
        /// <param name="def">ָ��δ�ҵ����ʱ��Ĭ�Ϸ���ֵ��Ϊnullʱ���ؿ��ַ�����</param>
        /// <param name="retVal">����ֵ��</param>
        /// <param name="bufferSize">��ȡʱ�Ļ�������С���ֽ�������</param>
        /// <param name="fileName">�ļ��������ļ�������������·����ϵͳ����ȫ��������</param>
        /// <returns>��ȡ����ֵ�Ĵ�С���ֽ�������</returns>
        /// <remarks>��Windows2000��֮��İ汾֧�֡�</remarks>
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileString(string section, string key, string def,
            [MarshalAs(UnmanagedType.LPArray)] byte[] retVal, int bufferSize, string fileName);

        /// <summary>
        /// ��INI�ļ���ָ��λ�ö�ȡ����ֵ��
        /// </summary>
        /// <param name="section">�����ơ�</param>
        /// <param name="key">�����ơ�</param>
        /// <param name="def">ָ��δ�ҵ����ʱ��Ĭ�Ϸ���ֵ��</param>
        /// <param name="fileName">�ļ��������ļ�������������·����ϵͳ����ȫ��������</param>
        /// <returns>��ȡ����ֵ��</returns>
        /// <remarks>��Windows2000��֮��İ汾֧�֡�</remarks>
        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileInt(string section, string key, int def, string fileName);

        /// <summary>
        /// ��ָ����ֵд��INI�ļ���ָ�����С�
        /// ������ֵ��Ϊ<c>null</c>��ɾ�������ڡ�
        /// ������Ϊ<c>null</c>��ֵ����Ϊ<c>null</c>��ɾ���ü���
        /// </summary>
        /// <param name="section">�����ơ�</param>
        /// <param name="key">�����ơ�</param>
        /// <param name="val">ֵ��</param>
        /// <param name="fileName">��������·�����ļ�ȫ����</param>
        /// <returns>�Ƿ�д��ɹ���</returns>
        /// <remarks>��Windows2000��֮��İ汾֧�֡�</remarks>
        [DllImport("kernel32")]
        internal static extern bool WritePrivateProfileString(string section, string key, string val, string fileName);
    }
}
