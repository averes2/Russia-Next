using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
using System.Reflection;

namespace ConsoleDA
{
    class persistentJSON
    {


        public Dictionary<string, Dictionary<string, object>> characterSettings = new Dictionary<string, Dictionary<string, object>>() 
            {
               {
                   "", new Dictionary<string, object>() 
                   {
                        { "", null }
                   }
               } 
            };

        public Dictionary<string, object> characterIndex = new Dictionary<string, object>() 
            {
            };
        
        public Dictionary<string, Dictionary<string,object>> GetData()
        {
            return this.characterSettings;
        }

        public void SaveCharacterControls(ClientTab tab)
        {
            Dictionary<string, Dictionary<string, object>> varCharacterSettings = MySettings.Load();
            //MySettings settings = MySettings.Load();

            Dictionary<string, object> controlSettings = new Dictionary<string, object>();

            List<Control> refControls = new List<Control>();
            tab.RecursiveAllControls(tab, ref refControls);

            //if value is in varCharacterSettings update it and hold it for json serialization
            /*We need to make this check all types of window forms, is there a better way than hard coding it? Dont forget to do the same for load*/
            foreach (Control innerControl in refControls)
            {
                string value = "";

                if (innerControl is TextBox || innerControl is System.Windows.Forms.ComboBox || innerControl is ListBox)
                {
                    PropertyInfo info = innerControl.GetType().GetProperty("Text");
                    string infoValue = info.GetValue(innerControl, null).ToString();
                    value = infoValue;
                    if (!controlSettings.ContainsKey(innerControl.Name))
                        controlSettings.Add(innerControl.Name, infoValue);
                    else
                        controlSettings[innerControl.Name] = infoValue;

                }
                else if (innerControl is CheckBox || innerControl is RadioButton)
                {
                    PropertyInfo info = innerControl.GetType().GetProperty("Checked");
                    bool infoValue = (bool)info.GetValue(innerControl, null);
                    value = infoValue.ToString();
                    if (!controlSettings.ContainsKey(innerControl.Name))
                        controlSettings.Add(innerControl.Name, infoValue);
                    else
                        controlSettings[innerControl.Name] = infoValue;
                }
                else if (innerControl is NumericUpDown || innerControl is TrackBar)
                {
                    PropertyInfo info = innerControl.GetType().GetProperty("Value");
                    object infoValue = (object)info.GetValue(innerControl, null);
                    value = infoValue.ToString();
                    if (!controlSettings.ContainsKey(innerControl.Name))
                        controlSettings.Add(innerControl.Name, infoValue);
                    else
                        controlSettings[innerControl.Name] = infoValue;
                }
                //if (controlSettings.ContainsKey(innerControl.Name))
                //    tab.Client.SendMessage(controlSettings[innerControl.Name] + " " + innerControl.Name, (byte)0);
            }

            if (!varCharacterSettings.ContainsKey(tab.Client.Base.Me.Name.ToLower()))
            {
                varCharacterSettings.Add(tab.Client.Base.Me.Name.ToLower(), controlSettings);
            }
            else //update the character settings
            {
                varCharacterSettings[tab.Client.Base.Me.Name.ToLower()] = controlSettings;
            }

            if(!tab.Client.Base.Me.Name.Equals("noname"))
                MySettings.Save(varCharacterSettings);
        }

        public void LoadCharacterControls(ClientTab tab)
        {
            Dictionary<string, Dictionary<string, object>> varCharacterSettings = MySettings.Load();

            List<Control> clientControls = new List<Control>();
            Dictionary<string, object> characterIndex = new Dictionary<string, object>();
            tab.RecursiveAllControls(tab,ref clientControls);

            foreach (var character in varCharacterSettings)
            {
                if (character.Key.Equals(tab.Client.Base.Me.Name.ToLower()))
                    characterIndex = character.Value;
            }
            foreach (var control in characterIndex)
            {
                foreach (var cControl in clientControls)
                {
                    /*We need to make this check all types of window forms, is there a better way than hard coding it? Dont forget to do the same for save*/
                    if (cControl.Name.Equals(control.Key))
                    {
                        if (cControl is TextBox)
                            (cControl as TextBox).Text = control.Value.ToString();
                        else if (cControl is ListBox)
                            (cControl as ListBox).Text = control.Value.ToString();
                        else if (cControl is ComboBox)
                            (cControl as ComboBox).Text = control.Value.ToString();
                        else if (cControl is CheckBox)
                            (cControl as CheckBox).Checked = (bool)control.Value;
                        else if (cControl is RadioButton)
                            (cControl as RadioButton).Checked = (bool)control.Value;
                        else if (cControl is TrackBar)
                            (cControl as TrackBar).Value = (int)control.Value;
                        else if (cControl is NumericUpDown)
                            (cControl as NumericUpDown).Value = (int)control.Value;
                    }
                }
            }
        }
       /* static void Main(string[] args)
        {
            MySettings settings = MySettings.Load();
            Console.WriteLine("Incrementing 'myInteger'...");

            Console.WriteLine("Saving settings...");
            settings.Save();
            Console.WriteLine("Done.");
            Console.ReadKey();
        }*/

        class MySettings : AppSettings<MySettings>
        {
            public Dictionary<string, Dictionary<string, object>> characterSettings = new Dictionary<string, Dictionary<string, object>>() 
            {
               {
                   "", new Dictionary<string, object>() 
                   {
                        { "", null }
                   }
               } 
            };
           

            

        }
    }

    public class AppSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "\\persistantCharacters.jsn";

        public static void Save(object DE, string fileName = DEFAULT_FILENAME)
        {
            fileName = Environment.CurrentDirectory + fileName;
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(DE));
        }


        public static void Save(T pSettings, object DE, string fileName = DEFAULT_FILENAME)
        {
            fileName = Environment.CurrentDirectory + fileName;
            File.WriteAllText(fileName, (new JavaScriptSerializer()).Serialize(DE));
        }

        public static Dictionary<string, Dictionary<string, object>> Load(string fileName = DEFAULT_FILENAME)
        {
            fileName = Environment.CurrentDirectory + fileName;
            Dictionary<string, Dictionary<string, object>> t = new Dictionary<string, Dictionary<string, object>>();
            if (File.Exists(fileName))
                t = (new JavaScriptSerializer()).Deserialize<Dictionary<string, Dictionary<string, object>>>(File.ReadAllText(fileName));
            return t;
        }
    }
}