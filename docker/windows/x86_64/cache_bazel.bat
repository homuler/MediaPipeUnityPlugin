SET /A tries=4

:repeat
IF %tries% LEQ 0 GOTO return

SET /A tries-=1
bazel --version && (GOTO return) || (GOTO repeat)

:return
EXIT /B
