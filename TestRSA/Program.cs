using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestRSA
{
    internal class Program
    {


        public static void Main(string[] args)
        {
            // Generate RSA keys
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // Export public and private keys
                string publicKey =
                    "<RSAKeyValue><Modulus>zNFYXyq2mfW3uxkT6fDdzRyRlL6zfwrm+Y+ZqJ1lWrwJRYHU3IcuxYfXdAE/7C6DWbMXbSv7CJEf4M1bRAXbHIapmv28FGl3sZAtjRrR2YHhtFdndcI9Uw6xTM0zTLBP+K41UWWPUpYiXM7D4MZTEx24K/MPLJvKCB2u0boNhZA5DWbcHrbCu63TSBt79eR3R1ubFnRsXJyy8L5gvwsTHlUvxSURc5gHftNl4EQRjj39L/1slm2lfnPogab7yTIeR4HW2O295eZMxHfP34RNnOtR4g6EHRlMhOV/LJ/M7VY1rdCAE97tPM6TnWwVdvI42tnZUQQ658vm4/1bh0ZQFQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";


                string privateKey =
                    "<RSAKeyValue><Modulus>zNFYXyq2mfW3uxkT6fDdzRyRlL6zfwrm+Y+ZqJ1lWrwJRYHU3IcuxYfXdAE/7C6DWbMXbSv7CJEf4M1bRAXbHIapmv28FGl3sZAtjRrR2YHhtFdndcI9Uw6xTM0zTLBP+K41UWWPUpYiXM7D4MZTEx24K/MPLJvKCB2u0boNhZA5DWbcHrbCu63TSBt79eR3R1ubFnRsXJyy8L5gvwsTHlUvxSURc5gHftNl4EQRjj39L/1slm2lfnPogab7yTIeR4HW2O295eZMxHfP34RNnOtR4g6EHRlMhOV/LJ/M7VY1rdCAE97tPM6TnWwVdvI42tnZUQQ658vm4/1bh0ZQFQ==</Modulus><Exponent>AQAB</Exponent><P>8qdGobsnlPC/RkNoa0kGfqtpYZqaWoTXDnfKJYd8dkK8OOwVNJAKBNUsXBKXh4nlZyzkHFnU7ospW2oGpMPB81omzyD2RwEbMAb5E4ZFoTHKwh5Q9GV1XNWyjbPxmekWt3fJKg/t/mUqjBttieBZ/FG8ycnTHQGXFeFa3HfjDQs=</P><Q>2BVRSuO0VlTn08yLrybcKn5vkM4he8qnyhEOIpb3rxLz0Lb4SlKwNHL6FKsvq6Q/x05UA+CsOaG1VoSn0XN5CxWyjjb0DEV56WZRFeQ7tRaXTQhHlVn11usrdjfU6/nvWIEsVBxnLZm+GnUHWPDimLBXSPTJn/x4tY8mpHB/C18=</Q><DP>C9CXQNKzB659MbeggvDITyybcfQsJdIoAn1Uq2Uga0Wuknr0QV3uDvQuN8Fz/VZ6g/6MkNDS7FZ8SgYskqMxc17lWtp5A1YLc9gzEn9MLqAVBkShnnS+NSn2iq3DSsItp/s+IT0rUmKsoqvHNppk50M3lP+tttDYAWm6mdeAh6c=</DP><DQ>mLggV2yt9WJW4wOrKeSuML5hvZdiZOqrQg5hziKi9bLQbZAT1fuxG7CzfU4sCASzKvr6OC9fqe/XiUmCjm8tep8gLpE6VT35VvOAlZdUd716u/ABH9aQARD/C7OUIh/ogMXy/ZOBfOIvUOWrhrnhfQcM+bxAkHlRGwkMx6XWtMM=</DQ><InverseQ>wtw1x9/WJo/FF1NMBN69DoKjagXve7e6AUWy7xmAVAenroeNJADT+hnddvSrEUq06zFRqlZWEdSw3p7r7ixDiKj0XJvAjvJEYkhzP4HwePC007w0/bhV0RLdBbx+adXNK5BC4erJYQVkyj4tfnvb2pIJlc8uSOiv0ImIVrONTHU=</InverseQ><D>lJUd7dP3qdL2mZVEvAaPcTfTaLu7TWNkUhDZoNel6l/UvgbEm8K5rHSyWSZ67+SCRzVDvmAUeY+GN7fiCIPTA2uxaI2/vnMQcNCuKtoU+Bxf10s889GYWUOkXZnTKEQTj80ZQtZRVdEyVm9s7AgTxh9eNwvZqzvNIXwfEWhOXOAJqTelyPPCMI4KGfF/rw5qL0BVC8mHmTZZ9duXbBBxqYgdFMhS/CLccQ+huYYqcD4u3jbvARSbiO6dK5t8knpbMEC4/F21IKRwgULNqb8gG8SDk6ivmvwsXTEsOcwl3BZudDgYtpZZiITfJfPCWzJdDWnUdur7De/3XhywaWoX9Q==</D></RSAKeyValue>";

                var computerId = "adcadac9b06e28717bffd154549ae15f0084a57681ae511255617a9f4ab39351";
                var expired = "2025.06.02";

                string message = computerId + "_" + expired;

                // Sign the message
                string signature = SignData(message, privateKey);


                var licenseString = message + Environment.NewLine + signature;
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\license.lic",licenseString);

           
                bool isVerified = VerifyData(message, signature, publicKey);
                Console.WriteLine("Signature Verified: " + isVerified);
            }
        }

        public static string SignData(string message, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);

                var data = Encoding.UTF8.GetBytes(message);
                var signature = rsa.SignData(data, new SHA256CryptoServiceProvider());

                return Convert.ToBase64String(signature);
            }
        }

        public static bool VerifyData(string message, string signature, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);

                var data = Encoding.UTF8.GetBytes(message);
                var signatureBytes = Convert.FromBase64String(signature);

                return rsa.VerifyData(data, new SHA256CryptoServiceProvider(), signatureBytes);
            }
        }
    }
}
