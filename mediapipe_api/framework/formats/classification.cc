// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/classification.h"

MpReturnCode mp_Packet__GetClassificationList(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::ClassificationList>(packet, value_out);
}

MpReturnCode mp_Packet__GetClassificationListVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::ClassificationList>(packet, value_out);
}
