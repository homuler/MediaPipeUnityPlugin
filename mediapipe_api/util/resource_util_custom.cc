#include "absl/strings/str_cat.h"
#include "mediapipe/framework/port/ret_check.h"
#include "mediapipe_api/util/resource_util_custom.h"

void mp__SetCustomGlobalResourceProvider__P(ResourceProvider* resource_provider) {
  mediapipe::SetCustomGlobalResourceProvider([resource_provider](const std::string& path, std::string* output) -> ::absl::Status {
    if (resource_provider(path.c_str(), output)) {
      return absl::OkStatus();
    }
    return absl::FailedPreconditionError(absl::StrCat("Failed to read ", path));
  });
}

void mp__SetCustomGlobalPathResolver__P(PathResolver* path_resolver) {
  mediapipe::SetCustomGlobalPathResolver([path_resolver](const std::string& path) -> ::absl::StatusOr<std::string> {
    auto resolved_path = path_resolver(path.c_str());

    RET_CHECK_NE(resolved_path, nullptr);
    return std::string(resolved_path);
  });
}