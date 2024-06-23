// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_H_
#define MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_H_

#include "mediapipe/framework/formats/matrix.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

namespace mp_api {
  constexpr int colMajor = 0;
  constexpr int rowMajor = 1;

  struct Matrix {
    float* data;
    int rows;
    int cols;
    int layout;
  };
}

extern "C" {

MP_CAPI(MpReturnCode) mp__MakeColMajorMatrixPacket__Pf_i_i(float* pcm_data, int rows, int cols, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeColMajorMatrixPacket_At__Pf_i_i_ll(float* pcm_data, int rows, int cols, int64_t timestamp_microsec, mediapipe::Packet** packet_out);

MP_CAPI(MpReturnCode) mp_Packet__GetMpMatrix(mediapipe::Packet* packet, mp_api::Matrix* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsMatrix(mediapipe::Packet* packet, absl::Status** status_out);

MP_CAPI(void) mp_api_Matrix__delete(mp_api::Matrix matrix);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_H_
