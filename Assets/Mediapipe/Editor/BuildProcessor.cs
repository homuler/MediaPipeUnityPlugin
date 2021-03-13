using UnityEditor;

[InitializeOnLoad]
class UnityEditorStartup {
  static UnityEditorStartup() {
    BuildPlayerWindow.RegisterBuildPlayerHandler(
      new System.Action<BuildPlayerOptions>(buildPlayerOptions => {
        CreateAssetBundles.BuildAllAssetBundles();
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(buildPlayerOptions);
      })
    );
  }
}
