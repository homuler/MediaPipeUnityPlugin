// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/rect.h"

MpReturnCode mp__MakeRectPacket__PKc_i(const char* serialized_data, int size, mediapipe::Packet** packet_out) {
  TRY_ALL
    auto rect = ParseFromStringAsProto<mediapipe::Rect>(serialized_data, size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Rect>(rect)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp__MakeRectPacket_At__PKc_i_Rt(const char* serialized_data, int size, mediapipe::Timestamp* timestamp,
                                             mediapipe::Packet** packet_out) {
  TRY_ALL
    auto rect = ParseFromStringAsProto<mediapipe::Rect>(serialized_data, size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Rect>(rect).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__GetRect(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::Rect>(packet, value_out);
}

MpReturnCode mp_Packet__GetRectVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::Rect>(packet, value_out);
}

MpReturnCode mp_Packet__ValidateAsRect(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::Rect>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeNormalizedRectPacket__PKc_i(const char* serialized_data, int size, mediapipe::Packet** packet_out) {
  TRY_ALL
    auto rect = ParseFromStringAsProto<mediapipe::NormalizedRect>(serialized_data, size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::NormalizedRect>(rect)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp__MakeNormalizedRectPacket_At__PKc_i_Rt(const char* serialized_data, int size, mediapipe::Timestamp* timestamp,
                                                       mediapipe::Packet** packet_out) {
  TRY_ALL
    auto rect = ParseFromStringAsProto<mediapipe::NormalizedRect>(serialized_data, size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::NormalizedRect>(rect).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__GetNormalizedRect(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::NormalizedRect>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedRectVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::NormalizedRect>(packet, value_out);
}

MpReturnCode mp_Packet__ValidateAsNormalizedRect(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::NormalizedRect>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
