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

    #region --- Versioning ---

    [Parameter("Overrides the branch name from git.")]
    readonly string BranchName;

    [Parameter("Sets the third number in the version 1.2.X.4")]
    readonly string BuildNumber = "0";

    private string Version
    {
        get
        {
            string version = System.IO.File.ReadAllText("version.info");
            version += "." + BuildNumber.ToString();

            if (!string.IsNullOrWhiteSpace(BranchName) && BranchName != "main")
            {
                version += "-" + BranchName.Replace("/", "-");
            }

            return version;
        }
    }

    #endregion

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
        
    Target Pack => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetVersion(Version)
                .SetConfiguration("Release_DesktopGL")
                .SetProject("src/AgateLib/AgateLib.csproj"));

            DotNetPack(s => s
                .SetVersion(Version)
                .SetConfiguration("Release_WindowsDX")
                .SetProject("src/AgateLib/AgateLib.csproj"));
                
            DotNetPack(s => s
                .SetVersion(Version)
                .SetConfiguration("Release_Android")
                .SetProject("src/AgateLib/AgateLib.csproj"));
        });

    #region --- Publish ---

    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() => 
        {
            GlobFiles(ArtifactsDirectory, "**/*.nupkg")
               .NotEmpty()
               .ForEach(x =>
               {
                   if (BranchName != "main" && BranchName != "devel") 
                   {
                       Console.WriteLine($"File {x} is not designated for pushing as release or prerelease Nuget package.");
                       return;
                   }

                   DotNetNuGetPush(s => s
                       .SetTargetPath(x)
                       .SetSource(NugetApiUrl)
                       .SetApiKey(NugetApiKey)
                   );
               });
        });
        
    [Parameter("The NuGet API key for publishing")]
    readonly string NugetApiKey = "";

    [Parameter] 
    readonly string NugetApiUrl = "https://api.nuget.org/v3/index.json"; //default

    #endregion

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