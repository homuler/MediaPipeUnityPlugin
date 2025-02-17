# Logging

To debug the application, it is useful to enable logging.

By default, the plugin outputs INFO level logs.\
You can change the log level at runtime as follows:

```cs
Mediapipe.Unity.Logger.MinLogLevel = Logger.LogLevel.Debug;
```

Additionally, MediaPipe uses [Google Logging Library(glog)](https://github.com/google/glog) internally.\
You can configure it by [setting flags](https://github.com/google/glog#setting-flags).

```cs
Glog.Logtostderr = true; // when true, log will be output to `Editor.log` / `Player.log`
Glog.Minloglevel = 0; // output INFO logs
Glog.V = 3; // output more verbose logs
```

To enable those flags, call `Glog.Initialize` when the application starts.

```cs
Glog.Initialize("MediaPipeUnityPlugin");
```

> :skull_and_crossbones: `Glog.Initialize` can be called only once. The second call will crash your application or UnityEditor.

> :bulb: If you look closely at `Editor.log`, you will notice the following warning log output.\
>
> ```txt
> WARNING: Logging before InitGoogleLogging() is written to STDERR
> ```
>
> To suppress it, you need to call `Glog.Initialize`.\
> However, without setting `true` to `Glog.Logtostderr`, glog won't output to `Editor.log` / `Player.log`.
