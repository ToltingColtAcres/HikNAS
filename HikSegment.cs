using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HikNAS
{
    public class HikSegment
    {
        public Int32 i_index;
        public Byte b_type;
        public Byte b_status;
        public List<Byte> b_res1 = new List<Byte>();
        public List<Byte> b_resolution = new List<Byte>();
        public DateTime dt_startTime;
        public DateTime dt_endTime;
        public DateTime dt_firstKeyFrame_absTime;
        public DateTime dt_firstKeyFrame_stdTime;
        public DateTime dt_lastFrame_stdTime;
        public UInt32 u_startOffset;
        public UInt32 u_endOffset;
        public List<Byte> b_res2 = new List<Byte>();
        public List<Byte> b_infoNum = new List<Byte>();
        public List<Byte> b_infoTypes = new List<Byte>();
        public List<Byte> b_infoStartTime = new List<Byte>();
        public List<Byte> b_infoEndTime = new List<Byte>();
        public List<Byte> b_infoStartOffset = new List<Byte>();
        public List<Byte> b_infoEndOffset = new List<Byte>();

        public HikSegment()
        {
        }

        public HikSegment(Byte[] b_buff)
        {
            UInt64 secondsPastEpoch = Convert.ToUInt64((DateTime.Now - HikConstants.dt_epoch).TotalSeconds);

            this.b_type = b_buff[0];
            this.b_status = b_buff[1];
            this.b_res1.AddRange(new Byte[] { b_buff[2], b_buff[3] });
            this.b_resolution.AddRange(new Byte[] { b_buff[4], b_buff[5], b_buff[6], b_buff[7] });
            this.dt_startTime = HikConstants.dt_epoch.AddSeconds(BitConverter.ToUInt64(b_buff, 8) & 0x00000000ffffffff).ToLocalTime();
            this.dt_endTime = HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt64(b_buff, 16) & 0x00000000ffffffff)).ToLocalTime();
            this.dt_firstKeyFrame_absTime = HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt64(b_buff, 24) & 0x00000000ffffffff)).ToLocalTime();
            this.dt_firstKeyFrame_stdTime = HikConstants.dt_epoch.AddSeconds(Convert.ToInt32(BitConverter.ToInt32(b_buff, 32))).ToLocalTime();
            this.dt_lastFrame_stdTime = HikConstants.dt_epoch.AddSeconds(Convert.ToInt64(BitConverter.ToUInt32(b_buff, 36))).ToLocalTime();
            this.u_startOffset = BitConverter.ToUInt32(b_buff, 40);
            this.u_endOffset = BitConverter.ToUInt32(b_buff, 44);
            this.b_res2.AddRange(new Byte[] { b_buff[48], b_buff[49], b_buff[50], b_buff[51] });
            this.b_infoNum.AddRange(new Byte[] { b_buff[52], b_buff[53], b_buff[54], b_buff[55] });
            this.b_infoTypes.AddRange(new Byte[] { b_buff[56], b_buff[57], b_buff[58], b_buff[59], b_buff[60], b_buff[61], b_buff[62], b_buff[63] });
            this.b_infoStartTime.AddRange(new Byte[] { b_buff[64], b_buff[65], b_buff[66], b_buff[67] });
            this.b_infoEndTime.AddRange(new Byte[] { b_buff[68], b_buff[69], b_buff[70], b_buff[71] });
            this.b_infoStartOffset.AddRange(new Byte[] { b_buff[72], b_buff[73], b_buff[74], b_buff[75] });
            this.b_infoEndOffset.AddRange(new Byte[] { b_buff[76], b_buff[77], b_buff[78], b_buff[79] });
        }

        public HikSegment(Int32 i_index, Byte b_type, Byte b_status, List<Byte> b_res1, List<Byte> b_resolution, DateTime dt_startTime, DateTime dt_endTime, DateTime dt_firstKeyFrame_absTime,
                          DateTime dt_firstKeyFrame_stdTime, DateTime dt_lastFrame_stdTime, UInt32 u_startOffset, UInt32 u_endOffset, List<Byte> b_res2, List<Byte> b_infoNum, List<Byte> b_infoTypes,
                          List<Byte> b_infoStartTime, List<Byte> b_infoEndTime, List<Byte> infoStartOffset, List<Byte> infoEndOffset)
        {
            this.i_index = i_index;
            this.b_type = b_type;
            this.b_status = b_status;
            this.b_res1.AddRange(b_res1);
            this.b_resolution.AddRange(b_resolution);
            this.dt_startTime = dt_startTime;
            this.dt_endTime = dt_endTime;
            this.dt_firstKeyFrame_absTime = dt_firstKeyFrame_absTime;
            this.dt_firstKeyFrame_stdTime = dt_firstKeyFrame_stdTime;
            this.dt_lastFrame_stdTime = dt_lastFrame_stdTime;
            this.u_startOffset = u_startOffset;
            this.u_endOffset = u_endOffset;
            this.b_res2.AddRange(b_res2);
            this.b_infoNum.AddRange(b_infoNum);
            this.b_infoTypes.AddRange(b_infoTypes);
            this.b_infoStartTime.AddRange(b_infoStartTime);
            this.b_infoEndTime.AddRange(b_infoEndTime);
            this.b_infoStartOffset.AddRange(b_infoStartOffset);
            this.b_infoEndOffset.AddRange(b_infoEndOffset);
        }

        public String ToCSV()
        {
            return i_index.ToString() + "," +
                   String.Format("{0:X}", b_type) + "," +
                   String.Format("{0:X}", b_status) + "," +
                   BitConverter.ToString(b_res1.ToArray()) + "," +
                   BitConverter.ToString(b_resolution.ToArray()) + "," +
                   dt_startTime.ToString() + "," +
                   dt_endTime.ToString() + "," +
                   dt_firstKeyFrame_absTime.ToString() + "," +
                   dt_firstKeyFrame_stdTime.ToString() + "," +
                   dt_lastFrame_stdTime.ToString() + "," +
                   u_startOffset.ToString() + "," +
                   u_endOffset.ToString() + "," +
                   BitConverter.ToString(b_res2.ToArray()) + "," +
                   BitConverter.ToString(b_infoNum.ToArray()) + "," +
                   BitConverter.ToString(b_infoTypes.ToArray()) + "," +
                   BitConverter.ToString(b_infoStartTime.ToArray()) + "," +
                   BitConverter.ToString(b_infoEndTime.ToArray()) + "," +
                   BitConverter.ToString(b_infoStartOffset.ToArray()) + "," +
                   BitConverter.ToString(b_infoEndOffset.ToArray());
        }
    }
}
