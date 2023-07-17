// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/gpu/gpu_shared_data_internal.h"

void mp_SharedGpuResources__delete(SharedGpuResources* gpu_resources) { delete gpu_resources; }

mediapipe::GpuResources* mp_SharedGpuResources__get(SharedGpuResources* gpu_resources) { return gpu_resources->get(); }

void mp_SharedGpuResources__reset(SharedGpuResources* gpu_resources) { gpu_resources->reset(); }

MpReturnCode mp_GpuResources_Create(absl::Status** status_out, SharedGpuResources** gpu_resources_out) {
  TRY
    auto status_or_gpu_resources = mediapipe::GpuResources::Create();
    *status_out = new absl::Status{status_or_gpu_resources.status()};
    if (status_or_gpu_resources.ok()) {
      *gpu_resources_out = new SharedGpuResources{status_or_gpu_resources.value()};
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MP_CAPI(MpReturnCode) mp_GpuResources_Create__Pv(mediapipe::PlatformGlContext external_context,
                                                 absl::Status** status_out, SharedGpuResources** gpu_resources_out) {
  TRY
    auto status_or_gpur_resources = mediapipe::GpuResources::Create(external_context);
    *status_out = new absl::Status{status_or_gpur_resources.status()};
    if (status_or_gpur_resources.ok()) {
      *gpu_resources_out = new SharedGpuResources{status_or_gpur_resources.value()};
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
