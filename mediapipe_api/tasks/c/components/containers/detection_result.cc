#include "mediapipe_api/tasks/c/components/containers/detection_result.h"

MpReturnCode mp_Packet__GetDetectionResult(mediapipe::Packet* packet, DetectionResult* value_out) {
  TRY_ALL
    // get std::vector<Detection> and convert it to DetectionResult*
    auto detections = packet->Get<std::vector<mediapipe::Detection>>();
    auto detection_result = mediapipe::tasks::components::containers::ConvertToDetectionResult(detections);
    mediapipe::tasks::c::components::containers::CppConvertToDetectionResult(detection_result, value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_tasks_c_components_containers_CppCloseDetectionResult(DetectionResult data) {
  mediapipe::tasks::c::components::containers::CppCloseDetectionResult(&data);
}
