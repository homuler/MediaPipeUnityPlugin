#include <string>
#include "mediapipe_api/framework/port/logging.h"

namespace {
  static const char* argv;
}

void InitGoogleLogging(const char* name, const char* log_dir) {
  std::string text = std::string(name);

  // TODO: caller should manage `name`
  argv = strcpy_to_heap(text);

  FLAGS_log_dir = log_dir;
  google::InitGoogleLogging(argv);
}

void ShutdownGoogleLogging() {
  google::ShutdownGoogleLogging();
  FLAGS_log_dir = "";
  delete argv;
}
