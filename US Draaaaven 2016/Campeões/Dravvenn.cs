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
        public static Menu Menu;
        public static AIHeroClient Hero = ObjectManager.Player;
        public static String NomeHeroi = Hero.ChampionName;
        public static Spell.Active Q, W;
        public static Spell.Skillshot E, R;
        public static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15, System.Drawing.FontStyle.Bold));
        static Item BOTRK, Bilgewater, Yumus, Mercurial, Bandana;   
        public static int PassiveCount// Total Crédit PassiveCount - Darakath
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
        public static void Spell(EventArgs args)
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

        public static void Menus(EventArgs args)
        {
            if (Hero.ChampionName != "Draven") return;
            Menu = MainMenu.AddMenu(NomeHeroi, NomeHeroi);
            Menu.AddSeparator(1);
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.Add("SkinHack", new ComboBox("✔ Select your Skin Hack", 6, "Classic Draven", "Soul Reaver Draven", "Gladiator Draven", "Primetime Draven", "Pool Party Draven", "Beast Hunter Draven", "Draven Draven"));
            Menu.Add("ModeE", new ComboBox("✔ Select Game Mode Using Skill [E]", 1, "Mode [Aggressive]", "Mode [Secure]"));
            Menu.Add("AxeGet", new ComboBox("✔ Select the Pick Axes Method", 0, "Mode [Always]", "Mode [Combo]", "Never"));
            Menu.Add("ModeAxe", new ComboBox("✔ Select Player Mode - This is OP    ★ ★ ★ ★ ★", 0, "Mode [Challenger] Axes", "Mode [Normal] Axes"));
            Menu.Add("DelayAX", new Slider("Delay Pick Axes Only (Mode [Normal] Axes) ", 250, 0, 500));
            Menu.AddLabel("Recommend Min 150 Max 250 Delay");
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

            Chat.Print("|| Draaaaaaaven!!! Load || Credit: <font color='#FF0000'>UnrealSkill99</font></b> || Challenger Addon To EB Draaaaaaaven!!!", Color.White);

        }
        public static void UpdateGame(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Farm();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Jungle();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                if(Inimigo != null) Combo();
            }
            PegarMachados();
            KS();
            SempreAtivo();
        }
        public static void SempreAtivo()
        {
            if (W.IsReady() && Player.HasBuffOfType(BuffType.Slow))
            {
                W.Cast();
            }
        }
        public static void Jungle()
        {
            if (Menu["QJungle"].Cast<CheckBox>().CurrentValue)
            {
                var jg = EntityManager.MinionsAndMonsters.GetJungleMonsters(Hero.Position, Hero.GetAutoAttackRange()).FirstOrDefault(e => e.IsValid && !e.IsDead);
                if (Q.IsReady() && jg != null)
                {
                    if (PassiveCount < 1) { Q.Cast(); }
                }
            }
        }
        public static void Farm()
        {
            if (Menu["FF"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var source in EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.IsValid && a.IsValidTarget(Q.Range) && !a.IsDead))
                {
                    if (PassiveCount < 1) { Q.Cast(); }
                }
            }

        }
        public static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var useQ = Menu["Q"].Cast<CheckBox>().CurrentValue;
            var useW = Menu["W"].Cast<CheckBox>().CurrentValue;
            var useE = Menu["E"].Cast<CheckBox>().CurrentValue;
            var EMode = Menu["ModeE"].Cast<ComboBox>().CurrentValue;

            if (PassiveCount < 1 && Inimigo.Distance(Hero.Position) <= Hero.GetAutoAttackRange() + 100 ){ Q.Cast(); }
            if (useW && W.IsReady() && !Player.HasBuff("dravenfurybuff")) { W.Cast(); }
            if (useE && E.IsReady() && Inimigo.IsValidTarget(E.Range))
            {
                if (EMode == 0)
                { E.Cast(Inimigo.Position); }
                else if (EMode == 1 && Inimigo.IsValidTarget(500))
                {  E.Cast(Inimigo.Position); }
            }
            if (Menu["IT"].Cast<CheckBox>().CurrentValue)
            {
                if (Inimigo.IsValidTarget(250) && Yumus.IsReady()) Core.DelayAction(()=> Yumus.Cast(), 200);
                if (Inimigo.IsValidTarget(400) && BOTRK.IsReady()) Core.DelayAction(() => BOTRK.Cast(Inimigo), 200);
                if (Inimigo.IsValidTarget(550) && Bilgewater.IsReady()) Core.DelayAction(() => Bilgewater.Cast(Inimigo), 200);
                //QSS
                if (Hero.HasBuffOfType(BuffType.Stun)
                 || Hero.HasBuffOfType(BuffType.Fear)
                 || Hero.HasBuffOfType(BuffType.Charm)
                 || Hero.HasBuffOfType(BuffType.Taunt)
                 || Hero.HasBuffOfType(BuffType.Blind))
                //|| Hero.HasBuffOfType(BuffType.Silence)
                //|| Hero.HasBuffOfType(BuffType.Snare)
                //|| Hero.HasBuffOfType(BuffType.Suppression)
                //|| Hero.HasBuffOfType(BuffType.Sleep)
                //|| Hero.HasBuffOfType(BuffType.Polymorph)
                //|| Hero.HasBuffOfType(BuffType.Frenzy)
                //|| Hero.HasBuffOfType(BuffType.Disarm)
                //|| Hero.HasBuffOfType(BuffType.NearSight)
                {
                    if (Mercurial.IsReady()) Core.DelayAction(() => Mercurial.Cast(), 200);
                    if (Bandana.IsReady()) Core.DelayAction(() => Bandana.Cast(), 200);
                }
            }

        }
        public static void PegarMachados()
        {
            var PegarMachado = Menu["AxeGet"].Cast<ComboBox>().CurrentValue;
            var ModoDesafiante = Menu["ModeAxe"].Cast<ComboBox>().CurrentValue;
            var DelayAxe = Menu["DelayAX"].Cast<Slider>().CurrentValue;
            foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead))
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
                                //Orbwalker.OrbwalkTo(AXE.Position);
                                Orbwalker.DisableMovement = true;
                                Orbwalker.DisableMovement = false;
                            }
                            break;
                        case 0:
                            if (AXE.Position.Distance(Hero.Position) > 110)
                            {
                                Core.DelayAction(() => Orbwalker.OrbwalkTo(AXE.Position), DelayAxe);
                               // Orbwalker.OrbwalkTo(AXE.Position);
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
        private static float GetSpellDamage(Obj_AI_Base target, SpellSlot slot)
        {
            var level = Player.GetSpell(slot).Level - 1;
            switch (slot)
            {
                case SpellSlot.E:
                    {
                        var damage = new float[] { 70, 105, 140, 175, 210 }[level] + (float)(0.5 * Player.Instance.FlatPhysicalDamageMod);
                        return Damage.CalculateDamageOnUnit(Player.Instance, target, DamageType.Physical, damage);
                    }
                case SpellSlot.R:
                    {
                        var damage = new float[] { 175, 275, 375 }[level] + (float)(1.1 * Player.Instance.FlatPhysicalDamageMod);
                        return Damage.CalculateDamageOnUnit(Player.Instance, target, DamageType.Physical, damage);
                    }
            }

            return 0;
        }
        public static void KS()
        {
            var KS = Menu["KS"].Cast<CheckBox>().CurrentValue;
            if (KS)
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => !enemy.IsDead && enemy.IsHPBarRendered == true && enemy.Health <= GetSpellDamage(enemy, SpellSlot.R) && enemy.Distance(Hero.Position) > Hero.GetAutoAttackRange()))
                {
                        R.Cast(R.GetPrediction(enemy).CastPosition); 
                }
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => !enemy.IsDead && enemy.Health <= GetSpellDamage(enemy, SpellSlot.E)))
                {
                        E.Cast(E.GetPrediction(enemy).CastPosition);                  
                }
            }
        }
        public static void Draw(EventArgs args)
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
            if (DrawE) Drawing.DrawCircle(Hero.Position, E.Range, color);
            if (DrawAX) Drawing.DrawCircle(Hero.Position, Hero.GetAutoAttackRange(), color);

            foreach (var AXE in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("Draven_Base_Q_reticle_self.troy") && !x.IsDead))
            {
                if (DrawAX)
                {
                    Drawing.DrawCircle(AXE.Position, 140, color);
                    Drawing.DrawLine(Hero.Position.WorldToScreen(), AXE.Position.WorldToScreen(), 5f, Color.FromArgb(90, color));
                }
            }
            var DrawKill = Menu["DK"].Cast<CheckBox>().CurrentValue;
            foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => !enemy.IsDead && enemy.IsHPBarRendered == true && enemy.Health <= GetSpellDamage(enemy, SpellSlot.R)))
            {
                if (DrawKill)
                {
                    Text.Position = Drawing.WorldToScreen(enemy.Position) - new Vector2(0, -60);
                    Text.Color = Color.White;
                    Drawing.DrawCircle(enemy.Position, 130, Color.White);
                    Text.TextValue = "◯ Kill";
                    Text.Draw();
                }
            }   

            }
        private static  void AntiGapcloserOnOnEnemyGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            var UseEInterrupt = Menu["EI"].Cast<CheckBox>().CurrentValue;
            if (UseEInterrupt)
            {
                E.Cast(sender);
            }
            return;
        }
        private static void Interrupter2OnOnInterruptableTarget(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            var UseEGapcloser = Menu["EG"].Cast<CheckBox>().CurrentValue;
            if (e.DangerLevel == DangerLevel.High && sender.IsValidTarget(E.Range) && UseEGapcloser)
            {
                if (E.IsReady() && E.IsInRange(sender)) E.Cast();
            }
            return;
        }

    }
}
