// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/gpu/gl_texture_buffer.h"

void mp_SharedGlTextureBuffer__delete(SharedGlTextureBuffer* gl_texture_buffer) { delete gl_texture_buffer; }

mediapipe::GlTextureBuffer* mp_SharedGlTextureBuffer__get(SharedGlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->get(); }

void mp_SharedGlTextureBuffer__reset(SharedGlTextureBuffer* gl_texture_buffer) { gl_texture_buffer->reset(); }

MpReturnCode mp_SharedGlTextureBuffer__ui_ui_i_i_ui_PF_PSgc(GLenum target, GLuint name, int width, int height, mediapipe::GpuBufferFormat format,
                                                            GlTextureBufferDeletionCallback* deletion_callback,
                                                            std::shared_ptr<mediapipe::GlContext>* producer_context,
                                                            SharedGlTextureBuffer** gl_texture_buffer_out) {
  TRY
    auto callback = [name, deletion_callback](mediapipe::GlSyncToken token) -> void {
      deletion_callback(name, new mediapipe::GlSyncToken{token});
    };
    *gl_texture_buffer_out = new SharedGlTextureBuffer{new mediapipe::GlTextureBuffer{
        GL_TEXTURE_2D,
        name,
        width,
        height,
        format,
        callback,
        *producer_context,
    }};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

GLuint mp_GlTextureBuffer__name(mediapipe::GlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->name(); }

GLenum mp_GlTextureBuffer__target(mediapipe::GlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->target(); }

int mp_GlTextureBuffer__width(mediapipe::GlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->width(); }

int mp_GlTextureBuffer__height(mediapipe::GlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->height(); }

mediapipe::GpuBufferFormat mp_GlTextureBuffer__format(mediapipe::GlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->format(); }

MpReturnCode mp_GlTextureBuffer__WaitUntilComplete(mediapipe::GlTextureBuffer* gl_texture_buffer) {
  TRY_ALL
    gl_texture_buffer->WaitUntilComplete();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_GlTextureBuffer__WaitOnGpu(mediapipe::GlTextureBuffer* gl_texture_buffer) {
  TRY_ALL
    gl_texture_buffer->WaitOnGpu();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_GlTextureBuffer__Reuse(mediapipe::GlTextureBuffer* gl_texture_buffer) {
  TRY_ALL
    gl_texture_buffer->Reuse();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_GlTextureBuffer__Updated__Pgst(mediapipe::GlTextureBuffer* gl_texture_buffer, mediapipe::GlSyncToken* prod_token) {
  TRY_ALL
    gl_texture_buffer->Updated(*prod_token);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_GlTextureBuffer__DidRead__Pgst(mediapipe::GlTextureBuffer* gl_texture_buffer, mediapipe::GlSyncToken* cons_token) {
  TRY_ALL
    gl_texture_buffer->DidRead(*cons_token);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_GlTextureBuffer__WaitForConsumers(mediapipe::GlTextureBuffer* gl_texture_buffer) {
  TRY_ALL
    gl_texture_buffer->WaitForConsumers();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_GlTextureBuffer__WaitForConsumersOnGpu(mediapipe::GlTextureBuffer* gl_texture_buffer) {
  TRY_ALL
    gl_texture_buffer->WaitForConsumersOnGpu();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

const SharedGlContext& mp_GlTextureBuffer__GetProducerContext(mediapipe::GlTextureBuffer* gl_texture_buffer) { return gl_texture_buffer->GetProducerContext(); }
