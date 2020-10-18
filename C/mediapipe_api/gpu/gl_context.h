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

typedef struct MpGlSyncToken {
  std::shared_ptr<mediapipe::GlSyncPoint> impl;
} MpGlSyncToken;

MP_CAPI_EXPORT extern MpGlContext* MpGlContextGetCurrent();
MP_CAPI_EXPORT extern mediapipe::GlContext* MpGlContextGet(MpGlContext* gl_context);

/** GlSyncToken API */
MP_CAPI_EXPORT extern void MpGlSyncTokenDestroy(MpGlSyncToken* token);
MP_CAPI_EXPORT extern void MpGlSyncTokenWait(MpGlSyncToken* token);
MP_CAPI_EXPORT extern void MpGlSyncTokenWaitOnGpu(MpGlSyncToken* token);
MP_CAPI_EXPORT extern bool MpGlSyncTokenIsReady(MpGlSyncToken* token);
MP_CAPI_EXPORT extern MpGlContext* MpGlSyncTokenGetContext(MpGlSyncToken* token);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_
