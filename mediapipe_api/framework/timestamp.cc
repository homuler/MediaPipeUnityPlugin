// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/framework/timestamp.h"

MpReturnCode mp_Timestamp__l(int64 timestamp, mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp(timestamp);
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

void mp_Timestamp__delete(mediapipe::Timestamp* timestamp) { delete timestamp; }

int64 mp_Timestamp__Value(mediapipe::Timestamp* timestamp) { return timestamp->Value(); }

double mp_Timestamp__Seconds(mediapipe::Timestamp* timestamp) { return timestamp->Seconds(); }

int64 mp_Timestamp__Microseconds(mediapipe::Timestamp* timestamp) { return timestamp->Microseconds(); }

MpReturnCode mp_Timestamp__DebugString(mediapipe::Timestamp* timestamp, const char** str_out) {
  TRY
    *str_out = strcpy_to_heap(timestamp->DebugString());
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

bool mp_Timestamp__IsSpecialValue(mediapipe::Timestamp* timestamp) { return timestamp->IsSpecialValue(); }

bool mp_Timestamp__IsRangeValue(mediapipe::Timestamp* timestamp) { return timestamp->IsRangeValue(); }

bool mp_Timestamp__IsAllowedInStream(mediapipe::Timestamp* timestamp) { return timestamp->IsAllowedInStream(); }

MpReturnCode mp_Timestamp__NextAllowedInStream(mediapipe::Timestamp* timestamp, mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{timestamp->NextAllowedInStream()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp__PreviousAllowedInStream(mediapipe::Timestamp* timestamp, mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{timestamp->PreviousAllowedInStream()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_FromSeconds__d(double seconds, mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::FromSeconds(seconds)};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_Unset(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::Unset()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_Unstarted(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::Unstarted()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_PreStream(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::PreStream()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_Min(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::Min()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_Max(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::Max()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_PostStream(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::PostStream()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_OneOverPostStream(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::OneOverPostStream()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}

MpReturnCode mp_Timestamp_Done(mediapipe::Timestamp** timestamp_out) {
  TRY
    *timestamp_out = new mediapipe::Timestamp{mediapipe::Timestamp::Done()};
    RETURN_CODE(MpReturnCode::Success);
  CATCH_EXCEPTION
}
