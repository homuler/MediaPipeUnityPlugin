// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/calculator.h"

#include "mediapipe/framework/calculator.pb.h"

bool mp_api__ConvertFromCalculatorGraphConfigTextFormat(const char* config_text, mp_api::SerializedProto* value_out) {
  return ConvertFromTextFormat<mediapipe::CalculatorGraphConfig>(config_text, value_out);
}
