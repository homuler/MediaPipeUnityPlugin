#include "mediapipe_api/common.h"

#ifndef _WIN32
thread_local struct sigaction mp_api::orig_act;
thread_local sigjmp_buf mp_api::abrt_jbuf;

void mp_api::sigabrt_handler(int sig) {
  siglongjmp(abrt_jbuf, 1);
}
#endif
