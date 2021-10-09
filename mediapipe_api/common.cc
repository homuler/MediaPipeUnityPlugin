// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/common.h"

#ifndef _WIN32
thread_local struct sigaction mp_api::orig_act;
thread_local sigjmp_buf mp_api::abrt_jbuf;

void mp_api::sigabrt_handler(int sig) { siglongjmp(abrt_jbuf, 1); }
#endif
