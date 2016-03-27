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
        public static SpellSlot Smite;
        public static int GetSmiteDamage()
        {
            return new int[] { 390, 410, 430, 450, 480, 510, 540, 570, 600, 640, 680, 720, 760, 800, 850, 900, 950, 1000 }
                [Player.Instance.Level - 1];
        }
        public static bool SmiteReady()
        {
            return Player.GetSpell(Smite).IsReady;
        }
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
        public static Text Text1 = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 20, System.Drawing.FontStyle.Bold));
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Bootstrap.Init(null);
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

            foreach (var spell in
                       Player.Instance.Spellbook.Spells.Where(
                         i =>
                               i.Name.ToLower().Contains("smite") &&
            (i.Slot == SpellSlot.Summoner1 || i.Slot == SpellSlot.Summoner2)))
            {
                Smite = spell.Slot;
            }

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
            Menu.AddSeparator(5);
            Menu.AddLabel("Logs:" + System.Environment.NewLine);
            Menu.AddLabel("✔ Add SkinHack");
                   Menu.AddLabel("✔ Add Killsteal Auto Jungle Baron & Dragon" + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add Logic Auto Check Enemy Range for Ultimate" + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add Use [Q] JungleClear"  + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add Use [Q] LaneClear"  + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add 2 Logic For using [w]" + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add 2 Logic For using [E]"  + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add Ant Gapcloser using [E]"  + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add Auto InterruptableSpell using [E]" + System.Environment.NewLine);
                   Menu.AddLabel("✔ Add Reset (AA) using Item" + System.Environment.NewLine);
                   Menu.AddLabel("✖ Version 1.4");

            Menu.AddGroupLabel("  ◣  Items ◥");
            Menu.Add("UseItemAtk", new CheckBox("✔  " + NomeChamp + "  Use Offensive Items", true));
            Menu.Add("UseItemDef", new CheckBox("✔  " + NomeChamp + "  Use Items Defensive", true));
            Menu.AddGroupLabel("  ◣  Combo ◥");
            Menu.Add("QCombo", new CheckBox("✔  " + NomeChamp + " Usar / Use Q [Combo]", true));
            Menu.Add("QLane", new CheckBox("✔  " + NomeChamp + " Usar / Use Q [LaneClear]", true));
            Menu.Add("QJungle", new CheckBox("✔  " + NomeChamp + " Usar / Use Q [JungleClear]", true));
            Menu.Add("Smith", new CheckBox("✔  " + NomeChamp + " Usar / Use Smith [Dragon/Baron]", true));
            Menu.AddGroupLabel("  ◣  [W] Modes ◥");
            Menu.Add("WCombo", new CheckBox("✔  " + NomeChamp + " Usar / Use W [Combo]", true));
            var ShieldMode = Menu.Add("ShieldMode", new Slider("Cast (W Mode)", 1, 1, 2));
            var Shield = new[] { NomeChamp, "[ Use Aways ]", "[ Use Logic Count Enemy ]" };
            ShieldMode.DisplayName = Shield[ShieldMode.CurrentValue];
            ShieldMode.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = Shield[changeArgs.NewValue]; };
            
            Menu.AddGroupLabel("  ◣  [E] Modes ◥");
            Menu.Add("ECombo", new CheckBox("✔  " + NomeChamp + " Usar / Use E [Combo]", true));
            var TauntMode = Menu.Add("TauntModes", new Slider("Cast (E Mode)   ||    1 Use Fast for Range || 2 Use Logic Max Range    ||", 1, 1, 2));
            var TMode = new[] { NomeChamp, "[ Use Fast ]", "[ Use Max Range ]" };
            TauntMode.DisplayName = TMode[TauntMode.CurrentValue];
            TauntMode.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = TMode[changeArgs.NewValue]; };
            Menu.AddGroupLabel("  ◣  [R] Ultimate ◥");
            Menu.Add("RCombo", new CheckBox("✔  " + NomeChamp + " Usar / Use R [Config]", true));
            foreach (var allies in EntityManager.Heroes.Allies.Where(i => !i.IsMe))
            { Menu.Add("SalvaAliado" + allies.ChampionName, new CheckBox("Ultimate in: " + allies.ChampionName, true));}
            Menu.Add("SaveUlt", new Slider("[R] Ultimate (Recommend 15% a 17%)", 17, 15, 30));
            Menu.AddGroupLabel("  ◣  SkinHack ◥");
            var SkinHack = Menu.Add("SkinID", new Slider("SkinHack Select", 3, 0, 6));
            var ID = new[] { "Classic", "SkinHack 1", "SkinHack 2", "SkinHack 3", "SkinHack 4", "SkinHack 5", "SkinHack 6"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID[changeArgs.NewValue]; };;
            Menu.AddGroupLabel("  ◣  Draw ◥");
            Menu.Add("DesabilitaDrawQ", new CheckBox("✔  " + NomeChamp + " Q Range", true));
            Menu.Add("DesabilitaDrawW", new CheckBox("✔  " + NomeChamp + " W Range", true));
            Menu.Add("DesabilitaDrawE", new CheckBox("✔  " + NomeChamp + " E Range", true));
            Menu.Add("DesabilitaText", new CheckBox("✔  " + NomeChamp + " ShowText Mode", true));
            Menu.Add("DesabilitaEspada", new CheckBox("✔  " + NomeChamp + " Draw [Sword]", true));

      
            
                       
            //Menu.Add("DesabilitaDrawLine", new CheckBox("✔  Get Line TargetSelector", true));
        /*    Menu.AddGroupLabel("  ◣  Kappa ◥");
            Menu.Add("ModelLoad", new KeyBind("Fizz Sword - Braziliam Hue", false, KeyBind.BindTypes.HoldActive, 'L'));*/
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

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
        private static void DrawHealths()
        {//Crédit Soresu From LeagueSharp
            float i = 0;
            foreach (var hero in EntityManager.Heroes.Allies.Where(hero => hero.IsAlly && !hero.IsMe && !hero.IsDead))
            {
                var playername = hero.Name;
                if (playername.Length > 13)
                {
                    playername = playername.Remove(9) + "...";
                }
                var champion = hero.ChampionName;
                if (champion.Length > 12)
                {
                    champion = champion.Remove(7) + "...";
                }
                var percent = (int)(hero.Health / hero.MaxHealth * 100);
                var color = Color.Red;
                if (percent > 25)
                {
                    color = Color.Orange;
                }
                if (percent > 50)
                {
                    color = Color.Yellow;
                }
                if (percent > 75)
                {
                    color = Color.LimeGreen;
                }
                Drawing.DrawText(Drawing.Width * 0.8f, Drawing.Height * 0.15f + i, color,"[ " + champion + " ]");
                Drawing.DrawText(Drawing.Width * 0.9f, Drawing.Height * 0.15f + i, color,((int)hero.Health).ToString() + " [" + percent.ToString() + "%]");
                i += 20f;
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
            var DrawText1 = Menu["DesabilitaText"].Cast<CheckBox>().CurrentValue;
            if (DrawText1)
            {
                foreach (var allies in EntityManager.Heroes.Allies.Where(i => !i.IsMe && !i.IsDead))// && i.CountEnemiesInRange(1000) >= 1 && i.HealthPercent <= 99))
                {
                    DrawHealths();

                }
            }

            var DrawEspada = Menu["DesabilitaEspada"].Cast<CheckBox>().CurrentValue;
            if (DrawEspada)
            {
                foreach (var Objeto in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.IsValid && !o.IsDead && o.BaseSkinName == "ShenSpirit"))
                {
                    Drawing.DrawText(30, 20, color, "Timer: " + DateTime.Now.ToShortTimeString());
                    Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Objeto.Position.WorldToScreen(), 5f, color);
                    Drawing.DrawCircle(Objeto.Position, 325f, color);
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
                var SelectModeShield = Program.Menu["ShieldMode"].DisplayName;
                Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -5);
                Text.Color = color;
                Text.TextValue = "[W] Mode: ";
                Text.Draw();
                Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(40, -5);
                Text.Color = Color.White;
                Text.TextValue = SelectModeShield;
                Text.Draw();
                foreach (var hero in EntityManager.Heroes.Allies.Where(hero => hero.IsAlly && !hero.IsMe && !hero.IsDead))
                {
                    if (hero.HealthPercent <= 25)
                    {
                        //Efeito 3D
                        Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(101, -58);
                        Text1.Color = Color.Black ;
                        Text1.TextValue = "[Prediction: Detected HP -25%]";
                        Text1.Draw();
                        Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -60);
                        Text1.Color = color;
                        Text1.TextValue = "[Prediction: Detected HP -25%]";
                        Text1.Draw();
                    }
                }


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
        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,Interrupter.InterruptableSpellEventArgs e)
        {
            if (e.Sender.IsEnemy && Player.Instance.Distance(sender, true) < E.Range)
            {
                E.Cast(sender);
            }
        }
        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if ( e.Sender.IsEnemy && Player.Instance.Distance(sender, true) < E.Range)
            {
              E.Cast(e.Sender);
              //  Q.Cast();
              //  W.Cast();
            }
        }
        public static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (target.IsValidTarget(Player.Instance.GetAutoAttackRange() + 200) && (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)));
            {
                if (Menu["UseItemAtk"].Cast<CheckBox>().CurrentValue)
                {
                    if (Tiamat.Cast()); //Orbwalker.ResetAutoAttack();
                    if (TitanicHydra.Cast()); //Orbwalker.ResetAutoAttack();
                    if (Hydra.Cast()) ; //Orbwalker.ResetAutoAttack();
                }
            }

            return;
        }
        public static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Physical);//Pega Target
            foreach (var Objeto in ObjectManager.Get<Obj_AI_Minion>().Where(o => o.IsValid && !o.IsDead && o.BaseSkinName == "ShenSpirit"))
            {
                //Q Logics
                if (Menu["QCombo"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(Player.Instance.GetAutoAttackRange() +200))//Ativação Rápida
                    {
                        Q.Cast();
                    }
                }
                //W Logics
                    if (W.IsReady() && Menu["WCombo"].Cast<CheckBox>().CurrentValue)
                    {
                        foreach (var Aliados in EntityManager.Heroes.Allies.Where(o => o.IsValid && !o.IsDead && o.CountAlliesInRange(250f) >= 1))
                            foreach (var Inimigos in EntityManager.Heroes.Enemies.Where(o => o.IsValid && !o.IsDead && o.CountEnemiesInRange(250f) >= 1))
                            {
                                {
                                    var SelectModeShield = Program.Menu["ShieldMode"].DisplayName;
                                    switch (SelectModeShield)
                                    {
                                        case "[ Use Logic Count Enemy ]":
                                            if (Aliados.IsInRange(Objeto, 325f) && Inimigo.IsInRange(Objeto, 325f)) //Objeto.CountAlliesInRange(325f) >= 1 && Objeto.CountEnemiesInRange(325f) >= 1)
                                            {
                                                W.Cast();
                                            }
                                            break;
                                        case "[ Use Aways ]":
                                            if (Q.IsOnCooldown)
                                            {
                                                W.Cast();
                                            }
                                            break;
                                    }
                                }
                            }
                    }
                    // else if (Objeto.Distance(Player.Instance) >= 500 && Inimigo.Distance(Player.Instance) <=400)
                
                if (Menu["ECombo"].Cast<CheckBox>().CurrentValue)
                {
                    var SelectModeTaunt = Program.Menu["TauntModes"].DisplayName;
                    switch (SelectModeTaunt)
                    {
                        case "[ Use Max Range ]":
                            if (Inimigo.IsValidTarget(600) && E.IsReady()) E.Cast(Inimigo.Position);
                            break;
                        case "[ Use Fast ]":
                            //Orbwalker.OrbwalkTo(Game.CursorPos);
                            if (E.IsReady()) E.Cast(Inimigo.Position);
                            break;
                    }
                }
            }

                if (Menu["UseItemAtk"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(250) && Yumus.IsReady()) Yumus.Cast();
                    if (Inimigo.IsValidTarget(400) && BOTRK.IsReady()) BOTRK.Cast(Inimigo);
                    if (Inimigo.IsValidTarget(550) && Bilgewater.IsReady()) Bilgewater.Cast(Inimigo);
                }

                if (Menu["UseItemDef"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(500) && Randuin.IsReady()) Randuin.Cast();
                }
        }
        public static void LaneClear()
        {
            if (Menu["QLane"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var source in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValid && a.IsValidTarget(250) && !a.IsDead))
                {
                    Q.Cast();
                }
            }
        }
        public static void JungleClear()
        {
            if (Menu["QJungle"].Cast<CheckBox>().CurrentValue)
            {
                var jg = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 250).FirstOrDefault(e => e.IsValid && !e.IsDead);
                if (Q.IsReady() && jg.IsValidTarget(250))
                {
                    Q.Cast();
                }
            }
        }
        public static void Execute()
        {
            if (true)
            {
                var creep = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 800).
                    Where(x => x.BaseSkinName == "SRU_Dragon" || x.BaseSkinName == "SRU_Baron");
                foreach (var x in creep.Where(y => Player.Instance.Distance(y.Position)
                        <= Player.Instance.BoundingRadius + 500 + y.BoundingRadius))
                {
                    if (x != null && x.Health <= GetSmiteDamage())
                        Player.Instance.Spellbook.CastSpell(Smite, x);
                }
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (Menu["Smith"].Cast<CheckBox>().CurrentValue) Execute();
         //   if (Player.Instance.CountEnemiesInRange(Q.Range) > 0)
            LoadingSkin();
            Ultimate();
         //   Kappa();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
               LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
                JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
            }
            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (!Inimigo.IsValid()) return;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
                Combo();
            }
        }
    }
}
