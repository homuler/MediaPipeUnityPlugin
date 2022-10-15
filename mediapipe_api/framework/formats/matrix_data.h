// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_DATA_H_
#define MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_DATA_H_

#include "mediapipe/framework/formats/matrix.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp__MakeMatrixPacket__PKc_i(const char* matrix_data_serialized, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeMatrixPacket_At__PKc_i_Rt(const char* matrix_data_serialized, int size, mediapipe::Timestamp* timestamp,
                                                        mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetMatrix(mediapipe::Packet* packet, mp_api::SerializedProto* value_out);
MP_CAPI(MpReturnCode) mp_Packet__ValidateAsMatrix(mediapipe::Packet* packet, absl::Status** status_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_FORMATS_MATRIX_DATA_H_
