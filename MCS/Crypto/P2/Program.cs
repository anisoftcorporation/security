

using System.Net.Http.Headers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.AccessControl;
using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Security.Cryptography;

namespace P2
{
    class Program
    {
        static string GetSaltedHash(string input,byte salt)
        {
         byte[] inputBytes = Encoding.UTF8.GetBytes(input); 
        Array.Resize(ref inputBytes, inputBytes.Length + 1);
      inputBytes[inputBytes.Length-1] = salt;
      string saltedHash = GetHashed(inputBytes);
      return saltedHash;

        }
        static string GetHashed(byte[] input)
        {
          MD5 md5 = MD5.Create();
           byte[] hashedBytes = md5.ComputeHash(input);
           byte[] result = new byte[5];
           Array.Copy(hashedBytes,0,result,0,5);
           string hashedValue = BitConverter.ToString(result).Replace("-", " ");
           return hashedValue;
        }
        
        static void Main(string[] args)
        {
            string allowed_characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            byte salt=(byte)Convert.ToInt32(args[0], 16);
           // byte[] salt = Encoding.UTF8.GetBytes(args[0]);
           Dictionary<string,string> hashDic = new Dictionary<string, string>();
     /*for(int i=0;i<62;i++)
     {
         for(int j=0;j<62;j++)
         {
            string password = ""+allowed_characters[i]+allowed_characters[j];
            string hashedPassword = GetSaltedHash(password,salt);
            hashDic.add(hashedPassword,password);

         }
     }*/


           
            while(true)
            {
                 string password1 ="";
           // string password2="";
            var rand = new Random();
            for(int i=0;i<10;i++)
            {
                password1=password1+allowed_characters[rand.Next(0, 62)];
           
            }
           string saltedHashPass1 = GetSaltedHash(password1,salt);
           try{
           hashDic.Add(saltedHashPass1,password1);
           }
           catch (ArgumentException)
           {
               string inDict = hashDic[saltedHashPass1];
               if(inDict != password1)
               {
                   Console.WriteLine(inDict+","+password1);
                   break;
               }
           }
      

      
          
         

            }
      
        }
    }
}
