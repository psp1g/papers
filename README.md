# PSP PAPERS PLS

This is a [Papers, Please](https://papersplea.se/) twitch integration mod for the
[Unity C# / IL2CPP version](https://store.steampowered.com/news/app/239030/view/3651894293966905793)
of the game, made for [PSP1G](https://www.twitch.tv/psp1g) by [LittleBigBug](https://github.com/LittleBigBug)

It integrates [Twitch](https://twitch.tv) chat with the game in endless mode, and randomly selects
a random chatter for each traveller.

This mod uses [BepInEx 6](https://github.com/BepInEx/BepInEx)

## Setup

- Install latest Steam version of _Papers, Please_
- Install [BepInEx Prerequisites](https://github.com/LavaGang/MelonLoader#requirements)
- Download [BepInEx 6 (BE) IL2CPP](https://builds.bepinex.dev/projects/bepinex_be)
(64 bit; last build used: [#679](https://builds.bepinex.dev/projects/bepinex_be/679/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.679%2B49775dc.zip))
- Drag and drop the contents of the ZIP file into your Papers, Please root directory
- Run the game once, then quit
- Edit the config file named `wtf.psp.papers.cfg` in the config directory of BepInEx
(ie. `C:\Program Files (x86)\Steam\steamapps\common\PapersPlease\BepInEx\config`)
to include bot credentials and your channel name.


Most of these should be straight forward.

`nerf_mod_weight`: whatever number this is will be multiplied against a moderator's 'weight' (chance)

(**ex:** 0.85 = 85% of what weight/chance they would usually have, 0 = no chance, 1 = normal chance, 2 = double chance (rigged))

### Notes

- **The first launch may take a while as it generates necessary DLLs**


- If a chatter is banned or timed out while they are the current chatter (ie. by a mod or nightbot for saying naughty things),
they will no longer be eligible to be the active chatter again during the session
- Chatters who were recently active chatters have a much lower chance of becoming an chatter again
- Chatters who were denied entry have slightly lower chance of becoming an active chatter again
- Chatters who type more frequently in chat have a higher chance of being selected as an active chatter
- Chatters who do not type in chat will never be selected as the active chatter until they do
- Moderators can optionally be "nerfed" since they can send messages more often (MODS FYOUcat)

## Development

- Install [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Install BepInEx 6 BE using the steps above.
- Launch the game with BepInEx installed so that all necessary DLLs can be generated.

Change the `PapersPleaseDir` MSBuild property (if needed) when building to point to the directory of the game
installation **without** trailing slashes.
The default is `C:\Program Files (x86)\Steam\steamapps\common\PapersPlease`.

This is necessary to link dependencies properly.