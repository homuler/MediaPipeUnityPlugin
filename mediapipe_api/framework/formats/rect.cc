// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/rect.h"

MpReturnCode mp_Packet__GetRect(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::Rect>(packet, value_out);
}

MpReturnCode mp_Packet__GetRectVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::Rect>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedRect(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::NormalizedRect>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedRectVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::NormalizedRect>(packet, value_out);
}
