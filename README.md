# PaliBot

![Build Status](https://github.com/tickleBritches/palibot/workflows/PaliBot/badge.svg?branch=main) [![Coverage Status](https://coveralls.io/repos/github/tickleBritches/palibot/badge.svg?branch=main)](https://coveralls.io/github/tickleBritches/palibot?branch=main) [![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/tickleBritches/palibot/blob/main/LICENSE)

## This is probably crazy, but here's the deal...
There are a ton of VRML teams playing Echo Arena. The saints that are Palidore, Sir Dimwi, et. al stream fantastic casts of a lot of matches every week, but unfortunately, they can only cast so many in a day.  There a lot of teams in lower tiers that rarely, if ever, get casted.  Additionally, VRML rules justifiably forbid spectators in VRML matches (unless both teams agree).  This make sense as a biased spectator could relay comms from the enemy team over another voice solution such as Discord.  Teams want to be casted so they can have an announcer commentating their match and so that "fans" can watch.  We can necessarily solve for the "fan" problem, but in an attempt to solve for the rest of it:

*I want to build an AI/Bot system that can record/cast VRML matches at the request of teams.*

This request could be made through a Discord bot using an atlas link, or online form, etc..  There are a number of complex problems to solve, but none of them seem insurmountable.

- Many teams could be playing at the same time.  How could we record/cast all of them?
- Spectator streams still require an actual Oculus account and PC to connect.  Even if we had multiple bots running, how could we have enough accounts to accommodate all of them?  And on what hardware would they run?
- Spectator streams still need some degree of camera control to decently record a match (even if that is only to switch the spectator to sideline camera)
- In order to avoid what is otherwise private team comms, comms must be disabled in the spectator stream.  But recording a match with no audio would feel dead and boring.  Teams want to be casted because they want an announcer.
- All of these recordings/casts need to be stored somewhere for teams to go back and review.

### So here's how I think it might be doable...

Imagine a crowd-sourced network of auto-casters made up of existing Echo Arena players that volunteer to allow their computer (and Oculus account) to be used to run software that automates connecting to matches, orchestrating video capture/streaming, in game camera control (via selections, not direct control) and injecting a usable AI announcer (or two) - think SETI Online.

This breaks down into three *major* legs of work:
1. Volunteer Network - users signing up to be auto-casters, downloading the software, and leaving the software running when their computer is idle so a centralized server can receive requests and assign cast duties
2. Stream/Recording Orchestration - integrating the software in some way with capture/streaming software such as OBS
3. Match Analysis and Announcers - using the Echo Arena API to extract match information and building up a system of sensors to recognize low and high level play information and translate that information into speech via TTS

I could even see this functionality being added to an existing software package that a lot of players already use like IgniteBot (but that conversation is a long way away).  And, I don't know if there would be any demand for this, or even if the RAD and VRML folks would have a problem with it.  Certainly it could never replace having Pali and company cast your match, but maybe it's worth it as a consolation cast.

### For now...
I've begun work on the third leg mentioned above, the Announcer, as that is the most interesting to me from a coding standpoint.  Maybe this doesn't go anywhere, but it's at least a fun experiment for now.  I intend to approach this in a very agile way.  I'd like to get basic components working simply to prove out the big picture.  So, the announcer will probably suck for a while.  But, my hope is that I (and others) can improve the sophistication of the announcer logic over time.  Whether that involves creating crazy logic trees, or hell, even ML solutions, I'll try to keep things modular enough to allow compartmentalized improvements over time.

As for the other legs, if I can't find other contributors, I'll get to them as I can.  I'll focus on interacting with the Echo process first, so a person could conceivably fire  up this software alongside Echo and handle their own streaming, etc.  Then I'll focus on orchestrating streaming software. And finally, the crowd-source side of things.
