#ifndef C_MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_
#define C_MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_

#include <memory>
#include <utility>
#include "mediapipe/gpu/gl_texture_buffer.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/gpu/gl_context.h"

extern "C" {

typedef struct MpGlTextureBuffer {
  std::shared_ptr<mediapipe::GlTextureBuffer> impl;
} MpGlTextureBuffer;

typedef void MpDeletionCallback(MpGlSyncToken* gl_sync_token);

MP_CAPI_EXPORT extern MpGlTextureBuffer* MpGlTextureBufferCreate(GLenum target, GLuint name, int width, int height,
    uint32_t format_code, MpDeletionCallback* deletion_callback, mediapipe::GlContext* producer_context = nullptr);

MP_CAPI_EXPORT extern void MpGlTextureBufferDestroy(MpGlTextureBuffer* gl_texture_buffer);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_
