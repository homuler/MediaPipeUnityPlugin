package com.github.homuler.mediapipe;

import android.os.Bundle;
import android.util.Log;
import com.google.mediapipe.framework.AndroidAssetUtil;
import com.unity3d.player.UnityPlayerActivity;

public class MediaPipeUnityPlayerActivity extends UnityPlayerActivity {
  static {
    // Load all native libraries needed by the app.
    System.loadLibrary("mediapipe_jni");
  }

  protected void onCreate(Bundle savedInstanceState) {
    super.onCreate(savedInstanceState);

    // Initialize asset manager so that MediaPipe native libraries can access the app assets, e.g., binary graphs.
    boolean res = AndroidAssetUtil.initializeNativeAssetManager(this);
  }
}
