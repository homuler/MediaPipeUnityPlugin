#ifndef C_MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_
#define C_MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_

#include <memory>
#include <utility>
#include "mediapipe/gpu/gpu_shared_data_internal.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/port/status.h"
#include "mediapipe_api/framework/port/statusor.h"

extern "C" {

typedef struct MpGpuResources {
  std::shared_ptr<mediapipe::GpuResources> impl;
} MpGpuResources;
typedef MpStatusOrValue<std::unique_ptr<MpGpuResources>> MpStatusOrGpuResources;

MP_CAPI_EXPORT extern void MpGpuResourcesDestroy(MpGpuResources* gpu_resources);
MP_CAPI_EXPORT extern mediapipe::GpuResources* MpGpuResourcesGet(MpGpuResources* gpu_resources);

MP_CAPI_EXPORT extern MpStatusOrGpuResources* MpGpuResourcesCreate();
MP_CAPI_EXPORT extern void MpStatusOrGpuResourcesDestroy(MpStatusOrGpuResources* status_or_gpu_resources);
MP_CAPI_EXPORT extern MpStatus* MpStatusOrGpuResourcesStatus(MpStatusOrGpuResources* status_or_gpu_resources);
MP_CAPI_EXPORT extern MpGpuResources* MpStatusOrGpuResourcesConsumeValue(MpStatusOrGpuResources* status_or_gpu_resources);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_
