# Development and Contributing

Read this for a quick-start tutorial to get started working on a "Papers, Please" mod.

# Development Set-up

## Using the Installer to help

You can [run the latest version of the installer](https://github.com/psp1g/papers/releases)
to quickly get set up with a lot of what you need to start development.

The installer will download .NET 8 SDK (`PapersPleaseDir/dotnet8sdk`) and compile the mod from source, and will make
the game generate hollowed assemblies that are needed for development.

### Development

First, `git clone` the repository in a separate folder. Please make a fork (or make a branch if possible) of
this repository to work on based on the [dev](https://github.com/psp1g/papers/tree/dev) branch.

The installer will also patch the game art assets with [papers-tools-rs](https://github.com/psp1g/papers-tools-rs) so you
don't have to run any of that manually.

If you need to work with patching art assets, take a look at the
[papers-tools-rs usage in the readme](https://github.com/psp1g/papers-tools-rs/blob/main/README.md).

## Full Manual Set-up

- Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Install [BepInEx 6](https://builds.bepinex.dev/projects/bepinex_be) in "Papers, Please", and launch the game once to
  generate required assemblies
- Change the `<PapersPleaseDir>` MSBuild property in your IDE/`psp-papers-mod.csproj` (if not in ProgramFiles) when
  building to point to the directory of the game
  installation **without** trailing slashes.
    - The default is `C:\Program Files (x86)\Steam\steamapps\common\PapersPlease`.
- Update NuGet packages: `dotnet restore` (your IDE may do this automatically)
- Build the solution, the output of the mod and dependency assemblies will be in `psp-papers-mod/bin/Debug/net6.0`
- Move all .dll files in that folder to `PapersPlease\BepInEx\plugins`
- To manually patch art assets, download and take a look at
  [papers-tools-rs](https://github.com/psp1g/papers-tools-rs).
- Configure the twitch auth tokens
- Done!

# Reverse Engineering / Ghidra Set-up 
 
A lot of modding this game requires use of reverse-engineering applications like Ghidra or IDA Pro.

Download one of the following:

- **[Ghidra](https://ghidra-sre.org/)** - Open source & free
- **[IDA Pro](https://hex-rays.com/ida-pro/)** - Paid

"Papers, Please" is a game written in Haxe originally for the OpenFL engine.

Recently, it was ported to Unity. So now it is Haxe compiled to C#, then compiled in Unity with
[il2cpp](https://docs.unity3d.com/Manual/IL2CPP.html). This makes modding this game very hard, but much more possible
now that we have more symbol information and Unity modding platforms than the OpenFL versions.

You can open `GameAssembly.dll` with Ghidra, and you can also run [Il2CppDumper](https://github.com/Perfare/Il2CppDumper)
on the game. This will provide some Ghidra scripts to map some function/class names in Ghidra.

You can also take a look at the hollowed assembly (no function code, but function/class names) by opening
`PapersPlease/BepInEx/interop/Assembly-CSharp.dll`

# Contributing

When making a pull request **ensure the base branch is [dev](https://github.com/psp1g/papers/tree/dev)**.

All code merges go into dev first and are later merged into [main](https://github.com/psp1g/papers/tree/main) along a
manually-reviewed release.

Please use and follow the included code style configs.