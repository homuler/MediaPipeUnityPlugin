#ifndef C_MEDIAPIPE_API_EXTERNAL_PROTOBUF_H_
#define C_MEDIAPIPE_API_EXTERNAL_PROTOBUF_H_

#include <vector>
#include "mediapipe_api/common.h"
#include "mediapipe/framework/port/parse_text_proto.h"

namespace mp_api {

struct SerializedProto {
  const char* str;
  int length;

  ~SerializedProto() {
    delete[] str;
  }
};

struct SerializedProtoVector {
  SerializedProto** data;
  int size;

  ~SerializedProtoVector() {
    for (auto i = 0; i < size; ++i) {
      delete data[i];
    }

    delete[] data;
  }
};

}  // namespace mp_api

template<class T>
inline struct mp_api::SerializedProto* SerializeProto(const T& proto) {
  auto str = proto.SerializeAsString();
  auto length = str.size();
  auto bytes = new char[length];
  memcpy(bytes, str.c_str(), length);

  return new mp_api::SerializedProto { bytes, static_cast<int>(length) };
}

template<class T>
inline struct mp_api::SerializedProtoVector* SerializeProtoVector(const std::vector<T>& proto_vec) {
  auto size = proto_vec.size();
  auto data = new mp_api::SerializedProto*[size];

  for (auto i = 0; i < size; ++i) {
    data[i] = SerializeProto(proto_vec[i]);
  }

  return new mp_api::SerializedProtoVector { data, static_cast<int>(size) };
}

template<class T>
inline struct mp_api::SerializedProto* ConvertFromTextFormat(const char* str) {
  T proto;
  auto result = google::protobuf::TextFormat::ParseFromString(str, &proto);

  return result ? SerializeProto(proto) : nullptr;
}

extern "C" {

typedef void LogHandler(int level, const char* filename, int line, const char* message);

MP_CAPI(MpReturnCode) google_protobuf__SetLogHandler__PF(LogHandler* handler);

MP_CAPI(void) mp_api_SerializedProto__delete(mp_api::SerializedProto* serialized_proto);
MP_CAPI(void) mp_api_SerializedProtoVector__delete(mp_api::SerializedProtoVector* serialized_proto_vector);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_EXTERNAL_PROTOBUF_H_
