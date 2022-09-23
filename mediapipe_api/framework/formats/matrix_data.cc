// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/matrix_data.h"

inline mediapipe::MatrixData& ParseFromStringAsMatrixData(const char* serialized_matrix_data, int size) {
  mediapipe::MatrixData matrix_data;
  CHECK(matrix_data.ParseFromString(std::string(serialized_matrix_data, size)));

  return matrix_data;
}

MpReturnCode mp__MakeMatrixPacket__PKc_i(const char* serialized_matrix_data, int size, mediapipe::Packet** packet_out) {
  TRY
    auto matrix_data = ParseFromStringAsMatrixData(serialized_matrix_data, size);

    mediapipe::Matrix matrix;
    mediapipe::MatrixFromMatrixDataProto(matrix_data, &matrix);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(matrix)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeMatrixPacket_At__PKc_i_Rt(const char* serialized_matrix_data, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    auto matrix_data = ParseFromStringAsMatrixData(serialized_matrix_data, size);

    mediapipe::Matrix matrix;
    mediapipe::MatrixFromMatrixDataProto(matrix_data, &matrix);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(matrix).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MP_CAPI(MpReturnCode) mp_Packet__ValidateAsMatrix(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::Matrix>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}