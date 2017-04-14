using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace OnedriveSwitch
{
    public sealed class Model
    {
        // Singleton instance.
        public static Model Instance { get; } = new Model();
        private Model(){ }
        
        public bool CurrentState { get; set; } = true;

        public void GetCurrentStatus()
        {
            switch (ReadReg(@"SOFTWARE\Policies\Microsoft\Windows\OneDrive", "DisableFileSyncNGSC"))
            {
                case 0:
                    CurrentState = true;
                    break;
                case 1:
                    CurrentState = false;
                    break;
                default:
                    CurrentState = true;
                    break;
            }
        }

        public void DoEnable()
        {
            WriteReg(@"SOFTWARE\Policies\Microsoft\Windows\OneDrive", "DisableFileSyncNGSC",0);
        }

        public void DoDisable()
        {
            WriteReg(@"SOFTWARE\Policies\Microsoft\Windows\OneDrive", "DisableFileSyncNGSC", 1);
        }


        public void WriteReg(string subKey, string keyName, int value)
        {
            var key = Registry.LocalMachine.CreateSubKey(subKey);
            key?.SetValue(keyName, value, RegistryValueKind.DWord);
            key?.Close();
        }

        public int ReadReg(string subKey, string keyName)
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(subKey);
                return (int?) key?.GetValue(keyName) ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
