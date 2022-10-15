// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/external/stdlib.h"

void delete_array__PKc(const char* str) { delete[] str; }

void delete_array__Pf(float* f) { delete[] f; }

void std_string__delete(std::string* str) { delete str; }

MpReturnCode std_string__PKc_i(const char* src, int size, std::string** str_out) {
  TRY
    *str_out = new std::string(src, size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void std_string__swap__Rstr(std::string* src, std::string* dst) { src->swap(*dst); }

void mp_StatusOrString__delete(absl::StatusOr<std::string>* status_or_string) { delete status_or_string; }

bool mp_StatusOrString__ok(absl::StatusOr<std::string>* status_or_string) { return status_or_string->ok(); }

MpReturnCode mp_StatusOrString__status(absl::StatusOr<std::string>* status_or_string, absl::Status** status_out) {
  return absl_StatusOr__status(status_or_string, status_out);
}

MpReturnCode mp_StatusOrString__value(absl::StatusOr<std::string>* status_or_string, const char** value_out) {
  TRY_ALL
    auto str = std::move(*status_or_string).value();
    *value_out = strcpy_to_heap(str);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_StatusOrString__bytearray(absl::StatusOr<std::string>* status_or_string, const char** value_out, int* size_out) {
  TRY_ALL
    auto str = std::move(*status_or_string).value();
    auto length = str.size();
    auto bytes = new char[length];
    memcpy(bytes, str.c_str(), length);

    *value_out = bytes;
    *size_out = length;
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}
