#include "mediapipe_api/gpu/gl_context.h"

void MpGlContextDestroy(MpGlContext* gl_context) {
  delete gl_context;
}

MpGlContext* MpGlContextGetCurrent() {
  return new MpGlContext { mediapipe::GlContext::GetCurrent() };
}

mediapipe::GlContext* MpGlContextGet(MpGlContext* gl_context) {
  return gl_context->impl.get();
}

void MpGlSyncTokenDestroy(MpGlSyncToken* token) {
  delete token;
}

void MpGlSyncTokenWait(MpGlSyncToken* token) {
  token->impl->Wait();
}

void MpGlSyncTokenWaitOnGpu(MpGlSyncToken* token) {
  token->impl->WaitOnGpu();
}

bool MpGlSyncTokenIsReady(MpGlSyncToken* token) {
  return token->impl->IsReady();
}

MpGlContext* MpGlSyncTokenGetContext(MpGlSyncToken* token) {
  auto context = token->impl->GetContext();

  return new MpGlContext { context };
}
