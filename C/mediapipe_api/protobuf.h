#ifndef C_MEDIAPIPE_API_PROTOBUF_H_
#define C_MEDIAPIPE_API_PROTOBUF_H_

#include <vector>
#include "mediapipe_api/common.h"
#include "google/protobuf/stubs/logging.h"

extern "C" {

typedef struct MpSerializedProto {
  const char* serialized_str;
  int length;

  ~MpSerializedProto() {
    delete[] serialized_str;
  }
} MpSerializedProto;

typedef struct MpSerializedProtoVector {
  MpSerializedProto** data;
  int size;

  ~MpSerializedProtoVector() {
    for (auto i = 0; i < size; ++i) {
      delete data[i];
    }

    delete[] data;
  }
} MpSerializedProtoVector;

typedef void LogHandler(int level, const char* filename, int line, const char* message);

MP_CAPI_EXPORT extern google::protobuf::LogHandler* SetProtobufLogHandler(LogHandler* handler);

MP_CAPI_EXPORT extern void MpSerializedProtoDestroy(MpSerializedProto* proto);
MP_CAPI_EXPORT extern void MpSerializedProtoVectorDestroy(MpSerializedProtoVector* proto_vec);

}  // extern "C"

template<class T>
extern inline MpSerializedProto* MpSerializedProtoInitialize(const T& proto) {
  auto str = proto.SerializeAsString();
  auto length = str.size();

  auto bytes = new char[length];
  memcpy(bytes, str.c_str(), length);

  return new MpSerializedProto { bytes, static_cast<int>(length) };
}

template<class T>
extern inline MpSerializedProtoVector* MpSerializedProtoVectorInitialize(const std::vector<T>& proto_vec) {
  auto size = proto_vec.size();
  auto data = new MpSerializedProto*[size];

  for (auto i = 0; i < size; ++i) {
    data[i] = MpSerializedProtoInitialize(proto_vec[i]);
  }

  return new MpSerializedProtoVector { data, static_cast<int>(size) };
}

#endif  // C_MEDIAPIPE_API_PROTOBUF_H_
