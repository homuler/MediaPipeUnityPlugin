#ifndef C_MEDIAPIPE_API_GPU_EGL_SURFACE_HOLDER_H_
#define C_MEDIAPIPE_API_GPU_EGL_SURFACE_HOLDER_H_

#include <memory>
#include "mediapipe/gpu/egl_surface_holder.h"
#include "mediapipe/gpu/gl_context.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

typedef std::unique_ptr<mediapipe::EglSurfaceHolder> EglSurfaceHolderUniquePtr;
typedef absl::StatusOr<EglSurfaceHolderUniquePtr> StatusOrEglSurfaceHolderUniquePtr;

MP_CAPI(MpReturnCode) mp_EglSurfaceHolderUniquePtr__(EglSurfaceHolderUniquePtr** egl_surface_holder_out);
MP_CAPI(void) mp_EglSurfaceHolderUniquePtr__delete(EglSurfaceHolderUniquePtr* egl_surface_holder);
MP_CAPI(mediapipe::EglSurfaceHolder*) mp_EglSurfaceHolderUniquePtr__get(EglSurfaceHolderUniquePtr* egl_surface_holder);
MP_CAPI(mediapipe::EglSurfaceHolder*) mp_EglSurfaceHolderUniquePtr__release(EglSurfaceHolderUniquePtr* egl_surface_holder);

MP_CAPI(void) mp_EglSurfaceHolder__SetFlipY__b(mediapipe::EglSurfaceHolder* egl_surface_holder, bool flip_y);
MP_CAPI(bool) mp_EglSurfaceHolder__flip_y(mediapipe::EglSurfaceHolder* egl_surface_holder);
MP_CAPI(MpReturnCode) mp_EglSurfaceHolder__SetSurface__P_Pgc(mediapipe::EglSurfaceHolder* egl_surface_holder,
                                                             EGLSurface surface,
                                                             mediapipe::GlContext* gl_context,
                                                             absl::Status** status_out);

MP_CAPI(MpReturnCode) mp__MakeEglSurfaceHolderUniquePtrPacket__Reshup(EglSurfaceHolderUniquePtr* egl_surface_holder,
                                                                 mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetEglSurfaceHolderUniquePtr(mediapipe::Packet* packet, const EglSurfaceHolderUniquePtr** value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsEglSurfaceHolderUniquePtr(mediapipe::Packet* packet, absl::Status** status_out);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GPU_EGL_SURFACE_HOLDER_H_
