
#include <cstdint>
#include <cstring>
#include <iomanip>
#include <limits>
#include <ostream>
#include <stdexcept>
#include <sstream>
#include <string>
#include <vector>
#include <tuple>

#include "InternalMsgSerialization.h"
#include "InternalMsg.h"

namespace sbe
{
  class InternalMsgEnDecoder
    {
	private:
		InternalMsgSerialization ENCODER = InternalMsgSerialization();
		InternalMsgSerialization DECODER = InternalMsgSerialization();


	public:
		static const std::uint64_t MAX_BUFFER_SIZE = 256;
		InternalMsgEnDecoder()
		{
		}

		char* encode(InternalMsg msg)
		{
			char* buffer = new char[MAX_BUFFER_SIZE];
			ENCODER.wrapForEncode(buffer, 0, MAX_BUFFER_SIZE)
				.id(msg.id)
				.available(msg.available ? (uint8_t)1 : (uint8_t)0)
				.type(msg.type)
				.quantity(msg.quantity)
				.price(msg.price)
				.putSymbol(msg.symbol)
				.putReqId(msg.reqId)
				.putCustId(msg.custId);
			
			return buffer;
		}

		

		InternalMsg decode(const char* buffer)
		{
			InternalMsg msg =  InternalMsg();

			DECODER.wrapForDecode((char*)buffer, 0, InternalMsgSerialization::sbeBlockLength(), InternalMsgSerialization::sbeSchemaVersion(), MAX_BUFFER_SIZE);

			msg.id = DECODER.id();
			msg.available = DECODER.available() == 1 ? true : false;
			msg.type = DECODER.type();
			msg.quantity = DECODER.quantity();
			msg.price = DECODER.price();
			msg.symbol = DECODER.getSymbolAsString();
			msg.reqId = DECODER.getReqIdAsString();
			msg.custId = DECODER.getCustIdAsString();

			return msg;
		}
	}
}
