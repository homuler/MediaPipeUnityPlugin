// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_PACKET_H_
#define MEDIAPIPE_API_FRAMEWORK_PACKET_H_

#include <map>
#include <memory>
#include <string>
#include <utility>
#include <vector>

#include "mediapipe/framework/packet.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"

namespace mp_api {

template <typename T>
class UnsafePacketHolder : public mediapipe::packet_internal::Holder<T> {
  using mediapipe::packet_internal::Holder<T>::ptr_;

 public:
  const T* Get() const { return ptr_; }
};

}  // namespace mp_api

extern "C" {

typedef std::map<std::string, mediapipe::Packet> PacketMap;

/** mediapipe::Packet API */
MP_CAPI(MpReturnCode) mp_Packet__(mediapipe::Packet** packet_out);
MP_CAPI(void) mp_Packet__delete(mediapipe::Packet* packet);
MP_CAPI(MpReturnCode) mp_Packet__At__Rt(mediapipe::Packet* packet, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(bool) mp_Packet__IsEmpty(mediapipe::Packet* packet);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsProtoMessageLite(mediapipe::Packet* packet, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_Packet__Timestamp(mediapipe::Packet* packet, mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Packet__DebugString(mediapipe::Packet* packet, const char** str_out);
MP_CAPI(MpReturnCode) mp_Packet__RegisteredTypeName(mediapipe::Packet* packet, const char** str_out);
MP_CAPI(MpReturnCode) mp_Packet__DebugTypeName(mediapipe::Packet* packet, const char** str_out);

// bool
MP_CAPI(MpReturnCode) mp__MakeBoolPacket__b(bool value, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeBoolPacket_At__b_Rt(bool value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetBool(mediapipe::Packet* packet, bool* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsBool(mediapipe::Packet* packet, absl::Status** status_out);

// float
MP_CAPI(MpReturnCode) mp__MakeFloatPacket__f(float value, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeFloatPacket_At__f_Rt(float value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetFloat(mediapipe::Packet* packet, float* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsFloat(mediapipe::Packet* packet, absl::Status** status_out);

// int
MP_CAPI(MpReturnCode) mp__MakeIntPacket__i(int value, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeIntPacket_At__i_Rt(int value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetInt(mediapipe::Packet* packet, int* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsInt(mediapipe::Packet* packet, absl::Status** status_out);

// float[]
MP_CAPI(MpReturnCode) mp__MakeFloatArrayPacket__Pf_i(float* value, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeFloatArrayPacket_At__Pf_i_Rt(float* value, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetFloatArray_i(mediapipe::Packet* packet, int size, const float** value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsFloatArray(mediapipe::Packet* packet, absl::Status** status_out);

// std::vector<float>
MP_CAPI(MpReturnCode) mp__MakeFloatVectorPacket__Pf_i(float* value, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeFloatVectorPacket_At__Pf_i_Rt(float* value, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetFloatVector(mediapipe::Packet* packet, mp_api::StructArray<float>* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsFloatVector(mediapipe::Packet* packet, absl::Status** status_out);

// String
MP_CAPI(MpReturnCode) mp__MakeStringPacket__PKc(const char* str, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeStringPacket_At__PKc_Rt(const char* str, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeStringPacket__PKc_i(const char* str, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeStringPacket_At__PKc_i_Rt(const char* str, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetString(mediapipe::Packet* packet, const char** value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetByteString(mediapipe::Packet* packet, const char** value_out, int* size_out);
MP_CAPI(MpReturnCode) mp_Packet__ConsumeString(mediapipe::Packet* packet, absl::Status** status_out, const char** value_out);
MP_CAPI(MpReturnCode) mp_Packet__ConsumeByteString(mediapipe::Packet* packet, absl::Status** status_out, const char** value_out, int* size_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsString(mediapipe::Packet* packet, absl::Status** status_out);

/** PacketMap API */
MP_CAPI(MpReturnCode) mp_PacketMap__(PacketMap** packet_map_out);
MP_CAPI(void) mp_PacketMap__delete(PacketMap* packet_map);
MP_CAPI(MpReturnCode) mp_PacketMap__emplace__PKc_Rp(PacketMap* packet_map, const char* key, mediapipe::Packet* packet);
MP_CAPI(MpReturnCode) mp_PacketMap__find__PKc(PacketMap* packet_map, const char* key, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_PacketMap__erase__PKc(PacketMap* packet_map, const char* key, int* count_out);
MP_CAPI(void) mp_PacketMap__clear(PacketMap* packet_map);
MP_CAPI(int) mp_PacketMap__size(PacketMap* packet_map);

}  // extern "C"

template <typename T>
inline MpReturnCode mp_Packet__Consume(mediapipe::Packet* packet, absl::Status** status_out, T** value_out) {
  TRY_ALL
    auto status_or_unique_ptr = packet->Consume<T>();

    *status_out = new absl::Status{status_or_unique_ptr.status()};
    if (status_or_unique_ptr.ok()) {
      *value_out = new T{std::move(*status_or_unique_ptr.value().release())};
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

template <typename T>
inline MpReturnCode mp_Packet__Get(mediapipe::Packet* packet, const T** value_out) {
  TRY_ALL
    auto holder = packet->IsEmpty() ? nullptr : mediapipe::packet_internal::GetHolder(*packet)->As<T>();
    auto unsafe_holder = static_cast<const mp_api::UnsafePacketHolder<T>*>(holder);

    if (unsafe_holder == nullptr) {
      absl::Status status = packet->ValidateAsType<T>();
      LOG(FATAL) << "mp_Packet__Get() failed: " << status.message();
    }
    *value_out = unsafe_holder->Get();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

// std::vector<T>

template <typename T>
inline MpReturnCode mp__MakeVectorPacket(const T* array, int size, mediapipe::Packet** packet_out) {
  TRY
    std::vector<T> vector(array, array + size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::vector<T>>(vector)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

template <typename T>
inline MpReturnCode mp__MakeVectorPacket_At(const T* array, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    std::vector<T> vector(array, array + size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::vector<T>>(vector).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

template <typename T>
inline MpReturnCode mp_Packet__GetStructVector(mediapipe::Packet* packet, mp_api::StructArray<T>* value_out) {
  TRY_ALL
    auto vec = packet->Get<std::vector<T>>();
    auto size = vec.size();
    auto data = new T[size];

    for (auto i = 0; i < size; ++i) {
      data[i] = vec[i];
    }
    value_out->data = data;
    value_out->size = static_cast<int>(size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

// SerializedProto

template <typename T>
inline MpReturnCode mp_Packet__GetSerializedProto(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  TRY_ALL
    auto proto = packet->Get<T>();
    SerializeProto(proto, value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

template <typename T>
inline MpReturnCode mp_Packet__GetSerializedProtoVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  TRY_ALL
    auto proto_vec = packet->Get<std::vector<T>>();
    SerializeProtoVector(proto_vec, value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

#endif  // MEDIAPIPE_API_FRAMEWORK_PACKET_H_
