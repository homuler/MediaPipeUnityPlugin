// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_
#define MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_

#include <memory>
#include <utility>

#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"

extern "C" {

MP_CAPI(bool) mp_api__ConvertFromCalculatorGraphConfigTextFormat(const char* config_text, mp_api::SerializedProto* value_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_CALCULATOR_H_
