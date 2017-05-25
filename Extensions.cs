using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HikNAS
{
    public static class HikConstants
    {
        public static Int32 FILE_LEN = 32;        // Length of the file struct in bytes.
        public static Int32 SEGMENT_LEN = 80;  // Length of the segment struct in bytes.
        public static Int32 INFO_LEN = 68;
        public static Int32 INDEX_LEN = 1280; // Length of the header struct in bytes.


        public static DateTime dt_epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);  // unix epoch date
    }

    public static class Extensions
    {
        public static String ToString(this Byte[] buffer, Int32 iStart, Int32 iCount)
        {
            String sResult = String.Empty;
            if (buffer.Length > iStart)
                for (Int32 idx = iStart; idx < Math.Min(buffer.Length, iStart + iCount); idx++)
                    sResult = sResult + (buffer[idx] != 0 ? buffer[idx].ToString() : "");
            return sResult;
        }

        public static String ToCSV(this List<HikFile> files)
        {
            String line = String.Empty;
            String s = String.Empty;
            Type t = typeof(HikFile);
            FieldInfo[] fields = t.GetFields();

            s += String.Join(",", fields.Select(f => f.Name).ToArray()) + Environment.NewLine;

            foreach (HikFile file in files)
            {
                s += file.ToCSV() + Environment.NewLine;
            }
            return s;
        }

        public static String ToCSV(this List<HikSegment> segments)
        {
            Int32 i = 0;
            String line = String.Empty;
            String s = String.Empty;
            Type t = typeof(HikSegment);
            FieldInfo[] fields = t.GetFields();

            s += String.Join(",", fields.Select(f => f.Name).ToArray()) + Environment.NewLine;

            foreach (HikSegment segment in segments)
            {
                i++;
                s += segment.ToCSV() + Environment.NewLine;
            }
            return s;
        }

    }
}
