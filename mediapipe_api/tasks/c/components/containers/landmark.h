// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_LANDMARK_H_
#define MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_LANDMARK_H_

#include "mediapipe/framework/packet.h"
#include "mediapipe/tasks/c/components/containers/landmark.h"
#include "mediapipe/tasks/c/components/containers/landmark_converter.h"
#include "mediapipe/tasks/cc/components/containers/landmark.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetNormalizedLandmarksVector(mediapipe::Packet* packet, mp_api::StructArray<NormalizedLandmarks>* value_out);
MP_CAPI(void) mp_api_NormalizedLandmarksArray__delete(mp_api::StructArray<NormalizedLandmarks> data);
MP_CAPI(MpReturnCode) mp_Packet__GetLandmarksVector(mediapipe::Packet* packet, mp_api::StructArray<Landmarks>* value_out);
MP_CAPI(void) mp_api_LandmarksArray__delete(mp_api::StructArray<Landmarks> data);

}  // extern "C"

#endif  // MEDIAPIPE_API_TASKS_C_COMPONENTS_CONTAINERS_LANDMARK_H_
