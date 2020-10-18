#include <utility>
#include "mediapipe_api/gpu/gl_calculator_helper.h"

MpGlCalculatorHelper* MpGlCalculatorHelperCreate() {
  return new MpGlCalculatorHelper();
}

void MpGlCalculatorHelperDestroy(MpGlCalculatorHelper* gpu_helper) {
  delete gpu_helper;
}

void MpGlCalculatorHelperInitializeForTest(MpGlCalculatorHelper* gpu_helper, mediapipe::GpuResources* gpu_resources) {
  gpu_helper->impl->InitializeForTest(gpu_resources);
}

MpStatus* MpGlCalculatorHelperRunInGlContext(MpGlCalculatorHelper* gpu_helper, MpGlStatusFunction* gl_func) {
  auto status = gpu_helper->impl->RunInGlContext([&gl_func]() -> ::mediapipe::Status {
    auto mp_status { (*gl_func)() };

    return *mp_status->impl.get();
  });

  return new MpStatus { std::move(status) };
}

mediapipe::GlTexture* MpGlCalculatorHelperCreateSourceTextureForImageFrame(
    MpGlCalculatorHelper* gpu_helper, mediapipe::ImageFrame* image_frame) {
  return new mediapipe::GlTexture { gpu_helper->impl->CreateSourceTexture(*image_frame) };
}

mediapipe::GlTexture* MpGlCalculatorHelperCreateSourceTextureForGpuBuffer(
    MpGlCalculatorHelper* gpu_helper, mediapipe::GpuBuffer* gpu_buffer) {
  return new mediapipe::GlTexture { gpu_helper->impl->CreateSourceTexture(*gpu_buffer) };
}

void MpGlCalculatorHelperBindFramebuffer(MpGlCalculatorHelper* gpu_helper, mediapipe::GlTexture* gl_texture) {
  gpu_helper->impl->BindFramebuffer(*gl_texture);
}

void MpGlTextureDestroy(mediapipe::GlTexture* gl_texture) {
  delete gl_texture;
}

int MpGlTextureWidth(mediapipe::GlTexture* gl_texture) {
  return gl_texture->width();
}

int MpGlTextureHeight(mediapipe::GlTexture* gl_texture) {
  return gl_texture->height();
}

void MpGlTextureRelease(mediapipe::GlTexture* gl_texture) {
  gl_texture->Release();
}

mediapipe::GpuBuffer* MpGlTextureGetGpuBufferFrame(mediapipe::GlTexture* gl_texture) {
  return gl_texture->GetFrame<mediapipe::GpuBuffer>().release();
}
