#include "mediapipe_api/gpu/gl_texture_buffer.h"

MpGlTextureBuffer* MpGlTextureBufferCreate(GLenum target, GLuint name, int width, int height,
    uint32_t format_code, MpDeletionCallback* deletion_callback, mediapipe::GlContext* producer_context /* =nullptr */) {
  auto callback = [&deletion_callback](mediapipe::GlSyncToken token) -> void {
    auto mpToken = new MpGlSyncToken { token };
    deletion_callback(mpToken);
  };
  auto buffer = std::make_shared<mediapipe::GlTextureBuffer>(
    GL_TEXTURE_2D,
    name,
    width,
    height,
    static_cast<mediapipe::GpuBufferFormat>(format_code),
    callback,
    std::shared_ptr<mediapipe::GlContext>(producer_context));

  return new MpGlTextureBuffer { std::move(buffer) };
}

void MpGlTextureBufferDestroy(MpGlTextureBuffer* gl_texture_buffer) {
  delete gl_texture_buffer;
}
