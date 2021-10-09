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
}

void HandleProtobufLog(LogLevel level, const char* filename, int line, const std::string& message) { logHandler(level, filename, line, message.c_str()); }

MpReturnCode google_protobuf__SetLogHandler__PF(LogHandler* handler) {
  TRY
    logHandler = handler;
    google::protobuf::SetLogHandler(&HandleProtobufLog);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_api_SerializedProtoArray__delete(mp_api::SerializedProto* serialized_proto_vector_data) { delete[] serialized_proto_vector_data; }
