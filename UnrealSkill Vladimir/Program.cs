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
        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }
        public static AIHeroClient Vlad = ObjectManager.Player;
        public static AIHeroClient Vladimir
        {
            get { return ObjectManager.Player; }
        }
        public AIHeroClient myTarget = null;
        public static Spell.Active W, E;
        public static Spell.Skillshot R, Flash;
        public static Spell.Targeted Ignite, Q;
        public static Menu Menu, DrawMenu, AddonMenu;
        static Item Mercurial;
        public static  String NomeChamp = "Vladimir";

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            EloBuddy.Hacks.RenderWatermark = false;
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////GAME ON START -> MENU -> SPELL -> ITEM/////////////////////////////////////////
        /// </summary>
        /// <param name="args"></param>
        private static void Game_OnStart(EventArgs args)
        {
            if (Player.Instance.ChampionName != NomeChamp)
                return;

            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 150);
            E = new Spell.Active(SpellSlot.E, 610);
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 150);

            if (HasSpell("summonerdot"))
                Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);

            Mercurial = new Item(3137);
            //-------------------------------------------------------CHAT -----------------------------------------------------------
            //Chat.Print(ObjectManager.Player.Model.ToString());
            Chat.Print("|| UnrealSkill " + NomeChamp + " || <font color='#7a7a7a'>Carregado / Load V1</font>", System.Drawing.Color.WhiteSmoke);
            //-------------------------------------------------------MENU ------------------------------------------------------------
            Menu = MainMenu.AddMenu("▐  U S  ▐", "Addon");
            Menu.AddSeparator(50);
         Menu.AddLabel("Logs:" + System.Environment.NewLine +
              "▐ Add SkinHack" + System.Environment.NewLine + 
              "" + System.Environment.NewLine +
              "▐ Add Killsteal Auto Q" + System.Environment.NewLine +
              "▐ Add Logic Auto check Enemy range for Ultimate" + System.Environment.NewLine + 
              "▐ Add JG Clear" + System.Environment.NewLine +
              "▐ Add LaneClear" + System.Environment.NewLine +
              "▐ Add LastHit Q"); 
          Menu.AddSeparator(100);
           Menu.AddLabel("▐ Special Credit to: Bloodimir / Mr.Articuno / Aka (References to create this Addon)");
            AddonMenu = Menu.AddSubMenu("▐  " + NomeChamp + " ▐", "Settings");
            AddonMenu.AddLabel("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| Combo");
            AddonMenu.Add("QCombo", new CheckBox("|| " + NomeChamp + " Usar / Use Q || (Targed)", true));
            AddonMenu.Add("WCombo", new CheckBox("|| " + NomeChamp + " Usar / Use W || (Active)", true));
            AddonMenu.Add("ECombo", new CheckBox("|| " + NomeChamp + " Usar / Use E || (Active)", true));
            AddonMenu.Add("RCombo", new CheckBox("|| " + NomeChamp + " Usar / Use R || (Targed)", true));
            AddonMenu.AddLabel("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| Ultimate");
            AddonMenu.Add("REnemy", new Slider("Minimum for Combo R", 2, 0, 5));
            AddonMenu.AddLabel("|| Automation ||");
            AddonMenu.Add("AutoKS", new CheckBox("|| " + NomeChamp + " Using Automatically (Killsteal) ||", true));
            AddonMenu.Add("Auto Ignity", new CheckBox("|| " + NomeChamp + " Using Automatically (Ignity) ||", true));
            AddonMenu.Add("LastHitMin", new CheckBox("|| " + NomeChamp + " LaneClear (Q LastHit Minions) ||", true));
            AddonMenu.Add("LaneClearFarmQ", new CheckBox("|| " + NomeChamp + " LaneClear (Q) ||", true));
            AddonMenu.Add("LaneClearFarmE", new CheckBox("|| " + NomeChamp + " LaneClear (E) ||", true));
            AddonMenu.AddLabel("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| Draw");
            AddonMenu.Add("DesabilitaDraw", new CheckBox("|| " + NomeChamp + " My Range || (Atk Range)", true));
            AddonMenu.Add("DesabilitaDrawQ", new CheckBox("|| " + NomeChamp + " Q Range || (My Spell)", true));
            AddonMenu.Add("DesabilitaDrawW", new CheckBox("|| " + NomeChamp + " W Range || (My Spell)", true));
            AddonMenu.Add("DesabilitaDrawE", new CheckBox("|| " + NomeChamp + " E Range || (My Spell)", true));
            AddonMenu.Add("DesabilitaDrawR", new CheckBox("|| " + NomeChamp + " R Range || (My Spell)", true));
            AddonMenu.Add("DesabilitaDrawLine", new CheckBox("|| Get Line TargetSelector || (Enemy)", true));
            AddonMenu.AddLabel("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||  SkinHack");
            AddonMenu.Add("UseSkinHack", new CheckBox("|| " + NomeChamp + "  Use SkinHack || (F5)", true));
            var SkinHack = AddonMenu.Add("SkinID", new Slider("SkinHack Select", 5, 0, 6));
            var ID = new[] { "Classic", "SkinHack 1", "SkinHack 2", "SkinHack 3", "SkinHack 4", "SkinHack 5", "SkinHack 6"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID[changeArgs.NewValue]; };;
            Player.SetModel("Vladimir");
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
        }


        ////////////////////////////////////////////////////////////////////SKIN////////////////////////////////////////////////////////////////////
        //Skins
        public static void LoadingSkin()
        {
            var SkinHackSelect = Program.AddonMenu["SkinID"].DisplayName;
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
            }
        }
        /// <summary>
        /// ////////////////////////////////////////////////////////////////DESENHOS////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="args"></param>
        /// 


        public List<Color> DrawsGame = new List<Color>{Color.Red,Color.Green,Color.Blue,Color.Yellow,Color.Purple,Color.Pink,};

        private static void Game_OnDraw(EventArgs args) // Drawing.DrawText(50, 80, System.Drawing.Color.White, "Nóis é BR Hue!",50);
        {
            var Inimigo = TargetSelector.GetTarget(700, DamageType.Magical);
            if (AddonMenu["DesabilitaDraw"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(ObjectManager.Player.Position);
                //new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = ObjectManager.Player.GetAutoAttackRange() }.Draw(ObjectManager.Player.Position);
            }

           Drawing.DrawText(20, 20, System.Drawing.Color.DeepPink, "Timer: " + DateTime.Now.ToShortTimeString(), 40);

            //------------------------------------------------------------------

            if (AddonMenu["DesabilitaDrawQ"].Cast<CheckBox>().CurrentValue)
            {
                if (Q.IsReady())
                {
                    new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = Q.Range }.Draw(ObjectManager.Player.Position);
                }
                if (!Q.IsReady())
                {
                    new Circle() { Color = Color.Red, BorderWidth = 1f, Radius = Q.Range }.Draw(ObjectManager.Player.Position);
                }
            }
            if (AddonMenu["DesabilitaDrawW"].Cast<CheckBox>().CurrentValue)
            {
                if (W.IsReady())
                {
                    new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = W.Range }.Draw(ObjectManager.Player.Position);
                }
                if (!W.IsReady())
                {
                    new Circle() { Color = Color.Red, BorderWidth = 1f, Radius = W.Range }.Draw(ObjectManager.Player.Position);
                }
            }

            if (AddonMenu["DesabilitaDrawE"].Cast<CheckBox>().CurrentValue)
            {
                if (E.IsReady())
                {
                    new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = E.Range }.Draw(ObjectManager.Player.Position);
                }
                if (!E.IsReady())
                {
                    new Circle() { Color = Color.Red, BorderWidth = 1f, Radius = E.Range }.Draw(ObjectManager.Player.Position);
                }
            }

            if (AddonMenu["DesabilitaDrawR"].Cast<CheckBox>().CurrentValue)
            {
                if (R.IsReady())
                {
                    new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = R.Range }.Draw(ObjectManager.Player.Position);
                }
                if (!R.IsReady())
                {
                    new Circle() { Color = Color.Red, BorderWidth = 1f, Radius = R.Range }.Draw(ObjectManager.Player.Position);
                }
            }

             if (AddonMenu["DesabilitaDrawLine"].Cast<CheckBox>().CurrentValue)
            {
              Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 2, Color.Black); 
             }

                }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////ULTIMATE + AUTO Q + LANECLEAR LAST HIT//////////////////////////////////////////////////////////////////
        /// </summary>
        /// 
        public static Obj_AI_Base GetEnemy(float range, GameObjectType t)
        {
            switch (t)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
                default:
                    return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(
                        a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
            }
        }

        public static void AutoIgnity()
        {
            if (!AddonMenu["Auto Ignity"].Cast<CheckBox>().CurrentValue || !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy && a.IsValidTarget(Ignite.Range) &&
                                a.Health < 50 + 20 * Vlad.Level - (a.HPRegenRate / 5 * 3)))
            {
                Ignite.Cast(source);
                return;
            }
        }

