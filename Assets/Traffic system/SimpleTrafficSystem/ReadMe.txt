Support and Discussion Forum: https://forum.unity.com/threads/.794268/
Contact: stephen@turnthegameon.com


This package uses the new C# Job System and Burst Compiler, make the following project configurations to enable these Unity features.

1. Open the Player Settings (Edit -> Project Settings -> Player), set API Compatibility Level .Net 4.x

2. Open the Package Manager (Window -> Package Manager), install Burst (1.2.3)

3. To use the burst compiler in standalone builds, install the Windows SDK and VC++ toolkit from the Visual Studio Installer.