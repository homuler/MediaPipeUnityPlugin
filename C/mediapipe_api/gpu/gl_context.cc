#include "mediapipe_api/gpu/gl_context.h"

void mp_SharedGlContext__delete(SharedGlContext* shared_gl_context) {
  delete shared_gl_context;
}

mediapipe::GlContext* mp_SharedGlContext__get(SharedGlContext* shared_gl_context) {
  return shared_gl_context->get();
}

void mp_SharedGlContext__reset(SharedGlContext* shared_gl_context) {
  shared_gl_context->reset();
}

MpReturnCode mp_GlContext_GetCurrent(SharedGlContext** shared_gl_context_out) {
  TRY {
    *shared_gl_context_out = new SharedGlContext { mediapipe::GlContext::GetCurrent() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_GlContext_Create__p_b(bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY {
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext { mediapipe::GlContext::Create(nullptr, create_thread) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_GlContext_Create__Rgc_b(mediapipe::GlContext* share_context,
                                        bool create_thread,
                                        StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY {
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext { mediapipe::GlContext::Create(*share_context, create_thread) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_GlContext_Create__ui_b(mediapipe::PlatformGlContext share_context,
                                       bool create_thread,
                                       StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY {
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext { mediapipe::GlContext::Create(share_context, create_thread) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

// GlSyncToken API
void mp_GlSyncToken__delete(mediapipe::GlSyncToken* gl_sync_token) {
  delete gl_sync_token;
}

mediapipe::GlSyncPoint* mp_GlSyncToken__get(mediapipe::GlSyncToken* gl_sync_token) {
  return gl_sync_token->get();
}

void mp_GlSyncToken__reset(mediapipe::GlSyncToken* gl_sync_token) {
  gl_sync_token->reset();
}

MpReturnCode mp_GlSyncPoint__Wait(mediapipe::GlSyncPoint* gl_sync_point) {
  TRY {
    gl_sync_point->Wait();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_GlSyncPoint__WaitOnGpu(mediapipe::GlSyncPoint* gl_sync_point) {
  TRY {
    gl_sync_point->WaitOnGpu();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_GlSyncPoint__IsReady(mediapipe::GlSyncPoint* gl_sync_point, bool* value_out) {
  TRY {
    *value_out = gl_sync_point->IsReady();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_GlSyncPoint__GetContext(mediapipe::GlSyncPoint* gl_sync_point, SharedGlContext** shared_gl_context_out) {
  TRY {
    *shared_gl_context_out = new SharedGlContext { gl_sync_point->GetContext() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}
