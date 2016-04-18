using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;
using System.Drawing;

namespace EloBuddy
{
    class Katarina
    {
        //-------------------------------------------------------------------------------------------------------Public
        public static AIHeroClient Hero = Player.Instance;
        public static string HeroName = Hero.ChampionName;
        public static Menu Menu, EvadeE;
        public static Spell.Targeted Q, E;
        public static Spell.Active W, R;
        public static Item Ward;
        public static string Status = "";
        public static string Checkar = "Surpressed";
        public static bool VerificarBuff = false;
        public static Color colorSkin = Color.HotPink;
        public static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold));
        public static Text Text1 = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 20, System.Drawing.FontStyle.Bold));
        //-------------------------------------------------------------------------------------------------------Evento
        public Katarina()
        {
            Loading.OnLoadingComplete += Menus;
            Loading.OnLoadingComplete += Spell;
            Bootstrap.Init(null);
        }
        //-------------------------------------------------------------------------------------------------------Spells + Item
        private static void Spell(EventArgs args)
        {
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Active(SpellSlot.W, 375);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);
            Ward = new Item(3340);
        }
        //-------------------------------------------------------------------------------------------------------Menu
        private static void Menus(EventArgs args)
        {
            if (Hero.ChampionName != "Katarina") return;
            Menu = MainMenu.AddMenu(HeroName, HeroName);
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddGroupLabel("★★☆☆☆  Segure   [ Only Kill ] ");
            Menu.AddLabel("This mode is Recommended always, it will only make the");
            Menu.AddLabel("Combo Skill With 'E' if you really Kill.");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddGroupLabel("★★★★☆  Burst   [ Full Combo ] ");
            Menu.AddLabel("This mode is recommended if you are very strong,");
            Menu.AddLabel("As always Ira Make Full Combo");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("✔ Info - Choose With Game Mode Your Agreement,Read the Instructions Below the Mode");
            Menu.Add("M.E1", new ComboBox("        ❖   " + HeroName + " - [ Jump E ] Combo Mode  ", 1, "Burst   [Full Combo]", "Segure   [Only Kill]"));
            Menu.Add("SkinHack", new ComboBox("        ❖   "+HeroName+ " - [SkinHack ] Select Now  ", 8, "Classic Katarina", "Mercenary Katarina", "Red Card Katarina", "Bilgewater Katarina", "Kitty Cat Katarina", "High Command Katarina", "Sandstorm Katarina", "Slay Belle Katarina", "Warring Kingdoms Katarina"));
            Menu.Add("SkinLoad", new KeyBind("        ♯   Select Your Skin [ And Press ]", false, KeyBind.BindTypes.HoldActive, 'N'));
            Menu.Add("C.F", new KeyBind("        ♯   Fast Changes [ Combo Mode ]", false, KeyBind.BindTypes.HoldActive, 'M'));
            Menu.Add("E.C", new KeyBind("        ♯   Fast Changes [ Evade Mode ]", false, KeyBind.BindTypes.HoldActive, 'L'));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Ward Jump");
            Menu.Add("W.J", new KeyBind("        ♯   Auto Ward & Jump [ E ] And Press ->", false, KeyBind.BindTypes.HoldActive, 'V'));
            Hero.SetSkinId(4);//Set Skin Padrao
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Combo");
            Menu.Add("Q.1", new CheckBox("        ❖   " + HeroName + " - [ Q ] Enemy", true));
            Menu.Add("W.1", new CheckBox("        ❖   " + HeroName + " - [ W ] Enemy ", true));
            Menu.Add("E.1", new CheckBox("        ❖   " + HeroName + " - [ E ] Enemy", true));
            Menu.Add("R.1", new CheckBox("        ❖   " + HeroName + " - [ R ] Enemy", true));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Humanize Delay");
            Menu.Add("H.K", new CheckBox("        ❖   " + HeroName + " - [ To also ] KS", false));
            Menu.AddLabel("Important: Enable if Suspecting You. Recommend [ Mark Off ]");
            Menu.Add("HumanMin", new Slider("        ❖   " + HeroName + " - [ Humanize ] Jump [ E ] Min ", 200, 100, 1000));
            Menu.Add("HumanMax", new Slider("        ❖   " + HeroName + " - [ Humanize ] Jump [ E ] Max ", 450, 300, 1000));
            Menu.AddLabel("Important: Recommend Delay Min: [ 200 To 500 ] & Delay Max: [ 450 To 1000 ]");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Auto Harras");
            Menu.Add("H.H", new CheckBox("        ❖   " + HeroName + " - [ Q / W ] Auto Harras", false));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ KillSteal");
            Menu.Add("K.S", new CheckBox("        ❖   " + HeroName + " - [ KS ] Enemy", true));
            Menu.Add("K.W", new CheckBox("        ❖   " + HeroName + " - [ Jump ] To KS", true));
            Menu.AddLabel("Use [ E ] Jump To Ward / Ally / Minions / Objects ");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Farm");
            Menu.Add("F.L", new CheckBox("        ❖   " + HeroName + " - [ Q/W ] Farm", true));
            Menu.Add("F.H", new CheckBox("        ❖   " + HeroName + " - [ Q/W ] LastHit", true));
            Menu.Add("F.J", new CheckBox("        ❖   " + HeroName + " - [ Q/W ] Jungle", true));
            Menu.Add("F.K", new CheckBox("        ❖   " + HeroName + " - [ KillSteal ] Jungle", true));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Draw");
            Menu.Add("D.Q", new CheckBox("        ❖   " + HeroName + " - [ Q ] Range", true));
            Menu.Add("D.W", new CheckBox("        ❖   " + HeroName + " - [ W ] Range", false));
            Menu.Add("D.E", new CheckBox("        ❖   " + HeroName + " - [ E ] Range", false));
            Menu.Add("D.R", new CheckBox("        ❖   " + HeroName + " - [ R ] Range", false));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Draw 2");
            Menu.Add("T.R", new CheckBox("        ❖   " + HeroName + " - [ Text ] Ultimate State", true));
            Menu.Add("T.N", new CheckBox("        ❖   " + HeroName + " - [ Circle ] Enemy Kill", true));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Misc");
            Menu.AddLabel("Important: For Him Not Spending ability to Allies Clear them in");
            Menu.AddLabel("Menu EB > Core > GapCloser = Clear its Allies are Marked");
            Menu.Add("E.G", new CheckBox("        ❖   " + HeroName + " - [ E ] Ant-Gapcloser", true));
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");

            // EVADE----------------------------------------------------------------------------------------------------
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇ Evade");
            Menu.Add("E.E", new CheckBox("        ❖   [ E ] Evade Spells [If possible]", false));
           // Menu.Add("C.E", new CheckBox("        ❖   Aways ON [Not Recommend]", false));
            Menu.AddLabel("Important: Not Enabled during Combo Non Loses Kill");
           // Menu.AddSeparator(2);
            foreach (AIHeroClient Inimigo in EntityManager.Heroes.Enemies)
            {
                var HabilidadeQ = Inimigo.Spellbook.GetSpell(SpellSlot.Q).SData.Name.ToString();
                var HabilidadeW = Inimigo.Spellbook.GetSpell(SpellSlot.W).SData.Name.ToString();
                var HabilidadeE = Inimigo.Spellbook.GetSpell(SpellSlot.E).SData.Name.ToString();
                var HabilidadeR = Inimigo.Spellbook.GetSpell(SpellSlot.R).SData.Name.ToString();
                Menu.AddLabel(Inimigo.ChampionName);
                Menu.Add(Inimigo.ChampionName + "Q1", new CheckBox("Q [ "+ HabilidadeQ +" ]", true));
                Menu.Add(Inimigo.ChampionName + "W1", new CheckBox("W [ "+ HabilidadeW +" ]", true));
                Menu.Add(Inimigo.ChampionName + "E1", new CheckBox("E [ "+ HabilidadeE +" ]", true));
                Menu.Add(Inimigo.ChampionName + "R1", new CheckBox("R [ "+ HabilidadeE + " ]", true));
            }
            Menu.AddLabel("⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇⑇");
            // EVADE----------------------------------------------------------------------------------------------------

            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Evade;
            //Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;
        }
        //-------------------------------------------------------------------------------------------------------Update + Orbwalk
        private static void Game_OnUpdate(EventArgs args)
        {
            SempreAtivo();//  Auto Interromper ult para KS
            LoadingSkin();

            foreach (var buff in Hero.Buffs) {
                if (buff.DisplayName == "KatarinaRSound")
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;
                    VerificarBuff = true;
                    return;
                }
            }
            Orbwalker.DisableAttacking = false;
            Orbwalker.DisableMovement = false;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) Farm();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHit();
        }
        //-------------------------------------------------------------------------------------------------------Farms + Combo + Ignity
        #region "Farm/Combo"
        public static void Farm()
        {
            LastHit();
            var UseSpell = Menu["F.L"].Cast<CheckBox>().CurrentValue;
            var LastQ = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.Distance(Hero.Position) <= Q.Range).OrderBy(x => x.Health).FirstOrDefault();
            if (LastQ != null && UseSpell) Q.Cast(LastQ);
        }
        public static void LastHit()
        {
            var UseSpell = Menu["F.H"].Cast<CheckBox>().CurrentValue;
            var LastQ = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.Distance(Hero.Position) <= Q.Range && x.Health <= Hero.GetSpellDamage(x, SpellSlot.Q)).OrderBy(x => x.Health).FirstOrDefault();
            if (LastQ != null && UseSpell) Q.Cast(LastQ);
            var LastW = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.Distance(Hero.Position) <= W.Range && x.Health <= Hero.GetSpellDamage(x, SpellSlot.W)).OrderBy(x => x.Health).FirstOrDefault();
            if (LastW != null && UseSpell) W.Cast();
        }
        public static void Jungle()
        {
            var UseSpell = Menu["F.J"].Cast<CheckBox>().CurrentValue;
            var Jungle = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, Q.Range).FirstOrDefault(e => e.IsValid && !e.IsDead);
            if (Jungle != null && UseSpell) Q.Cast(Jungle); if (Jungle.IsValidTarget(W.Range) && Q.IsOnCooldown) W.Cast();
        }
        public static void Combo()
        {
            var HumanizeMin = Menu["HumanMin"].Cast<Slider>().CurrentValue;
            var HumanizeMax = Menu["HumanMax"].Cast<Slider>().CurrentValue;

            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Magical);
            var UseQ = Menu["Q.1"].Cast<CheckBox>().CurrentValue;
            var UseW = Menu["W.1"].Cast<CheckBox>().CurrentValue;
            var UseE = Menu["E.1"].Cast<CheckBox>().CurrentValue;
            var UseR = Menu["R.1"].Cast<CheckBox>().CurrentValue;
            var ModoE = Menu["M.E1"].Cast<ComboBox>().CurrentValue;
            if (ModoE == 0)
              {
                //Metodo Burst
                if (UseQ) Q.Cast(Inimigo);
                if (UseE) Core.DelayAction(() => E.Cast(Inimigo), new Random().Next(HumanizeMin, HumanizeMax));
                if (UseW && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                if (UseR && Hero.CountEnemiesInRange(W.Range) >= 1 && !E.IsReady() && !W.IsReady() && !Q.IsReady() && R.IsReady())
                {
                    Orbwalker.DisableMovement = true;
                    Orbwalker.DisableAttacking = true;
                    Core.DelayAction(() => R.Cast(), 50);
                }
            }
            if (ModoE == 1)
            {
                //Metodo 1 para Kill
                if (Inimigo.Health <= GetComboDamage(Inimigo))
                {
                    if (UseQ) Q.Cast(Inimigo);
                    if (UseE) Core.DelayAction(() => E.Cast(Inimigo),  new Random().Next(HumanizeMin, HumanizeMax));
                    if (UseW && Q.IsOnCooldown  && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                    if (UseR && Hero.CountEnemiesInRange(W.Range) >= 1 && !E.IsReady() && !W.IsReady() && !Q.IsReady() && R.IsReady())
                    {
                        Orbwalker.DisableMovement = true;
                        Orbwalker.DisableAttacking = true;
                        Core.DelayAction(() => R.Cast(), 50);
                    }
                }
               //Metodo 2 Para Hit Apenas
                if (Inimigo.Health >= GetComboDamage(Inimigo))
                {
                    if (UseQ) Q.Cast(Inimigo);
                    if (UseW && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------PermaActive
        public static void SempreAtivo()
        {
            //-------------------------------------------------------------------------------------------------------Ward Jump
            if (Menu.Get<KeyBind>("W.J").CurrentValue)
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
                var Mouse = Game.CursorPos;
                Mouse = Hero.Position.Extend(Mouse, 600).To3D();

                var Ward2 = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Name.ToLower().Contains("ward") && x.Distance(Game.CursorPos) < 250).FirstOrDefault();
                var Alvo = ObjectManager.Get<Obj_AI_Base>().Where(x => Hero.Distance(x.ServerPosition) <= E.Range && x.Distance(Game.CursorPos) <= 250 && !x.IsMe && !x.IsInvulnerable).FirstOrDefault();
                if (Alvo != null) E.Cast(Alvo); else if (Ward2 == null && E.IsReady()) { Ward.Cast(Mouse); }
                if (Ward2 != null) { if (E.IsReady()) E.Cast(Ward2); return; }
            }

            //-------------------------------------------------------------------------------------------------------Muda Modo de Combo
            if (Menu.Get<KeyBind>("C.F").CurrentValue)
            {
                switch (Menu["M.E1"].Cast<ComboBox>().CurrentValue)
                {
                    case 0: Core.DelayAction(() => Menu["M.E1"].Cast<ComboBox>().CurrentValue = 1, 200); break;
                    case 1: Core.DelayAction(() => Menu["M.E1"].Cast<ComboBox>().CurrentValue = 0, 200); ; break;
                }
            }
            //-------------------------------------------------------------------------------------------------------KS
            var UseKS = Menu["K.S"].Cast<CheckBox>().CurrentValue;
            var UseWardKS = Menu["K.W"].Cast<CheckBox>().CurrentValue;
            var HumanizeMin = Menu["HumanMin"].Cast<Slider>().CurrentValue;
            var HumanizeMax = Menu["HumanMax"].Cast<Slider>().CurrentValue;
            var UseHumanize = Menu["H.K"].Cast<CheckBox>().CurrentValue;
            if (UseKS)
            {
                var KS = EntityManager.Heroes.Enemies.Where(i => i.IsValidTarget(2000) && !i.IsDead && i.Health <= GetComboDamage(i)).FirstOrDefault();
                if (KS != null && KS.Distance(Hero.Position) <= Q.Range)
                {
                    //Metodo 1 para KS Dentro do range
                    if (Q.IsReady()) Q.Cast(KS);
                    if (E.IsReady() && !UseHumanize) { Core.DelayAction(() => E.Cast(KS), 200); }
                    else if (E.IsReady() && UseHumanize) { Core.DelayAction(() => E.Cast(KS), new Random().Next(HumanizeMin, HumanizeMax)); }
                    if (W.IsReady() && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                    if (R.IsReady() && Hero.CountEnemiesInRange(W.Range) >= 1 && !E.IsReady() && !W.IsReady() && !Q.IsReady())
                    {
                        Orbwalker.DisableMovement = true;
                        Orbwalker.DisableAttacking = true;
                        Core.DelayAction(() => R.Cast(), 50);
                    }
                }

                if (VerificarBuff == true && KS.Position.Distance(Hero.Position) > R.Range)
                {
                    E.Cast(KS);
                    Orbwalker.DisableMovement = false;
                    Orbwalker.DisableAttacking = false;
                    VerificarBuff = false;
                }
                else if (KS.Position.Distance(Hero.Position) > R.Range)
                {
                    E.Cast(KS);
                }

                if (KS != null && UseWardKS && KS.Distance(Hero.Position) > R.Range) //Metodo 2 para KS fora do range Use Jump Ward/Aliado/Inimigo
                {
                    var WardRangeDoAlvo = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.Name.ToLower().Contains("ward") && !m.IsDead && m.Position.Distance(KS.Position) < 600 && !KS.IsValidTarget(E.Range)).FirstOrDefault();
                    var PegarMinionsAlly = EntityManager.MinionsAndMonsters.AlliedMinions.Where(m => m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Position.Distance(KS.Position) < 600 && !KS.IsValidTarget(E.Range)).FirstOrDefault();
                    var PegarMinionsEnemy = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Position.Distance(KS.Position) < 600 && !KS.IsValidTarget(E.Range)).FirstOrDefault();
                    var PegarAlly = EntityManager.Heroes.Allies.Where(m => m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && !m.IsMe && !m.IsDead && m.Position.Distance(KS.Position) < 600 && !KS.IsValidTarget(E.Range)).FirstOrDefault();

                    if (PegarMinionsAlly != null) { E.Cast(PegarMinionsAlly); }
                    else if (PegarAlly != null) { E.Cast(PegarAlly); }
                    else if (PegarMinionsEnemy != null) { E.Cast(PegarMinionsEnemy); }
                    else if (WardRangeDoAlvo != null) { E.Cast(WardRangeDoAlvo); }
                }
            }
            //-------------------------------------------------------------------------------------------------------AutoHarras
            var AutoHarras = Menu["K.W"].Cast<CheckBox>().CurrentValue;
            if (AutoHarras)
            {
                var Inimigo = EntityManager.Heroes.Enemies.FirstOrDefault(i => i.IsValidTarget(Q.Range) && !i.IsDead);
                if (Q.IsReady()) Q.Cast(Inimigo);
                if (W.IsReady() && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
            }
        }
        //-------------------------------------------------------------------------------------------------------Evade
        #region "Evade"

        public static void Evade(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            SpellEvade = args.SData.Name;//Nome de esell
            var Inimigo = EntityManager.Heroes.Enemies.Where(x => x.Distance(Hero.Position) <= 1000).FirstOrDefault();
            var HabilidadeQ = sender.Spellbook.GetSpell(SpellSlot.Q).SData.Name.ToString();
            var HabilidadeW = sender.Spellbook.GetSpell(SpellSlot.W).SData.Name.ToString();
            var HabilidadeE = sender.Spellbook.GetSpell(SpellSlot.E).SData.Name.ToString();
            var HabilidadeR = sender.Spellbook.GetSpell(SpellSlot.R).SData.Name.ToString();
            var UseEvade = Menu["E.E"].Cast<CheckBox>().CurrentValue;
            var UseAways = Menu["C.E"].Cast<CheckBox>().CurrentValue;

            var EvadeQ = Menu[Inimigo.ChampionName + "Q1"].Cast<CheckBox>().CurrentValue;
            var EvadeW = Menu[Inimigo.ChampionName + "W1"].Cast<CheckBox>().CurrentValue;
            var EvadeE = Menu[Inimigo.ChampionName + "E1"].Cast<CheckBox>().CurrentValue;
            var EvadeR = Menu[Inimigo.ChampionName + "R1"].Cast<CheckBox>().CurrentValue;

            //if (!UseAways)
            if (UseEvade && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (sender.Type == Hero.Type && sender.Team == Inimigo.Team && Hero.Distance(sender) <= 1300 || args.Target != null && args.Target.IsMe && sender.Team != Hero.Team)
                {
                    if (args.SData.Name == HabilidadeQ && EvadeQ) UsaE_Evade();
                    if (args.SData.Name == HabilidadeW && EvadeW) UsaE_Evade();
                    if (args.SData.Name == HabilidadeE && EvadeE) UsaE_Evade();
                    if (args.SData.Name == HabilidadeR && EvadeR) UsaE_Evade();
                }
            }
        }
       static string SpellEvade = "";
        public static void UsaE_Evade()
        {
            var Inimigo = EntityManager.Heroes.Enemies.Where(o => o.IsValidTarget(1000)).FirstOrDefault();

           foreach(var PegarMinionsAlly in EntityManager.MinionsAndMonsters.AlliedMinions.Where(m => m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Position.Distance(Inimigo.Position) >= 500))
            {
                if (PegarMinionsAlly != null) { E.Cast(PegarMinionsAlly); Chat.Print("<b><font color =\"#FFFFFF\">|| Evade Spell Detected || </font><font color=\"#FF0000\">" + "[" + SpellEvade + "]" + "</font><font color =\"#FFFFFF\"> || </font></b>"); return; }
            }
            foreach (var PegarMinionsEnemy in EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Position.Distance(Inimigo.Position) >= 500))
            {
                if (PegarMinionsEnemy != null) { E.Cast(PegarMinionsEnemy); Chat.Print("<b><font color =\"#FFFFFF\">|| Evade Spell Detected || </font><font color=\"#FF0000\">" + "[" + SpellEvade + "]" + "</font><font color =\"#FFFFFF\"> || </font></b>"); return; }
            }
            foreach (var PegarAlly in EntityManager.Heroes.Allies.Where(m => m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && !m.IsMe && !m.IsDead && m.Position.Distance(Inimigo.Position) >= 500))
            {
                if (PegarAlly != null) { E.Cast(PegarAlly); Chat.Print("<b><font color =\"#FFFFFF\">|| Evade Spell Detected || </font><font color=\"#FF0000\">" + "[" + SpellEvade + "]" + "</font><font color =\"#FFFFFF\"> || </font></b>"); return; }
            }
            foreach (var PegarWard in ObjectManager.Get<Obj_AI_Minion>().Where(m => m.Name.ToLower().Contains("ward") && m.Position.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Position.Distance(Inimigo.Position) >= 500))
            {
                if (PegarWard != null) { E.Cast(PegarWard); Chat.Print("<b><font color =\"#FFFFFF\">|| Evade Spell Detected || </font><font color=\"#FF0000\">" + "[" + SpellEvade + "]" + "</font><font color =\"#FFFFFF\"> || </font></b>"); return; }
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------SkinHack
        public static void LoadingSkin()
        {
                if (Menu.Get<KeyBind>("SkinLoad").CurrentValue)
                {
                    var SkinHackSelect = Menu["SkinHack"].Cast<ComboBox>().CurrentValue;
                    switch (SkinHackSelect)
                    {
                        case 0: Hero.SetSkinId(0); colorSkin = Color.Brown; break;
                        case 1: Hero.SetSkinId(1); colorSkin = Color.GhostWhite; break;
                        case 2: Hero.SetSkinId(2); colorSkin = Color.DimGray; break;
                        case 3: Hero.SetSkinId(3); colorSkin = Color.Tomato; break;
                        case 4: Hero.SetSkinId(4); colorSkin = Color.HotPink; break;
                        case 5: Hero.SetSkinId(5); colorSkin = Color.Red; break;
                        case 6: Hero.SetSkinId(6); colorSkin = Color.OrangeRed; break;
                        case 7: Hero.SetSkinId(7); colorSkin = Color.Gold; break;
                        case 8: Hero.SetSkinId(8); colorSkin = Color.Indigo; break;
                        case 9: Hero.SetSkinId(9); colorSkin = Color.MediumVioletRed; break;
                    }
                }
            
        }
        //-------------------------------------------------------------------------------------------------------Draw
        private static void Game_OnDraw(EventArgs args)
        {
            if (Hero.IsDead) return;
            var Inimigo = EntityManager.Heroes.Enemies.Where(i=> i.Distance(Hero.Position) <= 1000 && !i.IsDead && !i.IsZombie && !i.IsInvulnerable).FirstOrDefault();
            if (true)
            {
                if (Menu["T.R"].Cast<CheckBox>().CurrentValue)
                {
                    Color color = Color.Red;
                    if (R.State.ToString() == Checkar)
                    {
                        Status = "Yes";
                        color = Color.LimeGreen;
                    }
                    else
                    {
                        Status = "No";
                        color = Color.Gray;
                    }
                    Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(30, -20);
                    Text.Color = Color.WhiteSmoke;
                    Text.TextValue = "Ultimate: ";
                    Text.Draw();
                    Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(-25, -20);
                    Text.Color = color;
                    Text.TextValue = Status;
                    Text.Draw();

                    string Select = "Null";
                    switch (Menu["M.E1"].Cast<ComboBox>().CurrentValue)
                    {
                        case 0: Select = "Burst"; break;
                        case 1: Select = "Segure"; break;
                    }
                    Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(30, -35);
                    Text.Color = Color.WhiteSmoke;
                    Text.TextValue = "Mode: ";
                    Text.Draw();
                    Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(-10, -35);
                    Text.Color = colorSkin;
                    Text.TextValue = Select.ToString();
                    Text.Draw();
                }
                if (Menu["T.N"].Cast<CheckBox>().CurrentValue)// Informa par ao usuario que o Inimigo é Matavel
                {
                    foreach (var Get in EntityManager.Heroes.Enemies.Where(i => i.Distance(Hero.Position) <= 1200 && !i.IsDead && !i.IsZombie && !i.IsInvulnerable && i.Health <= GetComboDamage(i) && i.IsHPBarRendered == true))
                    {
                        new Circle() { Color = colorSkin, BorderWidth = 7f, Radius = 100 }.Draw(Inimigo.Position);
                        new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = 90 }.Draw(Inimigo.Position);
                        //Efeito 3D
                        Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(101, -58);
                        Text1.Color = Color.WhiteSmoke;
                        Text1.TextValue = "Prediction: Kill Enemy [Line]";
                        Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -60);
                        Text1.Color = colorSkin;
                        Text1.TextValue = "Prediction: Kill Enemy [Line]";
                        Text1.Draw();
                        Drawing.DrawLine(Hero.Position.WorldToScreen(), Get.Position.WorldToScreen(), 5f, colorSkin);  
                    }
                }
            }

            Drawing.DrawText(20, 20, System.Drawing.Color.WhiteSmoke, "Timer: " + DateTime.Now.ToShortTimeString(), 40);
            if (Menu["D.Q"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = colorSkin, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 5f }.Draw(ObjectManager.Player.Position);
                new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = Q.Range }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["D.W"].Cast<CheckBox>().CurrentValue)new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = W.Range }.Draw(ObjectManager.Player.Position);
            if (Menu["D.E"].Cast<CheckBox>().CurrentValue) new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = E.Range }.Draw(ObjectManager.Player.Position);
            if (Menu["D.R"].Cast<CheckBox>().CurrentValue)new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = R.Range }.Draw(ObjectManager.Player.Position);

            var PegarMinionsAlly = EntityManager.MinionsAndMonsters.AlliedMinions.Where(m => m.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Distance(Inimigo.Position) >= 500).FirstOrDefault();
            var PegarMinionsEnemy = EntityManager.MinionsAndMonsters.EnemyMinions.Where(m => m.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Distance(Inimigo.Position) >= 500).FirstOrDefault();
            var PegarAlly = EntityManager.Heroes.Allies.Where(m => m.Distance(Hero.Position) <= E.Range && !m.IsDead && !m.IsMe && !m.IsDead && m.Distance(Inimigo.Position) >= 500).FirstOrDefault();
            var PegarWard = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.Name.ToLower().Contains("ward") && m.Distance(Hero.Position) <= E.Range && !m.IsDead && m.Distance(Inimigo.Position) >= 500).FirstOrDefault();
            //var Alvo = ObjectManager.Get<Obj_AI_Base>().Where(x => Hero.Distance(x.ServerPosition) <= E.Range && Hero.Distance(PegarAlly.Position) <= 600 && !x.IsMe && !x.IsEnemy && x.IsTargetable && !x.IsInvulnerable && x.Distance(Hero.Position) >= 250 && x.Distance(Inimigo.Position) >= 450).FirstOrDefault();
            if (Menu["E.E"].Cast<CheckBox>().CurrentValue && E.IsReady())//Informa em qual target pular se o evade estiver ligado
            {
                if (PegarMinionsAlly != null) new Circle() { Color = Color.LimeGreen, BorderWidth = 6f, Radius = 80 }.Draw(PegarMinionsAlly.Position);
                else if (PegarAlly != null) new Circle() { Color = Color.LimeGreen, BorderWidth = 6f, Radius = 80 }.Draw(PegarAlly.Position);
                else if (PegarMinionsEnemy != null) new Circle() { Color = Color.LimeGreen, BorderWidth = 6f, Radius = 80 }.Draw(PegarMinionsEnemy.Position);
                else if (PegarWard != null) new Circle() { Color = Color.LimeGreen, BorderWidth = 6f, Radius = 80 }.Draw(PegarWard.Position);
            }

            foreach (var ObjetoWard in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("SightWard") && !x.IsDead))
            { new Circle() { Color = Color.WhiteSmoke, BorderWidth = 6f, Radius = 100 }.Draw(ObjetoWard.Position); }
        }
        //-------------------------------------------------------------------------------------------------------SpellDamagem
        public static float GetComboDamage(Obj_AI_Base Inimigo)
        {
            float DanoSpell = 0;
            if (Q.IsReady()) DanoSpell += Hero.GetSpellDamage(Inimigo, SpellSlot.Q) + Hero.GetSpellDamage(Inimigo, SpellSlot.Q) / 2;
            if (W.IsReady()) DanoSpell += Hero.GetSpellDamage(Inimigo, SpellSlot.W);
            if (E.IsReady()) DanoSpell += Hero.GetSpellDamage(Inimigo, SpellSlot.E);
            if (R.IsReady() || Status == "Yes") DanoSpell += Hero.GetSpellDamage(Inimigo, SpellSlot.R);
            return DanoSpell;
        }

    }
}
