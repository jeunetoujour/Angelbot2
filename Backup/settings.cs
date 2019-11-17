using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using AngelRead;
//using MemoryLib;
using Ini;

namespace AngelBot
{
    public partial class settings : Form
    {
       
        public settings()
        {
            InitializeComponent();
        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void loadsettings()
        {
            //Tab1
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");
            IniFile initemplate = new IniFile(Environment.CurrentDirectory + "\\template.ini");

            if (initemplate.Exists())
            {
                initemplate.Load();
            }
            else
            {
                MessageBox.Show("You have no template.ini file! ");
            }


            if (ini.Exists())
                ini.Load();
            else //copy file
            {
                using (FileStream fs = File.Create(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini")) { }
                File.Copy(Environment.CurrentDirectory + "\\template.ini", Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");
                ini.Load();
            }
            textBox1.Text = GetSetting(ini, initemplate, "limits", "RestHP");
            textBox2.Text = GetSetting(ini, initemplate, "limits", "RestMana");

            textBox4.Text = GetSetting(ini, initemplate, "limits", "PotHP");
            textBox5.Text = GetSetting(ini, initemplate, "limits", "PotMP");
            textBox6.Text = GetSetting(ini, initemplate, "limits", "IgnoreLevel");
            textBox7.Text = GetSetting(ini, initemplate, "limits", "IgnoreTime");
            txtooc.Text = GetSetting(ini, initemplate, "limits", "OOCHeal");
            comboBox1.SelectedItem = GetSetting(ini, initemplate, "limits", "OOCType");
            textBox25.Text = GetSetting(ini, initemplate, "limits", "Shutoff");
            textBox26.Text = GetSetting(ini, initemplate, "limits", "DeathRest");

            //Tab2
            if ("True" == GetSetting(ini, initemplate, "character", "Healer"))
            { checkBox1.Checked = true; }
            else { checkBox1.Checked = false; }

            if ("True" == GetSetting(ini, initemplate, "character", "Ranged"))
            { checkBox2.Checked = true; }
            else { checkBox2.Checked = false; }

            if ("True" == GetSetting(ini, initemplate, "character", "Antistuck"))
            { checkBox3.Checked = true; }
            else { checkBox3.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "FullInv"))
            { checkBox4.Checked = true; }
            else { checkBox4.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "Logging"))
            { checkBox5.Checked = true; }
            else { checkBox5.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "LeftRes"))
            { checkBox6.Checked = true; }
            else { checkBox6.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "KillSteal"))
            { checkBox7.Checked = true; checkBox3.Checked = false; }
            else { checkBox7.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "Gathering"))
            { checkBox8.Checked = true; }
            else { checkBox8.Checked = false; }
            if ("True" == GetSetting(ini, initemplate, "character", "DefendDeath"))
            { checkBox9.Checked = true; }
            else { checkBox9.Checked = false; }

            textBox8.Text = GetSetting(ini, initemplate, "character", "RangeDist");
            textBox19.Text = GetSetting(ini, initemplate, "character", "Lootdelay");
            txtgathersel.Text = GetSetting(ini, initemplate, "character", "GatheringSelect");
            txtgatherdis.Text = GetSetting(ini, initemplate, "character", "GatherDistance");
            //Tab3
            
            string buffstemp = GetSetting(ini, initemplate, "buffs", "Buffs");
            if (buffstemp != "")
            {
                if (buffstemp.StartsWith("|")) buffstemp = buffstemp.Substring(1, buffstemp.Length);
                if (buffstemp.Contains('\0').ToString() == "True")
                {
                    buffstemp = buffstemp.Substring(0, buffstemp.LastIndexOf('\0') - 0);
                }
                string[] listbuffs = buffstemp.Split('|');
                listBox3.Items.AddRange(listbuffs);
            }//Tab6
            
            //Tab7
            txtloot.Text = GetSetting(ini, initemplate, "keybinds", "LootBtn");
            txtrest.Text = GetSetting(ini, initemplate, "keybinds", "RestBtn");
            txthppot.Text = GetSetting(ini, initemplate, "keybinds", "Healthpot");
            txtmppot.Text = GetSetting(ini, initemplate, "keybinds", "Manapot");
            txttarget.Text = GetSetting(ini, initemplate, "keybinds", "TargetBtn");
            txtself.Text = GetSetting(ini, initemplate, "keybinds", "SelfTarget");
            txtturn.Text = GetSetting(ini, initemplate, "keybinds", "TurnAround");
            txtautoattack.Text = GetSetting(ini, initemplate, "keybinds", "Autoattack");
            textBox16.Text = GetSetting(ini, initemplate, "keybinds", "OOCH");
            textBox24.Text = GetSetting(ini, initemplate, "keybinds", "StrafeL");
            textBox23.Text = GetSetting(ini, initemplate, "keybinds", "StrafeR");
            textBox31.Text = GetSetting(ini, initemplate, "keybinds", "Return");
        
        }
        private string GetSetting(IniFile character, IniFile template, string section, string key)
        {
            if (character.HasKey(section, key))
                return character[section][key];
            else if (template.HasKey(section, key))
            {
                return template[section][key];
            }
            else
            {
                MessageBox.Show("You have no template.ini file or a corrupted copy! ");
                return "";
            }
        }
        private void settings_Load(object sender, EventArgs e)
        {
            
            loadsettings();

        }

        

        

