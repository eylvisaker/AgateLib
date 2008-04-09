echo "Building AgateLib.dll..."

call gmcs  -out:bin\mono\AgateLib.dll -t:library -r:System.Data -r:System.Drawing -r:System.Windows.Forms -unsafe src\*.cs src\ImplBase\*.cs src\PlatformSpecific\*.cs src\Utility\*.cs src\Properties\*.cs

echo "Building AgateDrawing.dll..."

call gmcs -out:bin\mono\AgateDrawing.dll -t:library -r:bin\mono\AgateLib.dll -r:System.Drawing -r:System.Windows.Forms drivers\AgateDrawing\*.cs drivers\AgateDrawing\Properties\*.cs

