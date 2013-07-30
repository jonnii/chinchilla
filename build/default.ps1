properties {
	$buildName = "dev"
}

task default -depends Package

task PreparePackage -depends Test {
	$packagesDirectory = '..\targets\packages'
	
	if (Test-Path -path $packagesDirectory)
	{
		rm $packagesDirectory -recurse -force
	}

	mkdir "$packagesDirectory\chinchilla"
	cp ..\build\package.nuspec ..\targets\packages\chinchilla

	mkdir ..\targets\packages\chinchilla\lib\net40
	cp ..\targets\chinchilla\chinchilla.* ..\targets\packages\chinchilla\lib\net40

	mkdir "$packagesDirectory\chinchilla.api"
	cp ..\build\package.api.nuspec ..\targets\packages\chinchilla.api

	mkdir ..\targets\packages\chinchilla.api\lib\net40
	cp ..\targets\chinchilla.api\chinchilla.api* ..\targets\packages\chinchilla.api\lib\net40
	
	mkdir "$packagesDirectory\chinchilla.logging.log4net"
	cp ..\build\package.logging.log4net.nuspec ..\targets\packages\chinchilla.logging.log4net

	mkdir ..\targets\packages\chinchilla.logging.log4net\lib\net40
	cp ..\targets\chinchilla.logging.log4net\chinchilla.logging.log4net* ..\targets\packages\chinchilla.logging.log4net\lib\net40

	mkdir "$packagesDirectory\chinchilla.logging.nlog"
	cp ..\build\package.logging.nlog.nuspec ..\targets\packages\chinchilla.logging.nlog

	mkdir ..\targets\packages\chinchilla.logging.nlog\lib\net40
	cp ..\targets\chinchilla.logging.nlog\chinchilla.logging.nlog* ..\targets\packages\chinchilla.logging.nlog\lib\net40
}

task PackagePre -depends PreparePackage {
	# update nuspec package
	$version = get-content ..\VERSION
	$when = (get-date).ToString("yyyyMMddHHmmss")
	$packageVersion = "$version-$buildName-$when"

	get-content ..\targets\packages\chinchilla\package.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla\package.nuspec.tmp
	
	mv ..\targets\packages\chinchilla\package.nuspec.tmp ..\targets\packages\chinchilla\package.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla\package.nuspec" -outputdirectory ".\..\targets\packages"

	get-content ..\targets\packages\chinchilla.api\package.api.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla.api\package.api.nuspec.tmp
	
	mv ..\targets\packages\chinchilla.api\package.api.nuspec.tmp ..\targets\packages\chinchilla.api\package.api.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla.api\package.api.nuspec" -outputdirectory ".\..\targets\packages"
	
	get-content ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec.tmp
	
	mv ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec.tmp ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec" -outputdirectory ".\..\targets\packages"
	
	get-content ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec.tmp
	
	mv ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec.tmp ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec" -outputdirectory ".\..\targets\packages"
}

task Package -depends PreparePackage {
	# update nuspec package
	$version = get-content ..\VERSION
	$packageVersion = "$version"

	get-content ..\targets\packages\chinchilla\package.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla\package.nuspec.tmp
	
	mv ..\targets\packages\chinchilla\package.nuspec.tmp ..\targets\packages\chinchilla\package.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla\package.nuspec" -outputdirectory ".\..\targets\packages"	

	get-content ..\targets\packages\chinchilla.api\package.api.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla.api\package.api.nuspec.tmp
	
	mv ..\targets\packages\chinchilla.api\package.api.nuspec.tmp ..\targets\packages\chinchilla.api\package.api.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla.api\package.api.nuspec" -outputdirectory ".\..\targets\packages"	
	
	get-content ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec.tmp
	
	mv ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec.tmp ..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla.logging.log4net\package.logging.log4net.nuspec" -outputdirectory ".\..\targets\packages"	
	
	get-content ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec | 
        %{$_ -replace '0.0.0.1', $packageVersion } > ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec.tmp
	
	mv ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec.tmp ..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec -force

	..\src\.nuget\nuget.exe pack "..\targets\packages\chinchilla.logging.nlog\package.logging.nlog.nuspec" -outputdirectory ".\..\targets\packages"	
}

task Publish -depends Package {
	gci .\..\targets\packages\*.nupkg | foreach { ..\src\.nuget\nuget.exe Push $_.fullname }
}

task PublishPre -depends PackagePre {
	gci .\..\targets\packages\*.nupkg | foreach { ..\src\.nuget\nuget.exe Push $_.fullname }
}

task CopyTools {
	
}

task Test -depends Clean,CopyTools,Compile { 
  # run specs
  ..\src\packages\Machine.Specifications.0.5.12\tools\mspec-clr4.exe .\..\targets\specifications\chinchilla.specifications.dll
  ..\src\packages\Machine.Specifications.0.5.12\tools\mspec-clr4.exe .\..\targets\specifications\chinchilla.api.specifications.dll

  # run integration tests somehow (need a local nunit-console.exe, or switch to xunit?)
}

task UpdateAssemblyInfo {
	$version = get-content ..\VERSION

	$assemblyVersion = 'AssemblyVersion("' + $version + '")';
	$assemblyFileVersion = 'AssemblyFileVersion("' + $version + '")';

	get-content ..\src\CommonAssemblyInfo.cs | 
        %{$_ -replace 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $assemblyVersion } |
        %{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $assemblyFileVersion }  > ..\src\CommonAssemblyInfo.cs.tmp
	
	mv ..\src\CommonAssemblyInfo.cs.tmp ..\src\CommonAssemblyInfo.cs -force
}

task Compile -depends Clean,UpdateAssemblyInfo { 
  # call msbuild
  # /p:TargetFrameworkVersion=4.0
  # /p:TargetFrameworkVersion=Silverlight

  $options = "/p:Configuration=Release"
  $msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
  
  Push-Location ..\src
  $build = $msbuild + ' "chinchilla.sln" ' + $options + " /t:Build"

  iex $build
  Pop-Location
}

task Clean { 
  if (Test-Path -path '..\targets') {
	rm -recurse -force ..\targets
  }
}