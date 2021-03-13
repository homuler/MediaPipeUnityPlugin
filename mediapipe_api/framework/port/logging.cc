#include "mediapipe_api/framework/port/logging.h"

namespace {
  const char* argv;
}

MpReturnCode google_InitGoogleLogging__PKc(const char* name, const char* log_dir) {
  TRY_ALL {
    argv = strcpy_to_heap(name);
    FLAGS_log_dir = log_dir;
    google::InitGoogleLogging(argv);
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode google_ShutdownGoogleLogging() {
  TRY_ALL {
    google::ShutdownGoogleLogging();
    FLAGS_log_dir = "";
    delete[] argv;
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}
