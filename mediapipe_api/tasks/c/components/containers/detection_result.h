// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_DETECTION_H_
#define MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_DETECTION_H_

#include "mediapipe/framework/packet.h"
#include "mediapipe/tasks/c/components/containers/detection_result.h"
#include "mediapipe/tasks/c/components/containers/detection_result_converter.h"
#include "mediapipe/tasks/cc/components/containers/detection_result.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetDetectionResult(mediapipe::Packet* packet, DetectionResult* value_out);
MP_CAPI(void) mp_tasks_c_components_containers_CppCloseDetectionResult(DetectionResult data);

}  // extern "C"

#endif  // MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_DETECTION_H_
