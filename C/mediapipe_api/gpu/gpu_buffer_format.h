#ifndef C_MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_
#define C_MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_

#include "mediapipe/gpu/gpu_buffer_format.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(mediapipe::ImageFormat::Format) mp__ImageFormatForGpuBufferFormat__ui(mediapipe::GpuBufferFormat format);
MP_CAPI(mediapipe::GpuBufferFormat) mp__GpuBufferFormatForImageFormat__ui(mediapipe::ImageFormat::Format format);
MP_CAPI(MpReturnCode) mp__GlTextureInfoForGpuBufferFormat__ui_i_ui(mediapipe::GpuBufferFormat format,
                                                                  int plane,
                                                                  mediapipe::GlVersion gl_version,
                                                                  mediapipe::GlTextureInfo** gl_texture_info_out);
MP_CAPI(void) mp_GlTextureInfo__delete(mediapipe::GlTextureInfo* gl_texture_info);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GPU_BUFFER_FORMAT_H_
