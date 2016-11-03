makecert.exe ^
-n "CN=AppSwitch" ^
-r ^
-pe ^
-a sha512 ^
-len 2048 ^
-cy authority ^
-sv AppSwitch.pvk ^
-sr LocalMachine ^
-ss Root ^
AppSwitch.cer 

pvk2pfx.exe ^
-pvk AppSwitch.pvk ^
-spc AppSwitch.cer ^
-pfx AppSwitch.pfx ^
-po 123