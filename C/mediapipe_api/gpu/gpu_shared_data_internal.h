#ifndef C_MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_
#define C_MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_

#include <memory>
#include <utility>
#include "mediapipe/gpu/gpu_shared_data_internal.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/port/statusor.h"

extern "C" {

typedef std::shared_ptr<mediapipe::GpuResources> SharedGpuResources;

MP_CAPI(void) mp_SharedGpuResources__delete(SharedGpuResources* gpu_resources);
MP_CAPI(mediapipe::GpuResources*) mp_SharedGpuResources__get(SharedGpuResources* gpu_resources);
MP_CAPI(void) mp_SharedGpuResources__reset(SharedGpuResources* gpu_resources);

MP_CAPI(MpReturnCode) mp_GpuResources_Create(mediapipe::StatusOr<SharedGpuResources>** status_or_gpu_resources_out);
MP_CAPI(MpReturnCode) mp_GpuResources_Create__Pv(mediapipe::PlatformGlContext external_context,
                                                 mediapipe::StatusOr<SharedGpuResources>** status_or_gpu_resources_out);

#if __APPLE__
MP_CAPI(MPPGraphGPUData*) mp_GpuResources__ios_gpu_data(mediapipe::GpuResources* gpu_resources);
#endif  // __APPLE__

MP_CAPI(void) mp_StatusOrGpuResources__delete(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources);
MP_CAPI(bool) mp_StatusOrGpuResources__ok(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources);
MP_CAPI(MpReturnCode) mp_StatusOrGpuResources__status(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources,
                                                      mediapipe::Status** status_out);
MP_CAPI(MpReturnCode) mp_StatusOrGpuResources__ValueOrDie(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources,
                                                          SharedGpuResources** value_out);
MP_CAPI(MpReturnCode) mp_StatusOrGpuResources__ConsumeValueOrDie(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources,
                                                                 SharedGpuResources** value_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GPU_SHARED_DATA_INTERNAL_H_
