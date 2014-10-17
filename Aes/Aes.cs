// Aes class represents the abstract base class from which all implementations 
// of the Advanced Encryption Standard (AES) must inherit.

// The following example demonstrates how to encrypt and decrypt sample data by using the Aes class.

// http://msdn.microsoft.com/en-us/library/system.security.cryptography.aes(v=vs.110).aspx

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Aes_Example
{
    class AesExample
    {
        public static void Main()
        {
            try
            {
                string plaintext = "A half loaf is better than no bread.";

                // Create a new instance of the Aes class.
                // This generates a new Key and an Initialization Vector (IV).
                // Creates a cryptographic object that is used to perform the symmetric algorithm.
                using (Aes myAes = Aes.Create())
                {
                    // Encrypt the string to an array of bytes. 
                    byte[] encrypted = EncryptStringToBytes_Aes(plaintext, myAes.Key, myAes.IV);

                    string cypertext = BytesToString(encrypted);

                    // Decrypt the bytes to a string. 
                    string decrypted = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                    // Display the original data and the decrypted data.
                    Console.WriteLine("Original:  {0}\n", plaintext);
                    Console.WriteLine("Encrypted: {0}\n", cypertext);
                    Console.WriteLine("Decrypted: {0}", decrypted);
                }

            }

            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }

            Console.ReadKey();
        }


        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check the validity of the arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("Plain Text");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Initialization Vector");
            
            byte[] encrypted;
            
            // Create an Aes object with the specified Key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transformation.
                // Creates a symmetric encryptor object, when overridden in a derived class, 
                // with the specified Key property and initialization vector (IV). 
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.

                // Create a stream whose backing store is memory.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Define a stream that links data streams to cryptographic transformations.
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        // Implement a TextReader that reads characters from a byte stream in a particular encoding.
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }


        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check the validity of the arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("Cipher Text");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Initialization Vector");

            byte[] decrypted;

            // Declare the string used to hold the decrypted text. 
            string plaintext = null;

            // Create an Aes object with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transformation.
                // When overridden in a derived class, creates a symmetric decryptor object with the 
                // specified Key property and initialization vector (IV).
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.

                // Create a stream whose backing store is memory.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    // Define a stream that links data streams to cryptographic transformations.
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        // Implement a TextReader that reads characters from a byte stream in a particular encoding.
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }

                        // decrypted = msDecrypt.ToArray();
                    }
                }
            }

            return plaintext;
        }

        // Convert byte array to a string.
        static string BytesToString(byte[] data)
        {
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
    }
}