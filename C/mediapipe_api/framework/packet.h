#ifndef C_MEDIAPIPE_API_FRAMEWORK_PACKET_H_
#define C_MEDIAPIPE_API_FRAMEWORK_PACKET_H_

#include <map>
#include <memory>
#include <string>
#include <utility>
#include <vector>
#include "mediapipe/framework/packet.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"

template <typename T>
class UnsafePacketHolder : public mediapipe::packet_internal::Holder<T> {
  using mediapipe::packet_internal::Holder<T>::ptr_;

 public:
  T* Get() const {
    return const_cast<T*>(ptr_);
  }
};

extern "C" {

typedef struct MpPacket {
  std::unique_ptr<mediapipe::Packet> impl;

  MpPacket() : impl { std::make_unique<mediapipe::Packet>() } {}
  MpPacket(mediapipe::Packet packet) : impl { std::make_unique<mediapipe::Packet>(std::move(packet)) } {}
} MpPacket;

typedef std::map<std::string, mediapipe::Packet> SidePacket;

/** mediapipe::Packet API */
MP_CAPI(MpReturnCode) mp_Packet__(mediapipe::Packet** packet_out);
MP_CAPI(void) mp_Packet__delete(mediapipe::Packet* packet);
MP_CAPI(MpReturnCode) mp_Packet__At__Rtimestamp(mediapipe::Packet* packet, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsProtoMessageLite(mediapipe::Packet* packet, mediapipe::Status** status_out);
MP_CAPI(MpReturnCode) mp_Packet__Timestamp(mediapipe::Packet* packet, mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Packet__DebugString(mediapipe::Packet* packet, const char** str_out);
MP_CAPI(MpReturnCode) mp_Packet__RegisteredTypeName(mediapipe::Packet* packet, const char** str_out);
MP_CAPI(MpReturnCode) mp_Packet__DebugTypeName(mediapipe::Packet* packet, const char** str_out);

// Boolean
MP_CAPI(MpReturnCode) mp__MakeBoolPacket__b(bool value, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeBoolPacket_At__b_Rtimestamp(bool value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetBool(mediapipe::Packet* packet, bool* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsBool(mediapipe::Packet* packet, mediapipe::Status** status_out);

// Float
MP_CAPI(MpReturnCode) mp__MakeFloatPacket__b(float value, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetFloat(mediapipe::Packet* packet, float* value_out);

// String
MP_CAPI(MpReturnCode)  mp__MakeStringPacket__PKc(const char* str, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode)  mp__MakeStringPacketAt__PKc_i(const char* str, int timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode)  MpPacketGetString(mediapipe::Packet* packet, const char** value_out);

/** SidePacket API */
MP_CAPI(MpReturnCode) mp_SidePacket__(SidePacket** side_packet_out);
MP_CAPI(void) mp_SidePacket__delete(SidePacket* side_packet);
MP_CAPI(MpReturnCode) mp_SidePacket__emplace(SidePacket* side_packet, const char* key, mediapipe::Packet* packet);

}  // extern "C"

template <class T>
inline MpSerializedProto* MpPacketGetProto(MpPacket* packet) {
  auto proto = packet->impl->Get<T>();

  return MpSerializedProtoInitialize(proto);
}

template <class T>
inline MpSerializedProtoVector* MpPacketGetProtoVector(MpPacket* packet) {
  auto proto_vec = packet->impl->Get<std::vector<T>>();

  return MpSerializedProtoVectorInitialize(proto_vec);
}

#endif  // C_MEDIAPIPE_API_FRAMEWORK_PACKET_H_
