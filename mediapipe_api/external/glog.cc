#include "mediapipe_api/external/glog.h"

void glog_LOG_INFO__PKc(const char* str) {
  LOG(INFO) << str;
}

void glog_LOG_WARNING__PKc(const char* str) {
  LOG(WARNING) << str;
}

void glog_LOG_ERROR__PKc(const char* str) {
  LOG(ERROR) << str;
}

void glog_LOG_FATAL__PKc(const char* str) {
  LOG(FATAL) << str;
}

void google_FlushLogFiles(google::LogSeverity severity) {
  google::FlushLogFiles(severity);
}
