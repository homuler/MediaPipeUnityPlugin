#include "mediapipe_api/gpu/gl_context.h"

MpGlContext* MpGlContextGetCurrent() {
  return new MpGlContext { mediapipe::GlContext::GetCurrent() };
}

mediapipe::GlContext* MpGlContextGet(MpGlContext* gl_context) {
  return gl_context->impl.get();
}
