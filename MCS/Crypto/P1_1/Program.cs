using System;
using System.IO;
using System.Collections;

namespace P1_1
{
    class Program
    {
       
    


       
        static void Main(string[] args)
        {
            
            byte[] bmpBytes = new byte[]
            {
                0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
                0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
                0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,0x18,0x00,
                0x00,0x00,0xFF,
                0xFF,0xFF,0xFF,
                0x00,0x00,0xFF,
                0xFF,0xFF,0xFF,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00,
                0xFF,0x00,0x00,
                0xFF,0xFF,0xFF,
                0xFF,0x00,0x00,
                0xFF,0xFF,0xFF,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00,
                0xFF,0xFF,0xFF,
                0x00,0x00,0x00
            };
           
               string secretBytes = args[0];
               string[] secretArray = secretBytes.Split(' ');
                 
                  string secretBits ="";
              foreach (var inputHex in secretArray)
              {
                  secretBits +=Convert.ToString(Convert.ToInt32(inputHex, 16), 2).PadLeft(8, '0');
              } 
              
               int startBit=26;
               for(int i=0;i<secretBits.Length;i=i+2)
               {
                   string toBeXored  = "";
                   toBeXored+=secretBits[i];
                   toBeXored+=secretBits[i+1];
                   string result = (bmpBytes[startBit]^Convert.ToByte(toBeXored,2)).ToString("X2");
                   bmpBytes[startBit] = Convert.ToByte(result,16);
                   startBit=startBit+1;
                  
               }
               Console.WriteLine(BitConverter.ToString(bmpBytes).Replace("-", " "));
           
        }

       
       
    }
}