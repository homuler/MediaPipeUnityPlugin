// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/matrix.h"


MpReturnCode mp__MakeColMajorMatrixPacket__Pf_i_i(float* pcm_data, int rows, int cols, mediapipe::Packet** packet_out) {
  TRY
    Eigen::Map<Eigen::MatrixXf> m(pcm_data, rows, cols);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(m)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeColMajorMatrixPacket_At__Pf_i_i_ll(float* pcm_data, int rows, int cols, int64 timestamp_microsec, mediapipe::Packet** packet_out) {
  TRY
    Eigen::Map<Eigen::MatrixXf> m(pcm_data, rows, cols);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(m).At(mediapipe::Timestamp(timestamp_microsec))};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__GetMpMatrix(mediapipe::Packet* packet, mp_api::Matrix* value_out) {
  TRY
    auto matrix = packet->Get<mediapipe::Matrix>();
    auto rows = matrix.rows();
    auto cols = matrix.cols();
    auto data = matrix.data();
    auto len = rows * cols;

    value_out->rows = rows;
    value_out->cols = cols;
    if (matrix.IsRowMajor) {
      value_out->layout = mp_api::rowMajor;
    } else {
      value_out->layout = mp_api::colMajor;
    }
    value_out->data = new float[len];
    memcpy(value_out->data, data, len * sizeof(float));

    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Packet__ValidateAsMatrix(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::Matrix>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}


void mp_api_Matrix__delete(mp_api::Matrix matrix) {
  delete[] matrix.data;
}
