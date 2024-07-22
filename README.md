# PSP PAPERS PLS

This is a [Papers, Please](https://papersplea.se/) twitch integration mod for the
[Unity C# / IL2CPP version](https://store.steampowered.com/news/app/239030/view/3651894293966905793)
of the game, made for [PSP1G](https://www.twitch.tv/psp1g) 

It integrates [Twitch](https://twitch.tv) chat with the game, and randomly selects a random chatter for each traveller.

If they get denied, shot, or detained at the border, they get banned in chat.

#### [GLORY TO SUSUSTERJA](https://sususterja.org)

## [Mod Setup / Install](Setup.md)

## Features

- Automated installer/updater
- Endless Chat Mode
- Custom countries/passports
  - Finland, Sweden, Estonia, Latvia, Lithuania, Belarus, Poland, and **Sususterja!**
- Random Chatters assigned as the travelers
  - Most active chatters are selected
  - Chatters who have typed more recently/more often have higher chance to be selected
  - Chatters who have been active chatters before have less chance of being active chatters again
    - Chatters who are shot, detained, and approved are not selected again
    - Chatters who were selected as attackers aren't selected again
    - After a new game, the above is all reset
    - Chatters who were banned while they were active chatters leave immediately and are never selected again (even after reset)
  - Active chatters who sub/gift subs or donate through twitch (ie bits) actually bribe the streamer with in game money
  - Active chatters or mods can run `!leave` to make the current chatter leave the booth
  - Moderators/Streamer can call `!force <username>` to force queue the chatter to be the next active chatter
  - Active chatters are checked if they are an xQc sub and shows appropriate
    [forged](https://7tv.app/emotes/646244085070b2cda24f25aa)/[real](https://7tv.app/emotes/63de797c1d40a5212f9a5f9b)
    juicer check ticket
- Sniping and Detaining is always available
  - Counter Strike AWP Dragon Lore texture
- Denied, Shot, and Detained chatters are banned in chat
- Attack events
  - Attack chance changes dynamically based on consecutive denies or many chatters being shot/detained
  - A chatter is randomly selected with the same rules as traveler (see above)
  - Chatters can type `!attack` to temporarily slightly increase attack chance
- Most options configurable

You can see [in-progress features and bug fixes here](https://github.com/psp1g/papers/issues).

If you have suggestions for new features or have bug reports, please
[create one here](https://github.com/psp1g/papers/issues/new).
(if one similar isn't already created)

## Credits

![pspL](https://static-cdn.jtvnw.net/emoticons/v2/emotesv2_b04ede1f936346c18d0338e840af1a35/default/dark/1.0)
![pspL](https://static-cdn.jtvnw.net/emoticons/v2/emotesv2_b04ede1f936346c18d0338e840af1a35/default/dark/1.0)
![pspL](https://static-cdn.jtvnw.net/emoticons/v2/emotesv2_b04ede1f936346c18d0338e840af1a35/default/dark/1.0)
![pspHappy](https://static-cdn.jtvnw.net/emoticons/v2/emotesv2_426169649bee4355944f946c1d27ea4e/default/dark/1.0)
![pspHappy](https://static-cdn.jtvnw.net/emoticons/v2/emotesv2_426169649bee4355944f946c1d27ea4e/default/dark/1.0)

- **[LittleBigBug](https://github.com/LittleBigBug)** - Mod author
- **[creepycode](https://github.com/ByteZ1337)** - Ideas, Lots of reverse engineering help, Author of the [art asset decrypt/patcher tool](https://github.com/psp1g/papers-tools-rs)
- **[ftk789](https://twitch.tv/ftk789)** - Ideas, Coding/Audio help, Custom art assets
- **[pigswitched](https://twitch.tv/pigswitched)** - Custom art assets

## [Development Setup & Contributing](Development_and_Contributing.md)