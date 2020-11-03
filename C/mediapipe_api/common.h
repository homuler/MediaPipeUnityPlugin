#ifndef C_MEDIAPIPE_API_COMMON_H_
#define C_MEDIAPIPE_API_COMMON_H_

#ifdef DLL_EXPORTS
#define MP_CAPI_EXPORT __declspec(dllexport)
#else
#define MP_CAPI_EXPORT
#endif

#include <csetjmp>
#include <csignal>
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
  UnknownError = 70,
  Unset = 128,
  Aborted = 134,
};

bool sigabrt_is_not_received();
void setup_sigaction();
void restore_sigaction();

// TODO(homuler): make code more readable
#define TRY             auto _mp_return_code = MpReturnCode::Unset;\
                        try
#define CATCH_EXCEPTION catch (std::exception& e) {\
                          LOG(ERROR) << e.what();\
                          google::FlushLogFiles(google::ERROR);\
                          _mp_return_code = MpReturnCode::StandardError;\
                        } catch (...) {\
                          LOG(ERROR) << "Unknown exception";\
                          google::FlushLogFiles(google::ERROR);\
                          _mp_return_code = MpReturnCode::UnknownError;\
                        }\
                        return _mp_return_code;

#define TRY_ALL   TRY {\
                    setup_sigaction();\
                    if (sigabrt_is_not_received())
#define CATCH_ALL   else {\
                      _mp_return_code = MpReturnCode::Aborted;\
                    }\
                    restore_sigaction();\
                  } CATCH_EXCEPTION
#define RETURN_CODE(code) _mp_return_code = code

#define MP_CAPI(rettype) MP_CAPI_EXPORT extern rettype
#endif  // C_MEDIAPIPE_API_COMMON_H_
