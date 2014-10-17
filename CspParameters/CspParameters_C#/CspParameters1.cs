// CspParameters class contains parameters that are passed to the 
// cryptographic service provider (CSP) that performs cryptographic computations. 

// The CspParameters class represents parameters that you can pass to managed cryptography classes that internally 
// use Microsoft Cryptographic Service Providers (CSPs) from the unmanaged Microsoft Cryptography API (CAPI). 
// Classes with names ending in "CryptoServiceProvider" are managed code wrappers for the corresponding CSP.
//
// Use the CspParameters class to do the following:
//
// Specify a particular CSP by passing the provider type to the ProviderType or ProviderName property. 
// You can also specify a CSP using an overload of the constructor.
//
// Create a key container where you can store cryptographic keys. Key containers provide the most secure way 
// to persist cryptographic keys and keep them secret from malicious third parties. For more information about 
// creating key containers, see How to: Store Asymmetric Keys in a Key Container.
//
// Specify whether to create an asymmetric signature key or an asymmetric exchange key using the KeyNumber property.


// The following code example creates a key container using the CspParameters class and saves the key in the container.

// http://msdn.microsoft.com/en-us/library/system.security.cryptography.cspparameters(v=vs.110).aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2

using System;
using System.IO;
using System.Security.Cryptography;

public class StoreKey
{
    public static void Main()
    {
        // Create the CspParameters object
        CspParameters cp = new CspParameters();

        // Set the key container name used to store the RSA key pair
        cp.KeyContainerName = "MyKeyContainerName";

        // Instantiate the rsa instance accessing the key container MyKeyContainerName
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);

        // Uncomment the below line to delete the key entry in MyKeyContainerName 
        // rsa.PersistKeyInCsp = false; 

        // Writes out the current key pair used in the rsa instance
        Console.WriteLine("Key is : \n" + rsa.ToXmlString(true));

        Console.ReadKey();
    }
}