using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBE_Sample
{
    public class InternalMsg 
    {
        public ulong id { get; set; }
        public bool available { get; set; }
        public uint type { get; set; }
        public double quantity { get; set; }
        public double price { get; set; }
        public string symbol { get; set; }
        public string reqId { get; set; }
        public string custId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("1=").Append(id).Append(",")
                .Append("2=").Append(available).Append(",")
                .Append("3=").Append(type).Append(",")
                .Append("4=").Append(quantity).Append(",")
                .Append("5=").Append(price).Append(",")
                .Append("6=").Append(symbol).Append(",")
                .Append("7=").Append(reqId).Append(",")
                .Append("8=").Append(custId);
            return sb.ToString();
        }

        public static byte[] ToByteFromString(InternalMsg msg)
        {
           return Encoding.ASCII.GetBytes(msg.ToString());
        }

        public static InternalMsg decode(byte[] bytes)
        {
            String s= Encoding.ASCII.GetString(bytes);
            //start split string to convert to object 
            
            String[] fields  = s.Split(",");
            if (fields.Length > 0)
            {
                InternalMsg msg = new InternalMsg();
                String[] field0 = fields[0].Split("=");
               if(field0.Length > 1)
               {
                  msg.id = Convert.ToUInt64(field0[1]);
               }

                String[] field1 = fields[1].Split("=");
                if (field1.Length > 1)
                {
                    msg.available = Convert.ToBoolean(field1[1]);
                }

                String[] field2 = fields[2].Split("=");
                if (field2.Length > 1)
                {
                    msg.type = Convert.ToUInt32(field2[1]);
                }

                String[] field3 = fields[3].Split("=");
                if (field3.Length > 1)
                {
                    msg.quantity = Convert.ToDouble(field3[1]);
                }

                String[] field4 = fields[4].Split("=");
                if (field4.Length > 1)
                {
                    msg.price = Convert.ToDouble(field4[1]);
                }

                String[] field5 = fields[5].Split("=");
                if (field5.Length > 1)
                {
                    msg.symbol = field5[1];
                }

                String[] field6 = fields[6].Split("=");
                if (field6.Length > 1)
                {
                    msg.reqId = field6[1];
                }

                String[] field7 = fields[7].Split("=");
                if (field7.Length > 1)
                {
                    msg.custId = field7[1];
                }
                
                return msg;
            }
            return null;
        } 
    }
}
