#ifndef C_MEDIAPIPE_API_EXTERNAL_PROTOBUF_H_
#define C_MEDIAPIPE_API_EXTERNAL_PROTOBUF_H_

#include <vector>
#include <sstream>
#include <iomanip>
#include "mediapipe_api/common.h"
#include "mediapipe/framework/port/parse_text_proto.h"

namespace mp_api {

typedef struct SerializedProto {
  const char* str;
  int length;
};

}  // namespace mp_api

template<class T>
inline struct mp_api::SerializedProto SerializeProto(const T& proto) {
  auto str = proto.SerializeAsString();
  auto size = str.size();
  auto bytes = new char[size];
  memcpy(bytes, str.c_str(), size);
  return mp_api:: SerializedProto { bytes, static_cast<int>(size) };
}

template<class T>
inline struct mp_api::StructArray<mp_api::SerializedProto> SerializeProtoVector(const std::vector<T>& proto_vec) {
  mp_api::StructArray<mp_api::SerializedProto> serialized_proto_vector;

  auto vec_size = proto_vec.size();
  serialized_proto_vector.data = new mp_api::SerializedProto[vec_size];

  for (auto i = 0; i < vec_size; ++i) {
    serialized_proto_vector.data[i] = SerializeProto(proto_vec[i]);
  }
  serialized_proto_vector.size = static_cast<int>(vec_size);
  return serialized_proto_vector;
}

template<class T>
inline bool ConvertFromTextFormat(const char* str, mp_api::SerializedProto& output) {
  T proto;
  auto result = google::protobuf::TextFormat::ParseFromString(str, &proto);

  if (result) {
    output = SerializeProto(proto);
  }
  return result;
}

extern "C" {

typedef void LogHandler(int level, const char* filename, int line, const char* message);

MP_CAPI(MpReturnCode) google_protobuf__SetLogHandler__PF(LogHandler* handler);

MP_CAPI(void) mp_api_SerializedProtoArray__delete(mp_api::SerializedProto* serialized_proto_vector_data);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_EXTERNAL_PROTOBUF_H_
