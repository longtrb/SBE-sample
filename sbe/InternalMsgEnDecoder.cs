using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.SbeTool.Sbe.Dll;
using Sbe;

namespace SBE_Sample.sbe
{
    public  class InternalMsgEnDecoder
    {
		private InternalMsgSerialization ENCODER = new InternalMsgSerialization();
		private InternalMsgSerialization DECODER = new InternalMsgSerialization();

		private  DirectBuffer ENCODE_BYTE_BUFFER;
		private DirectBuffer DECODE_BYTE_BUFFER;

		public InternalMsgEnDecoder()
		{
			ENCODE_BYTE_BUFFER = new DirectBuffer(new byte[256]);
			DECODE_BYTE_BUFFER = new DirectBuffer(new byte[256]);
		}

		
		public byte[] encode(InternalMsg msg)
		{
			ENCODER.WrapForEncode(ENCODE_BYTE_BUFFER, 0);
			ENCODER.Id = msg.id;
			ENCODER.Available = msg.available ? (byte)1 : (byte)0;
			ENCODER.Type = msg.type;
			ENCODER.Quantity = msg.quantity;
			ENCODER.Price = msg.price;
			ENCODER.SetSymbol(msg.symbol);
			ENCODER.SetReqId(msg.reqId);
			ENCODER.SetCustId(msg.custId);
			//byte[] bytes = new byte[ENCODER.Size];
			//ENCODE_BYTE_BUFFER.GetBytes(0,bytes,0, ENCODER.Size);
			byte[] bytes = ENCODE_BYTE_BUFFER.AsReadOnlySpan<byte>(0, ENCODER.Size).ToArray();
			return bytes;
		}

		public void decode(byte[] buffer, InternalMsg msg)
        {

			DECODE_BYTE_BUFFER.Wrap(buffer);

			DECODER.WrapForDecode(DECODE_BYTE_BUFFER, 0, InternalMsgSerialization.BlockLength, 0);

			msg.id = DECODER.Id;
			msg.available = DECODER.Available == 1? true: false;
			msg.type = DECODER.Type;
			msg.quantity = DECODER.Quantity;
			msg.price = DECODER.Price;
			msg.symbol = DECODER.GetSymbol();
			msg.reqId = DECODER.GetReqId();
			msg.custId = DECODER.GetCustId();

		}

		public InternalMsg decode(byte[] buffer)
		{
			InternalMsg msg = new InternalMsg();
			DECODE_BYTE_BUFFER.Wrap(buffer);

			DECODER.WrapForDecode(DECODE_BYTE_BUFFER, 0, InternalMsgSerialization.BlockLength, 0);

			msg.id = DECODER.Id;
			msg.available = DECODER.Available == 1 ? true : false;
			msg.type = DECODER.Type;
			msg.quantity = DECODER.Quantity;
			msg.price = DECODER.Price;
			msg.symbol = DECODER.GetSymbol();
			msg.reqId = DECODER.GetReqId();
			msg.custId = DECODER.GetCustId();

			return msg;
		}
	}
}
