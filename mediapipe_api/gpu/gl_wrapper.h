// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GL_WRAPPER_H_
#define MEDIAPIPE_API_GPU_GL_WRAPPER_H_

#include "mediapipe/gpu/gl_base.h"

#if defined(__EMSCRIPTEN__)
#include <emscripten.h>
#endif  // __EMSCRIPTEN__

// Wrap functions which are affected by `GL_EXPLICIT_UNIFORM_LOCATION` and `GL_EXPLICIT_UNIFORM_BINDING`,
// that Unity sets to 1 at compile time.

#ifdef __cplusplus
extern "C" {
#endif  // __cplusplus

GLint __wrap_glGetUniformLocation(GLuint program, const GLchar *name);
GLuint __wrap_glCreateShader(GLenum type);
void __wrap_glShaderSource(GLuint shader, GLsizei count, const GLchar *const *str, const GLint *length);
void __wrap_glAttachShader(GLuint program, GLuint shader);
void __wrap_glLinkProgram(GLuint program);
void __wrap_glUseProgram(GLuint program);

#ifdef __cplusplus
}
#endif  // __cplusplus

#endif  // MEDIAPIPE_API_GPU_GL_WRAPPER_H_
