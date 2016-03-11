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
//Addon by 능력 언리얼
namespace UnrealSkill_Shen
{
    class Program
    {
        public AIHeroClient myTarget = null;
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Targeted R;
        public static Menu Menu;
        static Item Tiamat, Hydra, BOTRK, Bilgewater,Yumus,TitanicHydra, Randuin;
        public static  String NomeChamp = "Shen";
        public static AIHeroClient Champion { get { return Player.Instance; } }
        public static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold));
        public static Text Text1 = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 13, System.Drawing.FontStyle.Bold));
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
            if (Player.Instance.ChampionName != NomeChamp)
                return;

            Q = new Spell.Active(SpellSlot.Q, 200);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Linear, 250, 1600, 50)
            { MinimumHitChance = HitChance.High };
            R = new Spell.Targeted(SpellSlot.R, 25000);


            //Atk
            BOTRK = new Item(3153, 550);
            Bilgewater = new Item(3144, 550);
            Hydra = new Item(3074, 400);
            Tiamat = new Item(3077, 400);
            Yumus = new Item(3142, 400);
            TitanicHydra = new Item(3748, 400);
            //Def
            Randuin = new Item(3143, 500);
            //-------------------------------------------------------CHAT -----------------------------------------------------------
            Chat.Print("||  UnrealSkill ||" + NomeChamp + "  <font color='#15c757'>Carregado / Load</font>", System.Drawing.Color.White);
            //-------------------------------------------------------MENU ------------------------------------------------------------
            Menu = MainMenu.AddMenu("[ Shen ]", "USV");
            Menu.AddGroupLabel("  ◣  Combo ◥");
            Menu.Add("QCombo", new CheckBox("✔  " + NomeChamp + " Usar / Use Q", true));
            Menu.Add("WCombo", new CheckBox("✔  " + NomeChamp + " Usar / Use W", true));
            Menu.Add("ECombo", new CheckBox("✔  " + NomeChamp + " Usar / Use E", true));
            Menu.Add("RCombo", new CheckBox("✔  " + NomeChamp + " Usar / Use R", true));
            Menu.AddGroupLabel("  ◣  [E] Modes ◥");
            var TauntMode = Menu.Add("TauntModes", new Slider("Cast (E Mode)   ||    1 Use Fast for Range || 2 Use Logic Max Range    ||", 1, 1, 2));
            var TMode = new[] { NomeChamp, "1 Use Fast", "2 Use Max Range" };
            TauntMode.DisplayName = TMode[TauntMode.CurrentValue];
            TauntMode.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = TMode[changeArgs.NewValue]; };
            Menu.AddGroupLabel("  ◣  [R] Ultimate ◥");
            foreach (var allies in EntityManager.Heroes.Allies.Where(i => !i.IsMe))
            { Menu.Add("SalvaAliado" + allies.ChampionName, new CheckBox("Save: " + allies.ChampionName, true));}
            Menu.Add("SaveUlt", new Slider("% HP For R (Recommend 15%) Auto check Enemy within the dangerous range", 15, 0, 100));
            Menu.AddGroupLabel("  ◣  Draw ◥");
           //Menu.Add("DesabilitaDraw", new CheckBox("✔  " + NomeChamp + " My Range", true));
            Menu.Add("DesabilitaDrawQ", new CheckBox("✔  " + NomeChamp + " Q Range", true));
            Menu.Add("DesabilitaDrawW", new CheckBox("✔  " + NomeChamp + " W Range", true));
            Menu.Add("DesabilitaDrawE", new CheckBox("✔  " + NomeChamp + " E Range", true));
            Menu.Add("DesabilitaText", new CheckBox("✔  " + NomeChamp + " ShowText Mode", true));
            Menu.Add("DesabilitaEspada", new CheckBox("✔  " + NomeChamp + " Draw [Sword]", true));
            //Menu.Add("DesabilitaDrawLine", new CheckBox("✔  Get Line TargetSelector", true));
            Menu.AddGroupLabel("  ◣  Items ◥");
            Menu.Add("UseItemAtk", new CheckBox("✔  " + NomeChamp + "  Use Offensive Items", true));
            Menu.Add("UseItemDef", new CheckBox("✔  " + NomeChamp + "  Use Items Defensive", true));
            Menu.AddGroupLabel("  ◣  SkinHack ◥");
            var SkinHack = Menu.Add("SkinID", new Slider("SkinHack Select", 3, 0, 6));
            var ID = new[] { "Classic", "SkinHack 1", "SkinHack 2", "SkinHack 3", "SkinHack 4", "SkinHack 5", "SkinHack 6"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID[changeArgs.NewValue]; };;

        /*    Menu.AddGroupLabel("  ◣  Kappa ◥");
            Menu.Add("ModelLoad", new KeyBind("Fizz Sword - Braziliam Hue", false, KeyBind.BindTypes.HoldActive, 'L'));*/
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            //EloBuddy.Hacks.RenderWatermark = false;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }


        ////////////////////////////////////////////////////////////////////SKIN////////////////////////////////////////////////////////////////////
        //Skins
        public static void LoadingSkin()
        {
             foreach (var Objeto in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.IsValid && o.BaseSkinName == "ShenSpirit"))
           // foreach (var Objeto in ObjectManager.Get<Obj_AI_Base>().Where(o => o.Name.ToLower().Equals("ShenSpiritUnit")))
            {
                var SkinHackSelect = Program.Menu["SkinID"].DisplayName;
                switch (SkinHackSelect)
                {
                    case "Classic":
                        Player.Instance.SetSkinId(0);
                       // Objeto.SetModel("Shop");
                       // Objeto.SetSkinId(0);
                        break;
                    case "SkinHack 1":
                        Player.Instance.SetSkinId(1);
                       // Objeto.SetSkinId(1);
                        break;
                    case "SkinHack 2":
                        Player.Instance.SetSkinId(2);
                       // Objeto.SetSkinId(2);
                        break;
                    case "SkinHack 3":
                        Player.Instance.SetSkinId(3);
                       // Objeto.SetSkinId(3);
                        break;
                    case "SkinHack 4":
                        Player.Instance.SetSkinId(4);
                       // Objeto.SetSkinId(4);
                        break;
                    case "SkinHack 5":
                        Player.Instance.SetSkinId(5);
                      //  Objeto.SetSkinId(5);
                        break;
                    case "SkinHack 6":
                        Player.Instance.SetSkinId(6);
                       // Objeto.SetSkinId(6);
                        break;
                }
            }
        }
        public static void Kappa()
        {
            foreach (var Objeto in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.IsValid && o.BaseSkinName == "ShenSpirit"))
            {
                if (Menu.Get<KeyBind>("ModelLoad").CurrentValue)
                {

                    Objeto.SetModel("HA_AP_Poro");
                }
        }
        }

        private static void Game_OnDraw(EventArgs args)
        {
            var SkinHackSelect = Program.Menu["SkinID"].DisplayName;
            Color color;
            switch (SkinHackSelect)
            {
                default:
                    color = Color.Transparent;
                    break;
                case "Classic":
                    color = Color.MediumVioletRed;
                    break;
                case "SkinHack 1":
                    color = Color.DeepSkyBlue;
                    break;
                case "SkinHack 2":
                    color = Color.OrangeRed;
                    break;
                case "SkinHack 3":
                    color = Color.MediumSeaGreen;
                    break;
                case "SkinHack 4":
                    color = Color.DarkRed;
                    break;
                case "SkinHack 5":
                    color = Color.OrangeRed;
                    break;
                case "SkinHack 6":
                    color = Color.BlueViolet;
                    break;
            }

            foreach (var allies in EntityManager.Heroes.Allies.Where(i => !i.IsMe && !i.IsDead && i.CountEnemiesInRange(1000) >= 1 && i.HealthPercent <= 10))
            {
                var Aliado = "";
                if (Aliado == "")
                {
                  //  Drawing.DrawText(20, 20, color, "Notification [HP -20%] " + allies.ChampionName, 40);
                    Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), allies.Position.WorldToScreen(), 5f, Color.Red);
                }
               /* if (Aliado != "")
                {
                    Aliado = "";
                    Drawing.DrawText(30, -30, color, "Notification [HP -20%] " + allies.ChampionName, 40);
                }*/
            }


            var DrawEspada = Menu["DesabilitaEspada"].Cast<CheckBox>().CurrentValue;
            if (DrawEspada)
            {
                foreach (var Objeto in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.IsValid && !o.IsDead && o.BaseSkinName == "ShenSpirit"))
                {
                    Drawing.DrawText(30, 20, color, "Timer: " + DateTime.Now.ToShortTimeString(), 40);
                    Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Objeto.Position.WorldToScreen(), 5f, color);
                    Drawing.DrawCircle(Objeto.Position, 100, color);
                    //Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Objeto.Position.WorldToScreen(), 4f, Color.FromArgb(90, color));
                }
            }


            var Inimigo = TargetSelector.GetTarget(1500, DamageType.Physical);
           // var DrawAutoAtk = Menu["DesabilitaDraw"].Cast<CheckBox>().CurrentValue;
            var DrawQ = Menu["DesabilitaDrawQ"].Cast<CheckBox>().CurrentValue;
            var DrawW = Menu["DesabilitaDrawW"].Cast<CheckBox>().CurrentValue;
            var DrawE = Menu["DesabilitaDrawE"].Cast<CheckBox>().CurrentValue;
            var DrawText = Menu["DesabilitaText"].Cast<CheckBox>().CurrentValue;
            //var Line = Menu["DesabilitaDrawLine"].Cast<CheckBox>().CurrentValue;
            //if (DrawAutoAtk) { Drawing.DrawCircle(ObjectManager.Player.Position, Champion.GetAutoAttackRange, color); }
            if (DrawQ)
            {
                Drawing.DrawCircle(Player.Instance.Position, Q.Range, color);
            }
            if (DrawW)
            {
                Drawing.DrawCircle(Player.Instance.Position, W.Range, color);
            }
            if (DrawE)
            {
                Drawing.DrawCircle(Player.Instance.Position, E.Range, color);
            }
          /*  if (Line)
            {
                Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 5f, color);
            }*/
            if (DrawText)
            {
                var SelectModeTaunt = Program.Menu["TauntModes"].DisplayName;
                Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -20);
                Text.Color = color;
                Text.TextValue = "[E] Mode: ";
                Text.Draw();
                Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(40, -20);
                Text.Color = Color.White;
                Text.TextValue = SelectModeTaunt;
                Text.Draw();


            }
            
        }  
        
        public static void Ultimate()
        {
            if (Menu["RCombo"].Cast<CheckBox>().CurrentValue)
            {
                var VerificaHPAliado = Menu["SaveUlt"].Cast<Slider>().CurrentValue;
                foreach (var allies in EntityManager.Heroes.Allies.Where(i => !i.IsMe && i.CountEnemiesInRange(600) >= 1))
                //foreach (var ally in EntityManager.Heroes.Allies.Where(x => x.IsValidTarget(R.Range) && x.HealthPercent < 95).Where(ally => R.IsReady()))// && ally.CountEnemiesInRange(650) >= 1))
                {
                    var SalvaVidas = Menu["SalvaAliado" + allies.ChampionName].Cast<CheckBox>().CurrentValue;
                    if (allies.HealthPercent < VerificaHPAliado)
                    {
                        if (SalvaVidas)
                        {
                            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            {
                                R.Cast(allies);
                            }
                        }
                    }
                }
            }
        }
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,Interrupter.InterruptableSpellEventArgs args)
        {
            var intTarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (E.IsReady() && sender.IsValidTarget(E.Range))
            {
                    E.Cast(intTarget.ServerPosition);
            }
        }
        public static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Physical);
         //   if (!Inimigo.IsValid()) return;
            if (Q.IsReady() && Inimigo.IsValidTarget(300) && (Menu["QCombo"].Cast<CheckBox>().CurrentValue))
                {
                   Q.Cast();
                }
            //W Logics
            foreach (var Objeto in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.IsValid && !o.IsDead && o.BaseSkinName == "ShenSpirit"))
            {
                if (Q.IsOnCooldown && W.IsInRange(Inimigo) && Menu["WCombo"].Cast<CheckBox>().CurrentValue)
                {
                    W.Cast();
                }
                else if (Objeto.Distance(Player.Instance) >= 500 && Inimigo.Distance(Player.Instance) <=400)
                {
                    W.Cast();
                }
            }
                if (Menu["ECombo"].Cast<CheckBox>().CurrentValue)
                {
                    var SelectModeTaunt = Program.Menu["TauntModes"].DisplayName;
                    switch (SelectModeTaunt)
                    {
                        case "2 Use Max Range":
                            if (Inimigo.IsValidTarget(600) && E.IsReady()) E.Cast(Inimigo);
                            break;
                        case "1 Use Fast":
                            if (E.IsReady() && E.IsInRange(Inimigo)) E.Cast(Inimigo);
                            break;
                    }
                }

                if (Menu["UseItemAtk"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(250) && TitanicHydra.IsReady()) TitanicHydra.Cast();
                    if (Inimigo.IsValidTarget(250) && Yumus.IsReady()) Yumus.Cast();
                    if (Inimigo.IsValidTarget(300) && Tiamat.IsReady())Tiamat.Cast();
                    if (Inimigo.IsValidTarget(300) && Hydra.IsReady()) Hydra.Cast();
                    if (Inimigo.IsValidTarget(400) && BOTRK.IsReady()) BOTRK.Cast(Inimigo);
                    if (Inimigo.IsValidTarget(550) && Bilgewater.IsReady()) Bilgewater.Cast(Inimigo);
                }

                if (Menu["UseItemDef"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(500) && Randuin.IsReady()) Randuin.Cast();
                }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
         //   if (Player.Instance.CountEnemiesInRange(Q.Range) > 0)
            LoadingSkin();
            Ultimate();
         //   Kappa();
            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (!Inimigo.IsValid()) return;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
        }
    }
}
