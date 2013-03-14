using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace TM2
{
    public class SettingsManager
    {
        [Serializable]
        public struct GameSettings
        {
            public bool PerferredFullScreen;
            public int PreferredWindowWidth;
            public int PreferredWindowHeight;
        }

        public static GameSettings Read(string settingsFilename)
        {
            GameSettings gameSettings;
            Stream stream = File.OpenRead(settingsFilename);
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            gameSettings = (GameSettings)serializer.Deserialize(stream);
            return gameSettings;
        }

        public static void Save(string settingsFilename, GameSettings gameSettings)
        {
            Stream stream = File.OpenWrite(settingsFilename);
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            serializer.Serialize(stream, gameSettings);
        }
    }
}
