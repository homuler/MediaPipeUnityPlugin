// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_
#define MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_

#include <memory>
#include <utility>

#include "mediapipe/gpu/gpu_shared_data_internal.h"
#include "mediapipe_api/common.h"

extern "C" {

typedef std::shared_ptr<mediapipe::GpuResources> SharedGpuResources;

MP_CAPI(void) mp_SharedGpuResources__delete(SharedGpuResources* gpu_resources);
MP_CAPI(mediapipe::GpuResources*) mp_SharedGpuResources__get(SharedGpuResources* gpu_resources);
MP_CAPI(void) mp_SharedGpuResources__reset(SharedGpuResources* gpu_resources);

MP_CAPI(MpReturnCode) mp_GpuResources_Create(absl::Status** status_out, SharedGpuResources** gpu_resources_out);
MP_CAPI(MpReturnCode) mp_GpuResources_Create__Pv(mediapipe::PlatformGlContext external_context,
                                                 absl::Status** status_out, SharedGpuResources** gpu_resources_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_
