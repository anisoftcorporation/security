

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

namespace P4
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
       

  static BigInteger extended_euc(BigInteger A, BigInteger B)
    {
        BigInteger m = B;
        BigInteger y = 0, x = 1;
 
        if (B == 1)
            return 0;
 
        while (A > 1) {
            // q is quotient
            BigInteger q = BigInteger.Divide(A,B);
 
            BigInteger t = B;
 
            // m is remainder now, process
            // same as Euclid's algo
            B = A % B;
            A = t;
            t = y;
 
            // Update x and y
            y = x - q * y;
            x = t;
        }
 
        // Make x positive
        if (x < 0)
            x += m;
 
        return x;
    }
        static void Main(string[] args)
        {

            BigInteger Enc_Key = 65537; 
            BigInteger P_e = BigInteger.Parse(args[0]);
            BigInteger P_c = BigInteger.Parse(args[1]);
            BigInteger Q_e = BigInteger.Parse(args[2]);
            BigInteger Q_c = BigInteger.Parse(args[3]);
            BigInteger Cipher = BigInteger.Parse(args[4]);
            BigInteger PT = BigInteger.Parse(args[5]);

            BigInteger P = BigInteger.Subtract(Pow(2,P_e), P_c);
            BigInteger Q = BigInteger.Subtract(Pow(2,Q_e), Q_c);
            BigInteger N = BigInteger.Multiply(P,Q);
            BigInteger phi_N = BigInteger.Multiply(BigInteger.Subtract(P,1),BigInteger.Subtract(Q,1));
            BigInteger Dec_Key = extended_euc(Enc_Key,phi_N);
         
            BigInteger decrypted  = BigInteger.ModPow(Cipher,Dec_Key,N);
             BigInteger encrypted  = BigInteger.ModPow(PT,Enc_Key,N);
            Console.WriteLine(decrypted.ToString()+","+encrypted.ToString());
          
             //Console.WriteLine(mod.ToString());
        }
    }
}
