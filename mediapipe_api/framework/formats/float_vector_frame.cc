#include "mediapipe_api/framework/formats/float_vector_frame.h"

#include <vector>

MpReturnCode mp__MakeFloatVectorFramePacket__PA_i(const float* value, int size, mediapipe::Packet** packet_out) {
  TRY
    std::vector<float> vector{};
    for (auto i = 0; i < size; ++i) {
      vector.push_back(value[i]);
    }
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::vector<float>>(vector)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeFloatVectorFramePacket_At__PA_i_Rt(const float* value, int size, mediapipe::Timestamp* timestamp,
                                                      mediapipe::Packet** packet_out) {
  TRY
    std::vector<float> vector{};
    for (auto i = 0; i < size; ++i) {
      vector.push_back(value[i]);
    }
    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<std::vector<float>>(vector).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetFloatVectorFrame(mediapipe::Packet* packet, const float** value_out) {
  TRY_ALL
    *value_out = packet->Get<std::vector<float>>().data();
    RETURN_CODE(MpReturnCode::Success);
  CATCH_ALL
}

void mp_FloatVectorFrame__delete(float* vector_data) { delete[] vector_data; }