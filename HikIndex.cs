using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

namespace HikNAS
{
    public class HikIndex
    {
        public UInt64 u_modifyTimes = 0;
        public UInt32 u_version = 0;
        public UInt32 u_fileCount = 0;
        public UInt32 u_nextFileRecNo = 0;
        public UInt32 u_lastFileRecNo = 0;
        public List<Byte> b_currFileRec = new List<Byte>();
        public List<Byte> b_res3 = new List<Byte>();
        public UInt32 u_checksum = 0;
        public List<HikFile> a_file = new List<HikFile>();
        public List<HikSegment> a_segment = new List<HikSegment>();

        public HikIndex(String s_path)
        {
            if (s_path != null && File.Exists(s_path + "\\index00.bin"))
            {
                using (FileStream f_index = new FileStream(s_path + "\\index00.bin", FileMode.Open, FileAccess.Read))
                {
                    Int32 i_bytes = 0;
                    Byte[] b_buff = new Byte[HikConstants.INDEX_LEN];

                    if ((i_bytes = f_index.Read(b_buff, 0, HikConstants.INDEX_LEN)) == HikConstants.INDEX_LEN)
                    {
                        this.u_modifyTimes = BitConverter.ToUInt64(b_buff, 0);
                        this.u_version = BitConverter.ToUInt32(b_buff, 8);
                        this.u_fileCount = BitConverter.ToUInt32(b_buff, 12);
                        this.u_nextFileRecNo = BitConverter.ToUInt32(b_buff, 16);
                        this.u_lastFileRecNo = BitConverter.ToUInt32(b_buff, 20);
                        for (Int32 idx = 0; idx < 1176; idx++)
                            this.b_currFileRec.Add(b_buff[24 + idx]);
                        for (Int32 idx = 0; idx < 76; idx++)
                            this.b_res3.Add(b_buff[1200 + idx]);
                        this.u_checksum = BitConverter.ToUInt32(b_buff, 1276);


                        b_buff = new Byte[HikConstants.FILE_LEN * this.u_fileCount];
                        for (Int32 idx = 0; idx < this.u_fileCount; idx++)
                        {
                            if ((i_bytes = f_index.Read(b_buff, 0, HikConstants.FILE_LEN)) == HikConstants.FILE_LEN)
                            {
                                if (BitConverter.ToUInt16(b_buff, 4) != 65535)
                                {
                                    this.a_file.Add(new HikFile(s_path + "\\hiv" + BitConverter.ToUInt32(b_buff, 0).ToString().PadLeft(5, '0') + ".mp4",
                                                                 BitConverter.ToUInt32(b_buff, 0),
                                                                 BitConverter.ToUInt16(b_buff, 4),
                                                                 BitConverter.ToUInt16(b_buff, 6),
                                                                 HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToInt32(b_buff, 8))).ToLocalTime(),
                                                                 HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToInt32(b_buff, 12))).ToLocalTime(),
                                                                 b_buff[16],
                                                                 b_buff[17],
                                                                 BitConverter.ToUInt16(b_buff, 18),
                                                                 new List<Byte>(new Byte[] { b_buff[20], b_buff[21], b_buff[22], b_buff[23] }),
                                                                 new List<Byte>(new Byte[] { b_buff[24], b_buff[25], b_buff[26], b_buff[27],
                                                                                         b_buff[28], b_buff[29], b_buff[30], b_buff[31] })));
                                }
                            }
                            else
                            {
                                throw new HikException("Error reading file data, expected: " + Convert.ToInt32(HikConstants.FILE_LEN * this.u_fileCount).ToString() + ", read: " + i_bytes.ToString());
                            }
                        }

                        b_buff = new Byte[HikConstants.SEGMENT_LEN];
                        for (Int32 idx = 0; idx < this.u_fileCount * 256; idx++)
                        {
                            if ((i_bytes = f_index.Read(b_buff, 0, HikConstants.SEGMENT_LEN)) == HikConstants.SEGMENT_LEN)
                            {
                                if (BitConverter.ToUInt64(b_buff, 16) != 0)
                                {
                                    this.a_segment.Add(new HikSegment(0,
                                                                       b_buff[0],
                                                                       b_buff[1],
                                                                       new List<Byte>(new Byte[] { b_buff[2], b_buff[3] }),
                                                                       new List<Byte>(new Byte[] { b_buff[4], b_buff[5], b_buff[6], b_buff[7] }),
                                                                       HikConstants.dt_epoch.AddSeconds(BitConverter.ToUInt64(b_buff, 8) & 0x00000000ffffffff).ToLocalTime(),
                                                                       HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt64(b_buff, 16) & 0x00000000ffffffff)).ToLocalTime(),
                                                                       HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt64(b_buff, 24) & 0x00000000ffffffff)).ToLocalTime(),
                                                                       HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt32(b_buff, 32) & 0x00000000ffffffff)).ToLocalTime(),
                                                                       HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt32(b_buff, 36) & 0x00000000ffffffff)).ToLocalTime(),
                                                                       BitConverter.ToUInt32(b_buff, 40),
                                                                       BitConverter.ToUInt32(b_buff, 44),
                                                                       new List<Byte>(new Byte[] { b_buff[48], b_buff[49], b_buff[50], b_buff[51] }),
                                                                       new List<Byte>(new Byte[] { b_buff[52], b_buff[53], b_buff[54], b_buff[55] }),
                                                                       new List<Byte>(new Byte[] { b_buff[56], b_buff[57], b_buff[58], b_buff[59], b_buff[60], b_buff[61], b_buff[62], b_buff[63] }),
                                                                       new List<Byte>(new Byte[] { b_buff[64], b_buff[65], b_buff[66], b_buff[67] }),
                                                                       new List<Byte>(new Byte[] { b_buff[68], b_buff[69], b_buff[70], b_buff[71] }),
                                                                       new List<Byte>(new Byte[] { b_buff[72], b_buff[73], b_buff[74], b_buff[75] }),
                                                                       new List<Byte>(new Byte[] { b_buff[76], b_buff[77], b_buff[78], b_buff[79] })
                                        ));
                                }
                            }
                            else
                            {
                                throw new HikException("Error reading segment data, expected: " + Convert.ToInt32(HikConstants.SEGMENT_LEN).ToString() + ", read: " + i_bytes.ToString());
                            }
                        }
                    }
                    else
                    {
                        throw new HikException("Error reading index data, expected: " + Convert.ToInt32(HikConstants.INDEX_LEN).ToString() + ", read: " + i_bytes.ToString());
                    }
                }
            }
        }
    }
}