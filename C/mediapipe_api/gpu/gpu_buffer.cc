#include <utility>
#include "mediapipe_api/gpu/gpu_buffer.h"

mediapipe::GpuBuffer* MpGpuBufferCreate(MpGlTextureBuffer* gl_texture_buffer) {
  return new mediapipe::GpuBuffer { gl_texture_buffer->impl };
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
