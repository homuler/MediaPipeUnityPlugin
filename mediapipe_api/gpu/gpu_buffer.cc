// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/gpu/gpu_buffer.h"

#include <utility>

void mp_GpuBuffer__delete(mediapipe::GpuBuffer* gpu_buffer) { delete gpu_buffer; }

#if MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER
#else
MpReturnCode mp_GpuBuffer__PSgtb(SharedGlTextureBuffer* gl_texture_buffer, mediapipe::GpuBuffer** gpu_buffer_out) {
  TRY
    *gpu_buffer_out = new mediapipe::GpuBuffer{*gl_texture_buffer};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
#endif  // MEDIAPIPE_GPU_BUFFER_USE_CV_PIXEL_BUFFER

int mp_GpuBuffer__width(mediapipe::GpuBuffer* gpu_buffer) { return gpu_buffer->width(); }

int mp_GpuBuffer__height(mediapipe::GpuBuffer* gpu_buffer) { return gpu_buffer->height(); }

mediapipe::GpuBufferFormat mp_GpuBuffer__format(mediapipe::GpuBuffer* gpu_buffer) { return gpu_buffer->format(); }

MpReturnCode mp__MakeGpuBufferPacket__Rgb(mediapipe::GpuBuffer* gpu_buffer, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::GpuBuffer>(std::move(*gpu_buffer))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeGpuBufferPacket_At__Rgb_Rts(mediapipe::GpuBuffer* gpu_buffer, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::GpuBuffer>(std::move(*gpu_buffer)).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__ConsumeGpuBuffer(mediapipe::Packet* packet, absl::Status** status_out, mediapipe::GpuBuffer** value_out) {
  return mp_Packet__Consume(packet, status_out, value_out);
}

MpReturnCode mp_Packet__GetGpuBuffer(mediapipe::Packet* packet, const mediapipe::GpuBuffer** value_out) { return mp_Packet__Get(packet, value_out); }

MpReturnCode mp_Packet__ValidateAsGpuBuffer(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::GpuBuffer>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
