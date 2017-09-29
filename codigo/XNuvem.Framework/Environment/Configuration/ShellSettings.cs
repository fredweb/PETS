/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace XNuvem.Environment.Configuration
{
    [Serializable]
    public class ShellSettings
    {
        [XmlIgnore]
        public Dictionary<string, string> GeneralSettings { get; set; }

        public ConnectionSettings ConnectionSettings { get; set; }
        public ShellSettings() {
            this.GeneralSettings = new Dictionary<string, string>();
            this.ConnectionSettings = new ConnectionSettings();
        }

        [XmlIgnore]
        public IList<Type> Entities { get; set; }

        [XmlIgnore]
        public IList<Type> EntityMaps { get; set; }
    }

    [Serializable]
    [XmlRoot("Item")]
    public class SettingItem
    {
        [XmlAttribute]
        public string Key { get; set;  }

        [XmlAttribute]
        public string Value { get; set; }
    }

    [Serializable]
    [XmlRoot("GeneralSettings")]
    public class GeneralSettingsHelper
    {
        public GeneralSettingsHelper() {
            this.Settings = new List<SettingItem>();
        }

        public GeneralSettingsHelper(Dictionary<string, string> settings) {
            this.Settings = settings.Select(kv => new SettingItem {
                Key = kv.Key,
                Value = kv.Value
            }).ToList();
        }

        [XmlElement("add")]
        public List<SettingItem> Settings { get; set; }

        public void FlushToDictionary(Dictionary<string, string> settings) {
            this.Settings.ForEach(kv => settings[kv.Key] = kv.Value);
        }
    }
}
