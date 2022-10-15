// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/formats/matrix_data.h"

MpReturnCode mp__MakeMatrixPacket__PKc_i(const char* serialized_matrix_data, int size, mediapipe::Packet** packet_out) {
  TRY
    auto matrix_data = ParseFromStringAsProto<mediapipe::MatrixData>(serialized_matrix_data, size);

    mediapipe::Matrix matrix;
    mediapipe::MatrixFromMatrixDataProto(matrix_data, &matrix);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(matrix)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp__MakeMatrixPacket_At__PKc_i_Rt(const char* serialized_matrix_data, int size, mediapipe::Timestamp* timestamp, mediapipe::Packet** packet_out) {
  TRY
    auto matrix_data = ParseFromStringAsProto<mediapipe::MatrixData>(serialized_matrix_data, size);

    mediapipe::Matrix matrix;
    mediapipe::MatrixFromMatrixDataProto(matrix_data, &matrix);

    *packet_out = new mediapipe::Packet{mediapipe::MakePacket<mediapipe::Matrix>(matrix).At(*timestamp)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MP_CAPI(MpReturnCode) mp_Packet__GetMatrix(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  TRY
    mediapipe::MatrixData matrix_data;
    auto matrix = packet->Get<mediapipe::Matrix>();
    mediapipe::MatrixDataProtoFromMatrix(matrix, &matrix_data);

    SerializeProto(matrix_data, value_out);

    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MP_CAPI(MpReturnCode) mp_Packet__ValidateAsMatrix(mediapipe::Packet* packet, absl::Status** status_out) {
  TRY
    *status_out = new absl::Status{packet->ValidateAsType<mediapipe::Matrix>()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
