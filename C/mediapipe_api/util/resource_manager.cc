#include "mediapipe/framework/port/canonical_errors.h"
#include "mediapipe/framework/port/logging.h"
#include "mediapipe/framework/port/ret_check.h"
#include "mediapipe_api/util/resource_manager.h"

namespace {
  CacheFilePathResolver* cache_file_path_resolver_ = nullptr;
  ReadFileHandler* read_file_handler_ = nullptr;
}

void mp_api__PrepareResourceManager(CacheFilePathResolver* resolver, ReadFileHandler* handler) {
  cache_file_path_resolver_ = resolver;
  read_file_handler_ = handler;
}

namespace mediapipe {

bool ResourceManager::ReadFile(const std::string& filename, std::string* output) {
  if (read_file_handler_ == nullptr) {
    LOG(ERROR) << "ResourceManager is not initialized";
    return false;
  }

  return read_file_handler_(filename.c_str(), output);
}

::mediapipe::StatusOr<std::string> ResourceManager::CachedFileFromAsset(const std::string& filename) {
  if (cache_file_path_resolver_ == nullptr) {
    return ::mediapipe::FailedPreconditionError("ResourceManager is not initialized");
  }

  auto asset_path = cache_file_path_resolver_(filename.c_str());
  RET_CHECK_NE(asset_path, nullptr);

  return std::string(asset_path);
}

}  // namespace mediapipe
