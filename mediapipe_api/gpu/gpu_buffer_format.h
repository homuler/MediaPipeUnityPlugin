// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_
#define MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_

#include "mediapipe/gpu/gpu_buffer_format.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(mediapipe::ImageFormat::Format) mp__ImageFormatForGpuBufferFormat__ui(mediapipe::GpuBufferFormat format);
MP_CAPI(mediapipe::GpuBufferFormat) mp__GpuBufferFormatForImageFormat__ui(mediapipe::ImageFormat::Format format);
MP_CAPI(MpReturnCode) mp__GlTextureInfoForGpuBufferFormat__ui_i_ui(mediapipe::GpuBufferFormat format, int plane, mediapipe::GlVersion gl_version,
                                                                   mediapipe::GlTextureInfo* gl_texture_info_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_
