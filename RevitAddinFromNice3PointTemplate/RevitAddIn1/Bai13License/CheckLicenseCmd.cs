using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Autodesk.Revit.Attributes;
using Microsoft.Win32;
using Nice3point.Revit.Toolkit.External;

namespace RevitAddIn1.Bai13License
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CheckLicenseCmd : ExternalCommand
    {
        string publicKey =
            "<RSAKeyValue><Modulus>zNFYXyq2mfW3uxkT6fDdzRyRlL6zfwrm+Y+ZqJ1lWrwJRYHU3IcuxYfXdAE/7C6DWbMXbSv7CJEf4M1bRAXbHIapmv28FGl3sZAtjRrR2YHhtFdndcI9Uw6xTM0zTLBP+K41UWWPUpYiXM7D4MZTEx24K/MPLJvKCB2u0boNhZA5DWbcHrbCu63TSBt79eR3R1ubFnRsXJyy8L5gvwsTHlUvxSURc5gHftNl4EQRjj39L/1slm2lfnPogab7yTIeR4HW2O295eZMxHfP34RNnOtR4g6EHRlMhOV/LJ/M7VY1rdCAE97tPM6TnWwVdvI42tnZUQQ658vm4/1bh0ZQFQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";


        public override void Execute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Set filter options and filter index
                Filter = "License Files|*.lic",
                FilterIndex = 1,
                Multiselect = false
            };


            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var licenseTextWithSignData = File.ReadAllLines(filePath);
                var licenseInfor = licenseTextWithSignData[0];
                var signData = licenseTextWithSignData[1];

                if (VerifyData(licenseInfor, signData, publicKey))
                {
                    MessageBox.Show("License Valid", "License infor", MessageBoxButton.OK, MessageBoxImage.Information);


                    if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\RevitAPIClass"))
                    {
                        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                                  "\\RevitAPIClass");
                    }

                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\RevitAPIClass\\license.lic", File.ReadAllText(filePath));

                    return;
                }

                MessageBox.Show("License is not Valid", "License infor", MessageBoxButton.OK, MessageBoxImage.Warning);

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
