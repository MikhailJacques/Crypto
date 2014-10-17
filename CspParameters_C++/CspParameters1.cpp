
// The following code example creates a key container using the CspParameters class and saves the key in the container.

// http://msdn.microsoft.com/en-us/library/system.security.cryptography.cspparameters(v=vs.110).aspx

#include "stdafx.h"

using namespace System;
using namespace System::IO;
using namespace System::Security::Cryptography;

int main()
{
	// Create the CspParameters object
	CspParameters^ cp = gcnew CspParameters;

	//  Set the key container name used to store the RSA key pair
	cp->KeyContainerName = "MyKeyContainerName";

	// Instantiates the rsa instance accessing the key container MyKeyContainerName
	RSACryptoServiceProvider^ rsa = gcnew RSACryptoServiceProvider( cp );

	// Uncomment the below line to delete the key entry in MyKeyContainerName 
	// rsa.PersistKeyInCsp = false;

	// Write out the current key pair used in the rsa instance
	Console::WriteLine( "Key is : \n{0}", rsa->ToXmlString( true ) );

	Console::ReadKey();
}