//FARMING

        internal class Misc
        {
            private static AIHeroClient Vladimir
            {
                get { return ObjectManager.Player; }
            }

            public static float Qdmg(Obj_AI_Base target)
            {
                return Vladimir.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 90, 125, 160, 195, 230 }[Program.Q.Level] + (0.6f * Vladimir.FlatMagicDamageMod)));
            }

            public static float Edmg(Obj_AI_Base target)
            {
                return Vladimir.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 60, 85, 110, 135, 160 }[Program.E.Level] + (0.45f * Vladimir.FlatMagicDamageMod)));
            }

            public static float Rdmg(Obj_AI_Base target)
            {
                return Vladimir.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 168, 280, 392 }[Program.E.Level] + (0.78f * Vladimir.FlatMagicDamageMod)));
            }
        }

        public enum AttackSpell
        {
            Q
        };

        public static Obj_AI_Base MinionLh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                                                                            &&
                                                                                                            a.Type ==
                                                                                                            type
                                                                                                            &&
                                                                                                            a.Distance(
                                                                                                                Vladimir) <=
                                                                                                            Program.Q
                                                                                                                .Range
                                                                                                            && !a.IsDead
                                                                                                            &&
                                                                                                            !a
                                                                                                                .IsInvulnerable
                                                                                                            &&
                                                                                                            a
                                                                                                                .IsValidTarget
                                                                                                                (
                                                                                                                    Program
                                                                                                                        .Q
                                                                                                                        .Range)
                                                                                                            &&
                                                                                                            a.Health <=
                                                                                                            Misc.Qdmg(a));
        }

        public static void LastHit()
        {
            var qcheck = Program.AddonMenu["LastHitMin"].Cast<CheckBox>().CurrentValue;
            if (!Q.IsReady()) return;
            {
                var minion = (Obj_AI_Minion)MinionLh(GameObjectType.obj_AI_Minion, AttackSpell.Q);
                if (minion != null)
                {
                    Program.Q.Cast(minion);
                }
            }
        }


        public static void LaneClear()
        {
            var enemyE = (Obj_AI_Minion)GetEnemy(Program.E.Range, GameObjectType.obj_AI_Minion);
            var enemyQ = (Obj_AI_Minion)GetEnemy(Program.Q.Range, GameObjectType.obj_AI_Minion);
             if (Q.IsReady() && enemyQ.IsValid() && AddonMenu["LaneClearFarmQ"].Cast<CheckBox>().CurrentValue)
             {
                 Program.Q.Cast(enemyQ);
             }

             if (E.IsReady() && enemyE.IsValid() && AddonMenu["LaneClearFarmE"].Cast<CheckBox>().CurrentValue)
              {
                      Program.E.Cast();
             }
      }

        public static void Jungle()
        {
            var monster =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,Q.Range)
                        .FirstOrDefault(e => e.IsValidTarget());

            if (E.IsReady() && monster.IsValidTarget(E.Range))
            {
                E.Cast();
            }

            if (Q.IsReady() && monster.IsValidTarget(Q.Range))
            {
                Q.Cast(monster);
            }
        }
           

   /*     private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,Interrupter.InterruptableSpellEventArgs args)
        {
            var intTarget = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (E.IsReady() && sender.IsValidTarget(E.Range))
            {
                    E.Cast(intTarget.ServerPosition);
            }
        }*/

        ///////////////////////////////////////////////////////////////////CONTAGEM DE INIMIGO//////////////////////////////////////////////////////////////
        public static void Rincombo()
        {
            foreach (var qtarget in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
            {
                if (Vlad.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health ||
                    (Vlad.GetSpellDamage(qtarget, SpellSlot.E) >= qtarget.Health))return;

                var rtarget = TargetSelector.GetTarget(700, DamageType.Magical);
                if (AddonMenu["RCombo"].Cast<CheckBox>().CurrentValue)
                    if (R.IsReady() && rtarget.CountEnemiesInRange(R.Width) >= AddonMenu["REnemy"].Cast<Slider>().CurrentValue)
                    {

                        R.Cast(rtarget.ServerPosition);
                    }
            }
        }
        //////////////////////////////////////////////////////////////////////KS////////////////////////////////////////////////////////
        private static void Killsteal()
        {
            var enemy = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (AddonMenu["AutoKS"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
                foreach (
                    var qtarget in
                        EntityManager.Heroes.Enemies.Where(
                            hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
                {
                    if (Vlad.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health && qtarget.Distance(Vlad) <= Q.Range)
                    {
                        Q.Cast(enemy);
                    }
                    if (AddonMenu["AutoKS"].Cast<CheckBox>().CurrentValue && E.IsReady())
                    {
                        foreach (var etarget in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(E.Range) && !hero.IsDead && !hero.IsZombie))
                        {
                            if (Vlad.GetSpellDamage(etarget, SpellSlot.E) >= etarget.Health &&
                                etarget.Distance(Vlad) <= E.Range)
                            {
                                E.Cast();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////GAME ON UPDATE -> COMBO///////////////////////////////////////////////////
        /// </summary>
        /// <param name="args"></param>
        /// 

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Jungle();
            }

            if (Player.Instance.CountEnemiesInRange(Q.Range) > 0) Killsteal();
            if (AddonMenu["UseSkinHack"].Cast<CheckBox>().CurrentValue)
            {
                LoadingSkin();
            }
            AutoIgnity();

            var Inimigo = TargetSelector.GetTarget(700, DamageType.Magical);
            if (!Inimigo.IsValid()) return;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                {
                     if (Q.IsReady() && Q.IsInRange(Inimigo) && (AddonMenu["QCombo"].Cast<CheckBox>().CurrentValue))
                        {
                            Q.Cast(Inimigo);
                        }
                }
                if (AddonMenu["WCombo"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady() && Player.Instance.CountEnemiesInRange(150) > 0)
                    {
                        W.Cast();
                    }
                }
                if (AddonMenu["ECombo"].Cast<CheckBox>().CurrentValue)
                {
                    if (E.IsReady() && Player.Instance.CountEnemiesInRange(610) > 0)
                    {
                        E.Cast();
                    }
                }

                Rincombo();
            }
          
        }
    }
}
