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

void MpGpuBufferDestroy(mediapipe::GpuBuffer* gpu_buffer) {
  delete gpu_buffer;
}

uint32_t MpGpuBufferFormat(mediapipe::GpuBuffer* gpu_buffer) {
  return static_cast<uint32_t>(gpu_buffer->format());
}

int MpGpuBufferWidth(mediapipe::GpuBuffer* gpu_buffer) {
  return gpu_buffer->width();
}

int MpGpuBufferHeight(mediapipe::GpuBuffer* gpu_buffer) {
  return gpu_buffer->height();
}

mediapipe::GpuBuffer* MpGlTextureGetGpuBufferFrame(mediapipe::GlTexture* gl_texture) {
  return gl_texture->GetFrame<mediapipe::GpuBuffer>().release();
}

MpPacket* MpMakeGpuBufferPacketAt(mediapipe::GpuBuffer* gpu_buffer, int timestamp) {
  auto packet = mediapipe::Adopt(gpu_buffer).At(mediapipe::Timestamp(timestamp));

  return new MpPacket { std::move(packet) };
}

mediapipe::GpuBuffer* MpPacketGetGpuBuffer(MpPacket* packet) {
  auto holder = static_cast<const UnsafePacketHolder<mediapipe::GpuBuffer>*>(mediapipe::packet_internal::GetHolder(*packet->impl));

  return holder->Get();
}

MpStatusOrGpuBuffer* MpPacketConsumeGpuBuffer(MpPacket* packet) {
  auto status_or_gpu_buffer = packet->impl->Consume<mediapipe::GpuBuffer>();

  return new MpStatusOrGpuBuffer { std::move(status_or_gpu_buffer) };
}

void MpStatusOrGpuBufferDestroy(MpStatusOrGpuBuffer* status_or_gpu_buffer) {
  delete status_or_gpu_buffer;
}

MpStatus* MpStatusOrGpuBufferStatus(MpStatusOrGpuBuffer* status_or_gpu_buffer) {
  return new MpStatus { *status_or_gpu_buffer->status };
}

mediapipe::GpuBuffer* MpStatusOrGpuBufferConsumeValue(MpStatusOrGpuBuffer* gpu_buffer) {
  return gpu_buffer->value.release();
}
