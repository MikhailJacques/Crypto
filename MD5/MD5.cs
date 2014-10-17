// Namespace:  System.Security.Cryptography
// Assembly:  mscorlib (in mscorlib.dll)

// MD5 Class represents the abstract class from which all implementations of the MD5 hash algorithm inherit.

// The following code example computes the MD5 hash value of a string and returns the hash as a 32-character, 
// hexadecimal-formatted string. The hash string created by this code example is compatible with any MD5 hash 
// function (on any platform) that creates a 32-character, hexadecimal-formatted hash string.

// The hash size for the MD5 algorithm is 128 bits.

// The ComputeHash methods of the MD5 class return the hash as an array of 16 bytes. 
// Note that some MD5 implementations produce a 32-character, hexadecimal-formatted hash. 
// To interoperate with such implementations, format the return value of the ComputeHash methods as a hexadecimal value.

// This code example produces the following output: 
// 
// The MD5 hash of 'Hello World! I love programming!' is:
//         42b8eb458ccf2cfe2df1be0f2537f15d.
// Verifying the hash...
// The hashes are the same.

// http://msdn.microsoft.com/en-us/library/system.security.cryptography(v=vs.110).aspx
// http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5(v=vs.110).aspx

using System;
using System.Security.Cryptography;
using System.Text;

namespace MD5Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = "Hello World! I love programming!";

            // Create an instance of the default implementation of the MD5 hash algorithm.
            //
            // Note: The reason for using the "using" statement is to ensure that the object is always
            // disposed of correctly, and it does not require explicit code to ensure that this happens.
            // It provides a convenient syntax that ensures the correct use of IDisposable objects.
            // It is essentially a shorthand for a try/finally block.
            // The using statement is only useful for objects with a lifetime that does not extend beyond 
            // the method in which the objects are constructed. 
            // Remember that the objects you instantiate must implement the System.IDisposable interface.
            // You should implement IDisposable only if your type uses unmanaged resources directly.
            using (MD5 md5Hash = MD5.Create())
            {
                string hashOfSource = GetMd5Hash(md5Hash, source);

                Console.WriteLine("The MD5 hash of '" + source + "' is: \n\t" + hashOfSource + ".");

                Console.WriteLine("Verifying the hash...");

                if (VerifyMd5Hash(md5Hash, source, hashOfSource))
                {
                    Console.WriteLine("The hashes are the same.");
                }
                else
                {
                    Console.WriteLine("The hashes are not same.");
                }
            }

            Console.ReadKey();

        }

        // Compute the hash value for the specified input string and return as a hexadecimal string.
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                // sBuilder.Append(data[i].ToString());
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }


        // Verify a hash against a string. 
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hashOfSource)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (comparer.Compare(hashOfInput, hashOfSource) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}