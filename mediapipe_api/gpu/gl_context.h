// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_GPU_GL_CONTEXT_H_
#define MEDIAPIPE_API_GPU_GL_CONTEXT_H_

#include <memory>
#include <utility>

#include "mediapipe/gpu/gl_context.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/absl/statusor.h"

extern "C" {

typedef std::shared_ptr<mediapipe::GlContext> SharedGlContext;
typedef absl::StatusOr<SharedGlContext> StatusOrSharedGlContext;

MP_CAPI(void) mp_SharedGlContext__delete(SharedGlContext* shared_gl_context);
MP_CAPI(mediapipe::GlContext*) mp_SharedGlContext__get(SharedGlContext* shared_gl_context);
MP_CAPI(void) mp_SharedGlContext__reset(SharedGlContext* shared_gl_context);

MP_CAPI(MpReturnCode) mp_GlContext_GetCurrent(SharedGlContext** shared_gl_context_out);
MP_CAPI(MpReturnCode) mp_GlContext_Create__P_b(bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out);
MP_CAPI(MpReturnCode) mp_GlContext_Create__Rgc_b(mediapipe::GlContext* share_context, bool create_thread,
                                                 StatusOrSharedGlContext** status_or_shared_gl_context_out);
MP_CAPI(MpReturnCode) mp_GlContext_Create__ui_b(mediapipe::PlatformGlContext share_context, bool create_thread,
                                                StatusOrSharedGlContext** status_or_shared_gl_context_out);
#if HAS_EAGL
MP_CAPI(MpReturnCode) mp_GlContext_Create__Pes_b(EAGLSharegroup* sharegroup, bool create_thread, StatusOrSharedGlContext** status_or_shared_gl_context_out);
#endif  // HAS_EAGL

#if defined(__EMSCRIPTEN__)
#elif HAS_EGL
MP_CAPI(EGLDisplay) mp_GlContext__egl_display(mediapipe::GlContext* gl_context);
MP_CAPI(EGLConfig) mp_GlContext__egl_config(mediapipe::GlContext* gl_context);
MP_CAPI(EGLContext) mp_GlContext__egl_context(mediapipe::GlContext* gl_context);
#elif HAS_EAGL
MP_CAPI(EAGLContext*) mp_GlContext__eagl_context(mediapipe::GlContext* gl_context);
// TODO: cv_texture_cache
#elif HAS_NSGL
MP_CAPI(NSOpenGLContext*) mp_GlContext__nsgl_context(mediapipe::GlContext* gl_context);
MP_CAPI(NSOpenGLPixelFormat*) mp_GlContext__nsgl_pixel_format(mediapipe::GlContext* gl_context);
// TODO: cv_texture_cache
#endif  // defined(__EMSCRIPTEN__)

MP_CAPI(bool) mp_GlContext__IsCurrent(mediapipe::GlContext* gl_context);
MP_CAPI(GLint) mp_GlContext__gl_major_version(mediapipe::GlContext* gl_context);
MP_CAPI(GLint) mp_GlContext__gl_minor_version(mediapipe::GlContext* gl_context);
MP_CAPI(int64_t) mp_GlContext__gl_finish_count(mediapipe::GlContext* gl_context);

// GlSyncToken API
MP_CAPI(void) mp_GlSyncToken__delete(mediapipe::GlSyncToken* gl_sync_token);
MP_CAPI(mediapipe::GlSyncPoint*) mp_GlSyncToken__get(mediapipe::GlSyncToken* gl_sync_token);
MP_CAPI(void) mp_GlSyncToken__reset(mediapipe::GlSyncToken* gl_sync_token);

MP_CAPI(MpReturnCode) mp_GlSyncPoint__Wait(mediapipe::GlSyncPoint* gl_sync_point);
MP_CAPI(MpReturnCode) mp_GlSyncPoint__WaitOnGpu(mediapipe::GlSyncPoint* gl_sync_point);
MP_CAPI(MpReturnCode) mp_GlSyncPoint__IsReady(mediapipe::GlSyncPoint* gl_sync_point, bool* value_out);
MP_CAPI(MpReturnCode) mp_GlSyncPoint__GetContext(mediapipe::GlSyncPoint* gl_sync_point, SharedGlContext** shared_gl_context_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_GPU_GL_CONTEXT_H_
