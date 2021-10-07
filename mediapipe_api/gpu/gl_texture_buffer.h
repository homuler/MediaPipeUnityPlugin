// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_
#define MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_

#include <memory>
#include <utility>

#include "mediapipe/gpu/gl_texture_buffer.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/gpu/gl_context.h"

extern "C" {

typedef std::shared_ptr<mediapipe::GlTextureBuffer> SharedGlTextureBuffer;
typedef void GlTextureBufferDeletionCallback(GLuint name, std::shared_ptr<mediapipe::GlSyncPoint>* sync_token);

MP_CAPI(void) mp_SharedGlTextureBuffer__delete(SharedGlTextureBuffer* gl_texture_buffer);
MP_CAPI(mediapipe::GlTextureBuffer*) mp_SharedGlTextureBuffer__get(SharedGlTextureBuffer* gl_texture_buffer);
MP_CAPI(void) mp_SharedGlTextureBuffer__reset(SharedGlTextureBuffer* gl_texture_buffer);

MP_CAPI(MpReturnCode) mp_SharedGlTextureBuffer__ui_ui_i_i_ui_PF_PSgc(GLenum target, GLuint name, int width, int height, mediapipe::GpuBufferFormat format,
                                                                     GlTextureBufferDeletionCallback* deletion_callback,
                                                                     std::shared_ptr<mediapipe::GlContext>* producer_context,
                                                                     SharedGlTextureBuffer** gl_texture_buffer_out);

MP_CAPI(GLuint) mp_GlTextureBuffer__name(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(GLenum) mp_GlTextureBuffer__target(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(int) mp_GlTextureBuffer__width(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(int) mp_GlTextureBuffer__height(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(mediapipe::GpuBufferFormat) mp_GlTextureBuffer__format(mediapipe::GlTextureBuffer* gl_texture_buffer);

MP_CAPI(MpReturnCode) mp_GlTextureBuffer__WaitUntilComplete(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(MpReturnCode) mp_GlTextureBuffer__WaitOnGpu(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(MpReturnCode) mp_GlTextureBuffer__Reuse(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(MpReturnCode) mp_GlTextureBuffer__Updated__Pgst(mediapipe::GlTextureBuffer* gl_texture_buffer, mediapipe::GlSyncToken* prod_token);
MP_CAPI(MpReturnCode) mp_GlTextureBuffer__DidRead__Pgst(mediapipe::GlTextureBuffer* gl_texture_buffer, mediapipe::GlSyncToken* cons_token);
MP_CAPI(MpReturnCode) mp_GlTextureBuffer__WaitForConsumers(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(MpReturnCode) mp_GlTextureBuffer__WaitForConsumersOnGpu(mediapipe::GlTextureBuffer* gl_texture_buffer);
MP_CAPI(const SharedGlContext&) mp_GlTextureBuffer__GetProducerContext(mediapipe::GlTextureBuffer* gl_texture_buffer);

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GL_TEXTURE_BUFFER_H_
