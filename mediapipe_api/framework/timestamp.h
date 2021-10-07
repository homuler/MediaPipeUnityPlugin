// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#ifndef MEDIAPIPE_API_FRAMEWORK_TIMESTAMP_H_
#define MEDIAPIPE_API_FRAMEWORK_TIMESTAMP_H_

#include "mediapipe/framework/timestamp.h"
#include "mediapipe_api/common.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Timestamp__l(int64 timestamp, mediapipe::Timestamp** timestamp_out);
MP_CAPI(void) mp_Timestamp__delete(mediapipe::Timestamp* timestamp);
MP_CAPI(int64) mp_Timestamp__Value(mediapipe::Timestamp* timestamp);
MP_CAPI(double) mp_Timestamp__Seconds(mediapipe::Timestamp* timestamp);
MP_CAPI(int64) mp_Timestamp__Microseconds(mediapipe::Timestamp* timestamp);
MP_CAPI(MpReturnCode) mp_Timestamp__DebugString(mediapipe::Timestamp* timestamp, const char** str_out);
MP_CAPI(bool) mp_Timestamp__IsSpecialValue(mediapipe::Timestamp* timestamp);
MP_CAPI(bool) mp_Timestamp__IsRangeValue(mediapipe::Timestamp* timestamp);
MP_CAPI(bool) mp_Timestamp__IsAllowedInStream(mediapipe::Timestamp* timestamp);
MP_CAPI(MpReturnCode) mp_Timestamp__NextAllowedInStream(mediapipe::Timestamp* timestamp, mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp__PreviousAllowedInStream(mediapipe::Timestamp* timestamp, mediapipe::Timestamp** timestamp_out);

MP_CAPI(MpReturnCode) mp_Timestamp_FromSeconds__d(double seconds, mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_Unset(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_Unstarted(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_PreStream(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_Min(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_Max(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_PostStream(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_OneOverPostStream(mediapipe::Timestamp** timestamp_out);
MP_CAPI(MpReturnCode) mp_Timestamp_Done(mediapipe::Timestamp** timestamp_out);

}  // extern "C"

#endif  // MEDIAPIPE_API_FRAMEWORK_TIMESTAMP_H_
