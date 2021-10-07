// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_MODULES_OBJECTRON_CALCULATORS_ANNOTATION_DATA_H_
#define MEDIAPIPE_API_MODULES_OBJECTRON_CALCULATORS_ANNOTATION_DATA_H_

#include "mediapipe/modules/objectron/calculators/annotation_data.pb.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetFrameAnnotation(mediapipe::Packet* packet, mp_api::SerializedProto* value_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_MODULES_OBJECTRON_CALCULATORS_ANNOTATION_DATA_H_
