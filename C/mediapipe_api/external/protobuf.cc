#include <functional>
#include <string>
#include "mediapipe_api/external/protobuf.h"

using google::protobuf::LogLevel;

namespace {
  LogHandler* logHandler;
}

void HandleProtobufLog(LogLevel level, const char* filename, int line, const std::string& message) {
  logHandler(level, filename, line, message.c_str());
}

MpReturnCode google_protobuf__SetLogHandler__PF(LogHandler* handler) {
  TRY {
    logHandler = handler;
    google::protobuf::SetLogHandler(&HandleProtobufLog);
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void MpSerializedProtoDestroy(MpSerializedProto* proto) {
  delete proto;
}

void MpSerializedProtoVectorDestroy(MpSerializedProtoVector* proto_vec) {
  delete proto_vec;
}
