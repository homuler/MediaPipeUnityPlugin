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

typedef struct MpSidePacket {
  std::shared_ptr<std::map<std::string, mediapipe::Packet>> impl;

  MpSidePacket() : impl { std::make_shared<std::map<std::string, mediapipe::Packet>>() } {}
} MpSidePacket;

/** mediapipe::Packet API */
MP_CAPI_EXPORT extern MpPacket* MpPacketCreate();
MP_CAPI_EXPORT extern void MpPacketDestroy(MpPacket* packet);

// Boolean
MP_CAPI_EXPORT extern MpPacket* MpMakeBoolPacket(bool value);
MP_CAPI_EXPORT extern bool MpPacketGetBool(MpPacket* packet);

// Float
MP_CAPI_EXPORT extern MpPacket* MpMakeFloatPacket(float value);
MP_CAPI_EXPORT extern float MpPacketGetFloat(MpPacket* packet);

// String
MP_CAPI_EXPORT extern MpPacket* MpMakeStringPacketAt(const char* str, int timestamp);
MP_CAPI_EXPORT extern const char* MpPacketGetString(MpPacket* packet);

/** SidePacket API */
MP_CAPI_EXPORT extern MpSidePacket* MpSidePacketCreate();
MP_CAPI_EXPORT extern void MpSidePacketDestroy(MpSidePacket* side_packet);
MP_CAPI_EXPORT extern void MpSidePacketInsert(MpSidePacket* side_packet, const char* key, MpPacket* packet);

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
