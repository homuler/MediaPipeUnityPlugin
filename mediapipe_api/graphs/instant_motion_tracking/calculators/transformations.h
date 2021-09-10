#ifndef C_MEDIAPIPE_API_GRAPHS_INSTANT_MOTION_TRACKING_CALCULATORS_TRANSFORMATIONS_H_
#define C_MEDIAPIPE_API_GRAPHS_INSTANT_MOTION_TRACKING_CALCULATORS_TRANSFORMATIONS_H_

#include "mediapipe/graphs/instant_motion_tracking/calculators/transformations.h"
#include "mediapipe_api/common.h"
#include "mediapipe_api/framework/packet.h"

extern "C" {

MP_CAPI(MpReturnCode) mp__MakeAnchor3dVectorPacket___PA_i(const mediapipe::Anchor3d* value, int size, mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp__MakeAnchor3dVectorPacket_At__PA_i_Rt(const mediapipe::Anchor3d* value,
                                                                                  int size,
                                                                                  mediapipe::Timestamp* timestamp,
                                                                                  mediapipe::Packet** packet_out);
MP_CAPI(MpReturnCode) mp_Packet__GetAnchor3dVector(mediapipe::Packet* packet, mp_api::StructArray<mediapipe::Anchor3d>* value_out);
MP_CAPI(void) mp_Anchor3dArray__delete(mediapipe::Anchor3d* anchor_vector_data);

}  // extern "C"

#endif  // C_MEDIAPIPE_API_GRAPHS_INSTANT_MOTION_TRACKING_CALCULATORS_TRANSFORMATIONS_H_
