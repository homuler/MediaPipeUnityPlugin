#ifndef C_MEDIAPIPE_API_FRAMEWORK_PORT_LOGGING_H_
#define C_MEDIAPIPE_API_FRAMEWORK_PORT_LOGGING_H_

#include "mediapipe/framework/port/logging.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) google_InitGoogleLogging__PKc(const char* name, const char* log_dir);
MP_CAPI(MpReturnCode) google_ShutdownGoogleLogging();

}  // extern "C"

#endif  // C_MEDIAPIPE_API_FRAMEWORK_PORT_LOGGING_H_
