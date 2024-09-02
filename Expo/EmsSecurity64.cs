using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Expo
{
    public class EmsSecurity64
    {
        /// <summary>
        /// 'EMSSecurityDll64.dll'
        /// </summary>
        private const string EMSSecurityDll = "D:\\projects\\dotnet\\Expo\\Expo\\bin\\x86\\Debug\\net7.0-windows10.0.19041.0\\win10-x64\\EMSSecurityDll64.dll";

        IntPtr license=0;

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_create", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static IntPtr Create();

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_delete", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void Delete(IntPtr license);

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_Init", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void Init(
            IntPtr license,
            int licensed_program,               // 0:ebsilon; 1:SRx; 2:EMSLicenseServer; 4: NT-Tools
            [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
            );

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_Exit", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void Exit(IntPtr license);


        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetValue", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void GetValue(IntPtr license,
            int productNumber,
            int flag,
            ref int flagValue,
            [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
            );

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetExpirationTime", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void GetExpirationTime(IntPtr license,
                                                                                                      ref bool bNoTimeLimit,
                                                                                                      ref UInt64 time, // __time64_t* time,
                                                                                                      [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
                                                                                                      );


        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetLicenceInformation", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void GetLicenceInformation(IntPtr license,
                                                                                                      [MarshalAs(UnmanagedType.BStr)] ref string strLicenceText, //  BSTR* strLicenceText,
                                                                                                      [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
                                                                                                      );









        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetLicenceType", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        //EMSSecurity::LicenceTypeEnum    
        internal extern static int GetLicenceType(IntPtr license);

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_add_option", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void AddOption(IntPtr license,
                                                                                                      [MarshalAs(UnmanagedType.BStr)] string key, //  BSTR* strLicenceText,
                                                                                                      [MarshalAs(UnmanagedType.BStr)] string value
                                                                                                      );

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetFlag", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void GetFlag(IntPtr license,
                                                                                                      int productNumber,
                                                                                                      int flag,
                                                                                                      bool isRelease,
                                                                                                      ref bool set,
                                                                                                      [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
                                                                                                      );

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_UpdateCachedLicence", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void UpdateCachedLicence(IntPtr license, EmsSecurityResult result);

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_DiscardCachedLicence", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void DiscardCachedLicence(IntPtr license);

        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetCachedFlag", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void GetCachedFlag(IntPtr license,
                                                                                                      int productNumber,
                                                                                                      int flag,
                                                                                                      bool isRelease,
                                                                                                      ref bool set,
                                                                                                      [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
                                                                                                      );


        [DllImport(EMSSecurityDll, EntryPoint = "AnyLicence_GetTimeLeft", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal extern static void GetTimeLeft(IntPtr license,
                                                                                                      ref bool bNoTimeLimit,
                                                                                                      ref UInt64 timeLeft, // __time64_t* timeLeft,
                                                                                                      [In][MarshalAs(UnmanagedType.LPStruct)] EmsSecurityResult result
                                                                                                      );

        static public DateTime UnixSecondsToDateTime(ulong timestamp, bool local = false)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds((long)timestamp);
            return local ? offset.LocalDateTime : offset.UtcDateTime;
        }
        
        public bool CheckLicence()
        {
            // create license object
            license = EmsSecurity64.Create();

            EmsSecurityResult r = new EmsSecurityResult();
            // acquire license
            EmsSecurity64.Init(license, 0, r);
            if (r.nativeResult != ResultEnum.ok)
            {
                // license error
                return false;
            }

            bool no_time_limit = false;
            ulong expiration_time = 0;
            EmsSecurity64.GetExpirationTime(license, ref no_time_limit, ref expiration_time, r);
            if (r.nativeResult != ResultEnum.ok)
            {
                // license error
                return false;
            }
            if (!no_time_limit)
            {
                var date = UnixSecondsToDateTime(expiration_time);
                if (date < DateTime.Now)
                {
                    // sorry license has runout
                    //MessageBox.Show("sorry license has runout");
                    return false;
                }
            }
            // read license values for products
            int flag_value = 0;
            EmsSecurity64.GetValue(license, 4, 1, ref flag_value, r);
            if (r.nativeResult != ResultEnum.ok)
            {
                // license error
                return false;
            }


            return true;
        } 

        public void ExitLicence()
        {
            if (license != 0)
            {
                EmsSecurity64.Exit(license);
                EmsSecurity64.Delete(license);
            }
        }
    }

    public enum ResultEnum
    {
        ok = 0
    };

    [StructLayout(LayoutKind.Sequential)]
    public class EmsSecurityResult
    {
        public EmsSecurityResult()
        {
            nativeResult = ResultEnum.ok;
            status = 0;
            netLastStatus = 0;
        }
        public ResultEnum nativeResult;
        public ulong status;
        public int netLastStatus;      // Par1 von Net-HASP LastStatus Aufruf
    };

}
