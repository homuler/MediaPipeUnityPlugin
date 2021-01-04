#include "mediapipe_api/gpu/gpu_shared_data_internal.h"

void mp_SharedGpuResources__delete(SharedGpuResources* gpu_resources) {
  delete gpu_resources;
}

mediapipe::GpuResources* mp_SharedGpuResources__get(SharedGpuResources* gpu_resources) {
  return gpu_resources->get();
}

void mp_SharedGpuResources__reset(SharedGpuResources* gpu_resources) {
  gpu_resources->reset();
}

MpReturnCode mp_GpuResources_Create(mediapipe::StatusOr<SharedGpuResources>** status_or_gpu_resources_out) {
  TRY {
    *status_or_gpu_resources_out = new mediapipe::StatusOr<SharedGpuResources> { mediapipe::GpuResources::Create() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MP_CAPI(MpReturnCode) mp_GpuResources_Create__Pv(mediapipe::PlatformGlContext external_context,
                                                 mediapipe::StatusOr<SharedGpuResources>** status_or_gpu_resources_out) {
  TRY {
    *status_or_gpu_resources_out = new mediapipe::StatusOr<SharedGpuResources> { mediapipe::GpuResources::Create(external_context) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

#if __APPLE__
MPPGraphGPUData* mp_GpuResources__ios_gpu_data(mediapipe::GpuResources* gpu_resources) {
  return gpu_resources->ios_gpu_data();
}
#endif  // __APPLE__

void mp_StatusOrGpuResources__delete(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources) {
  delete status_or_gpu_resources;
}

bool mp_StatusOrGpuResources__ok(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources) {
  return mp_StatusOr__ok(status_or_gpu_resources);
}

MpReturnCode mp_StatusOrGpuResources__status(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources,
                                             mediapipe::Status** status_out) {
  return mp_StatusOr__status(status_or_gpu_resources, status_out);
}

MpReturnCode mp_StatusOrGpuResources__ValueOrDie(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources,
                                                 SharedGpuResources** value_out) {
  return mp_StatusOr__ValueOrDie(status_or_gpu_resources, value_out);
}

MpReturnCode mp_StatusOrGpuResources__ConsumeValueOrDie(mediapipe::StatusOr<SharedGpuResources>* status_or_gpu_resources,
                                                        SharedGpuResources** value_out) {
  return mp_StatusOr__ConsumeValueOrDie(status_or_gpu_resources, value_out);
}
