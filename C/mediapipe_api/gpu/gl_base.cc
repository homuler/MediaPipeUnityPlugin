#include "mediapipe_api/gpu/gl_base.h"

void GlFlush() {
  glFlush();
}

void GlReadPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum gl_format, GLenum gl_type, uint8_t* pixels) {
  glReadPixels(x, y, width, height, gl_format, gl_type, pixels);
}
