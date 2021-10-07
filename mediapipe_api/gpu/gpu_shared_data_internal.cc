// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/gpu/gpu_shared_data_internal.h"

void mp_SharedGpuResources__delete(SharedGpuResources* gpu_resources) { delete gpu_resources; }

mediapipe::GpuResources* mp_SharedGpuResources__get(SharedGpuResources* gpu_resources) { return gpu_resources->get(); }

void mp_SharedGpuResources__reset(SharedGpuResources* gpu_resources) { gpu_resources->reset(); }

MpReturnCode mp_GpuResources_Create(absl::StatusOr<SharedGpuResources>** status_or_gpu_resources_out) {
  TRY
    *status_or_gpu_resources_out = new absl::StatusOr<SharedGpuResources>{mediapipe::GpuResources::Create()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MP_CAPI(MpReturnCode) mp_GpuResources_Create__Pv(mediapipe::PlatformGlContext external_context,
                                                 absl::StatusOr<SharedGpuResources>** status_or_gpu_resources_out) {
  TRY
    *status_or_gpu_resources_out = new absl::StatusOr<SharedGpuResources>{mediapipe::GpuResources::Create(external_context)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

#if __APPLE__
MPPGraphGPUData* mp_GpuResources__ios_gpu_data(mediapipe::GpuResources* gpu_resources) { return gpu_resources->ios_gpu_data(); }
#endif  // __APPLE__

void mp_StatusOrGpuResources__delete(absl::StatusOr<SharedGpuResources>* status_or_gpu_resources) { delete status_or_gpu_resources; }

bool mp_StatusOrGpuResources__ok(absl::StatusOr<SharedGpuResources>* status_or_gpu_resources) { return absl_StatusOr__ok(status_or_gpu_resources); }

MpReturnCode mp_StatusOrGpuResources__status(absl::StatusOr<SharedGpuResources>* status_or_gpu_resources, absl::Status** status_out) {
  return absl_StatusOr__status(status_or_gpu_resources, status_out);
}

MpReturnCode mp_StatusOrGpuResources__value(absl::StatusOr<SharedGpuResources>* status_or_gpu_resources, SharedGpuResources** value_out) {
  return absl_StatusOr__value(status_or_gpu_resources, value_out);
}
