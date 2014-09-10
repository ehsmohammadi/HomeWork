@echo off

set arg1=%1
set arg2=%2

set migrateComm=packages\FluentMigrator.1.3.0.0\tools\
set migrateComm1=packages\FluentMigrator.1.3.0.0\tools\migrate -a 
set migrateComm2=HomeWork.Persistance.Migrations\bin\Debug\HomeWork.Persistance.Migrations.dll 
set migrateComm3=-configPath packages\FluentMigrator.1.3.0.0\tools\Migrate.exe.config -db SqlServer2008 -connectionString 

set TestConn="HomeWorkDBConnection"
copy Migrate.exe.config  %migrateComm%
set DebugConn="HomeWorkDBConnection"

IF "%arg1%" == "test" ( 
	IF "%arg2%"=="u" (
	   GOTO TestMigrateUp
	) ELSE  GOTO TestMigrateDown 

) ELSE IF "%arg1%" == "debug" (
	  
	  IF "%arg2%" == "u" (
	    GOTO debugMigrateUp 
      ) ELSE GOTO debugMigrateDown  
    
) ElSE IF "%arg1%" == "both" (
	 
	  IF "%arg2%" == "u" ( 
	       GOTO bothMigrateUp 
      ) ELSE  GOTO bothMigrateDown 
)


:TestMigrateUp
echo "TestMigrateUp"
%migrateComm1% %migrateComm2%  %migrateComm3% %TestConn%    %3 %4
GOTO End

:TestMigrateDown
echo "TestMigrateDown"
%migrateComm1% %migrateComm2%  %migrateComm3%  %TestConn% -t migrate:down 
GOTO End

:DebugMigrateUp
echo "DebugMigrateUp"
%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn% -profile=%arg1%   %3 %4  
GOTO End

:DebugMigrateDown
echo "DebugMigrateDown"
%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn%  -t migrate:down 
GOTO End

:BothMigrateUp
echo "BothMigrateUp"
%migrateComm1% %migrateComm2%  %migrateComm3% %TestConn%   %3 %4
%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn%  %3 %4
GOTO End

:BothMigrateDown
echo "BothMigrateDown"
%migrateComm1% %migrateComm2%  %migrateComm3% %TestConn%  -t migrate:down 
%migrateComm1% %migrateComm2%  %migrateComm3% %DebugConn% -t migrate:down 
GOTO End


:End