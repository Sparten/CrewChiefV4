﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrewChiefV4
{
    public class Configuration
    {
        private static String UI_TEXT_FILENAME = "ui_text.txt";
        private static String SPEECH_RECOGNITION_CONFIG_FILENAME = "speech_recognition_config.txt";
        private static String SOUNDS_CONFIG_FILENAME = "sounds_config.txt";

        private static Dictionary<String, String> UIStrings = LoadUIStrings();
        private static Dictionary<String, String> SpeechRecognitionConfig = LoadSpeechRecognitionConfig();
        private static Dictionary<String, String> SoundsConfig = LoadSoundsConfig();

        public static String getUIString(String key) {
            if (UIStrings.ContainsKey(key)) {
                return UIStrings[key];
            }
            return key;
        }

        public static String getSoundConfigOption(String key)
        {
            if (SoundsConfig.ContainsKey(key))
            {
                return SoundsConfig[key];
            }
            return key;
        }

        public static String getSpeechRecognitionConfigOption(String key)
        {
            if (SpeechRecognitionConfig.ContainsKey(key))
            {
                return SpeechRecognitionConfig[key];
            }
            return key;
        }

        public static String[] getSpeechRecognitionPhrases(String key)
        {
            if (SpeechRecognitionConfig.ContainsKey(key))
            {
                String options = SpeechRecognitionConfig[key];
                if (options.Contains(":"))
                {
                    return options.Split(':');
                }
                else
                {
                    return new String[] {options};
                }
            }
            return new String[] {};
        }

        private static String getDefaultFileLocation(String filename) {
            if (Debugger.IsAttached)
            {
                return Application.StartupPath + @"\..\..\" + filename;
            }
            else
            {
                return Application.StartupPath + @"\" + filename;
            }
        }

        private static String getUserOverridesFileLocation(String filename)
        {
            return Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\CrewChiefV4\" + filename);
        }

        private static void merge(StreamReader file, Dictionary<String, String> dict)
        {
            String line;
            while ((line = file.ReadLine()) != null)
            {
                if (!line.StartsWith("#") && line.Contains("="))
                {
                    try
                    {
                        String[] split = line.Split('=');
                        String key = split[0].Trim();
                        if (dict.ContainsKey(key))
                        {
                            dict.Remove(key);
                        }
                        dict.Add(split[0].Trim(), split[1].Trim());
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        private static Dictionary<String, String> LoadSpeechRecognitionConfig()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            StreamReader file = new StreamReader(getDefaultFileLocation(SPEECH_RECOGNITION_CONFIG_FILENAME));
            try
            {
                merge(file, dict);
            }
            finally
            {
                file.Close();
            }
            Boolean gotUserOverride = false;
            try
            {
                file = new StreamReader(getUserOverridesFileLocation(SPEECH_RECOGNITION_CONFIG_FILENAME));
                gotUserOverride = true;
                merge(file, dict);
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (gotUserOverride)
                {
                    file.Close();
                }
            }
            return dict;
        }

        private static Dictionary<String, String> LoadSoundsConfig()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            StreamReader file = new StreamReader(getDefaultFileLocation(SOUNDS_CONFIG_FILENAME));
            try
            {
                merge(file, dict);
            }
            finally
            {
                file.Close();
            }
            Boolean gotUserOverride = false;
            try
            {
                file = new StreamReader(getUserOverridesFileLocation(SOUNDS_CONFIG_FILENAME));
                gotUserOverride = true;
                merge(file, dict);
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (gotUserOverride)
                {
                    file.Close();
                }
            }
            return dict;
        }

        private static Dictionary<String, String> LoadUIStrings()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            StreamReader file = new StreamReader(getDefaultFileLocation(UI_TEXT_FILENAME));
            try
            {
                merge(file, dict);
            }
            finally
            {
                file.Close();
            }
            Boolean gotUserOverride = false;
            try
            {
                file = new StreamReader(getUserOverridesFileLocation(UI_TEXT_FILENAME));
                gotUserOverride = true;
                merge(file, dict);
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (gotUserOverride)
                {
                    file.Close();
                }
            }
            return dict;
        }
    }
}
