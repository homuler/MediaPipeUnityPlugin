#ifndef C_MEDIAPIPE_API_GRAPHS_INSTANT_MOTION_TRACKING_CALCULATORS_TRANSFORMATIONS_H_
#define C_MEDIAPIPE_API_GRAPHS_INSTANT_MOTION_TRACKING_CALCULATORS_TRANSFORMATIONS_H_

#include "mediapipe/graphs/instant_motion_tracking/calculators/transformations.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp_Packet__GetInstantMotionTrackingAnchorVector(mediapipe::Packet* packet, mp_api::StructArray<mediapipe::Anchor>* value_out);
MP_CAPI(void) mp_InstantMotionTrackingAnchorArray__delete(mediapipe::Anchor* anchor_vector_data);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GRAPHS_INSTANT_MOTION_TRACKING_CALCULATORS_TRANSFORMATIONS_H_
