// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_PORT_LOGGING_H_
#define MEDIAPIPE_API_FRAMEWORK_PORT_LOGGING_H_

#include "mediapipe/framework/port/logging.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) google_InitGoogleLogging__PKc(const char* name);
MP_CAPI(MpReturnCode) google_ShutdownGoogleLogging();

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_PORT_LOGGING_H_
