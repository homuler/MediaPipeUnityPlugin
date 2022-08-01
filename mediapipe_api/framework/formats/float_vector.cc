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

MpReturnCode mp_Packet__GetFloatVector(mediapipe::Packet* packet, const float** data_out) {
  TRY_ALL
    *data_out = packet->Get<std::vector<float>>().data();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

// TODO: Return with vector size
// MpReturnCode mp_Packet__GetFloatVector(mediapipe::Packet* packet, const float** data_out, int* size_out) {
//  TRY_ALL
//    *data_out = packet->Get<std::vector<float>>().data();
//    RETURN_CODE(MpReturnCode::Success);
//  CATCH_ALL
//}
