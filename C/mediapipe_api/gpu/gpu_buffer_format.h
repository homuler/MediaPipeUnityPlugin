#ifndef C_MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_
#define C_MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_

#include "mediapipe/gpu/gpu_buffer_format.h"
#include "mediapipe_api/common.h"

extern "C" {

/** GpuBufferFormat API */
MP_CAPI_EXPORT extern int MpImageFormatForGpuBufferFormat(uint32_t gpu_format_code);
MP_CAPI_EXPORT extern mediapipe::GlTextureInfo* MpGlTextureInfoForGpuBufferFormat(uint32_t gpu_format_code, int plane);

/** GlTextureInfo API */
MP_CAPI_EXPORT extern void MpGlTextureInfoDestroy(mediapipe::GlTextureInfo* gl_texture_info);
MP_CAPI_EXPORT extern GLint MpGlTextureInfoGlInternalFormat(mediapipe::GlTextureInfo* gl_texture_info);
MP_CAPI_EXPORT extern GLenum MpGlTextureInfoGlFormat(mediapipe::GlTextureInfo* gl_texture_info);
MP_CAPI_EXPORT extern GLenum MpGlTextureInfoGlType(mediapipe::GlTextureInfo* gl_texture_info);
MP_CAPI_EXPORT extern int MpGlTextureInfoDownscale(mediapipe::GlTextureInfo* gl_texture_info);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_
