using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;

namespace RevitAddIn1.Bai13License
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ComputerIdCmd : ExternalCommand
    {
        public override void Execute()
        {
            string cpuId = GetCpuId();
            string biosSerialNumber = GetBiosSerialNumber();

            string uniqueId = GenerateUniqueId(cpuId + biosSerialNumber);

            var view = new ComputerIdView();
            view.TbComputerId.Text = uniqueId;
            view.ShowDialog();

        }

        static string GetCpuId()
        {
            string cpuId = string.Empty;
            try
            {
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    cpuId = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving CPU ID: " + ex.Message);
            }

            return cpuId;
        }

        static string GetBiosSerialNumber()
        {
            string biosSerialNumber = string.Empty;
            try
            {
                ManagementClass mc = new ManagementClass("Win32_BIOS");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    biosSerialNumber = mo.Properties["SerialNumber"].Value.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving BIOS serial number: " + ex.Message);
            }

            return biosSerialNumber;
        }

        static string GenerateUniqueId(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
