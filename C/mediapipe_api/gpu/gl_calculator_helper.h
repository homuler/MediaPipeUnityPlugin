#ifndef C_MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_
#define C_MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_

#include <memory>
#include "mediapipe/gpu/gl_calculator_helper.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/port/status.h"

extern "C" {

typedef struct MpGlCalculatorHelper {
  std::unique_ptr<mediapipe::GlCalculatorHelper> impl;

  MpGlCalculatorHelper() : impl { std::make_unique<mediapipe::GlCalculatorHelper>() } {}
} MpGlCalculatorHelper;

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

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_CALCULATOR_HELPER_H_
