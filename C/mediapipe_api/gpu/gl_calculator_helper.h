#ifndef C_MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_
#define C_MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_

#include <memory>
#include "mediapipe/gpu/gl_calculator_helper.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/framework/port/status.h"
#include "mediapipe_api/framework/port/statusor.h"

extern "C" {

typedef struct MpGlCalculatorHelper {
  std::unique_ptr<mediapipe::GlCalculatorHelper> impl;

  MpGlCalculatorHelper() : impl { std::make_unique<mediapipe::GlCalculatorHelper>() } {}
} MpGlCalculatorHelper;

typedef MpStatusOrValue<std::unique_ptr<mediapipe::GpuBuffer>> MpStatusOrGpuBuffer;
typedef MpStatus* MpGlStatusFunction();

/** GlCalculatorHelper API */
MP_CAPI_EXPORT extern MpGlCalculatorHelper* MpGlCalculatorHelperCreate();
MP_CAPI_EXPORT extern void MpGlCalculatorHelperDestroy(MpGlCalculatorHelper* gpu_helper);
MP_CAPI_EXPORT extern void MpGlCalculatorHelperInitializeForTest(MpGlCalculatorHelper* gpu_helper, mediapipe::GpuResources* gpu_resources);
MP_CAPI_EXPORT extern MpStatus* MpGlCalculatorHelperRunInGlContext(MpGlCalculatorHelper* gpu_helper, MpGlStatusFunction* gl_func);
MP_CAPI_EXPORT extern mediapipe::GlTexture* MpGlCalculatorHelperCreateSourceTextureForImageFrame(
    MpGlCalculatorHelper* gpu_helper, mediapipe::ImageFrame* image_frame);
MP_CAPI_EXPORT extern mediapipe::GlTexture* MpGlCalculatorHelperCreateSourceTextureForGpuBuffer(
    MpGlCalculatorHelper* gpu_helper, mediapipe::GpuBuffer* gpu_buffer);
MP_CAPI_EXPORT extern void MpGlCalculatorHelperBindFramebuffer(MpGlCalculatorHelper* gpu_helper, mediapipe::GlTexture* gl_texture);

/** GlTexture API */
MP_CAPI_EXPORT extern void MpGlTextureDestroy(mediapipe::GlTexture* gl_texture);
MP_CAPI_EXPORT extern int MpGlTextureWidth(mediapipe::GlTexture* gl_texture);
MP_CAPI_EXPORT extern int MpGlTextureHeight(mediapipe::GlTexture* gl_texture);
MP_CAPI_EXPORT extern void MpGlTextureRelease(mediapipe::GlTexture* gl_texture);
MP_CAPI_EXPORT extern mediapipe::GpuBuffer* MpGlTextureGetGpuBufferFrame(mediapipe::GlTexture* gl_texture);

/** GpuBuffer API */
MP_CAPI_EXPORT extern void MpGpuBufferDestroy(mediapipe::GpuBuffer* gpu_buffer);
MP_CAPI_EXPORT extern uint32_t MpGpuBufferFormat(mediapipe::GpuBuffer* gpu_buffer);
MP_CAPI_EXPORT extern int MpGpuBufferWidth(mediapipe::GpuBuffer* gpu_buffer);
MP_CAPI_EXPORT extern int MpGpuBufferHeight(mediapipe::GpuBuffer* gpu_buffer);

MP_CAPI_EXPORT extern MpPacket* MpMakeGpuBufferPacketAt(mediapipe::GpuBuffer* gpu_buffer, int timestamp);
MP_CAPI_EXPORT extern mediapipe::GpuBuffer* MpPacketGetGpuBuffer(MpPacket* packet);
MP_CAPI_EXPORT extern MpStatusOrGpuBuffer* MpPacketConsumeGpuBuffer(MpPacket* packet);
MP_CAPI_EXPORT extern void MpStatusOrGpuBufferDestroy(MpStatusOrGpuBuffer* status_or_gpu_buffer);
MP_CAPI_EXPORT extern MpStatus* MpStatusOrGpuBufferStatus(MpStatusOrGpuBuffer* status_or_gpu_buffer);
MP_CAPI_EXPORT extern mediapipe::GpuBuffer* MpStatusOrGpuBufferConsumeValue(MpStatusOrGpuBuffer* status_or_gpu_buffer);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_
