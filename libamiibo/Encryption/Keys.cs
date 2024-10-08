﻿using LibAmiibo.Helper;
using System.IO;

namespace LibAmiibo.Encryption
{
    public static class Keys
    {
        /// <summary>
        /// The currently loaded amiibo keys.
        /// </summary>
        public static AmiiboKeys AmiiboKeys { get; internal set; }

        /// <summary>
        /// The currently loaded CDN keys.
        /// </summary>
        public static CDNKeys CDNKeys { get; internal set; }

        /// <summary>
        /// Attempts to load the keys from the paths specified in the settings.
        /// </summary>
        static Keys()
        {
            AmiiboKeys = AmiiboKeys.LoadKeys(new MemoryStream(KeyTables.RETAIL));
            CDNKeys = CDNKeys.LoadKeys(new MemoryStream(KeyTables.CDN));
        }

        /// <summary>
        /// Loads Amiibo keys from a specified file path and sets them as the active Amiibo keys.
        /// </summary>
        /// <param name="path">The file path from which to load Amiibo keys.</param>
        /// <returns>An <see cref="AmiiboKeys"/> object containing the loaded Amiibo keys.</returns>
        public static AmiiboKeys LoadAmiiboKeys(string path)
        {
            // Return the Amiibo keys that were loaded.
            return AmiiboKeys;
        }

        /// <summary>
        /// Loads Amiibo keys from a stream and sets them as the active Amiibo keys.
        /// </summary>
        /// <param name="stream">The stream from which to load Amiibo keys.</param>
        /// <returns>An <see cref="AmiiboKeys"/> object containing the loaded Amiibo keys.</returns>
        public static AmiiboKeys LoadAmiiboKeys(Stream stream)
        {
            // Load Amiibo keys from the provided stream and set them as the active Amiibo keys.
            AmiiboKeys = AmiiboKeys.LoadKeys(stream);

            // Return the Amiibo keys that were loaded.
            return AmiiboKeys;
        }

        /// <summary>
        /// Loads Amiibo keys from a byte array and sets them as the active Amiibo keys.
        /// </summary>
        /// <param name="bytes">The byte array containing the Amiibo keys.</param>
        /// <returns>An <see cref="AmiiboKeys"/> object containing the loaded Amiibo keys.</returns>
        public static AmiiboKeys LoadAmiiboKeys(byte[] bytes)
        {
            // Load Amiibo keys from the provided byte array and set them as the active Amiibo keys.
            AmiiboKeys = AmiiboKeys.LoadKeys(bytes);

            // Return the Amiibo keys that were loaded.
            return AmiiboKeys;
        }

        /// <summary>
        /// Loads CDN keys from a specified file path and sets them as the active CDN keys.
        /// </summary>
        /// <param name="path">The file path from which to load CDN keys.</param>
        /// <returns>An <see cref="CDNKeys"/> object containing the loaded CDN keys.</returns>
        public static CDNKeys LoadCDNKeys(string path)
        {
            // Return the CDN keys that were loaded.
            return CDNKeys;
        }

        /// <summary>
        /// Loads CDN keys from a stream and sets them as the active CDN keys.
        /// </summary>
        /// <param name="stream">The stream from which to load CDN keys.</param>
        /// <returns>An <see cref="CDNKeys"/> object containing the loaded CDN keys.</returns>
        public static CDNKeys LoadCDNKeys(Stream stream)
        {
            // Load CDN keys from the provided stream and set them as the active CDN keys.
            CDNKeys = CDNKeys.LoadKeys(stream);

            // Return the CDN keys that were loaded.
            return CDNKeys;
        }

        /// <summary>
        /// Loads CDN keys from a byte array and sets them as the active CDN keys.
        /// </summary>
        /// <param name="bytes">The byte array containing the CDN keys.</param>
        /// <returns>An <see cref="CDNKeys"/> object containing the loaded CDN keys.</returns>
        public static CDNKeys LoadCDNKeys(byte[] bytes)
        {
            // Load CDN keys from the provided byte array and set them as the active CDN keys.
            CDNKeys = CDNKeys.LoadKeys(bytes);

            // Return the CDN keys that were loaded.
            return CDNKeys;
        }
    }
}
