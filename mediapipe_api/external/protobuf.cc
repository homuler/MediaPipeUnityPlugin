// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/external/protobuf.h"

#include <functional>
#include <string>

#include "google/protobuf/stubs/logging.h"

using google::protobuf::LogLevel;

namespace {
LogHandler* logHandler;
google::protobuf::LogHandler* defaultLogHandler;
}

void HandleProtobufLog(LogLevel level, const char* filename, int line, const std::string& message) { logHandler(level, filename, line, message.c_str()); }

MpReturnCode google_protobuf__SetLogHandler__PF(LogHandler* handler) {
  TRY
    logHandler = handler;
    auto prevLogHandler = google::protobuf::SetLogHandler(&HandleProtobufLog);

    if (defaultLogHandler == nullptr) {
      defaultLogHandler = prevLogHandler;
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode google_protobuf__ResetLogHandler() {
  TRY
    if (logHandler) {
      google::protobuf::SetLogHandler(defaultLogHandler);
    }
    logHandler = nullptr;
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_api_SerializedProtoArray__delete(mp_api::SerializedProto* serialized_proto_vector_data, int size) {
  auto serialized_proto = serialized_proto_vector_data;
  for (auto i = 0; i < size; ++i) {
    delete (serialized_proto++)->str;
  }
  delete[] serialized_proto_vector_data;
}
