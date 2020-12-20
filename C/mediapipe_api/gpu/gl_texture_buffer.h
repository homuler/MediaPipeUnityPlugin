#ifndef C_MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_
#define C_MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_

#include <memory>
#include <utility>
#include "mediapipe/gpu/gl_texture_buffer.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/gpu/gl_context.h"

extern "C" {

typedef std::shared_ptr<mediapipe::GlTextureBuffer> SharedGlTextureBuffer;
typedef void GlTextureBufferDeletionCallback(GLuint64 name, std::shared_ptr<mediapipe::GlSyncPoint>* sync_token);

MP_CAPI(void) mp_SharedGlTextureBuffer__delete(SharedGlTextureBuffer* gl_texture_buffer);
MP_CAPI(mediapipe::GlTextureBuffer*) mp_SharedGlTextureBuffer__get(SharedGlTextureBuffer* gl_texture_buffer);
MP_CAPI(void) mp_SharedGlTextureBuffer__reset(SharedGlTextureBuffer* gl_texture_buffer);

MP_CAPI(MpReturnCode) mp_SharedGlTextureBuffer__ui_ui_i_i_ui_PF_PSgc(GLenum target,
                                                                     GLuint name,
                                                                     int width,
                                                                     int height,
                                                                     mediapipe::GpuBufferFormat format,
                                                                     GlTextureBufferDeletionCallback* deletion_callback,
                                                                     std::shared_ptr<mediapipe::GlContext>* producer_context,
                                                                     SharedGlTextureBuffer** gl_texture_buffer_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_
