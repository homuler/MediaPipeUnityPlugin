// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/graphs/object_detection_3d/calculators/model_matrix.h"

MpReturnCode mp_Packet__GetTimedModelMatrixProtoList(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::TimedModelMatrixProtoList>(packet, value_out);
}
