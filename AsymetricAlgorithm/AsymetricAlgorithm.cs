﻿// AsymmetricAlgorithm class represents the abstract base class from which all 
// implementations of asymmetric algorithms must inherit.

// http://msdn.microsoft.com/en-us/library/system.security.cryptography.asymmetricalgorithm(v=vs.110).aspx

using System;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

// [assembly: AssemblyKeyFile("CustomCrypto.snk")]
// [assembly: AssemblyVersion("1.0.0.0")]
[assembly: CLSCompliant(true)]
namespace Contoso
{
    // Define a CustomCrypto class that inherits from the AsymmetricAlgorithm class. 
    public class CustomCrypto : System.Security.Cryptography.AsymmetricAlgorithm
    {
        // Declare local member variables. 
        private CspParameters cspParameters;
        private readonly KeySizes[] keySizes = { new KeySizes(8, 64, 8) };

        // Initialize a CustomCrypto with the default key size of 8. 
        public CustomCrypto()
        {
            this.KeySize = 8;
        }

        // Initialize a CustomCrypto with the specified key size. 
        public CustomCrypto(int keySize)
        {
            this.KeySize = keySize;
        }

        // Accessor function for keySizes member variable. 
        public override KeySizes[] LegalKeySizes
        {
            get { return (KeySizes[])keySizes.Clone(); }
        }

        // Modify the KeySizeValue property inherited from the Asymmetric class. 
        // Prior to setting the value, ensure it falls within the range identified 
        // in the local keySizes member variable. 
        public override int KeySize
        {
            get { return KeySizeValue; }

            set
            {
                for (int i = 0; i < keySizes.Length; i++)
                {
                    if (keySizes[i].SkipSize == 0)
                    {
                        if (keySizes[i].MinSize == value)
                        {
                            KeySizeValue = value;
                            return;
                        }
                    }
                    else
                    {
                        for (int j = keySizes[i].MinSize; j <= keySizes[i].MaxSize; j += keySizes[i].SkipSize)
                        {
                            if (j == value)
                            {
                                KeySizeValue = value;
                                return;
                            }
                        }
                    }
                }

                // If the key does not fall within the range identified  
                // in the keySizes member variable, throw an exception. 
                throw new CryptographicException("Invalid key size.");
            }
        }

        // Initialize the parameters with default values. 
        public void InitializeParameters()
        {
            cspParameters = new CspParameters();
            cspParameters.ProviderName = "Contoso";
            cspParameters.KeyContainerName = "SecurityBin1";
            cspParameters.KeyNumber = 1;
            cspParameters.ProviderType = 2;
        }

        // Parse specified xmlString for values to populate the CspParams 
        // Expected XML schema: 
        //  <CustomCryptoKeyValue> 
        //      <KeyContainerName></KeyContainerName> 
        //      <KeyNumber></KeyNumber>
        //      <ProviderName></ProviderName>
        //      <ProviderType></ProviderType> 
        //  </CustomCryptoKeyValue> 
        public override void FromXmlString(string xmlString)
        {
            if (xmlString != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                // XmlNode firstNode = doc.FirstChild;
                XmlNodeList nodeList;

                // Assemble parameters from values in each XML element.
                cspParameters = new CspParameters();

                // KeyContainerName is optional.
                nodeList = doc.GetElementsByTagName("KeyContainerName");
                string keyName = nodeList.Item(0).InnerText;
                if (keyName != null)
                {
                    cspParameters.KeyContainerName = keyName;
                }

                // KeyNumber is optional.
                nodeList = doc.GetElementsByTagName("KeyNumber");
                string keyNumber = nodeList.Item(0).InnerText;
                if (keyNumber != null)
                {
                    cspParameters.KeyNumber = Int32.Parse(keyNumber);
                }

                // ProviderName is optional.
                nodeList = doc.GetElementsByTagName("ProviderName");
                string providerName = nodeList.Item(0).InnerText;
                if (providerName != null)
                {
                    cspParameters.ProviderName = providerName;
                }

                // ProviderType is optional.
                nodeList = doc.GetElementsByTagName("ProviderType");
                string providerType = nodeList.Item(0).InnerText;
                if (providerType != null)
                {
                    cspParameters.ProviderType = Int32.Parse(providerType);
                }
            }
            else
            {
                throw new ArgumentNullException("xmlString");
            }
        }

