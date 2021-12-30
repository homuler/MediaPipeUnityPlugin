// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/external/stdlib.h"

void delete_array__PKc(const char* str) { delete[] str; }

void std_string__delete(std::string* str) { delete str; }

MpReturnCode std_string__PKc_i(const char* src, int size, std::string** str_out) {
  TRY
    *str_out = new std::string(src, size);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void std_string__swap__Rstr(std::string* src, std::string* dst) { src->swap(*dst); }
