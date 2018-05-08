using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml.Linq;

namespace JinRi.Air.Model.LanzSMSModel
{
    public static class AppConfig
    {
        private static string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\";
        private static XElement app = XElement.Load(path + "app.config");

        public static string GetAppSetting(string key)
        {
            var root = app.Element("appSettings");
            var node = root.Elements().FirstOrDefault(n => n.Attribute("key").Value == key);
            return node == null ? "" : (string)node.Attribute("value");
        }
        public static string GetConnectionSetting(string key)
        {
            var root = app.Element("connectionStrings");
            var node = root.Elements().FirstOrDefault(n => n.Attribute("name").Value == key);
            return node == null ? "" : (string)node.Attribute("connectionString");
        }
        public static void UpdateSettingValue(string key, string value)
        {
            var root = app.Element("appSettings");
            var node = root.Elements().FirstOrDefault(n => n.Attribute("key").Value == key);
            node.SetAttributeValue("value", value);
            app.Save(path + "app.config");

        }
    }
}