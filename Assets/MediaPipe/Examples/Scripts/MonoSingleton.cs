using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour {
  private static bool m_ShuttingDown = false;
  private static readonly object m_Lock = new object();
  private static T m_Instance;

  public static T Instance {
    get {
      if (m_ShuttingDown) {
        Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
        return null;
      }

      lock (m_Lock) {
        if (m_Instance == null) {
          // Search for existing instance.
          m_Instance = (T)FindObjectOfType(typeof(T));

          // Create new instance if one doesn't already exist.
          if (m_Instance == null) {
            // Need to create a new GameObject to attach the singleton to.
            var singletonObject = new GameObject();
            m_Instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T).ToString() + " (Singleton)";

            // Make instance persistent.
            DontDestroyOnLoad(singletonObject);
          }
        }

        return m_Instance;
      }
    }
  }

  private void OnApplicationQuit() {
    m_ShuttingDown = true;
  }

  private void OnDestroy() {
    m_ShuttingDown = true;
  }
}
