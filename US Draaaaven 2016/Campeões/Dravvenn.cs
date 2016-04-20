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
    class Dravvenn
    {
        private static Menu Menu;
        private static AIHeroClient Hero = ObjectManager.Player;
        private static String NomeHeroi = Hero.ChampionName;
        public static Obj_AI_Base Ini;
        private static Spell.Active Q, W;
        private static Spell.Skillshot E, R;
        public static Text Text1 = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 20, System.Drawing.FontStyle.Bold));
        private static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15, System.Drawing.FontStyle.Bold));
        private static Text Text0 = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold));
        static Item BOTRK, Bilgewater, Yumus, Mercurial, Bandana;   
        public static Color color = Color.DarkOrange;
        private static int PassiveCount// Total Crédit PassiveCount - Darakath
        {
            get
            {
                var data = Player.Instance.GetBuff("dravenspinningattack");
                if (data == null || data.Count == -1)
                {
                    return 0;
                }
                return data.Count == 0 ? 1 : data.Count;
            }
        }

        public Dravvenn()
        {
            Loading.OnLoadingComplete += Spell;
            Loading.OnLoadingComplete += Menus;
        }
        private static void Spell(EventArgs args)
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear, 250, 1400, 130);
            E.AllowedCollisionCount = 0;
            E.MinimumHitChance = HitChance.High;
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear);
            R.AllowedCollisionCount = 0;
            R.MinimumHitChance = HitChance.High;

            BOTRK = new Item(3153, 550);
            Bilgewater = new Item(3144, 550);
            Yumus = new Item(3142, 400);
            Mercurial = new Item(3139);
            Bandana = new Item(3140);
        }

        private static void Menus(EventArgs args)
        {
            if (Hero.ChampionName != "Draven") return;
            Menu = MainMenu.AddMenu(NomeHeroi, NomeHeroi);
            Menu.AddSeparator(1);
            Menu.AddLabel("Creator: UnrealSkill-VIP");

            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddGroupLabel("〈〈〈〈 Explaining Modes 〉〉〉〉");
            Menu.AddLabel("✔ Information - If You have Problems With the (Challenge Mode) or (C# Developer Mode)");
            Menu.AddLabel(" Use (Normal Mode), and Set Down Delay. ' Ping 1 to 120 ' Use Delay 100 or 350");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("★★☆☆☆ [Normal Mode] - It is More Humanized and you can");
            Menu.AddLabel("configure the Catch Axes time. Posibilitando if the player is a high ping also");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("★★★☆☆ [Challenger Mode] - Try to focus on not losing Axe No Way");
            Menu.AddLabel("70% probability of not losing the ax combo, but can not be configured delay");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("★★★★★ [C# Developer Mode] - Unbelievable Mechanica 2-3 Axes");
            Menu.AddLabel("(You Need This Item With ' Ghost Dancer ' and Speed Boot for 3 Axes) ");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.Add("ModeAxe", new ComboBox("〈〈〈〈  Select Player Mode  〉〉〉〉Always Test Other Modes",0, "1 - [Challenger Mode]", "2 - [Normal Mode]", "3 - [C# Developer Mode]"));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.Add("DelayAX", new Slider("Delay Pick Axes Only - (Normal Mode)", 250, 0, 500));
            Menu.AddLabel("Config in Core > Ticks Per Second: 20 a 25 '");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.Add("UseSkinHack", new CheckBox("❐ " + NomeHeroi + "  Use SkinHack", true));
            var Skin = Menu.Add("SkinHack", new ComboBox("✔ Select your Skin Hack", 6, "Classic Draven", "Soul Reaver Draven", "Gladiator Draven", "Primetime Draven", "Pool Party Draven", "Beast Hunter Draven", "Draven Draven"));
            Skin.OnValueChange += delegate
            {
                var Notificação = new EloBuddy.SDK.Notifications.SimpleNotification("SkinHack" ,"New Select: "+ Skin.SelectedText);
                EloBuddy.SDK.Notifications.Notifications.Show(Notificação, 5000);
            };

            Menu.Add("ModeE", new ComboBox("✔ Select Game Mode Using [E] (Recommecd Secure)", 1, "Mode [Aggressive]", "Mode [Safe]", "Mode [GapCloser]"));
            Menu.Add("AxeGet", new ComboBox("✔ Select the Pick Axes Method (Recommecd Aways)", 0, "Mode [Always]", "Mode [Combo]", "Never"));
            Menu.Add("AT", new CheckBox("❐   " + NomeHeroi + " - Catch [ Axe ] In Tower Enemy", false));
            Menu.Add("CR", new Slider("Catch Axes Distance [ Cursor Mouse ]  (Recommend 255) ", 255, 210, 700));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Combo");
            Menu.Add("Q", new CheckBox("❐   " + NomeHeroi + " - [ Q ]", true));
            Menu.Add("W", new CheckBox("❐   " + NomeHeroi + " - [ W ] ", true));
            Menu.Add("E", new CheckBox("❐   " + NomeHeroi + " - [ E ]", true));
            Menu.Add("R", new CheckBox("❐   " + NomeHeroi + " - [ R ]", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Farm");
            Menu.Add("FF", new CheckBox("❐   " + NomeHeroi + " - [ Q ] Farm", true));
            Menu.Add("FJ", new CheckBox("❐   " + NomeHeroi + " - [ Q ] Jungle", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Draw");
            Menu.Add("DAX", new CheckBox("❐   " + NomeHeroi + " - [ Axe ] Radius", true));
            Menu.Add("DE", new CheckBox("❐   " + NomeHeroi + " - [ E ] Range",  true));
            Menu.Add("DK", new CheckBox("❐   " + NomeHeroi + " - [ Text ] Kill", true));
            Menu.Add("DM", new CheckBox("❐   " + NomeHeroi + " - [ Text ] Modes", true));
            Menu.Add("DCR", new CheckBox("❐   " + NomeHeroi + " - [ Radius ] Catch Axe", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Misc");
            Menu.AddLabel("Important: For Him Not Spending ability to Allies Clear them in");
            Menu.AddLabel("Menu EB > Core > GapCloser = Clear its Allies are Marked");
            Menu.Add("EG", new CheckBox("❐   " + NomeHeroi + " - [ E ] Ant-Gapcloser", true));
            Menu.Add("EI", new CheckBox("❐   " + NomeHeroi + " - [ E ] Interrupt", true));

            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  KillSteal & Item");
            Menu.Add("KS", new CheckBox("❐   " + NomeHeroi + " - [ E & R ] KS", true));
            Menu.Add("IT", new CheckBox("❐   " + NomeHeroi + " - [ Items ] Use", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Drawing.OnDraw += Draw;
            Game.OnUpdate += UpdateGame;
            Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter2OnOnInterruptableTarget;

            var version = "1.4";
            Chat.Print("|| Draven 2016 || UnrealSkill99|| " + version + " || ", Color.DeepPink);
            Chat.Print("|| Draven 2016 || UnrealSkill99|| " + version + " ||", Color.WhiteSmoke);
            Chat.Print("|| Draven 2016 || UnrealSkill99|| " + version + " ||", Color.DeepSkyBlue);
            Chat.Print("|| Draven 2016 || UnrealSkill99|| " + version + " ||", Color.DeepPink);
            Chat.Print("|| Draven 2016 || UnrealSkill99|| " + version + " ||", Color.WhiteSmoke);
            Chat.Print("|| Draven 2016 || UnrealSkill99|| " + version + " ||", Color.DeepSkyBlue);

        }
        private static void UpdateGame(EventArgs args)
        {
            var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            try{
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) Farm(); 
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Farm(); //Jungle(); 
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) if (Inimigo != null) Combo(); 
                SempreAtivo(Ini); }
            catch (Exception Eror) { }
        }
        private static void SempreAtivo(Obj_AI_Base Inimigo)
        {
            PegarMachados();
            KS();
            if (W.IsReady() && Player.HasBuffOfType(BuffType.Slow) && Inimigo.Distance(Hero.Position) <= Hero.GetAutoAttackRange()) Core.DelayAction(()=> W.Cast(), 200);
            var SkinHackSelect = Menu["SkinHack"].Cast<ComboBox>().CurrentValue;
            if (Menu["UseSkinHack"].Cast<CheckBox>().CurrentValue && Hero.SkinId != SkinHackSelect)
            {
                //if (Menu.Get<KeyBind>("SkinLoad").CurrentValue)
                //{
                switch (SkinHackSelect)
                {
                    default: color = Color.AntiqueWhite; break;
                    case 0: Hero.SetSkinId(0); color = Color.AntiqueWhite; break;
                    case 1: Hero.SetSkinId(1); color = Color.CornflowerBlue; break;
                    case 2: Hero.SetSkinId(2); color = Color.DarkOrange; break;
                    case 3: Hero.SetSkinId(3); color = Color.Red; break;
                    case 4: Hero.SetSkinId(4); color = Color.Yellow; break;
                    case 5: Hero.SetSkinId(5); color = Color.Maroon; break;
                    case 6: Hero.SetSkinId(6); color = Color.LimeGreen; break;
                }
                Chat.Print("|| SkinHack Load || <font color='#84e2ab'>Carregado / Load ModelHack " + SkinHackSelect.ToString() + "</font>", System.Drawing.Color.White);
                // }
            }
        }
      /*  private static void Jungle()
        {
            foreach (var Jungle in EntityManager.MinionsAndMonsters.GetJungleMonsters(Hero.Position, Hero.GetAutoAttackRange()))
            {
                if (Menu["FJ"].Cast<CheckBox>().CurrentValue) if (Jungle != null && Hero.Position.Distance(Jungle.Position) <= Hero.GetAutoAttackRange()) if (PassiveCount == 0 && Q.IsReady()) Q.Cast();
            }   
        }*/
        private static void Farm()
        {
            foreach (var Minions in ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.Distance(Hero.Position) <= Hero.GetAutoAttackRange()))
            {
                if (Menu["FF"].Cast<CheckBox>().CurrentValue) if (Minions != null && Hero.Position.Distance(Minions.Position) <= Hero.GetAutoAttackRange()) if (PassiveCount == 0 && Q.IsReady()) Q.Cast();
            }
        }
        private static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var useQ = Menu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = Menu["W"].Cast<CheckBox>().CurrentValue;
            var useE = Menu["E"].Cast<CheckBox>().CurrentValue;
            var useR = Menu["R"].Cast<CheckBox>().CurrentValue;
            var EMode = Menu["ModeE"].Cast<ComboBox>().CurrentValue;

            if (PassiveCount < 1 && Inimigo.Distance(Hero.Position) <= Hero.GetAutoAttackRange() + 100 ) Q.Cast(); 
            if (useW && W.IsReady() && !Player.HasBuff("dravenfurybuff"))  W.Cast(); 
            if (useE && E.IsReady() && Inimigo.IsValidTarget(E.Range)) if (EMode == 0) E.Cast(Inimigo.Position); else if (EMode == 1 && Inimigo.IsValidTarget(500)) E.Cast(Inimigo.Position);

            /*foreach (var Inimigos in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.HealthPercent  <= 60 && x.IsValidTarget(Hero.GetAutoAttackRange() + 200)))
            {
             //   if (Inimigos != null && useR && R.IsReady()) R.Cast(Inimigos.Position);
            }*/

            if (Menu["IT"].Cast<CheckBox>().CurrentValue)
            {
                if (Inimigo.IsValidTarget(Hero.GetAutoAttackRange()) && Yumus.IsReady()) Core.DelayAction(()=> Yumus.Cast(), 200);
                if (Inimigo.IsValidTarget(Hero.GetAutoAttackRange()) && BOTRK.IsReady()) Core.DelayAction(() => BOTRK.Cast(Inimigo), 200);
                if (Inimigo.IsValidTarget(Hero.GetAutoAttackRange()) && Bilgewater.IsReady()) Core.DelayAction(() => Bilgewater.Cast(Inimigo), 200);
                //QSS
                if (Hero.HasBuffOfType(BuffType.Stun)|| Hero.HasBuffOfType(BuffType.Fear)|| Hero.HasBuffOfType(BuffType.Charm) || Hero.HasBuffOfType(BuffType.Taunt) || Hero.HasBuffOfType(BuffType.Blind))
                {
                    if (Mercurial.IsReady()) Core.DelayAction(() => Mercurial.Cast(), 200);
                    if (Bandana.IsReady()) Core.DelayAction(() => Bandana.Cast(), 200);
                }
            }

        }
        private static void PegarMachados()
        {
            var PegarMachado = Menu["AxeGet"].Cast<ComboBox>().CurrentValue;
            var ModoDesafiante = Menu["ModeAxe"].Cast<ComboBox>().CurrentValue;
            var DelayAxe = Menu["DelayAX"].Cast<Slider>().CurrentValue;
            var RadiusCatch = Menu["CR"].Cast<Slider>().CurrentValue;
            Vector3 Mouse = Game.CursorPos;
            foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead && !EntityManager.Turrets.Enemies.Any(t => t.IsValidTarget(1100))))
          //foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead && !EntityManager.Turrets.Enemies.Any(t => t.IsValidTarget(1100)) && x.Distance(Hero.Position) <= RadiusCatch).OrderBy(x => Game.CursorPos.Distance(x.Position) <= 550))//.OrderBy(x => x.Position.Distance(Hero.Position)))
                {
                if (ModoDesafiante == 1)//Normal
                {
                    Orbwalker.DisableMovement = false;
                    switch (PegarMachado)
                    {
                        default: break;
                        case 1: 
                            if (AXE.Position.Distance(Hero.Position) > 110 && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            {
                                Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), DelayAxe);
                                Orbwalker.DisableMovement = true;
                                Orbwalker.DisableMovement = false;
                            }
                            break;
                        case 0:
                            if (AXE.Position.Distance(Hero.Position) > 110 && AXE.Distance(Mouse) <= RadiusCatch)
                            {
                                Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), DelayAxe);
                                Orbwalker.DisableMovement = true;
                                Orbwalker.DisableMovement = false;
                            }
                            break;
                        case 3:
                            break;
                    }
                }
                else if (ModoDesafiante == 0)//Challenger
                {
                    switch (PegarMachado)
                    {
                        default: break;
                        case 1:
                            if (AXE.Distance(Hero.Position) > 100 && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            {
                                //Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 200);
                                var Go = 0;
                                if (Go == 0) Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 150); Go = 1;
                                if (Go == 1) Orbwalker.DisableMovement = true; Orbwalker.DisableMovement = false; Core.DelayAction(() => Go = 0, 50);
                            }
                            break;
                        case 0:
                            if (AXE.Distance(Hero.Position) > 100 && AXE.Distance(Mouse) <= RadiusCatch)
                            {
                                //Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 200);
                                var Go = 0;
                                if (Go == 0) Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 150); Go = 1;
                                if (Go == 1) Orbwalker.DisableMovement = true; Orbwalker.DisableMovement = false; Core.DelayAction(() => Go = 0, 50);
                            }
                            break;
                        case 2:
                            break;
                    }
                }
                else if (ModoDesafiante == 2)//Dev
                {
                    var Torre = EntityManager.Turrets.Enemies.Where(a => !a.IsDead && ObjectManager.Player.Distance(a) <= 1000);
                    var tor = EntityManager.Turrets.Enemies.Any(a => !a.IsDead && ObjectManager.Player.Distance(a) <= 1000);
                    switch (PegarMachado)
                    {
                        default: break;
                        case 1:
                            if (AXE.Distance(Hero.Position) > 140 && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            {
                                var Go = 0;
                                if (Go == 0) Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 200); Go = 1;
                                if (Go == 1) Orbwalker.DisableMovement = true; Orbwalker.DisableMovement = false; Core.DelayAction(() => Go = 0, 350);
                            }
                            else if (AXE.Distance(Hero.Position) < 135) Orbwalker.DisableMovement = false;
                            break;
                        case 0:
                            if (AXE.Distance(Hero.Position) > 140 && AXE.Distance(Mouse) <= RadiusCatch)
                            {
                               var Go = 0;
                               if (Go == 0) Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 200); Go = 1;
                               if (Go == 1) Orbwalker.DisableMovement = true; Orbwalker.DisableMovement = false; Core.DelayAction(() => Go = 0, 350);
                            }
                            else if (AXE.Distance(Hero.Position) < 135) Orbwalker.DisableMovement = false;
                            break;
                        case 2:
                            break;
                    }
                }
            }
        }
        private static void KS()
        {
            var KS = Menu["KS"].Cast<CheckBox>().CurrentValue;
                if (KS)
                {
                foreach (var Inimigos in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Health * 2 <= Player.Instance.GetSpellDamage(x, SpellSlot.R) * 2 && x.IsValidTarget(3000)))
                {
                    if (Inimigos != null) R.Cast(Inimigos.Position);
                    }
                    foreach (var Inimigos in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Health <= Player.Instance.GetSpellDamage(x, SpellSlot.E)))
                    {
                        E.Cast(Inimigos.Position);
                    }
                }
        }
        private static void Draw(EventArgs args)
        {
            if (Hero.IsDead) return;
            var DrawE = Menu["DE"].Cast<CheckBox>().CurrentValue;
            var DrawAX = Menu["DAX"].Cast<CheckBox>().CurrentValue;
            if (DrawE) { new Circle() { Color = color, BorderWidth = 3f, Radius = E.Range }.Draw(Hero.Position); new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = 1050 - 3 }.Draw(Hero.Position); }
            if (DrawAX) { new Circle() { Color = color, BorderWidth = 3f, Radius = Hero.GetAutoAttackRange() }.Draw(Hero.Position); new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = Hero.GetAutoAttackRange() - 3 }.Draw(Hero.Position); }

          //  var RadiusCatch = Menu["CR"].Cast<Slider>().CurrentValue;
            //new Circle() { Color = color, BorderWidth = 1f, Radius = RadiusCatch }.Draw(Game.CursorPos);

            var DrawRC = Menu["DCR"].Cast<CheckBox>().CurrentValue;
            var DrawRCA = Menu["CR"].Cast<Slider>().CurrentValue;
            if (DrawAX && DrawRC)
            {
                new Circle() { Color = color, BorderWidth = 2f, Radius = DrawRCA }.Draw(Game.CursorPos);
                new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = DrawRCA -3 }.Draw(Game.CursorPos);
            }
                foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead))
            {
                if (DrawAX)
                {
                    new Circle() { Color = color, BorderWidth = 5f, Radius = 140 }.Draw(AXE.Position);
                    new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = 137 }.Draw(AXE.Position);
                    Drawing.DrawLine(Hero.Position.WorldToScreen(), AXE.Position.WorldToScreen(), 8f, Color.FromArgb(80, color));
                }
            }


            if (true)
            {
                var ModoDePegaMachados = Menu["ModeAxe"].Cast<ComboBox>().CurrentValue;
                var ModoDeUsaE = Menu["ModeE"].Cast<ComboBox>().CurrentValue;
                string AxeModo = "";
                switch (ModoDePegaMachados)
                {
                    case 0: AxeModo = "[Challenger Mode]"; break;
                    case 1: AxeModo = "[Normal Mode]"; break;
                    case 2: AxeModo = "[C# Developer Mode]";break;
                    default: break;
                }
                string EModo = "";
                switch (ModoDeUsaE)
                {
                    case 0: EModo = "[Aggressive]"; break;
                    case 1: EModo = "[Safe]"; break;
                    case 2: EModo = "[GapCloser]"; break;
                    default: break;
                }
                var DrawTM = Menu["DM"].Cast<CheckBox>().CurrentValue;
                if (DrawTM)
                {
                    Text0.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -40);
                    Text0.Color = color;
                    Text0.TextValue = "[Axe] Mode: ";
                    Text0.Draw();
                    Text0.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(30, -40);
                    Text0.Color = Color.White;
                    Text0.TextValue = AxeModo;
                    Text0.Draw();

                    Text0.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -20);
                    Text0.Color = color;
                    Text0.TextValue = "[E] Mode: ";
                    Text0.Draw();
                    Text0.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(40, -20);
                    Text0.Color = Color.White;
                    Text0.TextValue = EModo;
                    Text0.Draw();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(1500)))
                    {
                        if (R.IsReady())
                        {
                            //credit GinjiBan prediction
                            var collision = new List<AIHeroClient>();
                            var startPos = Player.Instance.Position.To2D();
                            var endPos = hero.Position.To2D();
                            collision.Clear();
                            foreach (var colliHero in EntityManager.Heroes.Enemies.Where(colliHero => !colliHero.IsDead && colliHero.IsVisible && colliHero.IsInRange(hero, 5000) && colliHero.IsValidTarget(5000)))
                            {
                                if (Prediction.Position.Collision.LinearMissileCollision(colliHero, startPos, endPos, R.Speed, R.Width, R.CastDelay))
                                {
                                    collision.Add(colliHero);
                                }
                                if (collision.Count >= 2)
                                {
                                    //Efeito 3D
                                    Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(101, -58);
                                    Text1.Color = Color.Black;
                                    Text1.TextValue = "[Prediction [R] Hit +" + collision.Count.ToString() + " Available]";
                                    Text1.Draw();
                                    Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -60);
                                    Text1.Color = color;
                                    Text1.TextValue = "[Prediction [R] Hit +" + collision.Count.ToString() + " Available]";
                                    Text1.Draw();
                                    Player.Instance.Spellbook.CastSpell(SpellSlot.R, colliHero.Position);
                                }
                            }
                        }
                    }
                }

              /*  foreach (var hero in EntityManager.Heroes.Enemies.Where(hero => !hero.IsDead && Hero.Position.CountEnemiesInRange(Hero.GetAutoAttackRange() + 100) >= 1))
                {
                    if (hero.HealthPercent <= 60)
                    {
                        //Efeito 3D
                        Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(101, -58);
                        Text1.Color = Color.Black;
                        Text1.TextValue = "[Prediction Ult Hit + 2 Available]";
                        Text1.Draw();
                        Text1.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(100, -60);
                        Text1.Color = color;
                        Text1.TextValue = "[Prediction Ult Hit + 2 Available]";
                        Text1.Draw();
                    }
                }*/

                var DrawKill = Menu["DK"].Cast<CheckBox>().CurrentValue;
                foreach (var Inimigo in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.IsHPBarRendered == true && x.Health * 2 <= Player.Instance.GetSpellDamage(x, SpellSlot.R) * 2))
                {
                    if (DrawKill)
                    {
                        Text.Position = Drawing.WorldToScreen(Inimigo.Position) - new Vector2(0, -60);
                        Text.Color = Color.White;
                        new Circle() { Color = Color.White, BorderWidth = 1f, Radius = 130 }.Draw(Inimigo.Position);
                        new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = 127 }.Draw(Inimigo.Position);
                        Text.TextValue = "✖ Kill ✖";
                        Text.Draw();
                    }
                }
            }


        }
        private static void AntiGapcloserOnOnEnemyGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            var EMode = Menu["ModeE"].Cast<ComboBox>().CurrentValue;
            var UseEInterrupt = Menu["EI"].Cast<CheckBox>().CurrentValue;
            if (EMode == 2 || UseEInterrupt && sender.Type == Hero.Type && sender.Team != sender.Team && Hero.Distance(sender) <= E.Range)
           {
                    E.Cast(gapcloser.Sender);
           }
            
        }
        private static void Interrupter2OnOnInterruptableTarget(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var UseEGapcloser = Menu["EG"].Cast<CheckBox>().CurrentValue;
            if (UseEGapcloser && sender.Type == Hero.Type && sender.Team != sender.Team && Hero.Distance(sender) <= E.Range)
            {
                E.Cast(sender);
            }
        }
    }
}
