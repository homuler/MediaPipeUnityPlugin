#ifndef C_MEDIAPIPE_API_FRAMEWORK_PORT_STATUS_H_
#define C_MEDIAPIPE_API_FRAMEWORK_PORT_STATUS_H_

#include <memory>
#include <utility>
#include "mediapipe/framework/port/status.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Status__i_PKc(int code, const char* message, mediapipe::Status** status_out);
MP_CAPI(void) mp_Status__delete(mediapipe::Status* status);

MP_CAPI(MpReturnCode) mp_Status__ToString(mediapipe::Status* status, const char** str_out);
MP_CAPI(bool) mp_Status__ok(mediapipe::Status* status);
MP_CAPI(int) mp_Status__raw_code(mediapipe::Status* status);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_PORT_STATUS_H_
