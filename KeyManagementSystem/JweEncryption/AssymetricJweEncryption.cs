
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace JweEncryption;

 using System;
using Jose;
using System.Collections.Generic;
using System.Text;

 public class AssymetricJweEncryption

 {
	 public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	 public static void Main()
	 {
		 //encode  and decode using pair rsa key 
		 //extract rsa public key from p12 certificate
		 var publicKey=new X509Certificate2(Path.GetFullPath("/home/asma/certp12/keyStore.p12"), "1234").GetRSAPublicKey();
		
		 string token = Jose.JWT.Encode("hello world", publicKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
		 Console.WriteLine("token = " + token);
		 //extract rsa private key from p12 certificate
		 var privateKey=new X509Certificate2(Path.GetFullPath("/home/asma/certp12/keyStore.p12"), "1234").GetRSAPrivateKey();
		 
	    var data = Jose.JWT.Decode(token,privateKey);
		 Console.WriteLine(data);
		 
		 
		
	 }

 }