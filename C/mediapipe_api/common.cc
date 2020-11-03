#include "mediapipe_api/common.h"

namespace {
  thread_local struct sigaction orig_act;
  thread_local sigjmp_buf abrt_jbuf;

  void sigabrt_handler(int sig) {
    siglongjmp(abrt_jbuf, 1);
  }
}

bool sigabrt_is_not_received() {
  return sigsetjmp(abrt_jbuf, 1) == 0;
}

void setup_sigaction() {
  struct sigaction act;

  sigemptyset(&act.sa_mask);
  act.sa_flags = 0;
  act.sa_handler = sigabrt_handler;

  sigaction(SIGABRT, &act, &orig_act);
}

void restore_sigaction() {
  sigaction(SIGABRT, &orig_act, nullptr);
}

