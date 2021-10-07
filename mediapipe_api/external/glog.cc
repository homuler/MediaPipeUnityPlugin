// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/external/glog.h"

void glog_FLAGS_logtostderr(bool flag) { FLAGS_logtostderr = flag; }

void glog_FLAGS_stderrthreshold(int threshold) { FLAGS_stderrthreshold = threshold; }

void glog_FLAGS_minloglevel(int level) { FLAGS_minloglevel = level; }

void glog_FLAGS_log_dir(const char* dir) { FLAGS_log_dir = dir; }

void glog_FLAGS_v(int v) { FLAGS_v = v; }

void glog_LOG_INFO__PKc(const char* str) { LOG(INFO) << str; }

void glog_LOG_WARNING__PKc(const char* str) { LOG(WARNING) << str; }

void glog_LOG_ERROR__PKc(const char* str) { LOG(ERROR) << str; }

void glog_LOG_FATAL__PKc(const char* str) { LOG(FATAL) << str; }

void google_FlushLogFiles(google::LogSeverity severity) { google::FlushLogFiles(severity); }
