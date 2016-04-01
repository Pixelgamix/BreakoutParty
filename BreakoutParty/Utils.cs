using SDL2;
using System;
using System.IO;

namespace BreakoutParty
{
    /// <summary>
    /// Contains a collection of Utility methods.
    /// </summary>
    static class Utils
    {
        /// <summary>
        /// Company name.
        /// </summary>
        public const string CompanyName = "Pixelgamix";

        /// <summary>
        /// Game name.
        /// </summary>
        public const string GameName = "BreakoutParty";

        /// <summary>
        /// The folder to save in.
        /// </summary>
        public static readonly string SaveDirectory = GetSaveDirectory();

        /// <summary>
        /// Returns the save folder.
        /// </summary>
        /// <remarks>
        /// Source:
        /// https://github.com/FNA-XNA/FNA/wiki/4:-FNA-and-Windows-API#file-paths
        /// </remarks>
        /// <returns>The save folder.</returns>
        private static string GetSaveDirectory()
        {
            string platform = SDL.SDL_GetPlatform();
            if (platform.Equals("Windows"))
            {
                return Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments
                    ),
                    "SavedGames",
                    CompanyName,
                    GameName
                );
            }
            else if (platform.Equals("Mac OS X"))
            {
                string osConfigDir = Environment.GetEnvironmentVariable("HOME");
                if (String.IsNullOrEmpty(osConfigDir))
                {
                    return "."; // Oh well.
                }
                osConfigDir += "/Library/Application Support";
                return Path.Combine(osConfigDir, CompanyName, GameName);
            }
            else if (platform.Equals("Linux"))
            {
                string osConfigDir = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                if (String.IsNullOrEmpty(osConfigDir))
                {
                    osConfigDir = Environment.GetEnvironmentVariable("HOME");
                    if (String.IsNullOrEmpty(osConfigDir))
                    {
                        return "."; // Oh well.
                    }
                    osConfigDir += "/.local/share";
                }
                return Path.Combine(osConfigDir, CompanyName, GameName);
            }
            throw new Exception("SDL platform unhandled: " + platform);
        }
    }
}
