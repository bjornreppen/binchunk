# BinChunker for .Net
[![Build Status](https://travis-ci.org/bjornreppen/binchunk.svg?branch=master)](https://travis-ci.org/bjornreppen/binchunk)

Copyleft 2002 Bjorn Reppen <bjreppen@gmail.com>

binchunker converts a CD image in a ".bin / .cue" format (sometimes ".raw / .cue") to a set of .iso and .cdr tracks. The bin/cue format is used by some popular non-Unix cd-writing software, but is not supported on most other CD burning programs. A lot of CD/VCD images distributed on the Internet are in BIN/CUE format, I've been told.

This is a .net rewrite of the fine [bchunk software](http://he.fi/bchunk/).

*BEWARE: This code was written in or before the year 2002..*

## What on earth is this stuff

The .iso track contains an ISO file system, which can be mounted through a loop device on Linux systems, or written on a CD-R using cdrecord. The .cdr tracks are in the native CD audio format. They can be either written on a CD-R using cdrecord -audio, or converted to WAV (or any other sound format for that matter) using sox. bchunk 1.1.0 (and later versions) can also output audio tracks in WAV format.

OpenBSD, FreeBSD and NetBSD all have a port of bchunk available in their respective archives for a very good set of hardware platforms.

Debian GNU/Linux users can install bchunk 1.2.0 using 'apt-get install bchunk', as bchunk is maintained as a Debian package.

bchunk has apparently also been successfully compiled on BeOS without source modifications and the i386 binary can be downloaded here.

Download the (GPL'ed) source code below, read the README file, compile, and serve cold with a meal. Satisfaction not guaranteed. If it breaks, you get to keep both pieces.

## Tips and tricks

To record an MP3 image with a CUE sheet to an audio CD (for example the mixes at sicktracks.com):

convert the .mp3 to a raw PCM audio file:

`$ mpg123 -sv sicktracks8.mp3 > sicktracks8.pcm`

split into tracks according to the CUE file:

`$ bchunk sicktracks8.pcm sicktracks8.txt tracks`

burn it:

`$ cdrecord -v -dao -audio tracks??.cdr`

If you only get loud noise on the tracks, try either -swab on cdrecord or -s on bchunk to swap the byte order.

## License

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program; if not, write to the Free Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
