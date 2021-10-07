// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/modules/objectron/calculators/annotation_data.h"

MpReturnCode mp_Packet__GetFrameAnnotation(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::FrameAnnotation>(packet, value_out);
}
