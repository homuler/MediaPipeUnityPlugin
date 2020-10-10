#include <string>
#include "mediapipe_api/util/resource_util.h"

void MpSetResourceRootPath(const char* path) {
  mediapipe::SetResourceRootPath(std::string(path));
}
