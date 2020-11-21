#ifndef C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_
#define C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_

#include <memory>
#include <utility>
#include "mediapipe/gpu/gl_context.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/port/statusor.h"

extern "C" {

typedef std::shared_ptr<mediapipe::GlContext> SharedGlContext;
typedef mediapipe::StatusOr<SharedGlContext> StatusOrSharedGlContext;

MP_CAPI(void) mp_SharedGlContext__delete(SharedGlContext* shared_gl_context);
MP_CAPI(mediapipe::GlContext*) mp_SharedGlContext__get(SharedGlContext* shared_gl_context);
MP_CAPI(void) mp_SharedGlContext__reset(SharedGlContext* shared_gl_context);

MP_CAPI(MpReturnCode) mp_GlContext_GetCurrent(SharedGlContext** shared_gl_context_out);
MP_CAPI(MpReturnCode) mp_GlContext_Create__p_b(bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out);
MP_CAPI(MpReturnCode) mp_GlContext_Create__Rgc_b(mediapipe::GlContext* share_context,
                                                 bool create_thread,
                                                 StatusOrSharedGlContext** status_or_shared_gl_context_out);
MP_CAPI(MpReturnCode) mp_GlContext_Create__ui_b(mediapipe::PlatformGlContext share_context,
                                                 bool create_thread,
                                                 StatusOrSharedGlContext** status_or_shared_gl_context_out);

// GlSyncToken API
MP_CAPI(void) mp_GlSyncToken__delete(mediapipe::GlSyncToken* gl_sync_token);
MP_CAPI(mediapipe::GlSyncPoint*) mp_GlSyncToken__get(mediapipe::GlSyncToken* gl_sync_token);
MP_CAPI(void) mp_GlSyncToken__reset(mediapipe::GlSyncToken* gl_sync_token);

MP_CAPI(MpReturnCode) mp_GlSyncPoint__Wait(mediapipe::GlSyncPoint* gl_sync_point);
MP_CAPI(MpReturnCode) mp_GlSyncPoint__WaitOnGpu(mediapipe::GlSyncPoint* gl_sync_point);
MP_CAPI(MpReturnCode) mp_GlSyncPoint__IsReady(mediapipe::GlSyncPoint* gl_sync_point, bool* value_out);
MP_CAPI(MpReturnCode) mp_GlSyncPoint__GetContext(mediapipe::GlSyncPoint* gl_sync_point, SharedGlContext** shared_gl_context_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_GL_CONTEXT_H_
