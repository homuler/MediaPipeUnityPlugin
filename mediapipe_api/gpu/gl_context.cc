// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/gpu/gl_context.h"

void mp_SharedGlContext__delete(SharedGlContext* shared_gl_context) { delete shared_gl_context; }

mediapipe::GlContext* mp_SharedGlContext__get(SharedGlContext* shared_gl_context) { return shared_gl_context->get(); }

void mp_SharedGlContext__reset(SharedGlContext* shared_gl_context) { shared_gl_context->reset(); }

MpReturnCode mp_GlContext_GetCurrent(SharedGlContext** shared_gl_context_out) {
  TRY
    auto gl_context = mediapipe::GlContext::GetCurrent();

    if (gl_context.get() == nullptr) {
      *shared_gl_context_out = nullptr;
    } else {
      *shared_gl_context_out = new SharedGlContext{gl_context};
    }
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_GlContext_Create__P_b(bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext{mediapipe::GlContext::Create(nullptr, create_thread)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_GlContext_Create__Rgc_b(mediapipe::GlContext* share_context, bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext{mediapipe::GlContext::Create(*share_context, create_thread)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_GlContext_Create__ui_b(mediapipe::PlatformGlContext share_context, bool create_thread,
                                       StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext{mediapipe::GlContext::Create(share_context, create_thread)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

#if HAS_EAGL
MpReturnCode mp_GlContext_Create__Pes_b(EAGLSharegroup* sharegroup, bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out) {
  TRY
    *status_or_shared_gl_context_out = new StatusOrSharedGlContext{mediapipe::GlContext::Create(sharegroup, create_thread)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
#endif  // HAS_EAGL

#if defined(__EMSCRIPTEN__)
#elif HAS_EGL
EGLDisplay mp_GlContext__egl_display(mediapipe::GlContext* gl_context) { return gl_context->egl_display(); }

EGLConfig mp_GlContext__egl_config(mediapipe::GlContext* gl_context) { return gl_context->egl_config(); }

EGLContext mp_GlContext__egl_context(mediapipe::GlContext* gl_context) { return gl_context->egl_context(); }
#elif HAS_EAGL
EAGLContext* mp_GlContext__eagl_context(mediapipe::GlContext* gl_context) { return gl_context->eagl_context(); }
#elif HAS_NSGL
NSOpenGLContext* mp_GlContext__nsgl_context(mediapipe::GlContext* gl_context) { return gl_context->nsgl_context(); }

NSOpenGLPixelFormat* mp_GlContext__nsgl_pixel_format(mediapipe::GlContext* gl_context) { return gl_context->nsgl_pixel_format(); }
#endif  // defined(__EMSCRIPTEN__)

bool mp_GlContext__IsCurrent(mediapipe::GlContext* gl_context) { return gl_context->IsCurrent(); }

GLint mp_GlContext__gl_major_version(mediapipe::GlContext* gl_context) { return gl_context->gl_major_version(); }

GLint mp_GlContext__gl_minor_version(mediapipe::GlContext* gl_context) { return gl_context->gl_minor_version(); }

int64_t mp_GlContext__gl_finish_count(mediapipe::GlContext* gl_context) { return gl_context->gl_finish_count(); }

// GlSyncToken API
void mp_GlSyncToken__delete(mediapipe::GlSyncToken* gl_sync_token) { delete gl_sync_token; }

mediapipe::GlSyncPoint* mp_GlSyncToken__get(mediapipe::GlSyncToken* gl_sync_token) { return gl_sync_token->get(); }

void mp_GlSyncToken__reset(mediapipe::GlSyncToken* gl_sync_token) { gl_sync_token->reset(); }

MpReturnCode mp_GlSyncPoint__Wait(mediapipe::GlSyncPoint* gl_sync_point) {
  TRY
    gl_sync_point->Wait();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_GlSyncPoint__WaitOnGpu(mediapipe::GlSyncPoint* gl_sync_point) {
  TRY
    gl_sync_point->WaitOnGpu();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_GlSyncPoint__IsReady(mediapipe::GlSyncPoint* gl_sync_point, bool* value_out) {
  TRY
    *value_out = gl_sync_point->IsReady();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_GlSyncPoint__GetContext(mediapipe::GlSyncPoint* gl_sync_point, SharedGlContext** shared_gl_context_out) {
  TRY
    *shared_gl_context_out = new SharedGlContext{gl_sync_point->GetContext()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
