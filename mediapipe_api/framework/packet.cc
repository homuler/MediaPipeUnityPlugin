// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/packet.h"

#include <string>
#include <utility>

MpReturnCode mp_Packet__(mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_Packet__delete(mediapipe::Packet* packet) { delete packet; }

MpReturnCode mp_Packet__At__Rt(mediapipe::Packet* packet, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    // not move but copy
    *packet_out = new mediapipe::Packet{packet->At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_Packet__IsEmpty(mediapipe::Packet* packet) { return packet->IsEmpty(); }

MpReturnCode mp_Packet__ValidateAsProtoMessageLite(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsProtoMessageLite()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__Timestamp(mediapipe::Packet* packet, mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{packet->Timestamp()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__DebugString(mediapipe::Packet* packet, const char** str_out) {
  TRY
    *str_out = strcpy_to_heap(packet->DebugString());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__RegisteredTypeName(mediapipe::Packet* packet, const char** str_out) {
  TRY
    *str_out = strcpy_to_heap(packet->RegisteredTypeName());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__DebugTypeName(mediapipe::Packet* packet, const char** str_out) {
  TRY
    *str_out = strcpy_to_heap(packet->DebugTypeName());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

// BoolPacket
MpReturnCode mp__MakeBoolPacket__b(bool value, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<bool>(value)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeBoolPacket_At__b_Rt(bool value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<bool>(value).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetBool(mediapipe::Packet* packet, bool* value_out) {
  TRY_ALL
    *value_out = packet->Get<bool>();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsBool(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<bool>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

// FloatPacket
MpReturnCode mp__MakeFloatPacket__f(float value, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<float>(value)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeFloatPacket_At__f_Rt(float value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<float>(value).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetFloat(mediapipe::Packet* packet, float* value_out) {
  TRY_ALL
    *value_out = packet->Get<float>();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsFloat(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<float>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

// IntPacket
MpReturnCode mp__MakeIntPacket__i(int value, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<int>(value)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeIntPacket_At__i_Rt(int value, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<int>(value).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetInt(mediapipe::Packet* packet, int* value_out) {
  TRY_ALL
    *value_out = packet->Get<int>();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsInt(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<int>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

// FloatArrayPacket
MpReturnCode mp__MakeFloatArrayPacket__Pf_i(float* value, int size, mediapipe::Packet** packet_out) {
  TRY
    float* array = new float[size];
    std::memcpy(array, value, size * sizeof(float));
    *packet_out = new mediapipe::Packet{mediapipe::Adopt(reinterpret_cast<float(*)[]>(array))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeFloatArrayPacket_At__Pf_i_Rt(float* value, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    float* array = new float[size];
    std::memcpy(array, value, size * sizeof(float));
    *packet_out = new mediapipe::Packet{mediapipe::Adopt(reinterpret_cast<float(*)[]>(array)).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetFloatArray_i(mediapipe::Packet* packet, int size, const float** value_out) {
  TRY_ALL
    auto src = packet->Get<float[]>();
    auto dst = new float[size];
    std::memcpy(dst, src, size * sizeof(float));
    *value_out = dst;
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsFloatArray(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<float[]>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

// FloatVectorPacket
MpReturnCode mp__MakeFloatVectorPacket__Pf_i(float* value, int size, mediapipe::Packet** packet_out) { return mp__MakeVectorPacket(value, size, packet_out); }

MpReturnCode mp__MakeFloatVectorPacket_At__Pf_i_Rt(float* value, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  return mp__MakeVectorPacket_At(value, size, timestamp, packet_out);
}

MpReturnCode mp_Packet__GetFloatVector(mediapipe::Packet* packet, mp_api::StructArray<float>* value_out) {
  return mp_Packet__GetStructVector(packet, value_out);
}

MpReturnCode mp_Packet__ValidateAsFloatVector(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<std::vector<float>>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

// StringPacket
MpReturnCode mp__MakeStringPacket__PKc(const char* str, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::string>(std::string(str))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeStringPacket_At__PKc_Rt(const char* str, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::string>(std::string(str)).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeStringPacket__PKc_i(const char* str, int size, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::string>(std::string(str, size))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeStringPacket_At__PKc_i_Rt(const char* str, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::string>(std::string(str, size)).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetString(mediapipe::Packet* packet, const char** value_out) {
  TRY_ALL
    *value_out = strcpy_to_heap(packet->Get<std::string>());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__GetByteString(mediapipe::Packet* packet, const char** value_out, int* size_out) {
  TRY_ALL
    auto& str = packet->Get<std::string>();
    auto length = str.size();
    auto bytes = new char[length];
    memcpy(bytes, str.c_str(), length);

    *value_out = bytes;
    *size_out = length;
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ConsumeString(mediapipe::Packet* packet, absl::Status** status_out, const char** value_out) {
  TRY_ALL
    auto status_or_string = packet->Consume<std::string>();

    *status_out = new absl::Status{status_or_string.status()};
    if (status_or_string.ok()) {
      auto& str = status_or_string.value();
      *value_out = strcpy_to_heap(std::move(*str));
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ConsumeByteString(mediapipe::Packet* packet, absl::Status** status_out, const char** value_out, int* size_out) {
  TRY_ALL
    auto status_or_string = packet->Consume<std::string>();

    *status_out = new absl::Status{status_or_string.status()};
    if (status_or_string.ok()) {
      auto& str = status_or_string.value();
      auto length = str->size();
      auto bytes = new char[length];
      memcpy(bytes, str->c_str(), length);

      *value_out = bytes;
      *size_out = length;
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__ValidateAsString(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<std::string>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

/** PacketMap */
MpReturnCode mp_PacketMap__(PacketMap** packet_map_out) {
  TRY
    *packet_map_out = new PacketMap();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_PacketMap__delete(PacketMap* packet_map) { delete packet_map; }

MpReturnCode mp_PacketMap__emplace__PKc_Rp(PacketMap* packet_map, const char* key, mediapipe::Packet* packet) {
  TRY
    packet_map->emplace(std::string(key), std::move(*packet));
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_PacketMap__find__PKc(PacketMap* packet_map, const char* key, mediapipe::Packet** packet_out) {
  TRY
    auto iter = packet_map->find(std::string(key));

    if (iter == packet_map->end()) {
      *packet_out = nullptr;
    } else {
      // copy
      auto packet = iter->second;
      *packet_out = new mediapipe::Packet{packet};
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_PacketMap__erase__PKc(PacketMap* packet_map, const char* key, int* count_out) {
  TRY
    *count_out = packet_map->erase(std::string(key));
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_PacketMap__clear(PacketMap* packet_map) { packet_map->clear(); }

int mp_PacketMap__size(PacketMap* packet_map) { return packet_map->size(); }
