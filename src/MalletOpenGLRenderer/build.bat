@echo off

set compiler_flags=-MTd -nologo -Gm- -GR- -EHa- -Oi -WX -W4 -wd4201 -wd4100 -wd4189 -wd4505 -FC -Z7
set linker_flags=-incremental:no -opt:ref user32.lib Gdi32.lib opengl32.lib

IF NOT EXIST .\data mkdir .\data
IF NOT EXIST .\build mkdir .\build

pushd .\build

del *.pdb > NUL 2> NUL

cl %compiler_flags% ..\code\windows_main.cpp /link %linker_flags% -OUT:mallet_by_opengl.exe

popd
