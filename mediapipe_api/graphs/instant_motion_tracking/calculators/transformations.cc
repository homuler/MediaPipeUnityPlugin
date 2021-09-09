#include "mediapipe_api/graphs/instant_motion_tracking/calculators/transformations.h"

MpReturnCode mp_Packet__GetInstantMotionTrackingAnchorVector(mediapipe::Packet* packet, mp_api::StructArray<mediapipe::Anchor>* value_out) {
  return mp_Packet__GetStructVector<mediapipe::Anchor>(packet, value_out);
}

void mp_InstantMotionTrackingAnchorArray__delete(mediapipe::Anchor* anchor_vector_data) {
  delete[] anchor_vector_data;
}
