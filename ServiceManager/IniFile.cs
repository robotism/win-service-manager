using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ServiceManagament
{
    class IniFile
    {
        private readonly Mutex mutex = new Mutex();


        private string file;

        private readonly static string CONFIG_INIT = @"

[config]
auto-restart=8000


[service.<Name>]
path=<WorkingDir>
exec=ExeFileNameWithExt
args=
show-window=false
        
        ";

        public IniFile(string file)
        {
            this.file = Path.GetFullPath(file); ;

            if (!File.Exists(this.file))
            {
                StreamWriter streamWriter = new StreamWriter(this.file);
                streamWriter.Write(CONFIG_INIT);
                streamWriter.Flush();
                streamWriter.Close();
            }

        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringA(string section, string key, string def, Byte[] retVal, int size, string filePath);

        public List<string> getAllSections()
        {
            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringA(null, null, null, buf, buf.Length, this.file);
            int j = 0;
            for (int i = 0; i < len; i++)
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            return result;
        }
        public bool SetString(string section, string key, string val)
        {
            if (mutex.WaitOne())
            {
                long liRet = WritePrivateProfileString(section, key, val, this.file);
                return (liRet != 0);
            }
            return false;
        }
        public string GetString(string section, string key, string defaultValue)
        {
            StringBuilder sb = new StringBuilder(1024);
            if (GetPrivateProfileString(section, key, "", sb, 1024, this.file) >= 1)
            {
                return sb.ToString();
            }
            return defaultValue;
        }

        public bool SetLong(string section, string key, long val)
        {
            return SetString(section, key, "" + val);
        }

        public long GetLong(string section, string key, long defaultValue)
        {
            string l = GetString(section, key, "" + defaultValue);
            try
            {
                return int.Parse(l);
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }

    }
}
