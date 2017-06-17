using System;
using System.IO;

namespace BinChunker
{
    public static class BinChunk
    {
        static readonly string Usage = @"Usage: binchunk [-v] [-p (PSX)] [-w (wav)] [-s (SwapByteOrder)]
         <image.cue> <basename>
Example: BinChunk foo.cue foo
  -v  Verbose mode
  -p  PSX mode: truncate MODE2/2352 to 2336 bytes instead of normal 2048
  -w  Output audio files in WAV format
  -s  SwapByteOrder: swap byte order in audio tracks" + Environment.NewLine;

        static readonly string VersionString = @"BinChunker for .net, v" +
                                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version +
                                        @" by Bjorn Reppen <bjreppen@gmail.com>
	Ported from bchunk for Unix, Heikki Hannikainen <hessu@hes.iki.fi>
	Released under the GNU GPL, version 2 or later (at your option)." + Environment.NewLine + Environment.NewLine;

        public const int SectorLength = 2352;
        public static bool Verbose;
        private const string CueExtension = ".cue";

        static string outFileNameBase;
        static string cueFileName;
    
        static int Main(string[] args)
        {
            try
            {
                Console.WriteLine(VersionString);

                ParseArguments(args);

                CueFile cueFile;
                try
                {
                    cueFileName = Path.ChangeExtension(cueFileName, CueExtension);
                    cueFile = new CueFile(cueFileName);
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Could not read CUE {cueFileName}:\n{e.Message}");
                }


                Stream binStream;
                try
                {
                    binStream = File.OpenRead(cueFile.BinFileName);
                }
                catch (Exception e)
                {
                    throw new ApplicationException($"Could not open BIN {cueFile.BinFileName}: {e.Message}");
                }

                Console.WriteLine(Environment.NewLine + "Writing tracks:");
                foreach (Track curTrack in cueFile.TrackList)
                {
                    // Include track number when more than 1 track.
                    string outFileName;
                    if (cueFile.TrackList.Count > 1)
                        outFileName =
                            $"{outFileNameBase}{curTrack.TrackNumber:00}.{curTrack.FileExtension.ToString().ToLower()}";
                    else
                        outFileName = $"{outFileNameBase}.{curTrack.FileExtension.ToString().ToLower()}";
                    curTrack.Write(binStream, outFileName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
            return 0;
        }

        static void ParseArguments(string[] args)
        {
            int optIndex = 0;
            for (; optIndex < args.Length; optIndex++)
            {
                string arg = args[optIndex].ToLower();
                if ((arg.Length < 2) || (arg[0] != '-'))
                    break;

                switch (arg[1])
                {
                    case 'v':
                        Verbose = true;
                        break;
                    case 'w':
                        Track.WavFormat = true;
                        break;
                    case 'p':
                        Track.TruncatePsx = true;
                        break;
                    case 's':
                        Track.SwapAudioByteOrder = true;
                        break;
                    default:
                        Console.WriteLine(Usage);
                        Environment.Exit(1);
                        break;
                }
            }

            if (args.Length - optIndex != 2)
            {
                Console.WriteLine(Usage);
                Environment.Exit(1);
            }

            for (; optIndex < args.Length; optIndex++)
                switch (args.Length - optIndex)
                {
                    case 2:
                        cueFileName = args[optIndex];
                        break;
                    case 1:
                        outFileNameBase = args[optIndex];
                        break;
                    default:
                        Console.WriteLine(Usage);
                        Environment.Exit(1);
                        break;
                }
        }
    }
}