        // Create an XML string representation of the parameters in the 
        // current customCrypto object. 
        public override string ToXmlString(bool includePrivateParameters)
        {
            string keyContainerName = "";
            string keyNumber = "";
            string providerName = "";
            string providerType = "";

            if (cspParameters != null)
            {
                keyContainerName = cspParameters.KeyContainerName;
                keyNumber = cspParameters.KeyNumber.ToString();
                providerName = cspParameters.ProviderName;
                providerType = cspParameters.ProviderType.ToString();
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<CustomCryptoKeyValue>");

            sb.Append("<KeyContainerName>");
            sb.Append(keyContainerName);
            sb.Append("</KeyContainerName>");

            sb.Append("<KeyNumber>");
            sb.Append(keyNumber);
            sb.Append("</KeyNumber>");

            sb.Append("<ProviderName>");
            sb.Append(providerName);
            sb.Append("</ProviderName>");

            sb.Append("<ProviderType>");
            sb.Append(providerType);
            sb.Append("</ProviderType>");

            sb.Append("</CustomCryptoKeyValue>");
            return (sb.ToString());
        }

        // Return the name for the key exchange algorithm. 
        public override string KeyExchangeAlgorithm
        {
            get { return "RSA-PKCS1-KeyEx"; }
        }

        // Retrieves the name of the signature alogrithm. 
        public override string SignatureAlgorithm
        {
            get { return "http://www.w3.org/2000/09/xmldsig#rsa-sha1"; }
        }

        // Required member for implementing the AsymmetricAlgorithm class. 
        protected override void Dispose(bool disposing) { }

        // Call the Create method using the CustomCrypto assembly name. 
        // The create function attempts to create a CustomCrypto object using 
        // the assembly name. This functionality requires modification of the 
        // machine.config file. Add the following section to the configuration 
        // element and modify the values of the cryptoClass to reflect what is 
        // installed in your machines GAC. 
        //<mscorlib> 
        //  <cryptographySettings> 
        //    <cryptoNameMapping> 
        //      <cryptoClasses> 
        //        <cryptoClass CustomCrypto="Contoso.CustomCrypto,  
        //          CustomCrypto,  
        //          Culture=neutral,  
        //          PublicKeyToken=fdb9f9c4851028bf,  
        //          Version=1.0.1448.27640" /> 
        //      </cryptoClasses> 
        //      <nameEntry name="Contoso.CustomCrypto" class="CustomCrypto" />
        //      <nameEntry name="CustomCrypto" class="CustomCrypto" />
        //    </cryptoNameMapping> 
        //  </cryptographySettings> 
        //</mscorlib> 
        new static public CustomCrypto Create()
        {
            return Create("CustomCrypto");
        }

        // Create a CustomCrypto object by calling CrytoConfig's 
        // CreateFromName method and casting the type to CustomCrypto. 
        // The create function attempts to create a CustomCrypto object using 
        // the assembly name. This functionality requires modification of the 
        // machine.config file. Add the following section to the configuration 
        // element and modify the values of the cryptoClass to reflect what is 
        // installed in your machines GAC. 
        //<mscorlib> 
        // <cryptographySettings> 
        //   <cryptoNameMapping> 
        //     <cryptoClasses> 
        //       <cryptoClass CustomCrypto="Contoso.CustomCrypto,  
        //         CustomCrypto,  
        //         Culture=neutral,  
        //         PublicKeyToken=fdb9f9c4851028bf,  
        //         Version=1.0.1448.27640" /> 
        //     </cryptoClasses> 
        //     <nameEntry name="Contoso.CustomCrypto" class="CustomCrypto" />
        //     <nameEntry name="CustomCrypto" class="CustomCrypto" />
        //    </cryptoNameMapping> 
        //  </cryptographySettings> 
        //</mscorlib> 
        new static public CustomCrypto Create(String algorithmName)
        {
            return (CustomCrypto)CryptoConfig.CreateFromName(algorithmName);
        }
    }

