
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

/// <summary>
/// Benchmark result:
/// 
///		         Method |      Mean |     Error |    StdDev | Completed Work Items | Lock Contentions |  Gen 0 | Allocated |
///    |--------------- |----------:|----------:|----------:|---------------------:|-----------------:|-------:|----------:|
///    | benchSBEEncode |  60.44 ns |  0.152 ns |  0.142 ns |                    - |                - | 0.0497 |     104 B |
///    | benchSBEDecode | 141.45 ns |  0.921 ns |  0.816 ns |                    - |                - | 0.1032 |     216 B |
///    | benchNVSEncode | 586.11 ns | 11.462 ns | 10.160 ns |                    - |                - | 0.4129 |     864 B |
///    | benchNVSDecode | 809.03 ns |  1.537 ns |  1.363 ns |                    - |                - | 0.7114 |   1,488 B |
/// </summary>

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
