// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_FRAME_H_
#define MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_FRAME_H_

#include "mediapipe_api/framework/formats/matrix_frame.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"
#include "mediapipe/framework/formats/matrix.h"
#include "mediapipe/framework/port/file_helpers.h"

extern "C" {

MP_CAPI(MpReturnCode) mp__MakeMatrixFramePacket__PKc_i(const char* matrix_data_serialized, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeMatrixFramePacket_At__PA_i_Rt(const char* matrix_data_serialized, int size, mediapipe::Timestamp* timestamp,
                                                               mediapipe::Packet** packet_out);
// MP_CAPI(MpReturnCode) mp_Packet__GetMatrixFrame(mediapipe::Packet* packet, const mediapipe::Matrix** value_out);
// MP_CAPI(void) mp_MatrixFrame__delete(mediapipe::Matrix* matrix);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_FRAME_H_
