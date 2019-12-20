using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Festispec.UI.ViewModels.Employees
{
    public class PasswordWithVerification
    {
        public SecureString Password { get; set; }
        public SecureString VerificationPassword { get; set; }

        public bool Empty()
        {
            return Password.Length == 0 || VerificationPassword.Length == 0;
        }

        public bool Equal()
        {
            IntPtr valuePtrPassword = IntPtr.Zero;
            IntPtr valuePtrVerificationPassword = IntPtr.Zero;

            try
            {
                valuePtrPassword = Marshal.SecureStringToGlobalAllocUnicode(Password);
                valuePtrVerificationPassword = Marshal.SecureStringToGlobalAllocUnicode(VerificationPassword);
                return Marshal.PtrToStringUni(valuePtrPassword) == Marshal.PtrToStringUni(valuePtrVerificationPassword);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtrPassword);
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtrVerificationPassword);
            }
        }
    }
}