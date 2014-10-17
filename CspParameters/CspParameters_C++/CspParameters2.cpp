
// The following code example uses the CspParameters class to select a Smart Card Cryptographic Service Provider. 
// It then signs and verifies data using the smart card.

#include "stdafx.h"

using namespace System;
using namespace System::IO;
using namespace System::Security::Cryptography;

int main()
{
	// To idendify the Smart Card CryptoGraphic Providers on your 
	// computer, use the Microsoft Registry Editor (Regedit.exe). 
	// The available Smart Card CryptoGraphic Providers are listed 
	// in HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider.

	// Create a new CspParameters object that identifies a  
	// Smart Card CryptoGraphic Provider. 
	// The 1st parameter comes from HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider Types. 
	// The 2nd parameter comes from HKEY_LOCAL_MACHINE\Software\Microsoft\Cryptography\Defaults\Provider.
	CspParameters^ csp = gcnew CspParameters( 1, L"Microsoft Strong Cryptographic Provider");
	csp->Flags = CspProviderFlags::UseDefaultKeyContainer;

	// Aside: The handle declarator (^, pronounced "hat"), modifies the type specifier to mean that the
	// declared object should be automatically deleted when the system determines that the object is no longer accessible.
	// A variable that is declared with the handle declarator behaves like a pointer to the object. 
	// However, the variable points to the entire object, cannot point to a member of the object, 
	// and it does not support pointer arithmetic. Use the indirection operator (*) to access the object, 
	// and the arrow member-access operator (->) to access a member of the object.
	// http://msdn.microsoft.com/en-us/library/yk97tc08.aspx

	// In C++/CLI, a handle is a pointer to an object located on the GC heap. 
	// Creating an object on the (unmanaged) C++ heap is achieved using new and 
	// the result of a new expression is a "normal" pointer. 
	// A managed object is allocated on the GC (managed) heap with a gcnew expression. 
	// The result will be a handle. You can not do pointer arithmetic on handles. 
	// You don= not free handles. The GC will take care of them. 
	// Also, the GC is free to relocate objects on the managed heap and update the handles 
	// to point to the new locations while the program is running.

	// Aside: The purpose of the L prefix in front of a string in C++:
	// It is optional in a C++/CLI program if the string literal only contains ASCII characters. 
	// The compiler automatically converts it to a utf-16 encoded string that is interned either way.
	// L mean wchar_t and it is coded in 16 bit rather than 8 bit example:
	// "A"    = 41
	// L"A"   = 00 41

	// Initialize an RSACryptoServiceProvider object using 
	// the CspParameters object.
	RSACryptoServiceProvider^ rsa = gcnew RSACryptoServiceProvider( csp );

	// Create some data to sign. 
	array<Byte>^data = gcnew array<Byte>{0,1,2,3,4,5,6,7};

	Console::WriteLine( L"Data       : {0}", BitConverter::ToString( data ) );

	// Sign the data using the Smart Card CryptoGraphic Provider. 
	array<Byte>^sig = rsa->SignData( data, L"SHA1" );

	Console::WriteLine( L"Signature  : {0}", BitConverter::ToString( sig ) );

	// Verify the data using the Smart Card CryptoGraphic Provider. 
	bool verified = rsa->VerifyData( data, L"SHA1", sig );

	Console::WriteLine( L"Verified   : {0}", verified );

	Console::ReadKey();
}