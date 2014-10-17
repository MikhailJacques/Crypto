// CryptoStreamSampleCP.cpp : main project file.
// <summary>
// This sample is designed to show how to use the composable stream CryptoStream
// to read and write encrypted data to a file. The sample uses the DES encryption
// standard to encrypt data.
// </summary>

// Youtube

// Cryptographic Key Management APIs
// https://www.youtube.com/watch?v=BX4aTOyV1Ts

// Lecture 1: Introduction to Cryptography by Christof Paar
// https://www.youtube.com/watch?v=2aHkqB2-46k


// http://msdn.microsoft.com/en-us/library/ms867086.aspx
// http://www.winsocketdotnetworkprogramming.com/managediostreamreaderwriter2g.html
// http://msdn.microsoft.com/en-us/library/windows/desktop/aa388162(v=vs.85).aspx
// http://msdn.microsoft.com/en-us/library/92f9ye3s(v=vs.110).aspx
// http://msdn.microsoft.com/en-us/library/system.security.cryptography(v=vs.110).aspx
// http://msdn.microsoft.com/en-us/library/system.security.cryptography.cryptostream(v=vs.110).aspx
 
#include "stdafx.h"
 
#include <iostream>

using namespace std;

using namespace System;
using namespace System::IO;
using namespace System::Security::Cryptography;
 
// <summary>
// The main entry point for the application.
// </summary>
[STAThread]
int main(array<System::String ^> ^args)
{
    // Let's create a private key that will be used to encrypt and decrypt the data stored in the file hantu.dat.
    array<Byte>^ DESKey = gcnew array<Byte>{200, 5, 78, 232, 9, 6, 0, 4};
    array<Byte>^ DESInitializationVector = gcnew array<Byte>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
	String^ FileName = "hantu.dat"; // "D:\\Temp\\hantu.dat";
    CryptoStream^ MyStreamEncrypter = nullptr;
 
    try
    {
        // Let's create a file named hantu.dat in the project directory
        FileStream^ MyFileStream = nullptr;
        try
        {
             MyFileStream = gcnew FileStream(FileName, FileMode::Create, FileAccess::Write);
             Console::WriteLine("{0} file created/opened successfully", FileName);
        }
        catch (Exception^ e)
        {
            throw gcnew Exception("Failed to create/open filestream with error: " + e->Message);
        }
 
        // Let's create a Symmetric crypto stream using the DES algorithm to encode all the bytes written to the file hantu.
        try
        {
            Console::WriteLine("Instantiate DESCryptoServiceProvider & CryptoStream, encrypting {0}...", FileName);
            DES^ DESAlgorithm = gcnew DESCryptoServiceProvider();
            MyStreamEncrypter = gcnew CryptoStream(MyFileStream, DESAlgorithm->CreateEncryptor(DESKey, DESInitializationVector), CryptoStreamMode::Write);
        }
        catch (Exception^ e)
        {
            MyFileStream->Close();
            throw gcnew Exception("Failed to create DES Symmetric CryptoStream with error: " + e->Message);
        }
 
        // Let's write 10 bytes to our crypto stream. 
		// For simplicity we will write an array of 10 bytes where each byte contains a numeric value 0 - 9.
        array< Byte >^ MyByteArray = gcnew array< Byte >(10);
        
         Console::WriteLine("Writing...");
         for (short i = 0; i < MyByteArray->Length; i++)
        {
            MyByteArray[i] = (Byte)i;
            Console::Write("{0}, ", MyByteArray[i]);
         }
             Console::WriteLine();
 
        try
        {
              MyStreamEncrypter->Write(MyByteArray, 0, MyByteArray->Length);
              Console::WriteLine("Writing 10 bytes to the crypto stream...");
        }
        catch (Exception^ e)
        {
            throw gcnew Exception("Write failed with error: " + e->Message);
        }
    }
    catch (Exception^ e)
    {
        Console::WriteLine(e->Message);
        return 0;
    }
    finally
    {
        // Let's close the crypto stream now that we are finished writing data.
        Console::WriteLine("Closing the encrypter stream...");
        MyStreamEncrypter->Close();
    }

    // Now let's open the encrypted file Jim.dat and decrypt the contents.
    Console::WriteLine();
    Console::WriteLine("Opening the encrypted {0} file and decrypting it...", FileName);
    CryptoStream^ MyStreamDecrypter = nullptr;
 
    try
    {
        FileStream^ MyFileStream = nullptr;
 
        try
        {
             MyFileStream = gcnew FileStream(FileName, FileMode::Open, FileAccess::Read);
             Console::WriteLine("Decrypted {0} file opened successfully", FileName);
        }
        catch (Exception^ e)
        {
            throw gcnew Exception("Failed to open filestream with error: " + e->Message);
        }
 
        try
        {
            Console::WriteLine("Instantiate DESCryptoServiceProvider & CryptoStream, decrypting {0}...", FileName);
            DES^ DESAlgorithm = gcnew DESCryptoServiceProvider();
            MyStreamDecrypter = gcnew CryptoStream(MyFileStream, DESAlgorithm->CreateDecryptor(DESKey, DESInitializationVector), CryptoStreamMode::Read);
        }
        catch (Exception^ e)
        {
            MyFileStream->Close();
            throw gcnew Exception("Failed to create DES Symmetric CryptoStream with error: " + e->Message);
        }
 
        array< Byte >^ MyReadBuffer = gcnew array< Byte >(1);
        Console::WriteLine("Reading the decrypted {0} file content...", FileName);
        while (true)
        {
            int BytesRead;
 
            try
            {
                BytesRead = MyStreamDecrypter->Read(MyReadBuffer, 0, MyReadBuffer->Length);
            }
            catch (Exception^ e)
            {
                throw gcnew Exception("Read failed with error: " + e->Message);
            }
            if (BytesRead == 0)
            {
                Console::WriteLine("No more bytes to read...");
                break;
            }
            Console::WriteLine("Read byte -> " + MyReadBuffer[0].ToString());
        }
    }
    catch (Exception^ e)
    {
              Console::WriteLine(e->Message);
    }
    finally
    {
        // We are finished performing IO on the file. We need to close the file to release operating system resources related to the file.
        Console::WriteLine("Closing the decrypter stream...");
        MyStreamDecrypter->Close();
    }

	// system("pause");
	cin.get();

    return 0;
}