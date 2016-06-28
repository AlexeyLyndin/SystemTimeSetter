using System;
using System.Runtime.InteropServices;
using SystemTimeSetter.PrivelegeSetter;

namespace SystemTimeSetter.TimeSetter
{
    public static class TimeSetter
    {
        public static bool SetSystemTime(DateTime updateDateTime)
        {
            if (!SetTimePrivilege())
            {
                return false;
            }

            SystemTime systemTime = new SystemTime();
            InitializeSystemTimeStructure(updateDateTime, ref systemTime);

            bool setTimeResult = Win32SetSystemTime(ref systemTime);

            return setTimeResult;
        }

        private static void InitializeSystemTimeStructure(DateTime updatedDateTime, ref SystemTime systemTime)
        {
            systemTime.Year = (ushort)updatedDateTime.Year;
            systemTime.Month = (ushort)updatedDateTime.Month;
            systemTime.Day = (ushort)updatedDateTime.Day;
            systemTime.Hour = (ushort)updatedDateTime.Hour;
            systemTime.Minute = (ushort)updatedDateTime.Minute;
            systemTime.Second = (ushort)updatedDateTime.Second;
            systemTime.Millisecond = (ushort)updatedDateTime.Millisecond;
            systemTime.DayOfWeek = (ushort) updatedDateTime.DayOfWeek;
        }

        private static bool SetTimePrivilege()
        {
            return TimePrivilegeSetter.SetSystemTimePrivilege();
        }

        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime", SetLastError = true)]
        private extern static void Win32GetSystemTime(ref SystemTime sysTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", SetLastError = true)]
        private extern static bool Win32SetSystemTime(ref SystemTime sysTime);

        private struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        };
    }
}
