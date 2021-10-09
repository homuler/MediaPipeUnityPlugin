// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/gpu/gpu_buffer_format.h"

mediapipe::ImageFormat::Format mp__ImageFormatForGpuBufferFormat__ui(mediapipe::GpuBufferFormat format) {
  return mediapipe::ImageFormatForGpuBufferFormat(format);
}

mediapipe::GpuBufferFormat mp__GpuBufferFormatForImageFormat__ui(mediapipe::ImageFormat::Format format) {
  return mediapipe::GpuBufferFormatForImageFormat(format);
}

MpReturnCode mp__GlTextureInfoForGpuBufferFormat__ui_i_ui(mediapipe::GpuBufferFormat format, int plane, mediapipe::GlVersion gl_version,
                                                          mediapipe::GlTextureInfo* gl_texture_info_out) {
  TRY
    *gl_texture_info_out = mediapipe::GlTextureInfoForGpuBufferFormat(format, plane, gl_version);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
