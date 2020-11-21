#include "mediapipe_api/gpu/gpu_buffer_format.h"

mediapipe::ImageFormat::Format mp__ImageFormatForGpuBufferFormat__ui(mediapipe::GpuBufferFormat format) {
  return mediapipe::ImageFormatForGpuBufferFormat(format);
}

mediapipe::GpuBufferFormat mp__GpuBufferFormatForImageFormat__ui(mediapipe::ImageFormat::Format format) {
  return mediapipe::GpuBufferFormatForImageFormat(format);
}

MpReturnCode mp__GlTextureInfoForGpuBufferFormat__ui_i_ui(mediapipe::GpuBufferFormat format,
                                                         int plane,
                                                         mediapipe::GlVersion gl_version,
                                                         mediapipe::GlTextureInfo** gl_texture_info_out) {
  TRY {
    *gl_texture_info_out = new mediapipe::GlTextureInfo { mediapipe::GlTextureInfoForGpuBufferFormat(format, plane, gl_version) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_GlTextureInfo__delete(mediapipe::GlTextureInfo* gl_texture_info) {
  delete gl_texture_info;
}
