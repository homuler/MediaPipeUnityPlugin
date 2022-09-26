// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/matrix_data.h"

MpReturnCode mp__MakeMatrixPacket__PKc_i(const char* matrix_data_serialized, int size, mediapipe::Packet** packet_out) {
  TRY
    mediapipe::Matrix matrix;

    // convert matrix data from char into mediapipe::MatrixData
    std::string content;
    mediapipe::MatrixData matrix_data;
    CHECK(matrix_data.ParseFromString(std::string(matrix_data_serialized, size)));

    // fill matrix with data from matrix_data_serialized
    mediapipe::MatrixFromMatrixDataProto(matrix_data, &matrix);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(matrix)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeMatrixPacket_At__PKc_i_Rt(const char* matrix_data_serialized, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    mediapipe::Matrix matrix;

    // convert matrix data from char into mediapipe::MatrixData
    std::string content;
    mediapipe::MatrixData matrix_data;
    CHECK(matrix_data.ParseFromString(std::string(matrix_data_serialized, size)));

    // fill matrix with data from matrix_data_serialized
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