#include "mediapipe_api/gpu/egl_surface_holder.h"
#include "mediapipe/framework/port/ret_check.h"

MpReturnCode mp_EglSurfaceHolderUniquePtr__(EglSurfaceHolderUniquePtr** egl_surface_holder_out) {
  TRY {
    *egl_surface_holder_out = new EglSurfaceHolderUniquePtr(new mediapipe::EglSurfaceHolder());
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

void mp_EglSurfaceHolderUniquePtr__delete(EglSurfaceHolderUniquePtr* egl_surface_holder) {
  delete egl_surface_holder;
}

mediapipe::EglSurfaceHolder* mp_EglSurfaceHolderUniquePtr__get(EglSurfaceHolderUniquePtr* egl_surface_holder) {
  return egl_surface_holder->get();
}

mediapipe::EglSurfaceHolder* mp_EglSurfaceHolderUniquePtr__release(EglSurfaceHolderUniquePtr* egl_surface_holder) {
  return egl_surface_holder->release();
}

void mp_EglSurfaceHolder__SetFlipY__b(mediapipe::EglSurfaceHolder* egl_surface_holder, bool flip_y) {
  egl_surface_holder->flip_y = flip_y;
}

bool mp_EglSurfaceHolder__flip_y(mediapipe::EglSurfaceHolder* egl_surface_holder) {
  return egl_surface_holder->flip_y;
}

MpReturnCode mp_EglSurfaceHolder__SetSurface__P_Pgc(mediapipe::EglSurfaceHolder* egl_surface_holder,
                                                    EGLSurface surface,
                                                    mediapipe::GlContext* gl_context,
                                                    absl::Status** status_out) {
  TRY {
    if (gl_context == nullptr) {
      *status_out = new absl::Status { absl::StatusCode::kFailedPrecondition, "GPU shared data not created" };
    } else {
      EGLSurface old_surface = EGL_NO_SURFACE;

      {
        absl::MutexLock lock(&egl_surface_holder->mutex);
        if (egl_surface_holder->owned) {
          old_surface = egl_surface_holder->surface;
        }
        egl_surface_holder->surface = surface;
        egl_surface_holder->owned = false;
      }

      if (old_surface != EGL_NO_SURFACE) {
        auto status = gl_context->Run([gl_context, old_surface]() -> absl::Status {
          RET_CHECK(eglDestroySurface(gl_context->egl_display(), old_surface))
              << "eglDestroySurface failed:" << eglGetError();
          return mediapipe::OkStatus();
        });
        *status_out = new absl::Status { std::move(status) };
      } else {
        *status_out = new absl::Status { mediapipe::OkStatus() };
      }
    }

    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp__MakeEglSurfaceHolderUniquePtrPacket__Reshup(EglSurfaceHolderUniquePtr* egl_surface_holder,
                                                           mediapipe::Packet** packet_out) {
  TRY {
    *packet_out = new mediapipe::Packet { mediapipe::MakePacket<EglSurfaceHolderUniquePtr>(std::move(*egl_surface_holder)) };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetEglSurfaceHolderUniquePtr(mediapipe::Packet* packet, const EglSurfaceHolderUniquePtr** value_out) {
  return mp_Packet__Get(packet, value_out);
}

MpReturnCode mp_Packet__ValidateAsEglSurfaceHolderUniquePtr(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY {
    *status_out = new absl::Status { packet->ValidateAsType<EglSurfaceHolderUniquePtr>() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}
