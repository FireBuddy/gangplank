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
        public static String NomeChamp = Player.Instance.ChampionName;
        public static readonly Text Text = new Text("",new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 12, System.Drawing.FontStyle.Bold));
        public static readonly Text Text1 = new Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 10, System.Drawing.FontStyle.Bold));
        //EVENTO///////////////////////////////////////////////////////////////////////////////
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_Iniciar;
            EloBuddy.Hacks.RenderWatermark = false;
        }
        //CARREGAMENTO DE GAME COMPLETO + MENU //////////////////////////////////////////////////////////
        private static void Game_Iniciar(EventArgs args)
        {
            var MeuCampeao = ObjectManager.Player.ChampionName;
            Menu = MainMenu.AddMenu("SkinHack 2016 V3", "SkinModelHack");
            Menu.AddGroupLabel("─────────────────────────────────────────");
            Menu.AddGroupLabel("  ◣  SkinHack ◥");
            Menu.Add("UseSkinHack", new CheckBox("✔   " + MeuCampeao + " - ( Use SkinHack 1 To 11)", true));
            var SkinHack = Menu.Add("SkinID", new Slider("SkinHack Select", 1, 0, 11));
            var ID = new[] {"Classic","SkinHack 1","SkinHack 2","SkinHack 3","SkinHack 4","SkinHack 5","SkinHack 6","SkinHack 7","SkinHack 8","SkinHack 9","SkinHack 10","SkinHack 11"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs){sender.DisplayName = ID[changeArgs.NewValue];};
            Menu.AddGroupLabel("─────────────────────────────────────────");
            Menu.AddGroupLabel("  ◤  ModelHack  ◢");
            Menu.Add("UseModelHack", new CheckBox("✔   " + MeuCampeao + " - ( Use ModelHack 1 To 128)", false));
            Menu.Add("ModelLoad", new KeyBind("Active / Ligar", false, KeyBind.BindTypes.HoldActive, 'N'));
            var ModelHack =  Menu.Add("ModelID", new Slider("ModelHack Select", 1, 0, 128));
            var ID1 = new[] {
            "Aatrox","Ahri","Akali","Alistar","Amumu","Anivia","Annie","Ashe","Azir","Bard","Blitzcrank","Brand","Braum","Caitlyn",
            "Cassiopeia","ChoGath","Corki","Darius","Diana","Draven","Ekko","Elise","Evelynn","Ezreal","Fiddlesticks","Fiora","Fizz",
            "Galio","Gangplank","Garen","Gnar","Gragas","Graves"," Hecarim","Heimerdinger","Illaoi","Irelia","Janna"," Jarvan IV ","Jax",
            "Jayce","Jhin","Jinx","Kalista","Karma","Karthus","Kassadin","Katarina","Kayle","Kennen","Khazix","Kindred","KogMaw","LeBlanc",
            "LeeSin","Leona","Lissandra","Lucian","Lulu","Lux","Malphite","Malzahar","Maokai"," MasterYi","MissFortune","Mordekaiser","Morgana",
            "Dr.Mundo","Nami","Nasus","Nautilus","Nidalee","Nocturne","Nunu","Olaf","Orianna","Pantheon","Poppy","Quin","Rammus","RekSai",
            "Renekton","Rengar","Riven","Rumble","Ryze","Sejuani","Shaco","Shen","Shyvana","Singed","Sion","Sivir"," Skarner","Sona","Soraka",
            "Swain","Syndra","TahmKench","Talon","Taric","Teemo","Thresh","Tristana","Trundle","Tryndamere","TwistedFate","Twitch","Udyr","Urgot",
            "Varus","Vayne","Veigar","Velkoz","Vi","Viktor","Vladimir","Volibear","Warwick","Wukong","Xerath","XinZhao","Yasuo","Yorick","Zac",
            "Zed","Ziggs","Zilean","Zyra"};
            ModelHack.DisplayName = ID1[ModelHack.CurrentValue];
            ModelHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID1[changeArgs.NewValue]; };
            Menu.AddGroupLabel("─────────────────────────────────────────");
            Menu.AddGroupLabel("  ◣  Extra  ◥");
            Menu.Add("DrawTarget", new CheckBox("✔   GetLine ( TargetSelector )", true));
            Menu.Add("DrawTEXT", new CheckBox("✔   Show Text ( coming soon )", true));
            Menu.AddGroupLabel("─────────────────────────────────────────");
            Menu.AddGroupLabel("By: UnrealSkill99");

            Chat.Print("|| SkinHack 2016 V3 || <font color='#5e5e5e'>By: UnrealSkill99 </font>", System.Drawing.Color.White);
            Player.Instance.SetModel(NomeChamp);//Reset My Champion!
            Chat.Print("|| SkinHack 2016 V3 || <font color='#ff0000'>Model Original Load " + NomeChamp + "</font>", System.Drawing.Color.White);
            //Chat.Print(Player.Instance.BaseSkinName);
            Game.OnUpdate += Game_Atualizar;
            Drawing.OnDraw += Game_OnDraw;
        }

        //Drawing
        public static void Game_OnDraw(EventArgs args)
        {
       
            if (Menu["DrawTEXT"].Cast<CheckBox>().CurrentValue)
            {
               /* Drawing.DrawText(Drawing.Width - 290, 100, Color.Gray, "SkinHack ID: ");
                Drawing.DrawText(Drawing.Width - 200, 100, Color.White, Program.Menu["SkinID"].DisplayName);

                Drawing.DrawText(Drawing.Width - 290, 80, Color.Gray, "ModelHack ID: ");
                Drawing.DrawText(Drawing.Width - 190, 80, Color.White, Program.Menu["ModelID"].DisplayName);

                Drawing.DrawText(Drawing.Width - 290, 60, Color.Gray, "Original ID: ");
                Drawing.DrawText(Drawing.Width - 215, 60, Color.White, NomeChamp.ToString());
                
                Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(650, 400);
                Text1.Color = Color.White ;
                Text1.TextValue = "✔ Original: ";
                Text1.Draw();
                Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(564, 400);
                Text1.Color = Color.Red;
                Text1.TextValue = NomeChamp.ToString();
                Text1.Draw();

                Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(650, 380);
                Text1.Color = Color.White;
                Text1.TextValue = "✔ ModelHack: ";
                Text1.Draw();
                Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(544, 380);
                Text1.Color = Color.Red;
                Text1.TextValue = Program.Menu["ModelID"].DisplayName;
                Text1.Draw();

                Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(650, 360);
                Text1.Color = Color.White;
                Text1.TextValue = "✔ SkinHack: ";
                Text1.Draw();
                Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(552, 360);
                Text1.Color = Color.Red;
                Text1.TextValue = Program.Menu["SkinID"].DisplayName;
                Text1.Draw();*/
            }
            //Drawing TargetSelect
            if (Menu["DrawTarget"].Cast<CheckBox>().CurrentValue)
            {
                var Inimigo = TargetSelector.GetTarget(1500, DamageType.Physical);
                new Circle() { Color = Color.White, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 4 }.Draw(ObjectManager.Player.Position);
                Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 3, Color.White);
                foreach (var ai in EntityManager.Heroes.Enemies)
                {
                    if (ai.IsValidTarget())
                    {
                        Text.Position = Drawing.WorldToScreen(Inimigo.Position) - new Vector2(0, 0);
                        Text.Color = Color.White;
                        Text.TextValue = "✖ " + Inimigo.ChampionName + "";
                        Text.Draw();
                    }
                }
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
                case "SkinHack 11":
                    Player.Instance.SetSkinId(11);
                    break;
            }
        }

        //ModelHack
     public static void ActiveModelHack()
        {
            if (Menu["UseModelHack"].Cast<CheckBox>().CurrentValue)
            {
                if (Menu.Get<KeyBind>("ModelLoad").CurrentValue)
                {
                    var ModelHackSelect = Program.Menu["ModelID"].DisplayName;
                    Player.Instance.SetModel(ModelHackSelect);
                    Chat.Print("|| SkinHack 2016 V3 || <font color='#ff0000'>Carregado / Load ModelHack " + ModelHackSelect + "</font>", System.Drawing.Color.White);
                }
            }
        }

        private static void Game_Atualizar(EventArgs args)
        {
            if (Menu["UseSkinHack"].Cast<CheckBox>().CurrentValue)
            {
                LoadingSkin();
                ActiveModelHack();
            }
        }
    }
}
