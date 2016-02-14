using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Microsoft.Win32;
using SharpDX;
using System.Threading;
using System.Net;
using EloBuddy.SDK.Rendering;
using System.Diagnostics;
using Color = System.Drawing.Color;
using System.Drawing;

namespace Skin
{
    class Program
    {
        public static Menu Menu;
        //EVENTO///////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_Iniciar;
            Game.OnUpdate += Game_Atualizar;
            
            EloBuddy.Hacks.RenderWatermark = false;
            
        }
        //CARREGAMENTO DE GAME COMPLETO + MENU //////////////////////////////////////////////////////////
        private static void Game_Iniciar(EventArgs args)
        {
            var MeuCampeao = ObjectManager.Player.ChampionName;
            Menu = MainMenu.AddMenu("SkinHack 2016 V2", "SkinModelHack");
            Menu.AddLabel("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||  SkinHack (Selecione de 1 a 10");
            Menu.Add("UseSkinHack", new CheckBox("|| " + MeuCampeao + " Use SkinHack ||", true));
            var SkinHack = Menu.Add("SkinID", new Slider("SkinHack Select", 1, 0, 10));
            var ID = new[] { 
                "Classic",
                "SkinHack 1", 
                "SkinHack 2", 
                "SkinHack 3", 
                "SkinHack 4", 
                "SkinHack 5", 
                "SkinHack 6", 
                "SkinHack 7", 
                "SkinHack 8", 
                "SkinHack 9", 
                "SkinHack 10" };
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs){sender.DisplayName = ID[changeArgs.NewValue];};

            Menu.AddLabel("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||  ModelHack (Ainda em Criação)");
            Menu.Add("UseModelHack", new CheckBox("|| " + MeuCampeao + "  Use ModelHack || (F5)", false));
            var ModelHack =  Menu.Add("ModelID", new Slider("ModelHack Select", 1, 0, 100));
            Menu.AddLabel("Coming soon");

            Menu.AddLabel("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||  Enemy (Desenha Linha Reta de Inimigo)");
            Menu.Add("DrawTarget", new CheckBox("|| TargetSelector Line (Enemy)", true));

            Menu.AddSeparator(100);
            Menu.AddGroupLabel("By: UnrealSkill99");


            Chat.Print("SkinHack 2016 Version 2", System.Drawing.Color.Black);
            Chat.Print("By: UnrealSkill99", System.Drawing.Color.Black);
          //  Player.Instance.SetSkinId(0);//Reset load
            Drawing.OnDraw += Game_OnDraw;
        }

        //Drawing
        private static void Game_OnDraw(EventArgs args)
        {
            var Inimigo = TargetSelector.GetTarget(700, DamageType.Physical);
            if (Menu["DrawTarget"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 4f }.Draw(ObjectManager.Player.Position);
                Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 3, Color.Black);
            }
        }


        //Skins
        public static void LoadingSkin()
        {
            var SkinHackSelect = Program.Menu["SkinID"].DisplayName;
            switch (SkinHackSelect)
            {
                case "Classic":
                   Player.Instance.SetSkinId(0);
                    break;
                case "SkinHack 1":
                  Player.Instance.SetSkinId(1);
                    break;
                case "SkinHack 2":
                  Player.Instance.SetSkinId(2);
                    break;
                case "SkinHack 3":
                 Player.Instance.SetSkinId(3);
                    break;
                case "SkinHack 4":
                  Player.Instance.SetSkinId(4);
                    break;
                case "SkinHack 5":
                    Player.Instance.SetSkinId(5);
                    break;
                case "SkinHack 6":
                  Player.Instance.SetSkinId(6);
                    break;
                case "SkinHack 7":
                  Player.Instance.SetSkinId(7);
                    break;
                case "SkinHack 8":
                   Player.Instance.SetSkinId(8);
                    break;
                case "SkinHack 9":
                   Player.Instance.SetSkinId(9);
                    break;
                case "SkinHack 10":
                Player.Instance.SetSkinId(10);
                    break;
            }
        }

        private static void Game_Atualizar(EventArgs args)
        {
            if (Menu["UseSkinHack"].Cast<CheckBox>().CurrentValue)
            {
                LoadingSkin();
            }
        }
    }
}
