

using System.Text;
using System;
using System.Security.Cryptography;
using System.IO;
namespace P1_2
{
    class Program
    {
        
private static string Encrypt(byte[] key, string secretString)
{
DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
MemoryStream ms = new MemoryStream();
CryptoStream cs = new CryptoStream(ms,
csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
StreamWriter sw = new StreamWriter(cs);
sw.Write(secretString);
sw.Flush();
cs.FlushFinalBlock();
sw.Flush();
return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
}
        static void Main(string[] args)
        {
         ///////Calculate the range
         DateTime startDate = new DateTime(2020,07,03,11,00,00);
         TimeSpan ts1 = startDate.Subtract(new DateTime(1970, 1, 1));
         int startSeed = (int)ts1.TotalMinutes;
        

   DateTime endDate = new DateTime(2020,07,04,11,00,00);
         TimeSpan ts2 = endDate.Subtract(new DateTime(1970, 1, 1));
         int endSeed = (int)ts2.TotalMinutes;

           
string secretString=args[0];
string cipherString = args[1];
for(int i=startSeed;i<=endSeed;i++)
{
Random rng = new Random(i);
byte[] key = BitConverter.GetBytes(rng.NextDouble());
string resultString = Encrypt(key, secretString);
//Console.WriteLine(result);
if(cipherString == resultString)
{
Console.WriteLine(i);
break;
}
}

        }
    }
}