    class CustomCryptoImpl
    {
        // STA = Single Thread Apartment.
        // When the STAThreadAttribute is applied, it changes the apartment state 
        // of the current thread to be single threaded. Without getting into a huge 
        // discussion about COM and threading, this attribute ensures the communication 
        // mechanism between the current thread and other threads that may want to talk to it via COM.  
        // When you're using Windows Forms, depending on the feature you're using, it may be using COM 
        // interop in order to communicate with operating system components. 
        // Good examples of this are the Clipboard and the File Dialogs. 
        // To over-simplify: WinForms programming requires the use of [STAThread] ... 
        // That, of course, does not stop you from creating and using other threads, 
        // as long as they don't "touch" the UI thread.
        [STAThread] 
        static void Main(string[] args)
        {
            // Construct a CustomCrypto object and initialize its CspParameters.
            CustomCrypto customCrypto = new CustomCrypto();
            customCrypto.InitializeParameters();

            // Display properties of the current customCrypto object.
            Console.WriteLine("*** CustomCrypto created with default " + "parameters:");
            DisplayProperties(customCrypto);

            // Release all the resources used by this instance of CustomCrytpo.
            customCrypto.Clear();

            customCrypto = new CustomCrypto(64);
            
            // Create new parameters and set them by using the FromXmlString method. 
            string parameterXml = "<CustomCryptoKeyValue>";
            parameterXml += "<KeyContainerName>SecurityBin2";
            parameterXml += "</KeyContainerName>";
            parameterXml += "<KeyNumber>1</KeyNumber>";
            parameterXml += "<ProviderName>Contoso</ProviderName>";
            parameterXml += "<ProviderType>2</ProviderType>";
            parameterXml += "</CustomCryptoKeyValue>";
            customCrypto.FromXmlString(parameterXml);

            // Display the properties of a customCrypto object created with custom parameters.
            Console.WriteLine("\n*** CustomCrypto created with custom parameters:");
            DisplayProperties(customCrypto);

            // Create an object by using the assembly name. 
            try
            {
                CustomCrypto myCryptoA = CustomCrypto.Create("CustomCrypto");
                if (myCryptoA != null)
                {
                    Console.Write("\n*** Successfully created CustomCrytpo from");
                    Console.WriteLine(" the Create method.");

                    DisplayProperties(myCryptoA);
                }
                else
                {
                    Console.Write("Unable to create CustomCrytpo from ");
                    Console.WriteLine(" the Create method.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("This sample completed successfully; press Enter to exit.");

            Console.ReadKey();
        }

        // Display the properties of the specified CustomCrypto object to the console. 
        public static void DisplayProperties(CustomCrypto customCrypto)
        {
            try
            {
                // Retrieve the class description for the customCrypto object. 
                string classDescription = customCrypto.ToString();

                Console.WriteLine(classDescription);
                Console.Write("KeyExchangeAlgorithm: ");
                Console.WriteLine(customCrypto.KeyExchangeAlgorithm);
                Console.Write("SignatureAlgorithm: ");
                Console.WriteLine(customCrypto.SignatureAlgorithm);
                Console.WriteLine("KeySize: " + customCrypto.KeySize);
                Console.WriteLine("Parameters described in Xml format:");
                Console.WriteLine(customCrypto.ToXmlString(true));

                // Display the MinSize, MaxSize, and SkipSize properties of  
                // each KeySize item in the local keySizes member variable.
                KeySizes[] legalKeySizes = customCrypto.LegalKeySizes;

                if (legalKeySizes.Length > 0)
                {
                    for (int i = 0; i < legalKeySizes.Length; i++)
                    {
                        Console.Write("Keysize" + i + " min, max, step: ");
                        Console.Write(legalKeySizes[i].MinSize + ", ");
                        Console.Write(legalKeySizes[i].MaxSize + ", ");
                        Console.WriteLine(legalKeySizes[i].SkipSize + ", ");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught unexpected exception: " + ex.ToString());
            }
        }
    }
}

// 
// This sample produces the following output: 
// 
// *** CustomCrypto created with default parameters: 
// Contoso.vbCustomCrypto 
// KeyExchangeAlgorithm: RSA-PKCS1-KeyEx 
// SignatureAlgorithm: http://www.w3.org/2000/09/xmldsig#rsa-sha1 
// KeySize: 8 
// Parameters described in Xml format: 
// <CustomCryptoKeyValue><KeyContainerName>SecurityBin1</KeyContainerName> 
// <KeyNumber>1</KeyNumber><ProviderName>Contoso</ProviderName> 
// <ProviderType>2</ProviderType></CustomCryptoKeyValue> 
// Keysize0 min, max, step: 8, 64, 8,  
//  
// *** CustomCrypto created with custom parameters: 
// Contoso.vbCustomCrypto 
// KeyExchangeAlgorithm: RSA-PKCS1-KeyEx 
// SignatureAlgorithm: http://www.w3.org/2000/09/xmldsig#rsa-sha1 
// KeySize: 64 
// Parameters described in Xml format: 
// <CustomCryptoKeyValue><KeyContainerName>SecurityBin2</KeyContainerName> 
// <KeyNumber>1</KeyNumber><ProviderName>Contoso</ProviderName> 
// <ProviderType>2</ProviderType></CustomCryptoKeyValue> 
// Keysize0 min, max, step: 8, 64, 8,  
// Unable to create CustomCrytpo from  the Create method 
// This sample completed successfully; press Exit to continue.