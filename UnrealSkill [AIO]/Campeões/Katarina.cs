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
        public static AIHeroClient Hero = Player.Instance;
        public static string HeroName = Hero.ChampionName;
        public static Menu Menu, EvadeE;
        public static Spell.Targeted Q, E;
        public static Spell.Active W, R;
        public static Item Ward;
        private static int whenToCancelR;
        public static string Status = "";
        public static string Checkar = "Surpressed";
        public static float DanoCalculado = 0;// nao usando mais
        public static Color colorSkin = Color.HotPink;
        public static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 9, System.Drawing.FontStyle.Bold));
        public static Text Text1 = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 15, System.Drawing.FontStyle.Bold));

        public Katarina()
        {
            Loading.OnLoadingComplete += Menus;
            Loading.OnLoadingComplete += Spell;
            Bootstrap.Init(null);
        }
        private static void Spell(EventArgs args)
        {
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Active(SpellSlot.W, 375);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);
            Ward = new Item(3340);
        }

        private static void Menus(EventArgs args)
        {
            if (Hero.ChampionName != "Katarina") return;
            Menu = MainMenu.AddMenu(HeroName, HeroName);
            Menu.AddSeparator(1);
            Menu.AddLabel("Creator: UnrealSkill-VIP");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddGroupLabel("〈〈〈〈 Combo Modes 〉〉〉〉");
            Menu.Add("C.F", new KeyBind("Fast Changes [Combo Mode]", false, KeyBind.BindTypes.HoldActive, 'L'));
            Menu.AddLabel("✔ Info - Choose With Game Mode Your Agreement,Read the Instructions Below the Mode");
            Menu.Add("M.E1", new ComboBox("        ❐   " + HeroName + " - [ Jump E ] Combo Mode", 1, "Burst", "Segure"));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("★★☆☆☆ [Segure] - This mode is Recommended always, it will only make the");
            Menu.AddLabel("Combo Skill With 'E' if you really Kill.");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("★★★☆☆ [Burst] - This mode is recommended if you are very strong,");
            Menu.AddLabel("As always Ira Make Full Combo");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈  Combo  〉〉〉〉");
            Menu.Add("Q.1", new CheckBox("❐   " + HeroName + " - [ Q ] Enemy", true));
            Menu.Add("W.1", new CheckBox("❐   " + HeroName + " - [ W ] Enemy ", true));
            Menu.Add("E.1", new CheckBox("❐   " + HeroName + " - [ E ] Enemy", true));
            Menu.Add("R.1", new CheckBox("❐   " + HeroName + " - [ R ] Enemy", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈  Ward Jump  〉〉〉〉");
            Menu.Add("W.J", new KeyBind("Auto Ward & Jump [ E ] And Press ->", false, KeyBind.BindTypes.HoldActive, 'V'));
            // EVADE----------------------------------------------------------------------------------------------------
            EvadeE = Menu.AddSubMenu("Evade");
            EvadeE.AddLabel("______________________________________________________________________________________");
            EvadeE.AddLabel("〈〈〈〈  Evade  〉〉〉〉");
            EvadeE.Add("E.E", new CheckBox("[ E ] Evade Spells", true));
            foreach (AIHeroClient Inimigo in EntityManager.Heroes.Enemies)
            {
                EvadeE.AddLabel(Inimigo.ChampionName);
                EvadeE.Add(Inimigo.ChampionName + "Q", new CheckBox("Q", false));
                EvadeE.Add(Inimigo.ChampionName + "W", new CheckBox("W", false));
                EvadeE.Add(Inimigo.ChampionName + "E", new CheckBox("E", false));
                EvadeE.Add(Inimigo.ChampionName + "R", new CheckBox("R", false));
            }
            EvadeE.AddLabel("______________________________________________________________________________________");
            // EVADE----------------------------------------------------------------------------------------------------
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈  SkinHack  〉〉〉〉");
            Menu.Add("SkinLoad", new KeyBind("Select your Skin Hack And Press", false, KeyBind.BindTypes.HoldActive, 'N'));
            Menu.Add("UseSkinHack", new CheckBox("❐ " + HeroName + "  Use SkinHack", true));
            Menu.Add("SkinHack", new ComboBox("        ❐ " + HeroName + "  Select your SkinHack", 8, "Classic Katarina", "Mercenary Katarina", "Red Card Katarina", "Bilgewater Katarina", "Kitty Cat Katarina", "High Command Katarina", "Sandstorm Katarina", "Slay Belle Katarina", "Warring Kingdoms Katarina"));
            Hero.SetSkinId(4);//Set Skin Padrao
            Menu.AddLabel("______________________________________________________________________________________");
           /* Menu.AddLabel("〈〈〈〈  KillSteal  〉〉〉〉");
            Menu.Add("K.S", new CheckBox("❐   " + HeroName + " - [ KS ] Enemy", true));
            Menu.Add("K.W", new CheckBox("❐   " + HeroName + " - [ Ward ] To Max Range", true));
            Menu.AddLabel("______________________________________________________________________________________");*/
            Menu.AddLabel("〈〈〈〈  Farm  〉〉〉〉");
            Menu.Add("F.L", new CheckBox("❐   " + HeroName + " - [ Q/W ] Farm", true));
            Menu.Add("F.H", new CheckBox("❐   " + HeroName + " - [ Q/W ] LastHit", true));
            Menu.Add("F.J", new CheckBox("❐   " + HeroName + " - [ Q/W ] Jungle", true));
            Menu.Add("F.K", new CheckBox("❐   " + HeroName + " - [ KillSteal ] Jungle", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈  Draw  〉〉〉〉");
            Menu.Add("D.Q", new CheckBox("❐   " + HeroName + " - [ Q ] Range", true));
            Menu.Add("D.W", new CheckBox("❐   " + HeroName + " - [ W ] Range", false));
            Menu.Add("D.E", new CheckBox("❐   " + HeroName + " - [ E ] Range", false));
            Menu.Add("D.R", new CheckBox("❐   " + HeroName + " - [ R ] Range", false));
            Menu.AddLabel("〈〈〈〈  Draw 2  〉〉〉〉");
            Menu.Add("T.R", new CheckBox("❐   " + HeroName + " - [ Text ] Ultimate State", true));
            Menu.Add("T.N", new CheckBox("❐   " + HeroName + " - [ Circle ] Enemy Kill", true));
            Menu.AddLabel("______________________________________________________________________________________");
            /*Menu.AddLabel("〈〈〈〈  Misc  〉〉〉〉");
            Menu.AddLabel("Important: For Him Not Spending ability to Allies Clear them in");
            Menu.AddLabel("Menu EB > Core > GapCloser = Clear its Allies are Marked");
            Menu.Add("E.G", new CheckBox("❐   " + HeroName + " - [ E ] Ant-Gapcloser", true));
            Menu.AddLabel("______________________________________________________________________________________");*/
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Evade;
            //Obj_AI_Base.OnProcessSpellCast += PegaHabilidade;
            //Gapcloser.OnGapcloser += AntiGapcloserOnOnEnemyGapcloser;
        }
        public static void Game_OnUpdate(EventArgs args)
        {
            foreach (var buff in Hero.Buffs)
            {
                if (buff.DisplayName == "KatarinaRSound")
                {
                    Orbwalker.DisableAttacking = true;
                    Orbwalker.DisableMovement = true;
                    return;
                }
            }
            Orbwalker.DisableAttacking = false;
            Orbwalker.DisableMovement = false;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) Farm();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungle();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHit();
            LoadingSkin();
            SempreAtivo();
        }
        public static void Farm()
        {
            var UseSpell = Menu["F.L"].Cast<CheckBox>().CurrentValue;
            var LastQ = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.Distance(Hero.Position) <= Q.Range).OrderBy(x => x.Health).FirstOrDefault();
            if (LastQ != null && UseSpell) Q.Cast(LastQ);
            // var LastW = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(x => x.Distance(Hero.Position) <= W.Range).OrderBy(x => x.Health).FirstOrDefault();
            // if (LastW != null && UseSpell) W.Cast();
            LastHit();
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
            if (Jungle != null && UseSpell) Q.Cast(Jungle); if (Jungle.IsValidTarget(W.Range)) W.Cast();
        }
        public static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Magical);
            /*double QDano = Hero.GetSpellDamage(Inimigo, SpellSlot.Q);
            double WDano = Hero.GetSpellDamage(Inimigo, SpellSlot.W);
            double EDano = Hero.GetSpellDamage(Inimigo, SpellSlot.E);
            double RDano = Hero.GetSpellDamage(Inimigo, SpellSlot.R);
            var QCheck = 0;
            var WCheck = 0;
            var ECheck = 0;
            var RCheck = 0;
            if (Q.IsReady() && QCheck == 0) { QCheck = 1; DanoCalculado = +QDano; } else if (!Q.IsReady() && QCheck == 1) { QCheck = 0; DanoCalculado = -QDano; }
            if (W.IsReady() && WCheck == 0) { WCheck = 1; DanoCalculado = +WDano; } else if (!W.IsReady() && WCheck == 1) { WCheck = 0; DanoCalculado = -WDano; }
            if (E.IsReady() && ECheck == 0) { ECheck = 1; DanoCalculado = +EDano; } else if (!E.IsReady() && ECheck == 1) { ECheck = 0; DanoCalculado = -EDano; }
            if (R.IsReady() && RCheck == 0) { RCheck = 1; DanoCalculado = +RDano; } else if (!R.IsReady() && RCheck == 1) { RCheck = 0; DanoCalculado = -RDano; }*/

            // var Modo = Menu["M.C"].Cast<ComboBox>().CurrentValue;
            var UseQ = Menu["Q.1"].Cast<CheckBox>().CurrentValue;
            var UseW = Menu["W.1"].Cast<CheckBox>().CurrentValue;
            var UseE = Menu["E.1"].Cast<CheckBox>().CurrentValue;
            var UseR = Menu["R.1"].Cast<CheckBox>().CurrentValue;
            var ModoE = Menu["M.E1"].Cast<ComboBox>().CurrentValue;
            if (ModoE == 0)
              {
                if (UseQ) Q.Cast(Inimigo);
                if (UseE) Core.DelayAction(() => E.Cast(Inimigo), new Random().Next(400, 800));
                if (UseW && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                if (UseR && Hero.CountEnemiesInRange(W.Range) >= 1 && !E.IsReady() && !W.IsReady() && !Q.IsReady() && R.IsReady())
                {
                    Orbwalker.DisableMovement = true;
                    Orbwalker.DisableAttacking = true;
                    Core.DelayAction(() => R.Cast(), 100);
                }
            }
            if (ModoE == 1)
            {
                double Dano = Hero.GetSpellDamage(Inimigo, SpellSlot.Q) + Hero.GetSpellDamage(Inimigo, SpellSlot.W) + Hero.GetSpellDamage(Inimigo, SpellSlot.E);
                double Dano2 = Hero.GetSpellDamage(Inimigo, SpellSlot.Q) + Hero.GetSpellDamage(Inimigo, SpellSlot.W) + Hero.GetSpellDamage(Inimigo, SpellSlot.E) + Hero.GetSpellDamage(Inimigo, SpellSlot.R);

                //Metodo 1 para Kill
                if (Inimigo.Health <= Dano)
                {
                    if (UseQ) Q.Cast(Inimigo);
                    if (UseE) Core.DelayAction(() => E.Cast(Inimigo), new Random().Next(400, 800));
                    if (UseW && Q.IsOnCooldown && Q.IsOnCooldown  && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                    if (UseR && Hero.CountEnemiesInRange(W.Range) >= 1 && !E.IsReady() && !W.IsReady() && !Q.IsReady() && R.IsReady())
                    {
                        Orbwalker.DisableMovement = true;
                        Orbwalker.DisableAttacking = true;
                        Core.DelayAction(() => R.Cast(), 100);
                    }
                }
                else if (Inimigo.Health <= Dano2 && R.IsReady())
                {
                    if (UseQ) Q.Cast(Inimigo);
                    if (UseE) Core.DelayAction(() => E.Cast(Inimigo), new Random().Next(400, 800));
                    if (UseW && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                    if (UseR && Hero.CountEnemiesInRange(W.Range) >= 1 && !E.IsReady() && !W.IsReady() && !Q.IsReady() && R.IsReady())
                    {
                        Orbwalker.DisableMovement = true;
                        Orbwalker.DisableAttacking = true;
                        Core.DelayAction(() => R.Cast(), 100);
                    }
                }
               //Metodo 2 Para Hit Apenas
                if (Inimigo.Health >= Dano || Inimigo.Health >= Dano2)
                {
                    if (UseQ) Q.Cast(Inimigo);
                    if (UseW && Q.IsOnCooldown && Hero.CountEnemiesInRange(W.Range) >= 1) W.Cast();
                }
            }
        }
        public static void SempreAtivo()
        {
            //Ward Jump
            if (Menu.Get<KeyBind>("W.J").CurrentValue)
            {
                Orbwalker.OrbwalkTo(Game.CursorPos);
                var Ward2 = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Name.ToLower().Contains("ward") && x.Distance(Game.CursorPos) < 250).FirstOrDefault();
                var Alvo = ObjectManager.Get<Obj_AI_Base>().Where(x => ObjectManager.Player.Distance(x.ServerPosition) <= E.Range && x.Distance(Game.CursorPos) <= 250 && !x.IsMe && !x.IsEnemy && !x.IsInvulnerable).OrderBy(x => x.Distance(Hero.Position) >= 550).FirstOrDefault();
                if (Alvo != null) E.Cast(Alvo);
                else if (Ward2 == null && E.IsReady())
                {
                    Vector3 cursorPos = Game.CursorPos;
                    Vector3 myPos = Hero.Position;
                    Vector3 delta = cursorPos - myPos;
                    delta.Normalize();
                    Vector3 wardPosition = myPos + delta * 600;
                    Ward.Cast(wardPosition);
                }
                if (Ward2 != null)
                { if (E.IsReady()) E.Cast(Ward2); return; }
            }

            //Change mode Combo
            if (Menu.Get<KeyBind>("C.F").CurrentValue)
            {
                switch (Menu["M.E1"].Cast<ComboBox>().CurrentValue)
                {
                    case 0: Core.DelayAction(() => Menu["M.E1"].Cast<ComboBox>().CurrentValue = 1, 200); break;
                    case 1: Core.DelayAction(() => Menu["M.E1"].Cast<ComboBox>().CurrentValue = 0, 200); ; break;
                }
            }
            //KS
            if (false)
            {
                foreach (var x in EntityManager.Heroes.Enemies.Where (x=> !x.IsDead && x.Distance(Hero.Position) <= 2500 && x.Health <= Hero.GetSpellDamage(x, SpellSlot.Q) && !x.IsZombie).OrderBy(x=> x.Health))
                {
                   if (x != null && x.Distance(Hero.Position) <= Q.Range)
                    {
                        if (x.IsValidTarget(Q.Range)) Q.Cast(x);
                        if (x.IsValidTarget(W.Range)) W.Cast();
                        if (x.IsValidTarget(E.Range)) Core.DelayAction(() => E.Cast(x), new Random().Next(750, 1000));
                    }
                    /*if (x != null && x.Distance(Hero.Position) > Q.Range)
                    {
                        var Ward2 = ObjectManager.Get<Obj_AI_Minion>().Where(ax => x.Name.ToLower().Contains("ward") && ax.Distance(Game.CursorPos) < 250).FirstOrDefault();
                        var Alvo = ObjectManager.Get<Obj_AI_Base>().Where(a => ObjectManager.Player.Distance(a.ServerPosition) <= E.Range + 100 && !a.IsMe && a.IsTargetable && !a.IsInvulnerable).OrderBy(a => a.Distance(a.Position) <= E.Range + 100).FirstOrDefault();
                        if (Alvo != null) E.Cast(Alvo);
                        else if (Ward2 == null && E.IsReady())
                        {
                            Vector3 cursorPos = x.Position;
                            Vector3 myPos = Hero.Position;
                            Vector3 delta = cursorPos - myPos;
                            delta.Normalize();
                            Vector3 wardPosition = myPos + delta * 600;
                            Ward.Cast(wardPosition);
                        }
                        if (Ward2 != null)
                        { if (E.IsReady()) E.Cast(Ward2); return; }

                        if (x.IsValidTarget(Q.Range)) Q.Cast(x);
                        if (x.IsValidTarget(W.Range)) W.Cast();
                        return;
                    }*/
                }
            }
        }
        static float CalculateDamage(Obj_AI_Base Inimigo)
        {
            double damage = 0d;
            if (Q.IsReady())
            {
                damage += Hero.GetSpellDamage(Inimigo, SpellSlot.Q) *1;
            }
            if (Inimigo.HasBuff("katarinaqmark"))
            {
                damage += Hero.GetSpellDamage(Inimigo, SpellSlot.Q) * 1;
            }
            if (W.IsReady())
            {
                damage += Hero.GetSpellDamage(Inimigo, SpellSlot.W);
            }
            if (E.IsReady())
            {
                damage += Hero.GetSpellDamage(Inimigo, SpellSlot.E);
            }
            if (R.IsReady() || Status == "Yes")
            {
                damage += Hero.GetSpellDamage(Inimigo, SpellSlot.R);
            }
            return (float)damage;
        }
        static void Evade(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (EvadeE["E.E"].Cast<CheckBox>().CurrentValue && !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && sender.Type == Hero.Type && sender.Team != Hero.Team)
            {
                if (Hero.Distance(sender) <= E.Range + 100)
                {
                    AIHeroClient unit = (AIHeroClient)sender;
                    if (Habilidade_Ativado(unit, args.SData.Name) && E.IsReady())
                    {
                        var Inimigo = EntityManager.Heroes.Enemies.Where(o => o.IsValidTarget(E.Range + 100)).FirstOrDefault();
                        var Alvo = ObjectManager.Get<Obj_AI_Base>().Where(x => ObjectManager.Player.Distance(x.ServerPosition) <= E.Range && !x.IsMe &&  !x.IsDead  &&  !x.IsEnemy && x.IsTargetable && !x.IsInvulnerable && x.Distance(Hero.Position) >= 250 && x.Distance(Inimigo.Position) >= 450).FirstOrDefault();
                        E.Cast(Alvo);
                        Chat.Print("Spell Detectada: (" + args.SData.Name + ")", colorSkin);
                        return;
                    }
                }
            }
        }
     /*   static void PegaHabilidade(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            //Chat.Print(args.SData.Name.ToString());
            if (sender.IsMe && args.SData.Name == "KatarinaR")
            {
               Orbwalker.DisableAttacking = true;
               Orbwalker.DisableMovement = true;    
            }
        }*/
        static bool Habilidade_Ativado(AIHeroClient Inimigo, string Nome_Habilidade)
        {
            string Habilidade = "Q";
            if (Nome_Habilidade.Equals(Inimigo.Spellbook.GetSpell(SpellSlot.Q).SData.Name.ToString())) Habilidade = "Q";
            if (Nome_Habilidade.Equals(Inimigo.Spellbook.GetSpell(SpellSlot.W).SData.Name.ToString())) Habilidade = "W";
            if (Nome_Habilidade.Equals(Inimigo.Spellbook.GetSpell(SpellSlot.E).SData.Name.ToString())) Habilidade = "E";
            if (Nome_Habilidade.Equals(Inimigo.Spellbook.GetSpell(SpellSlot.R).SData.Name.ToString())) Habilidade = "R";
            return EvadeE[Inimigo.ChampionName + Habilidade].Cast<CheckBox>().CurrentValue;
        }
        public static void LoadingSkin()
        {
            if (Menu["UseSkinHack"].Cast<CheckBox>().CurrentValue)
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
        }
        private static void Game_OnDraw(EventArgs args)
        {
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
                    double Dano = Hero.GetSpellDamage(Inimigo, SpellSlot.Q) + Hero.GetSpellDamage(Inimigo, SpellSlot.W) + Hero.GetSpellDamage(Inimigo, SpellSlot.E);
                    double Dano2 = Hero.GetSpellDamage(Inimigo, SpellSlot.Q) + Hero.GetSpellDamage(Inimigo, SpellSlot.W) + Hero.GetSpellDamage(Inimigo, SpellSlot.E) + Hero.GetSpellDamage(Inimigo, SpellSlot.R);
                    foreach (var Get in EntityManager.Heroes.Enemies.Where(i => i.Distance(Hero.Position) <= 1000 && !i.IsDead && !i.IsZombie && !i.IsInvulnerable))
                    {
                        if (Inimigo.Health <= Dano && Inimigo.IsHPBarRendered == true)
                        {
                            new Circle() { Color = Color.Red, BorderWidth = 8f, Radius = 100 }.Draw(Inimigo.Position);
                            new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = 90 }.Draw(Inimigo.Position);
                        }
                        else if (Inimigo.Health <= Dano2 && Inimigo.IsHPBarRendered == true && R.IsReady())
                        {
                            new Circle() { Color = Color.Blue, BorderWidth = 8f, Radius = 100 }.Draw(Inimigo.Position);
                            new Circle() { Color = Color.Black, BorderWidth = 5f, Radius = 90 }.Draw(Inimigo.Position);
                        }
                    }
                }
            }

            Drawing.DrawText(20, 20, System.Drawing.Color.WhiteSmoke, "Timer: " + DateTime.Now.ToShortTimeString(), 40);
            if (Menu["D.Q"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = colorSkin, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 5f }.Draw(ObjectManager.Player.Position);
                new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = Q.Range }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["D.W"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = W.Range }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["D.E"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = E.Range }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["D.R"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = colorSkin, BorderWidth = 4f, Radius = R.Range }.Draw(ObjectManager.Player.Position);
            }

            var Alvo = ObjectManager.Get<Obj_AI_Base>().Where(x => ObjectManager.Player.Distance(x.ServerPosition) <= E.Range && !x.IsMe && !x.IsEnemy && x.IsTargetable && !x.IsInvulnerable && x.Distance(Hero.Position) >= 250 && x.Distance(Inimigo.Position) >= 450).FirstOrDefault();
            if (EvadeE["E.E"].Cast<CheckBox>().CurrentValue)//Informa em qual target pular se o evade estiver ligado
            { new Circle() { Color = Color.LimeGreen, BorderWidth = 6f, Radius = 100 }.Draw(Alvo.Position); }

            foreach (var ObjetoWard in ObjectManager.Get<GameObject>().Where(x => x.Name.Equals("SightWard") && !x.IsDead))
            { new Circle() { Color = Color.WhiteSmoke, BorderWidth = 6f, Radius = 100 }.Draw(ObjetoWard.Position); }
        }


    }
}
