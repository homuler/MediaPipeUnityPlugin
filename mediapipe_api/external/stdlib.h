// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_EXTERNAL_STDLIB_H_
#define MEDIAPIPE_API_EXTERNAL_STDLIB_H_

#include <string>

#include "mediapipe_api/common.h"
#include "mediapipe_api/external/absl/statusor.h"

extern "C" {

MP_CAPI(void) delete_array__PKc(const char* str);

// string API
MP_CAPI(void) std_string__delete(std::string* str);
MP_CAPI(MpReturnCode) std_string__PKc_i(const char* src, int size, std::string** str_out);
MP_CAPI(void) std_string__swap__Rstr(std::string* src, std::string* dst);

// StatusOr API
MP_CAPI(void) mp_StatusOrString__delete(absl::StatusOr<std::string>* status_or_string);
MP_CAPI(bool) mp_StatusOrString__ok(absl::StatusOr<std::string>* status_or_string);
MP_CAPI(MpReturnCode) mp_StatusOrString__status(absl::StatusOr<std::string>* status_or_string, absl::Status** status_out);
MP_CAPI(MpReturnCode) mp_StatusOrString__value(absl::StatusOr<std::string>* status_or_string, const char** value_out);
MP_CAPI(MpReturnCode) mp_StatusOrString__bytearray(absl::StatusOr<std::string>* status_or_string, const char** value_out, int* size_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_EXTERNAL_STDLIB_H_
