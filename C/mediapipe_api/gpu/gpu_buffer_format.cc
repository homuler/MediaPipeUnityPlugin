#include "mediapipe_api/gpu/gpu_buffer_format.h"

void MpGlTextureInfoDestroy(mediapipe::GlTextureInfo* gl_texture_info) {
  delete gl_texture_info;
}

int MpImageFormatForGpuBufferFormat(uint32_t gpu_format_code) {
  auto gpu_format { static_cast<mediapipe::GpuBufferFormat>(gpu_format_code) };

  return mediapipe::ImageFormatForGpuBufferFormat(gpu_format);
}

mediapipe::GlTextureInfo* MpGlTextureInfoForGpuBufferFormat(uint32_t gpu_format_code, int plane) {
  auto gpu_format { static_cast<mediapipe::GpuBufferFormat>(gpu_format_code) };

  return new mediapipe::GlTextureInfo { mediapipe::GlTextureInfoForGpuBufferFormat(gpu_format, plane) };
}

GLint MpGlTextureInfoGlInternalFormat(mediapipe::GlTextureInfo* gl_texture_info) {
  return gl_texture_info->gl_internal_format;
}

GLenum MpGlTextureInfoGlFormat(mediapipe::GlTextureInfo* gl_texture_info) {
  return gl_texture_info->gl_format;
}

GLenum MpGlTextureInfoGlType(mediapipe::GlTextureInfo* gl_texture_info) {
  return gl_texture_info->gl_type;
}

int MpGlTextureInfoDownscale(mediapipe::GlTextureInfo* gl_texture_info) {
  return gl_texture_info->downscale;
}
