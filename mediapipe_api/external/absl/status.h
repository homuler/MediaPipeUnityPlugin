#ifndef C_MEDIAPIPE_API_EXTERNAL_ABSL_STATUS_H_
#define C_MEDIAPIPE_API_EXTERNAL_ABSL_STATUS_H_

#include "absl/status/status.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) absl_Status__i_PKc(int code, const char* message, absl::Status** status_out);
MP_CAPI(void) absl_Status__delete(absl::Status* status);

MP_CAPI(MpReturnCode) absl_Status__ToString(absl::Status* status, const char** str_out);
MP_CAPI(bool) absl_Status__ok(absl::Status* status);
MP_CAPI(int) absl_Status__raw_code(absl::Status* status);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_EXTERNAL_ABSL_STATUS_H_
