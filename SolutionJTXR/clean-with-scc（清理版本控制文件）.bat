del /s /ah /f *.suo
del /s /f *.user
del /s /f *.cache
del /s /f *.scc
del /s /f *.vssscc
del /s /f *.vspscc
del /s /f *.keep
del /s /ah /f vssver2.scc
del /s /ah StyleCop.Cache
del /s /ah .hgignore

rd /s /q bin obj ClientBin _Resharper.* _Upgrade* TestResults .svn .hg .vs

del dirs.txt
dir /s /b /ad bin > dirs.txt
dir /s /b /ad obj >> dirs.txt
dir /s /b /ad ClientBin >> dirs.txt
dir /s /b /ad _Resharper.* >> dirs.txt
dir /s /b /ad _Upgrade* >> dirs.txt
dir /s /b /ad TestResults >> dirs.txt
dir /s /b /ad .svn >> dirs.txt
dir /s /b /ad .hg >> dirs.txt

for /f "delims=;" %%i in (dirs.txt) DO rd /s /q "%%i"
del dirs.txt
