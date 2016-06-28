
using System.Runtime.InteropServices;

namespace SystemTimeSetter.PrivelegeSetter
{
    internal static class TimePrivilegeSetter
    {
        public static bool SetSystemTimePrivilege()
        {
            return SetPrivilege("SeSystemtimePrivilege");
        }

        private static bool SetPrivilege(string privilege)
        {
            int i1 = 0;
            var luid = new LUID();
            var token_PRIVILEGES = new TOKEN_PRIVILEGES();
            int i2 = OpenProcessToken(GetCurrentProcess(), 40, ref i1);
            if (i2 == 0)
            {
                return false;
            }

            i2 = LookupPrivilegeValue(null, privilege, ref luid);
            if (i2 == 0)
            {
                return false;
            }

            token_PRIVILEGES.PrivilegeCount = 1;
            token_PRIVILEGES.Attributes = 2;
            token_PRIVILEGES.Luid = luid;
            i2 = AdjustTokenPrivileges(i1, 0, ref token_PRIVILEGES, 1024, 0, 0);
            if (i2 == 0)
            {
                return false;
            }

            return true;
        }

        [PreserveSig]
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        private static extern int AdjustTokenPrivileges(int tokenhandle, int disableprivs,
                                                        [MarshalAs(UnmanagedType.Struct)] ref TOKEN_PRIVILEGES newstate,
                                        int bufferlength, int preivousState, int returnLength);

        [PreserveSig]
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        private static extern int GetCurrentProcess();

        [PreserveSig]
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        private static extern int LookupPrivilegeValue(string lpsystemname, string lpname,
        [MarshalAs(UnmanagedType.Struct)] ref LUID lpLuid);

        [PreserveSig]
        [DllImport("advapi32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        private static extern int OpenProcessToken(int processHandle, int desiredAccess, ref int tokenhandle);

        internal struct LUID
        {
            public int HighPart;
            public int LowPart;
        }

        internal struct TOKEN_PRIVILEGES
        {
            public int Attributes;
            public LUID Luid;
            public int PrivilegeCount;
        }
    }
}
