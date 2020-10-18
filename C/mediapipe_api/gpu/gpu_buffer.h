#ifndef C_MEDIAPIPE_API_GPU_GPU_BUFFER_H_
#define C_MEDIAPIPE_API_GPU_GPU_BUFFER_H_

#include <memory>
#include "mediapipe/gpu/gpu_buffer.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/framework/port/status.h"
#include "mediapipe_api/framework/port/statusor.h"
#include "mediapipe_api/gpu/gl_texture_buffer.h"

extern "C" {

typedef MpStatusOrValue<std::unique_ptr<mediapipe::GpuBuffer>> MpStatusOrGpuBuffer;

MP_CAPI_EXPORT extern mediapipe::GpuBuffer* MpGpuBufferCreate(MpGlTextureBuffer* gl_texture_buffer);
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

#endif  // C_MEDIAPIPE_API_GPU_GPU_BUFFER_H_
