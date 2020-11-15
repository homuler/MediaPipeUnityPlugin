#include "mediapipe_api/framework/formats/classification.h"

MpReturnCode mp_Packet__GetClassificationList(mediapipe::Packet* packet, mp_api::SerializedProto** value_out) {
  return mp_Packet__GetSerializedProto<mediapipe::ClassificationList>(packet, value_out);
}
