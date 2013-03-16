using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;

namespace TM2
{
    public class SettingsManager
    {
        [Serializable]
        public struct KeyboardSettings
        {
            public Keys A;
            public Keys B;
            public Keys X;
            public Keys Y;
            public Keys LeftShoulder;
            public Keys RightShoulder;
            public Keys LeftTrigger;
            public Keys RightTrigger;
            public Keys LeftStick;
            public Keys RightStick;
            public Keys Back;
            public Keys Start;

            public Keys DPadDown;
            public Keys DPadLeft;
            public Keys DPadRight;
            public Keys DPadUp;

            public Keys LeftThumbstickDown;
            public Keys LeftThumbstickLeft;
            public Keys LeftThumbstickRight;
            public Keys LeftThumbstickUp;
            public Keys RightThumbstickDown;
            public Keys RightThumbstickLeft;
            public Keys RightThumbstickRight;
            public Keys RightThumbstickUp;
        }

        [Serializable]
        public struct GameSettings
        {
            public int DefaultWindowWidth;
            public int DefaultWindowHeight;
            public int PreferredWindowWidth;
            public int PreferredWindowHeight;
            public bool PreferredFullScreen;
            public KeyboardSettings[] KeyboardSettings;
        }

        public static GameSettings Read(string settingsFilename)
        {
            GameSettings gameSettings;
            Stream stream = File.OpenRead(settingsFilename);
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            gameSettings = (GameSettings)serializer.Deserialize(stream);
            stream.Close();
            return gameSettings;
        }

        public static void Save(string settingsFilename, GameSettings gameSettings)
        {
            Stream stream = File.OpenWrite(settingsFilename);
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            serializer.Serialize(stream, gameSettings);
            stream.Close();
        }

        public static Dictionary<Buttons, Keys>
            GetKeyboardDictionary(KeyboardSettings keyboard)
        {
            Dictionary<Buttons, Keys> dictionary =
                new Dictionary<Buttons, Keys>();

            dictionary.Add(Buttons.A, keyboard.A);
            dictionary.Add(Buttons.B, keyboard.B);
            dictionary.Add(Buttons.X, keyboard.X);
            dictionary.Add(Buttons.Y, keyboard.Y);

            return dictionary;
        }
    }
}
