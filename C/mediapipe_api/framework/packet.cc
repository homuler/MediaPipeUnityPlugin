#include <string>
#include <utility>
#include "mediapipe_api/framework/packet.h"

MpReturnCode mp_Packet__(mediapipe::Packet** packet_out) {
  TRY {
    *packet_out = new mediapipe::Packet();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_Packet__delete(mediapipe::Packet* packet) {
  delete packet;
}

MpReturnCode mp_Packet__At__Rtimestamp(mediapipe::Packet* packet, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY {
    // not move but copy
    *packet_out = new mediapipe::Packet { packet->At(*timestamp) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__ValidateAsProtoMessageLite(mediapipe::Packet* packet, mediapipe::Status** status_out) {
  TRY {
    *status_out = new mediapipe::Status { packet->ValidateAsProtoMessageLite() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__Timestamp(mediapipe::Packet* packet, mediapipe::Timestamp** timestamp_out) {
  TRY {
    *timestamp_out = new mediapipe::Timestamp { packet->Timestamp() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__DebugString(mediapipe::Packet* packet, const char** str_out) {
  TRY {
    *str_out = strcpy_to_heap(packet->DebugString());
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__RegisteredTypeName(mediapipe::Packet* packet, const char** str_out) {
  TRY {
    *str_out = strcpy_to_heap(packet->RegisteredTypeName());
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__DebugTypeName(mediapipe::Packet* packet, const char** str_out) {
  TRY {
    *str_out = strcpy_to_heap(packet->DebugTypeName());
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp__MakeBoolPacket__b(bool value, mediapipe::Packet** packet_out) {
  TRY {
    *packet_out = new mediapipe::Packet { mediapipe::MakePacket<bool>(value) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp__MakeBoolPacket_At__b_Rtimestamp(bool value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY {
    *packet_out = new mediapipe::Packet { mediapipe::MakePacket<bool>(value).At(*timestamp) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetBool(mediapipe::Packet* packet, bool* value_out) {
  TRY_ALL {
    *value_out = packet->Get<bool>();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsBool(mediapipe::Packet* packet, mediapipe::Status** status_out) {
  TRY {
    *status_out = new mediapipe::Status { packet->ValidateAsType<bool>() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpPacket* MpMakeFloatPacket(float value) {
  auto packet = mediapipe::MakePacket<float>(value);

  return new MpPacket { std::move(packet) };
}

float MpPacketGetDouble(MpPacket* packet) {
  return packet->impl->Get<float>();
}

MpPacket* MpMakeStringPacketAt(const char* string, int timestamp) {
  auto packet = mediapipe::MakePacket<std::string>(std::string(string)).At(mediapipe::Timestamp(timestamp));

  return new MpPacket { std::move(packet) };
}

const char* MpPacketGetString(MpPacket* packet) {
  auto text = packet->impl->Get<std::string>();

  return strcpy_to_heap(text);
}

/** SidePacket API */
MpReturnCode mp_SidePacket__(SidePacket** side_packet_out) {
  TRY {
    *side_packet_out = new SidePacket();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_SidePacket__delete(SidePacket* side_packet) {
  delete side_packet;
}

MpReturnCode mp_SidePacket__emplace(SidePacket* side_packet, const char* key, mediapipe::Packet* packet) {
  TRY {
    side_packet->emplace(std::string(key), *packet);
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}
