// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GL_BASE_H_
#define MEDIAPIPE_API_GPU_GL_BASE_H_

#include "mediapipe/gpu/gl_base.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(void) glFlush();
MP_CAPI(void) glReadPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum gl_format, GLenum gl_type, uint8_t* pixels);

#if HAS_EGL
MP_CAPI(EGLContext) eglGetCurrentContext();
#endif  // HAS_EGL

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GL_BASE_H_
