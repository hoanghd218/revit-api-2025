using Autodesk.Revit.DB;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RevitAddIn1.Utils
{
    public static class LicenseUtils
    {
      static  string publicKey =
            "<RSAKeyValue><Modulus>zNFYXyq2mfW3uxkT6fDdzRyRlL6zfwrm+Y+ZqJ1lWrwJRYHU3IcuxYfXdAE/7C6DWbMXbSv7CJEf4M1bRAXbHIapmv28FGl3sZAtjRrR2YHhtFdndcI9Uw6xTM0zTLBP+K41UWWPUpYiXM7D4MZTEx24K/MPLJvKCB2u0boNhZA5DWbcHrbCu63TSBt79eR3R1ubFnRsXJyy8L5gvwsTHlUvxSURc5gHftNl4EQRjj39L/1slm2lfnPogab7yTIeR4HW2O295eZMxHfP34RNnOtR4g6EHRlMhOV/LJ/M7VY1rdCAE97tPM6TnWwVdvI42tnZUQQ658vm4/1bh0ZQFQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public static bool IsLicenseValid()
        {
           var path= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RevitAPIClass\\license.lic";

            var licenseTextWithSignData = File.ReadAllLines(path);
            var licenseInfor = licenseTextWithSignData[0];
            var signData = licenseTextWithSignData[1];

            if (LicenseUtils.VerifyData(licenseInfor, signData, publicKey))
            {
                return true;
            }


            MessageBox.Show("License is not Valid", "License infor", MessageBoxButton.OK, MessageBoxImage.Warning);

            return false;
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
