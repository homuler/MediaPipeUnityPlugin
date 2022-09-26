#include "mediapipe_api/framework/formats/float_vector.h"

#include <vector>

MpReturnCode mp__MakeFloatVectorPacket__PA_i(const float* value, int size, mediapipe::Packet** packet_out) {
  TRY
    std::vector<float> vector(value, value + size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::vector<float>>(vector)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeFloatVectorPacket_At__PA_i_Rt(const float* value, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    std::vector<float> vector(value, value + size);
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::vector<float>>(vector).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetFloatVector(mediapipe::Packet* packet, const float** value_out) {
  TRY_ALL
    *value_out = packet->Get<std::vector<float>>().data();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

MpReturnCode mp_Packet__GetFloatVector(mediapipe::Packet* packet, const float** value_out, int* size_out) {
 TRY_ALL
   auto& vector_float = packet->Get<std::vector<float>>();
   auto length = vector_float.size();
   
   *value_out = vector_float.data();
   *size_out = length;
   RETURN_CODE(MpReturnCode::Success);
 CATCH_ALL
}

MP_CAPI(MpReturnCode) mp_Packet__ValidateAsFloatVector(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<std::vector<float>>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
