#include "mediapipe_api/gpu/gl_texture_buffer.h"

void mp_SharedGlTextureBuffer__delete(SharedGlTextureBuffer* gl_texture_buffer) {
  delete gl_texture_buffer;
}

mediapipe::GlTextureBuffer* mp_SharedGlTextureBuffer__get(SharedGlTextureBuffer* gl_texture_buffer) {
  return gl_texture_buffer->get();
}

void mp_SharedGlTextureBuffer__reset(SharedGlTextureBuffer* gl_texture_buffer) {
  gl_texture_buffer->reset();
}

MpReturnCode mp_SharedGlTextureBuffer__ui_ui_i_i_ui_PF_PSgc(GLenum target,
                                                            GLuint name,
                                                            int width,
                                                            int height,
                                                            mediapipe::GpuBufferFormat format,
                                                            GlTextureBufferDeletionCallback* deletion_callback,
                                                            std::shared_ptr<mediapipe::GlContext>* producer_context,
                                                            SharedGlTextureBuffer** gl_texture_buffer_out) {
  TRY {
    auto callback = [name, deletion_callback](mediapipe::GlSyncToken token) -> void {
      deletion_callback(name, new mediapipe::GlSyncToken { token });
    };
    *gl_texture_buffer_out = new SharedGlTextureBuffer {
      new mediapipe::GlTextureBuffer {
        GL_TEXTURE_2D,
        name,
        width,
        height,
        format,
        callback,
        *producer_context,
      }
    };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}
