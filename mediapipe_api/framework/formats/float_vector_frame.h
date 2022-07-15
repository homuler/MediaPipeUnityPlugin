// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_FORMATS_FLOAT_VECTOR_FRAME_H_
#define MEDIAPIPE_API_FRAMEWORK_FORMATS_FLOAT_VECTOR_FRAME_H_

#include "mediapipe_api/framework/formats/float_vector_frame.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/external/protobuf.h"
#include "mediapipe_api/framework/packet.h"


extern "C" {

MP_CAPI(MpReturnCode) mp__MakeFloatVectorFramePacket__PA_i(const float* value, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeFloatVectorFramePacket_At__PA_i_Rt(const float* value, int size, mediapipe::Timestamp* timestamp,
                                                               mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetFloatVectorFrame(mediapipe::Packet* packet, const float** value_out);
MP_CAPI(void) mp_FloatVectorFrame__delete(float* vector_data);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_FORMATS_FLOAT_VECTOR_FRAME_H_
