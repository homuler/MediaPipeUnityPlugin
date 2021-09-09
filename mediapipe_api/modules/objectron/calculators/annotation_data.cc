#include "mediapipe_api/modules/objectron/calculators/annotation_data.h"

MpReturnCode mp_Packet__GetFrameAnnotation(mediapipe::Packet* packet, mp_api::SerializedProto* value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::FrameAnnotation>(packet, value_out);
}
