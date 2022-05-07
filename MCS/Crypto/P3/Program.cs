


using System.Runtime.InteropServices.ComTypes;
using System.Net.Http.Headers;

using System.Net.NetworkInformation;

using System.Runtime.InteropServices;
using System.Configuration.Assemblies;
using System.Reflection;
using System.Net;
using System.Globalization;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;
namespace P3
{
    class Program
    {
static BigInteger Pow(BigInteger a, BigInteger b) {
     BigInteger total = 1;
     while (b > int.MaxValue) {
        b -= int.MaxValue ;
        total = total * BigInteger.Pow(a, int.MaxValue);
     }
     total =  total * BigInteger.Pow(a, (int)b);
     return total;
  }

  static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
           
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                
                
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                          
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

           
            string plaintext = null;

           
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
              
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

static byte[] get_bytes_from_hex_string(string input)
{
    var input_split = input.Split(' ');
    byte[] inputBytes = new byte[input_split.Length];
    int i = 0;
    foreach (string item in input_split)
    {
        inputBytes.SetValue(Convert.ToByte(item, 16), i);
        i++;
    }
    return inputBytes;
}
        static void Main(string[] args)
        {
            Aes myAes = Aes.Create();
           
           string iv_string = args[0];
         byte[] iv = get_bytes_from_hex_string(iv_string);
            BigInteger G_e = BigInteger.Parse(args[1]);
            BigInteger G_c = BigInteger.Parse(args[2]);
            BigInteger N_e = BigInteger.Parse(args[3]);
            BigInteger N_c = BigInteger.Parse(args[4]);
            BigInteger x = BigInteger.Parse(args[5]);
            BigInteger gy = BigInteger.Parse(args[6]);

            BigInteger N = BigInteger.Subtract(Pow(2,N_e), N_c);
            BigInteger gxy = BigInteger.ModPow(gy,x,N);
            byte[] key = gxy.ToByteArray();
       
          string cipher_hex = args[7];
           
            byte[] C = get_bytes_from_hex_string(cipher_hex);


            string PlainText = args[8];

            string decrypted=Decrypt(C,key,iv);
          
           byte[] enc = Encrypt(PlainText,key,iv);
             string encString = BitConverter.ToString(enc).Replace("-"," ");
             Console.WriteLine(decrypted+","+encString);
             //Console.WriteLine( BitConverter.ToString(G_e));
             //Console.WriteLine( BitConverter.ToString(myAes.IV));
            
        }
    }
}
