def _dotnet_library_impl(ctx):
    solution_file = ctx.file.solution_file
    workspace_root = ctx.attr.solution_file.label.workspace_root
    outputs = [ctx.actions.declare_file(path) for path in ctx.attr.outs]
    copy_commands = [
      "cp {} {}".format(workspace_root + "/" + actual_path, output.path) for (actual_path, output) in zip(ctx.attr.outs, outputs)
    ]

    ctx.actions.run_shell(
        inputs = ctx.files.srcs,
        outputs = outputs,
        arguments = [solution_file.path],
        command = "\n".join(["dotnet restore $1", "dotnet build -c Release $1"] + copy_commands),
        progress_message = "building {}...".format(solution_file.basename),
        use_default_shell_env = True,
    )

    return [
      DefaultInfo(files = depset(outputs)),
    ]

dotnet_library = rule(
    implementation = _dotnet_library_impl,
    attrs = {
        "srcs": attr.label_list(allow_files = True),
        "outs": attr.string_list(default = []),
        "solution_file": attr.label(allow_single_file = True),
    },
)
