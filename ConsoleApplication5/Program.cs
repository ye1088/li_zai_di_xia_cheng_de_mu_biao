using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

namespace ConsoleApplication5
{
    class Program
    {
        private string iv = @"x]kR\i$s+|x{HMCgHgG~XP-H{RsDWlFs";
        private string ivLocal = @"yCkM\i$B+|#{HKJcDg3~XU-L{RsDWaGT";
        private string key = "jd)=Pdj_d@_IYjR[=MWsx#&>19Yc2:]J";
        private string keyLocal = "axLd3dlod9@_IYjR[=Msx#&>1jJh2:]L";

        public byte[] DecryptAES(byte[] to_decrypt)
        {
            return this.DecryptAES(to_decrypt, this.key, this.iv);
        }

        public byte[] DecryptAES(byte[] to_decrypt, string use_key, string use_iv)
        {
            ICryptoTransform transform = new RijndaelManaged
            {
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC,
                KeySize = 0x100,
                BlockSize = 0x100
            }.CreateDecryptor(Encoding.UTF8.GetBytes(use_key), Encoding.UTF8.GetBytes(use_iv));
            MemoryStream stream = new MemoryStream(to_decrypt);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer = new byte[to_decrypt.Length];
            stream2.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public byte[] DecryptAESLocal(byte[] to_decrypt)
        {
            return this.DecryptAES(to_decrypt, this.keyLocal, this.ivLocal);
        }

      

        public byte[] EncryptAES(byte[] encrypt_from, string use_key, string use_iv)
        {
            ICryptoTransform transform = new RijndaelManaged
            {
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC,
                KeySize = 0x100,
                BlockSize = 0x100
            }.CreateEncryptor(Encoding.UTF8.GetBytes(use_key), Encoding.UTF8.GetBytes(use_iv));
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(encrypt_from, 0, encrypt_from.Length);
            stream2.FlushFinalBlock();
            return stream.ToArray();
        }

        static void decDirs(String path) {
            DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfo[] fsi = di.GetFileSystemInfos();
            foreach (FileSystemInfo fo in fsi)
            {

                if (Directory.Exists(fo.FullName))
                {
                    decDirs(fo.FullName);
                    continue;
                }
               // Debug.WriteLine(fo.DirectoryName);
                FileStream fs = new FileStream(fo.FullName, FileMode.Open);
                
                FileStream fs2 = new FileStream(fo.FullName + "_", FileMode.Create);
                byte[] b = new byte[fs.Length];

                fs.Read(b, 0, b.Length);
                Program aes = new Program();
                byte[] buff = aes.DecryptAES(b);

                fs2.Write(buff, 0, buff.Length);
            }
            
        }

        public byte[] EncryptAES(byte[] encrypt_from)
        {
            return this.EncryptAES(encrypt_from, this.key, this.iv);
        }

 


        static void enDirs(String path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfo[] fsi = di.GetFileSystemInfos();
            foreach (FileSystemInfo fo in fsi)
            {

                if (Directory.Exists(fo.FullName))
                {
                    decDirs(fo.FullName);
                    continue;
                }
                // Debug.WriteLine(fo.DirectoryName);
                FileStream fs = new FileStream(fo.FullName, FileMode.Open);

                FileStream fs2 = new FileStream(fo.FullName + "_", FileMode.Create);
                byte[] b = new byte[fs.Length];

                fs.Read(b, 0, b.Length);
                Program aes = new Program();
                byte[] buff = aes.EncryptAES(b);

                fs2.Write(buff, 0, buff.Length);
            }

        }
    

        static void Main(string[] args)
        {
            string path = "D:\\工作目录\\lizai立在地下城的墓标\\bin2\\";
            string name = "00af10f01372742b584ca16f9f0ef47e__2_3.(1).txt";

            enDirs(path);

        }
    }
}
