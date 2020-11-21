#ifndef C_MEDIAPIPE_API_FRAMEWORK_PORT_STATUSOR_H_
#define C_MEDIAPIPE_API_FRAMEWORK_PORT_STATUSOR_H_

#include <memory>
#include <utility>
#include "mediapipe/framework/port/statusor.h"
#include "mediapipe_api/common.h"

template <class T>
inline bool mp_StatusOr__ok(mediapipe::StatusOr<T>* status_or) {
  return status_or->ok();
}

template <class T>
inline MpReturnCode mp_StatusOr__status(mediapipe::StatusOr<T>* status_or, mediapipe::Status** status_out) {
  TRY {
    *status_out = new mediapipe::Status { status_or->status() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

template <class T>
inline MpReturnCode mp_StatusOr__ValueOrDie(mediapipe::StatusOr<T>* status_or, T* value_out) {
  TRY_ALL {
    *value_out = status_or->ValueOrDie();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

template <class T>
inline MpReturnCode mp_StatusOr__ValueOrDie(mediapipe::StatusOr<T>* status_or, T** value_out) {
  TRY_ALL {
    *value_out = new T { status_or->ValueOrDie() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

template <class T>
inline MpReturnCode mp_StatusOr__ConsumeValueOrDie(mediapipe::StatusOr<T>* status_or, T* value_out) {
  TRY_ALL {
    *value_out = status_or->ConsumeValueOrDie();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

template <class T>
inline MpReturnCode mp_StatusOr__ConsumeValueOrDie(mediapipe::StatusOr<T>* status_or, T** value_out) {
  TRY_ALL {
    *value_out = new T { status_or->ConsumeValueOrDie() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

#endif  // C_MEDIAPIPE_API_FRAMEWORK_PORT_STATUSOR_H_
