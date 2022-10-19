#include <cstdint>
#include <cstring>
#include <ostream>
#include <stdexcept>
#include <sstream>
#include <string>

namespace sbe {
	class InternalMsg {
	public:
        InternalMsg() {};
        uint64_t id;
        bool available;
        uint32_t type;
        double quantity;
        double price;
        std::string symbol;
        std::string reqId;
        std::string custId;
	};
}
