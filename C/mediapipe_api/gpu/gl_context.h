#ifndef C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_
#define C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_

#include <memory>
#include <utility>
#include "mediapipe/gpu/gl_context.h"
#include "mediapipe_api/common.h"

extern "C" {

typedef struct MpGlContext {
  std::shared_ptr<mediapipe::GlContext> impl;
} MpGlContext;

MP_CAPI_EXPORT extern MpGlContext* MpGlContextGetCurrent();
MP_CAPI_EXPORT extern MpGlContext* MpGlContextGet();

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_
