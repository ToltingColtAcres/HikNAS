using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikNAS
{
    public class HikFile
    {
        public String s_fileName = String.Empty;

        public UInt32 u_fileNo = 0;
        public UInt16 u_chan = 0;
        public UInt16 u_segRecNums = 0;
        public DateTime dt_startTime = new DateTime();
        public DateTime dt_endTime = new DateTime();
        public Byte b_status = 0;
        public Byte b_res1 = 0;
        public UInt16 u_lockedSegNum = 0;
        public List<Byte> b_res2 = new List<Byte>();
        public List<Byte> b_infoTypes = new List<Byte>();

        public String ToCSV()
        {
            return this.s_fileName + "," +
                   this.u_fileNo + "," +
                   this.u_chan + "," +
                   this.u_segRecNums + "," +
                   this.dt_startTime.ToString() + "," +
                   this.dt_endTime + "," +
                   String.Format("{0:X}", this.b_status) + "," +
                   String.Format("{0:X}", this.b_res1) + "," +
                   this.u_lockedSegNum + "," +
                   BitConverter.ToString(this.b_res2.ToArray()) + "," +
                   BitConverter.ToString(this.b_infoTypes.ToArray());
        }

        public HikFile()
        {
        }

        public HikFile(Byte[] b_buff, String s_path)
        {
            this.u_fileNo = BitConverter.ToUInt32(b_buff, 0);
            this.u_chan = BitConverter.ToUInt16(b_buff, 4);
            this.u_segRecNums = BitConverter.ToUInt16(b_buff, 6);
            this.dt_startTime = HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToInt32(b_buff, 8))).ToLocalTime();
            this.dt_endTime = HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToInt32(b_buff, 12))).ToLocalTime();
            this.b_status = b_buff[16];
            this.b_res1 = b_buff[17];
            this.u_lockedSegNum = BitConverter.ToUInt16(b_buff, 18);
            this.b_res2.AddRange(new Byte[] { b_buff[20], b_buff[21], b_buff[22], b_buff[23] });
            this.b_infoTypes.AddRange(new Byte[] { b_buff[24], b_buff[25], b_buff[26], b_buff[27], b_buff[28], b_buff[29], b_buff[30], b_buff[31] } );
            this.s_fileName = s_path + "\\hiv" + u_fileNo.ToString().PadLeft(5, '0') + ".mp4";
        }

        public HikFile(String s_fileName, UInt32 u_fileNo, UInt16 u_chan, UInt16 u_segRecNums, DateTime dt_startTime, DateTime dt_endTime, Byte b_status, Byte b_res1, UInt16 u_lockedSegNum, List<Byte> b_res2, List<Byte> b_infoTypes)
        {
            this.s_fileName = s_fileName;
            this.u_fileNo = u_fileNo;
            this.u_chan = u_chan;
            this.u_segRecNums = u_segRecNums;
            this.dt_startTime = dt_startTime;
            this.dt_endTime = dt_endTime;
            this.b_status = b_status;
            this.b_res1 = b_res1;
            this.u_lockedSegNum = u_lockedSegNum;
            this.b_res2.AddRange(b_res2);
            this.b_infoTypes.AddRange(b_infoTypes);            
        }
    }
}
