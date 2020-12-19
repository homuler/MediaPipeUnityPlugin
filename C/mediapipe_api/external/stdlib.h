#ifndef C_MEDIAPIPE_API_EXTERNAL_STDILIB_H_
#define C_MEDIAPIPE_API_EXTERNAL_STDILIB_H_

#include <string>
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(void) delete_array__PKc(const char* str);

// string API
MP_CAPI(void) std_string__delete(std::string* str);
MP_CAPI(MpReturnCode) std_string__PKc_i(const char* src, int size, std::string** str_out);
MP_CAPI(void) std_string__swap__Rstr(std::string* src, std::string* dst);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_EXTERNAL_STDILIB_H_
