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
        private static Spell.Active Q, W;
        private static Spell.Skillshot E, R;
        private static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15, System.Drawing.FontStyle.Bold));
        static Item BOTRK, Bilgewater, Yumus, Mercurial, Bandana;   
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
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear);

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
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.Add("SkinHack", new ComboBox("✔ Select your Skin Hack", 6, "Classic Draven", "Soul Reaver Draven", "Gladiator Draven", "Primetime Draven", "Pool Party Draven", "Beast Hunter Draven", "Draven Draven"));
            Menu.Add("ModeE", new ComboBox("✔ Select Game Mode Using Skill [E]", 1, "Mode [Aggressive]", "Mode [Secure]"));
            Menu.Add("AxeGet", new ComboBox("✔ Select the Pick Axes Method", 0, "Mode [Always]", "Mode [Combo]", "Never"));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("If You have Problems With the (Challenge Mode) Use (Normal Mode)");
            Menu.AddLabel("and Set Down Delay. ''Ping 1 to 60'' Use Delay 200 or 250");
            Menu.Add("ModeAxe", new ComboBox("✔ Select Player Mode - This is OP    ★ ★ ★ ★ ★", 0, "Mode [Challenger] Axes", "Mode [Normal] Axes"));
            Menu.Add("DelayAX", new Slider("Delay Pick Axes Only (Mode [Normal] Axes) ", 250, 0, 500));
            Menu.AddLabel("Recommend Min 150 Max 250 Delay & Set in ''Core > Ticks Per Second: 40''");
            Menu.AddLabel("______________________________________________________________________________________"); 
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Combo");
            Menu.Add("Q", new CheckBox("✖   " + NomeHeroi + " - [ Q ]", true));
            Menu.Add("W", new CheckBox("✖   " + NomeHeroi + " - [ W ] ", true));
            Menu.Add("E", new CheckBox("✖   " + NomeHeroi + " - [ E ]", true));
            Menu.Add("R", new CheckBox("✖   " + NomeHeroi + " - [ R ]", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Farm");
            Menu.Add("FF", new CheckBox("✖   " + NomeHeroi + " - [ Q ] Farm", true));
            Menu.Add("FJ", new CheckBox("✖   " + NomeHeroi + " - [ Q ] Jungle", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Draw");
            Menu.Add("DAX", new CheckBox("✖   " + NomeHeroi + " - [ Axe ] Radius", true));
            Menu.Add("DE", new CheckBox("✖   " + NomeHeroi + " - [ E ] Range",  true));
            Menu.Add("DK", new CheckBox("✖   " + NomeHeroi + " - [ Text ] Kill", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  Misc");
            Menu.Add("EG", new CheckBox("✖   " + NomeHeroi + " - [ E ] Ant-Gapcloser", true));
            Menu.Add("EI", new CheckBox("✖   " + NomeHeroi + " - [ E ] Interrupt", true));
            Menu.AddLabel("  ◣  " + NomeHeroi + "  ◥  KillSteal & Item");
            Menu.Add("KS", new CheckBox("✖   " + NomeHeroi + " - [ E & R ] KS", true));
            Menu.Add("IT", new CheckBox("✖   " + NomeHeroi + " - [ Items ] Use", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Drawing.OnDraw += Draw;
            Game.OnUpdate += UpdateGame;
            Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter2OnOnInterruptableTarget;

            Chat.Print("|| Draven 2016 || UnrealSkill99|| 1.1 ||", Color.White);

        }
        private static void UpdateGame(EventArgs args)
        {
            var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            try{
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) Farm(); 
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungle(); 
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) if (Inimigo != null) Combo(); 
            PegarMachados(); KS(); SempreAtivo(); }
            catch (Exception Eror) { }
        }
        private static void SempreAtivo()
        {
            if (W.IsReady() && Player.HasBuffOfType(BuffType.Slow)) Core.DelayAction(()=> W.Cast(), 200);
        }
        private static void Jungle()
        {
            foreach (var Jungle in EntityManager.MinionsAndMonsters.GetJungleMonsters(Hero.Position, Hero.GetAutoAttackRange()))
            {
                if (Menu["QJungle"].Cast<CheckBox>().CurrentValue) if (Jungle != null && Hero.Position.Distance(Jungle.Position) <= Hero.GetAutoAttackRange()) if (PassiveCount < 1 && Q.IsReady()) Q.Cast();
            }   
        }
        private static void Farm()
        {
            foreach (var Minions in ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy && a.Distance(Hero.Position) <= Hero.GetAutoAttackRange()))
            {
                if (Menu["FF"].Cast<CheckBox>().CurrentValue) if (Minions != null && Hero.Position.Distance(Minions.Position) <= Hero.GetAutoAttackRange()) if (PassiveCount < 1 && Q.IsReady()) Q.Cast();
            }
        }
        private static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var useQ = Menu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = Menu["W"].Cast<CheckBox>().CurrentValue;
            var useE = Menu["E"].Cast<CheckBox>().CurrentValue;
            var EMode = Menu["ModeE"].Cast<ComboBox>().CurrentValue;

            if (PassiveCount < 1 && Inimigo.Distance(Hero.Position) <= Hero.GetAutoAttackRange() + 100 ) Q.Cast(); 
            if (useW && W.IsReady() && !Player.HasBuff("dravenfurybuff"))  W.Cast(); 
            if (useE && E.IsReady() && Inimigo.IsValidTarget(E.Range)) if (EMode == 0) E.Cast(Inimigo.Position); else if (EMode == 1 && Inimigo.IsValidTarget(500)) E.Cast(Inimigo.Position); 
            
            if (Menu["IT"].Cast<CheckBox>().CurrentValue)
            {
                if (Inimigo.IsValidTarget(250) && Yumus.IsReady()) Core.DelayAction(()=> Yumus.Cast(), 200);
                if (Inimigo.IsValidTarget(400) && BOTRK.IsReady()) Core.DelayAction(() => BOTRK.Cast(Inimigo), 200);
                if (Inimigo.IsValidTarget(550) && Bilgewater.IsReady()) Core.DelayAction(() => Bilgewater.Cast(Inimigo), 200);
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
            foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead).OrderBy(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy")))
            {
                if (ModoDesafiante == 1)
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
                            if (AXE.Position.Distance(Hero.Position) > 110)
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
               else
                {
                    switch (PegarMachado)
                    {
                        default: break;
                        case 1:
                            if (AXE.Distance(Hero.Position) > 100 && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                            {
                                Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 200);
                            }
                            break;
                        case 0:
                            if (AXE.Distance(Hero.Position) > 100)
                            {
                                Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), 200);
                            }
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
                    foreach (var Inimigos in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.IsHPBarRendered == true && x.Health *2 <= Player.Instance.GetSpellDamage(x, SpellSlot.R) *2 && x.Distance(Hero.Position) > Hero.GetAutoAttackRange()))
                    {
                        R.Cast(Inimigos.ServerPosition);
                    }
                    foreach (var Inimigos in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Health <= Player.Instance.GetSpellDamage(x, SpellSlot.E)))
                    {
                        E.Cast(Inimigos.Position);
                    }
                }
        }
        private static void Draw(EventArgs args)
        {
            var SkinHackSelect = Menu["SkinHack"].Cast<ComboBox>().CurrentValue;
            Color color;
            switch (SkinHackSelect)
            {
                default : color = Color.AntiqueWhite; break;  
                case 0: Hero.SetSkinId(0); color = Color.AntiqueWhite; break;
                case 1: Hero.SetSkinId(1); color = Color.CornflowerBlue; break;
                case 2: Hero.SetSkinId(2); color = Color.DarkOrange;  break;
                case 3: Hero.SetSkinId(3);  color = Color.Red;   break;
                case 4: Hero.SetSkinId(4);color = Color.Yellow; break;
                case 5: Hero.SetSkinId(5);  color = Color.Maroon;  break;
                case 6: Hero.SetSkinId(6); color = Color.LimeGreen; break;
            }

            var DrawE = Menu["DE"].Cast<CheckBox>().CurrentValue;
            var DrawAX = Menu["DAX"].Cast<CheckBox>().CurrentValue;
            if (DrawE) new Circle() { Color = color, BorderWidth = 2f, Radius = E.Range }.Draw(Hero.Position); new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = 1050 - 2 }.Draw(Hero.Position);
            if (DrawAX) new Circle() { Color = color, BorderWidth = 2f, Radius = Hero.GetAutoAttackRange() }.Draw(Hero.Position); new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = Hero.GetAutoAttackRange() -2 }.Draw(Hero.Position);

            foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead))
            {
                if (DrawAX)
                {
                    new Circle() { Color = color, BorderWidth = 3f, Radius = 140 }.Draw(AXE.Position);
                    new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = 137 }.Draw(AXE.Position);
                    Drawing.DrawLine(Hero.Position.WorldToScreen(), AXE.Position.WorldToScreen(), 5f, Color.FromArgb(80, color));
                }
            }
            var DrawKill = Menu["DK"].Cast<CheckBox>().CurrentValue;
            foreach (var Inimigo in EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.IsHPBarRendered == true && x.Health *2 <= Player.Instance.GetSpellDamage(x, SpellSlot.R) *2))
            {
                if (DrawKill)
                {
                    Text.Position = Drawing.WorldToScreen(Inimigo.Position) - new Vector2(0, -60);
                    Text.Color = Color.White;
                    new Circle() { Color = Color.White, BorderWidth = 2f, Radius = 130 }.Draw(Inimigo.Position);
                    new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = 128 }.Draw(Inimigo.Position);
                    Text.TextValue = "◯ Kill";
                    Text.Draw();
                }
            }   

            }
        private static void AntiGapcloserOnOnEnemyGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            var UseEInterrupt = Menu["EI"].Cast<CheckBox>().CurrentValue;
            if (!UseEInterrupt || !sender.IsValidTarget()) return;
            if (ObjectManager.Player.Distance(gapcloser.Sender, true) <
                E.Range * E.Range && sender.IsValidTarget())
            {
                E.Cast(gapcloser.Sender);
            }
        }
        private static void Interrupter2OnOnInterruptableTarget(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var UseEGapcloser = Menu["EG"].Cast<CheckBox>().CurrentValue;
            if (!UseEGapcloser || !sender.IsValidTarget()) return;

            if (ObjectManager.Player.Distance(sender, true) < E.Range * E.Range)
            {
                E.Cast(sender);
            }
        }
    }
}