        private void savesettings()
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            IniFile ini = new IniFile(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");

            if (ini.Exists())
                ini.Load();
            else //copy file
            {
                using (FileStream fs = File.Create(Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini")) { }
                File.Copy(Environment.CurrentDirectory + "\\template.ini", Environment.CurrentDirectory + "\\" + f1.pc.Name + ".ini");
                ini.Load();
            }

            ini["limits"]["RestHP"] = textBox1.Text;
            ini["limits"]["RestMana"] = textBox2.Text;
            ini["limits"]["PotHP"] = textBox4.Text;
            ini["limits"]["PotMP"] = textBox5.Text;
            ini["limits"]["IgnoreLevel"] = textBox6.Text;
            ini["limits"]["IgnoreTime"] = textBox7.Text;
            ini["limits"]["OOCHeal"] = txtooc.Text;
            ini["limits"]["OOCType"] = comboBox1.SelectedItem.ToString();
            
            ini["limits"]["Shutoff"] = textBox25.Text;
            ini["limits"]["DeathRest"] = textBox26.Text;

            if (checkBox1.Checked == true)
            {
                ini["character"]["Healer"] = "True";
            }
            else { ini["character"]["Healer"] = "False"; }

            if (checkBox2.Checked == true)
            {
                ini["character"]["Ranged"] = "True";
            }
            else { ini["character"]["Ranged"] = "False"; }

            if (checkBox3.Checked == true)
            {
                ini["character"]["Antistuck"] = "True";
            }
            else { ini["character"]["Antistuck"] = "False"; }

            if (checkBox4.Checked == true)
            {
                ini["character"]["FullInv"] = "True";
            }
            else { ini["character"]["FullInv"] = "False"; }

            if (checkBox5.Checked == true)
            {
                ini["character"]["Logging"] = "True";
            }
            else { ini["character"]["Logging"] = "False"; }
            if (checkBox6.Checked == true)
            {
                ini["character"]["LeftRes"] = "True";
            }
            else { ini["character"]["LeftRes"] = "False"; }
            if (checkBox7.Checked == true)
            {
                ini["character"]["KillSteal"] = "True";
                ini["character"]["Antistuck"] = "False";
                checkBox3.Checked = false;
            }
            else { ini["character"]["KillSteal"] = "False"; }
            if (checkBox8.Checked == true)
            {
                ini["character"]["Gathering"] = "True";
            }
            else { ini["character"]["Gathering"] = "False"; }
            if (checkBox9.Checked == true)
            {
                ini["character"]["DefendDeath"] = "True";
            }
            else { ini["character"]["DefendDeath"] = "False"; }

            ini["character"]["RangeDist"] = textBox8.Text;
            ini["character"]["Lootdelay"] = textBox19.Text;
            ini["character"]["GatheringSelect"] = txtgathersel.Text;
            ini["character"]["GatherDistance"] = txtgatherdis.Text;

            string listbuffs = "";//buffs
            foreach (object item in listBox3.Items)
            {
                listbuffs = listbuffs + Convert.ToString(item).TrimEnd(null) + "|";
            }
            listbuffs = listbuffs.TrimEnd('|');
            ini["buffs"]["Buffs"] = listbuffs; 

            ini["keybinds"]["LootBtn"] = txtloot.Text;
            ini["keybinds"]["RestBtn"] = txtrest.Text;
            ini["keybinds"]["Healthpot"] = txthppot.Text;
            ini["keybinds"]["Manapot"] = txtmppot.Text;
            ini["keybinds"]["TargetBtn"] = txttarget.Text;
            ini["keybinds"]["SelfTarget"] = txtself.Text;
            ini["keybinds"]["TurnAround"] = txtturn.Text;
            ini["keybinds"]["Autoattack"] = txtautoattack.Text;
            ini["keybinds"]["OOCH"] = textBox16.Text;
            ini["keybinds"]["StrafeL"] = textBox24.Text;
            ini["keybinds"]["StrafeR"] = textBox23.Text;
            ini["keybinds"]["Return"] = textBox31.Text;

            ini.Save();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            savesettings(); //writes to ini
            Form1 f1 = (Form1)Application.OpenForms["Form1"];

            if (f1.savelog == true) { f1.savelog = false; f1.tw.Close(); }
            f1.loadsettings(); //loads new settings into RAM
            this.Close();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            f1.Opacity = (Convert.ToDouble(textBox13.Text) / 100);
        }



        private void button18_Click(object sender, EventArgs e)
        {
            string newitem = textBox14.Text + ":" + textBox15.Text;
            listBox3.Items.Add(newitem);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            listBox3.Items.RemoveAt(listBox3.SelectedIndex);
        }

        private void button16_Click(object sender, EventArgs e)
        {

            int i = this.listBox3.SelectedIndex;
            object o = this.listBox3.SelectedItem;

            if (i > 0)
            {
                this.listBox3.Items.RemoveAt(i);
                this.listBox3.Items.Insert(i - 1, o);
                this.listBox3.SelectedIndex = i - 1;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int i = this.listBox3.SelectedIndex;
            object o = this.listBox3.SelectedItem;

            if (i < this.listBox3.Items.Count - 1)
            {
                this.listBox3.Items.RemoveAt(i);
                this.listBox3.Items.Insert(i + 1, o);
                this.listBox3.SelectedIndex = i + 1;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string butt = (string)listBox3.SelectedItem; ;
            if (butt != null)
            {
                string[] theitem = butt.Split(':');
                textBox14.Text = theitem[0];
                textBox15.Text = theitem[1];
            }
            button13.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int i = this.listBox3.SelectedIndex;
            object o = this.listBox3.SelectedItem;
            object newobj = textBox14.Text + ":" + textBox15.Text;


            this.listBox3.Items[i] = newobj;

        }  

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)Application.OpenForms["Form1"];
            if (checkBox3.Checked == false)
            {
                f1.antistuck = true;
            }
            else f1.antistuck = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabCharacter_Click(object sender, EventArgs e)
        {

        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
