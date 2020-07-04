using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Builder : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Builder>(x => x.Build);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly AgateLibConfiguration Configuration = AgateLibConfiguration.Debug;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });


    Target Test => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration("Test")
                .EnableNoRestore());
        });

    Target Build => _ => _
        .DependsOn(Build_WindowsDX)
        .DependsOn(Build_DesktopGL);

    Target Build_WindowsDX => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration("Release_WindowsDX")
                .EnableNoRestore());
        });
        
    Target Build_DesktopGL => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration("Release_DesktopGL")
                .EnableNoRestore());
        });
        
    // Target Build_Android => _ => _
    //     .DependsOn(Test)
    //     .Executes(() =>
    //     {
    //         MsBuild(s => s
    //             .SetProjectFile(Solution)
    //             .SetConfiguration("Release_Android")
    //             .EnableNoRestore());
    //     });
}


public enum AgateLibConfiguration
{
    Debug,
    Release_WindowsDX,
    Release_DesktopGL,
    Release_WindowsUniversal,
    Release_Android
}