// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#include "mediapipe_api/tasks/cc/vision/face_geometry/proto/face_geometry.h"

MpReturnCode mp_Packet__GetFaceGeometry(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::tasks::vision::face_geometry::proto::FaceGeometry>(packet, value_out);
}

MpReturnCode mp_Packet__GetFaceGeometryVector(mediapipe::Packet* packet, mp_api::StructArray<mp_api::SerializedProto>* value_out) {
  return mp_Packet__GetSerializedProtoVector<mediapipe::tasks::vision::face_geometry::proto::FaceGeometry>(packet, value_out);
}
