// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/landmark.h"

MpReturnCode mp_Packet__GetLandmarkList(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::LandmarkList>(packet, value_out);
}

MpReturnCode mp_Packet__GetLandmarkListVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::LandmarkList>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedLandmarkList(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::NormalizedLandmarkList>(packet, value_out);
}

MpReturnCode mp_Packet__GetNormalizedLandmarkListVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::NormalizedLandmarkList>(packet, value_out);
}
