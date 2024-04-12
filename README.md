# PSP PAPERS PLS

This is a [Papers, Please](https://papersplea.se/) twitch integration mod for the
[Unity C# / IL2CPP version](https://store.steampowered.com/news/app/239030/view/3651894293966905793)
of the game, made for [PSP1G](https://www.twitch.tv/psp1g) 

It integrates [Twitch](https://twitch.tv) chat with the game, and randomly selects a random chatter for each traveller.

If they get denied at the border, they get banned in chat.

### GLORY TO [SUSUSTERJA](https://sususterja.org)

## Setup

- Install latest Steam version of [_Papers, Please_](https://store.steampowered.com/app/239030/Papers_Please/)
- Download and run the [**PSP Paper Mod Installer**](https://github.com/psp1g/papers/releases)
- The installer will automatically pull and compile the latest version of the mod
- Follow prompts and configure the twitch settings in the installer
- **Done!**

### Notes

- If a chatter is banned or timed out while they are the current chatter (ie. by a mod or nightbot for saying naughty things),
they will no longer be eligible to be the active chatter again during the session
- Chatters who were recently active chatters have a much lower chance of becoming an chatter again
- Chatters who were denied entry have slightly lower chance of becoming an active chatter again
- Chatters who type more frequently in chat have a higher chance of being selected as an active chatter
- Chatters who do not type in chat will never be selected as the active chatter until they do
- Moderators can optionally be "nerfed" since they can send messages more often (MODS FYOUcat)

### Getting a Twitch API token:

- Create an app on twitch: https://dev.twitch.tv/console/apps/
- Specify `http://localhost:3000` as the OAuth Redirect URL
- Log into your bot on twitch
- Navigate to `https://id.twitch.tv/oauth2/authorize?response_type=code&client_id=YOURCLIENTIDHERE&redirect_uri=http://localhost:3000&scope=chat:read+chat:edit+moderator:manage:banned_users+channel:manage:predictions&state=pspHappy123`
- Copy the value of the `code` parameter
- Make a POST request to get the oauth token:
  ```sh
  curl -X POST "https://id.twitch.tv/oauth2/token" -H "Content-Type: application/x-www-form-urlencoded" \
    -d "client_id=<YOUR CLIENT ID>&client_secret=<YOUR CLIENT SECRET>&code=<CODE FROM PREV STEP>&grant_type=authorization_code&redirect_uri=http://localhost:3000"
  ```
- Done!

## Development Set-up

- Install [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Install the mod at least once with the [installer](https://github.com/psp1g/papers/releases) to generate referenced assemblies
  - (Or you can manually install [BepInEx 6](https://builds.bepinex.dev/projects/bepinex_be), and launch the game once to generate assemblies)
- Change the `<PapersPleaseDir>` MSBuild property in `psp-papers-mod.csproj` (if needed) when building to point to the directory of the game
installation **without** trailing slashes.
  - The default is `C:\Program Files (x86)\Steam\steamapps\common\PapersPlease`.
- Update NuGet packages: `dotnet restore`
- Build the solution, the output of the mod and dependency assemblies will be in `psp-papers-mod/bin/Debug/net6.0`
- Move all .dll files in that folder to `PapersPlease\BepInEx\plugins`