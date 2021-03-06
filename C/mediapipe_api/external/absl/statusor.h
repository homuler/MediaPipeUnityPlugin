#ifndef C_MEDIAPIPE_API_EXTERNAL_ABSL_STATUSOR_H_
#define C_MEDIAPIPE_API_EXTERNAL_ABSL_STATUSOR_H_

#include "absl/status/statusor.h"
#include "mediapipe_api/common.h"

template <class T>
inline bool absl_StatusOr__ok(absl::StatusOr<T>* status_or) {
  return status_or->ok();
}

template <class T>
inline MpReturnCode absl_StatusOr__status(absl::StatusOr<T>* status_or, absl::Status** status_out) {
  TRY {
    *status_out = new absl::Status { status_or->status() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_EXCEPTION
}

template <class T>
inline MpReturnCode absl_StatusOr__value(absl::StatusOr<T>* status_or, T* value_out) {
  TRY_ALL {
    *value_out = std::move(*status_or).value();
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

template <class T>
inline MpReturnCode absl_StatusOr__value(absl::StatusOr<T>* status_or, T** value_out) {
  TRY_ALL {
    *value_out = new T { std::move(*status_or).value() };
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

#endif  // C_MEDIAPIPE_API_EXTERNAL_ABSL_STATUSOR_H_
