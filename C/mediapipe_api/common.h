#ifndef C_MEDIAPIPE_API_COMMON_H_
#define C_MEDIAPIPE_API_COMMON_H_

#ifdef DLL_EXPORTS
#define MP_CAPI_EXPORT __declspec(dllexport)
#else
#define MP_CAPI_EXPORT
#endif

#include <string>
#include "mediapipe/framework/port/logging.h"

extern inline const char* strcpy_to_heap(const std::string& str) {
  if (str.empty()) { return nullptr; }

  auto str_ptr = new char[str.length() + 1];
  snprintf(str_ptr, str.length() + 1, str.c_str());

  return str_ptr;
}


enum class MpReturnCode : int {
  Success = 0,
  StandardError = 1,
  UnknownError = 2
};

#define TRY try
#define CATCH_ALL catch (std::exception& e) {\
                    LOG(ERROR) << e.what();\
                    google::FlushLogFiles(google::ERROR);\
                    return MpReturnCode::StandardError;\
                  } catch (...) {\
                    LOG(ERROR) << "Unknown exception";\
                    google::FlushLogFiles(google::ERROR);\
                    return MpReturnCode::UnknownError;\
                  }

#define MP_CAPI(rettype) MP_CAPI_EXPORT extern rettype
#endif  // C_MEDIAPIPE_API_COMMON_H_
