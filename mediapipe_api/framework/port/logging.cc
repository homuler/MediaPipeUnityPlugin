#include "mediapipe_api/framework/port/logging.h"

namespace {
  const char* argv;
}

MpReturnCode google_InitGoogleLogging__PKc(const char* name) {
  TRY_ALL {
    argv = strcpy_to_heap(name);
    google::InitGoogleLogging(argv);
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}

MpReturnCode google_ShutdownGoogleLogging() {
  TRY_ALL {
    google::ShutdownGoogleLogging();
    delete[] argv;
    RETURN_CODE(MpReturnCode::Success);
  } CATCH_ALL
}
