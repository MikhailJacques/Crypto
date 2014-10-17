
// The following code example uses the CspParameters class to select a Smart Card Cryptographic Service Provider. 
// It then signs and verifies data using the smart card.

// http://msdn.microsoft.com/en-us/library/system.security.cryptography.cspparameters(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2

using System;
using System.Security.Cryptography;

namespace SmartCardSign
{
    class SCSign
    {
        static void Main(string[] args)
        {
            // To idendify the Smart Card CryptoGraphic Providers on your 
            // computer, use the Microsoft Registry Editor (Regedit.exe). 
            // The available Smart Card CryptoGraphic Providers are listed 
            // in HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider. 

            // Create a new CspParameters object that identifies a  
            // Smart Card CryptoGraphic Provider. 
            // The 1st parameter comes from HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider Types. 
            // The 2nd parameter comes from HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider.
            // CspParameters csp = new CspParameters(1, "Schlumberger Cryptographic Service Provider");
            CspParameters csp = new CspParameters(1, "Microsoft Strong Cryptographic Provider");

            csp.Flags = CspProviderFlags.UseDefaultKeyContainer;

            // Initialize an RSACryptoServiceProvider object using the CspParameters object.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

            // Create some data to sign. 
            byte[] data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 };

            Console.WriteLine("Data:      " + BitConverter.ToString(data));

            // Sign the data using the Smart Card CryptoGraphic Provider. 
            byte[] signature = rsa.SignData(data, "SHA1");

            Console.WriteLine("\nSignature: " + BitConverter.ToString(signature));

            // Verify the data using the Smart Card CryptoGraphic Provider. 
            bool verified = rsa.VerifyData(data, "SHA1", signature);

            Console.WriteLine("\nVerified:  " + verified);

            Console.ReadKey();
        }
    }
}