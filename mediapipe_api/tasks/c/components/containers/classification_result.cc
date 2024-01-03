#include "mediapipe_api/tasks/c/components/containers/classification_result.h"

MpReturnCode mp_Packet__GetClassificationsVector(mediapipe::Packet* packet, ClassificationResult* value_out) {
  TRY_ALL
    // get std::vector<ClassificationList> and convert it to ClassificationResult
    auto proto_vec = packet->Get<std::vector<mediapipe::ClassificationList>>();
    auto vec_size = proto_vec.size();

    auto classifications_vec = std::vector<mediapipe::tasks::components::containers::Classifications>(vec_size);
    for (auto i = 0; i < vec_size; ++i) {
      auto classifications = mediapipe::tasks::components::containers::ConvertToClassifications(proto_vec[i]);
      classifications_vec[i] = classifications;
    }

    mediapipe::tasks::components::containers::ClassificationResult result;
    result.classifications = std::move(classifications_vec);

    mediapipe::tasks::c::components::containers::CppConvertToClassificationResult(result, value_out);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_tasks_c_components_containers_CppCloseClassificationResult(ClassificationResult data) {
  mediapipe::tasks::c::components::containers::CppCloseClassificationResult(&data);
}
