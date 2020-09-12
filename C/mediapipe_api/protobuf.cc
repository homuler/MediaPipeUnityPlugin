#include <functional>
#include <string>
#include "mediapipe_api/protobuf.h"

using google::protobuf::LogLevel;

namespace {
  LogHandler* logHandler;
}

void HandleProtobufLog(LogLevel level, const char* filename, int line, const std::string& message) {
  logHandler(level, filename, line, message.c_str());
}

google::protobuf::LogHandler* SetProtobufLogHandler(LogHandler* lh) {
  logHandler = lh;

  return google::protobuf::SetLogHandler(&HandleProtobufLog);
}

void MpSerializedProtoDestroy(MpSerializedProto* proto) {
  delete proto;
}

void MpSerializedProtoVectorDestroy(MpSerializedProtoVector* proto_vec) {
  delete proto_vec;
}
