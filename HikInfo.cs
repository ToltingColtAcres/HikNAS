using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace HikNAS
{
    public class HikInfo
    {
        public String s_serialNumber = "";
        public String s_macAddress = "";
        public Byte[] b_res = new Byte[] { 0, 0 };
        public UInt32 u_bsize = 0;
        public UInt32 u_blocks = 0;
        public UInt32 u_dataDirs = 0;
        public List<HikIndex> a_index = new List<HikIndex>();

        public HikInfo(String s_path)
        {
            if (s_path != null && File.Exists(s_path + "\\info.bin "))
            {
                using (FileStream f_info = new FileStream(s_path + "\\info.bin", FileMode.Open, FileAccess.Read))
                {
                    Byte[] b_buff = new Byte[HikConstants.INFO_LEN];

                    if (f_info.Read(b_buff, 0, HikConstants.INFO_LEN) == HikConstants.INFO_LEN)
                    {
                        this.s_serialNumber = b_buff.ToString(0, 48);
                        this.s_macAddress = BitConverter.ToString(b_buff, 48, 6);
                        this.b_res[0] = b_buff[54];
                        this.b_res[1] = b_buff[55];
                        this.u_bsize = BitConverter.ToUInt32(b_buff, 56);
                        this.u_blocks = BitConverter.ToUInt32(b_buff, 60);
                        this.u_dataDirs = BitConverter.ToUInt32(b_buff, 64);

                        for (Int32 idx = 0; idx < this.u_dataDirs; idx++)
                            this.a_index.Add(new HikIndex(s_path + "\\datadir" + idx.ToString()));
                    }
                    b_buff = null;
                }
            }
        }
    }
}