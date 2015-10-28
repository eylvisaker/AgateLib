param(
	$installPath, 
	$toolsPath, 
	$package, 
	$project
)

$filenames = "libFLAC-8.dll", "libmikmod-2.dll", "libmodplug-1.dll", "libogg-0.dll", "libvorbis-0.dll", "libvorbisfile-3.dll", "SDL2.dll", "SDL2_mixer.dll", "smpeg2.dll", "LICENSE.FLAC.txt", "LICENSE.mikmod.txt", "LICENSE.modplug.txt", "LICENSE.smpeg.txt", "README-SDL.txt", "README-SDL_mixer.txt"
$libfolders = "lib32", "lib64"

ForEach ($libfolder in $libfolders) 
{
	$folderItem = $project.ProjectItems.AddFolder($libfolder);
	
	if ($folderItem -eq $null) 
	{
		"Failed to find folder $libfolder.";
		exit 1;
	}
	
	ForEach ($filename in $filenames) 
	{
		$path = ("{0}\{1}\{2}" -f $toolsPath,$libfolder,$filename);
		"$path exists: $(test-path $path)"

		$file = $folderItem.ProjectItems.AddFromFile($filename);
		
		if ($file -ne $null)
		{
			$copyToOutput = $file.Properties.Item("CopyToOutputDirectory");
			$copyToOutput.Value = 2;
			"Set CopyToOutputDirectory of $path to 2."
		}
		else 
		{
			"Failed to find $path"
		}
	}
}

$project.Save()
