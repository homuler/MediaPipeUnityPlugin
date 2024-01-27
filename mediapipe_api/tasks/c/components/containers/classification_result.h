// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_CLASSIFICATION_RESULT_H_
#define MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_CLASSIFICATION_RESULT_H_

#include "mediapipe/framework/packet.h"
#include "mediapipe/tasks/c/components/containers/classification_result.h"
#include "mediapipe/tasks/c/components/containers/classification_result_converter.h"
#include "mediapipe/tasks/cc/components/containers/classification_result.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetClassificationResult(mediapipe::Packet* packet, ClassificationResult* value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetClassificationsVector(mediapipe::Packet* packet, ClassificationResult* value_out);
MP_CAPI(void) mp_tasks_c_components_containers_CppCloseClassificationResult(ClassificationResult data);

MP_CAPI(MpReturnCode) mp_Packet__GetClassificationResultVector(mediapipe::Packet* packet, mp_api::StructArray<ClassificationResult>* value_out);
MP_CAPI(void) mp_api_ClassificationResultArray__delete(mp_api::StructArray<ClassificationResult> array);

}  // extern "C"

#endif  // MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_CLASSIFICATION_RESULT_H_
