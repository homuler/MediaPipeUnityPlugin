// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GPU_BUFFER_H_
#define MEDIAPIPE_API_GPU_GPU_BUFFER_H_

#include <memory>

#include "mediapipe/gpu/gpu_buffer.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/absl/status.h"
#include "mediapipe_api/external/absl/statusor.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe_api/gpu/gl_texture_buffer.h"

extern "C" {

typedef absl::StatusOr<mediapipe::GpuBuffer> StatusOrGpuBuffer;

MP_CAPI(void) mp_GpuBuffer__delete(mediapipe::GpuBuffer* gpu_buffer);

#if MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER
// TODO
#else
MP_CAPI(MpReturnCode) mp_GpuBuffer__PSgtb(SharedGlTextureBuffer* gl_texture_buffer, mediapipe::GpuBuffer** gpu_buffer_out);
MP_CAPI(const SharedGlTextureBuffer&) mp_GpuBuffer__GetGlTextureBufferSharedPtr(mediapipe::GpuBuffer* gpu_buffer);
#endif  // MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER

MP_CAPI(int) mp_GpuBuffer__width(mediapipe::GpuBuffer* gpu_buffer);
MP_CAPI(int) mp_GpuBuffer__height(mediapipe::GpuBuffer* gpu_buffer);
MP_CAPI(mediapipe::GpuBufferFormat) mp_GpuBuffer__format(mediapipe::GpuBuffer* gpu_buffer);

MP_CAPI(void) mp_StatusOrGpuBuffer__delete(StatusOrGpuBuffer* status_or_gpu_buffer);
MP_CAPI(bool) mp_StatusOrGpuBuffer__ok(StatusOrGpuBuffer* status_or_gpu_buffer);
MP_CAPI(MpReturnCode) mp_StatusOrGpuBuffer__status(StatusOrGpuBuffer* status_or_gpu_buffer, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_StatusOrGpuBuffer__value(StatusOrGpuBuffer* status_or_gpu_buffer, mediapipe::GpuBuffer** value_out);

MP_CAPI(MpReturnCode) mp__MakeGpuBufferPacket__Rgb(mediapipe::GpuBuffer* gpu_buffer, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeGpuBufferPacket_At__Rgb_Rts(mediapipe::GpuBuffer* gpu_buffer, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__ConsumeGpuBuffer(mediapipe::Packet* packet, StatusOrGpuBuffer** status_or_value_out);
MP_CAPI(MpReturnCode) mp_Packet__GetGpuBuffer(mediapipe::Packet* packet, const mediapipe::GpuBuffer** value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsGpuBuffer(mediapipe::Packet* packet, absl::Status** status_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GPU_BUFFER_H_
