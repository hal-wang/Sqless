﻿using HTools.Config;

namespace Demo.Sqless.Wpf.Helpers
{
    class ConfigHelper : SettingConfigBase
    {
        private ConfigHelper() { }

        public static ConfigHelper Instance { get; } = new ConfigHelper();

        public string Account
        {
            get => Get("AccessTestUid");
            set => Set(value);
        }
    }
}
