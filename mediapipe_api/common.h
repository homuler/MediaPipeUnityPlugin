// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_COMMON_H_
#define MEDIAPIPE_API_COMMON_H_

#ifdef _WIN32
#define MP_CAPI_EXPORT __declspec(dllexport)
#else
#define MP_CAPI_EXPORT
#endif

#include <csetjmp>
#include <csignal>
#include <string>

#include "mediapipe/framework/port/logging.h"

extern inline const char* strcpy_to_heap(const std::string& str) {
  if (str.empty()) {
    return nullptr;
  }

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

namespace mp_api {

template <typename T>
struct StructArray {
  T* data;
  int size;
};

#if defined(_WIN32) || defined(__EMSCRIPTEN__)
#define MEDIAPIPE_DISABLE_SIGABRT_HANDLER
#endif

#if defined(__EMSCRIPTEN__)
#define MEDIAPIPE_IGNORE_EXCEPTION
#endif

#ifndef MEDIAPIPE_DISABLE_SIGABRT_HANDLER
extern thread_local struct sigaction orig_act;
extern thread_local sigjmp_buf abrt_jbuf;

extern void sigabrt_handler(int sig);
#endif

}  // namespace mp_api

// TODO: make code more readable
#ifdef MEDIAPIPE_IGNORE_EXCEPTION
#define TRY            \
  auto volatile _mp_return_code = MpReturnCode::Unset; \
  {
#else
#define TRY             \
  auto volatile _mp_return_code = MpReturnCode::Unset; \
  try {
#endif  // MEDIAPIPE_IGNORE_EXCEPTION

#ifdef MEDIAPIPE_IGNORE_EXCEPTION
#define CATCH_EXCEPTION \
  }                     \
  return _mp_return_code;
#else
#define CATCH_EXCEPTION                            \
  }                                                \
  catch (std::exception & e) {                     \
    LOG(ERROR) << e.what();                        \
    google::FlushLogFiles(google::GLOG_ERROR);     \
    _mp_return_code = MpReturnCode::StandardError; \
  }                                                \
  catch (...) {                                    \
    LOG(ERROR) << "Unknown exception occured";     \
    google::FlushLogFiles(google::GLOG_ERROR);     \
    _mp_return_code = MpReturnCode::UnknownError;  \
  }                                                \
  return _mp_return_code;
#endif

#ifdef MEDIAPIPE_DISABLE_SIGABRT_HANDLER
#define TRY_ALL TRY
#define CATCH_ALL CATCH_EXCEPTION
#else
#define TRY_ALL                                  \
  TRY                                            \
    struct sigaction act;                        \
    sigemptyset(&act.sa_mask);                   \
    act.sa_flags = 0;                            \
    act.sa_handler = mp_api::sigabrt_handler;    \
    sigaction(SIGABRT, &act, &mp_api::orig_act); \
    if (sigsetjmp(mp_api::abrt_jbuf, 1) == 0) {
#define CATCH_ALL                                 \
  }                                               \
  else {                                          \
    LOG(ERROR) << "Aborted";                      \
    google::FlushLogFiles(google::GLOG_ERROR);    \
    _mp_return_code = MpReturnCode::Aborted;      \
  }                                               \
  sigaction(SIGABRT, &mp_api::orig_act, nullptr); \
  CATCH_EXCEPTION
#endif  // MEDIAPIPE_DISABLE_SIGABRT_HANDLER

#define RETURN_CODE(code) _mp_return_code = code

#ifdef _WIN32
#define CDECL __cdecl
#else
#define CDECL
#endif  // _WIN32

#define MP_CAPI(rettype) MP_CAPI_EXPORT extern rettype CDECL
#endif  // MEDIAPIPE_API_COMMON_H_
