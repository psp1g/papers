# Setup

These instructions assume you have Windows installed and have your own legal copy of "Papers, Please" on Steam

- Install latest Steam version of [_Papers, Please_](https://store.steampowered.com/app/239030/Papers_Please/)
- Download and run the [**PSP Paper Mod Installer**](https://github.com/psp1g/papers/releases)
  - If not installed in the default steam games directory (in Windows Program Files) you must manually specify the game directory
- The installer will automatically pull and compile the latest version of the mod
- Follow prompts and configure the twitch auth settings in the installer
- Configure the rest of the mod to your liking
- **Done!**

## Getting a Temporary Twitch API token:

- Create an app on twitch: https://dev.twitch.tv/console/apps/
- Specify `http://localhost:3000` as the OAuth Redirect URL
- Log into your bot on the twitch site
- Navigate to `https://id.twitch.tv/oauth2/authorize?response_type=code&client_id=YOURCLIENTIDHERE&redirect_uri=http://localhost:3000&scope=chat:read+chat:edit+moderator:manage:banned_users+channel:manage:predictions&state=pspHappy123`
- Copy the value of the `code` parameter
- Make a POST request to get the oauth token:
  ```sh
  curl -X POST "https://id.twitch.tv/oauth2/token" -H "Content-Type: application/x-www-form-urlencoded" \
    -d "client_id=<YOUR CLIENT ID>&client_secret=<YOUR CLIENT SECRET>&code=<CODE FROM PREV STEP>&grant_type=authorization_code&redirect_uri=http://localhost:3000"
  ```
- Done!

### Notes

- If a chatter is banned or timed out while they are the current chatter (ie. by a mod or nightbot for saying naughty things),
  they will no longer be eligible to be the active chatter again during the session
- Chatters who were recently active chatters have a much lower chance of becoming an chatter again
- Chatters who were denied entry have slightly lower chance of becoming an active chatter again
- Chatters who type more frequently in chat have a higher chance of being selected as an active chatter
- Chatters who do not type in chat will never be selected as the active chatter until they do
- Moderators can optionally be "nerfed" since they can send messages more often (MODS FYOUcat)