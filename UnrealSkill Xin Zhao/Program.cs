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
namespace UnrealSkill
{
    class Program
    {
        public static bool HasSpell(string s)
        {
            return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null;
        }
        public static AIHeroClient Campeao = ObjectManager.Player;
        public static AIHeroClient Champion
        {
            get { return ObjectManager.Player; }
        }
        public AIHeroClient myTarget = null;
        public static Spell.Active W, Q, R;
        public static Spell.Skillshot Flash;
        public static Spell.Targeted Ignite, E;
        public static Menu Menu, AddonMenu;
        static Item Mercurial;
        static Item Tiamat, Hydra, BOTRK, Bilgewater, Hextech, Youmuu;
        public static String NomeChamp = "XinZhao";

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

            Q = new Spell.Active(SpellSlot.Q, 180);
            W = new Spell.Active(SpellSlot.W, 180);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R, 187);

            if (HasSpell("summonerdot"))
                Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);

            Mercurial = new Item(3137);
            BOTRK = new Item(3153, 550);
            Bilgewater = new Item(3144, 550);
            Hydra = new Item(3074, 400);
            Tiamat = new Item(3077, 400);
            Hextech = new Item(3146,550);
            Youmuu = new Item(3142);


            //-------------------------------------------------------CHAT -----------------------------------------------------------
            //Chat.Print(ObjectManager.Player.Model.ToString());
            Chat.Print("|| UnrealSkill " + NomeChamp + " || <font color='#7a7a7a'>Carregado / Load V1</font>", System.Drawing.Color.WhiteSmoke);
            //-------------------------------------------------------MENU ------------------------------------------------------------
            Menu = MainMenu.AddMenu("▐  U S  ▐", "Addon");
            Menu.AddSeparator(50);
         Menu.AddLabel("Logs:" + System.Environment.NewLine +
              "▐ Add SkinHack" + System.Environment.NewLine + 
              "" + System.Environment.NewLine +
              "▐ Add Killsteal Auto E & R & Ignity" + System.Environment.NewLine +
              "▐ Add Logic Auto check Enemy range for Ultimate" + System.Environment.NewLine + 
              "▐ Add JG Clear" + System.Environment.NewLine +
              "▐ Add LaneClear" + System.Environment.NewLine +
              "▐ Add LastHit Q"); 
          Menu.AddSeparator(100);
          // Menu.AddLabel("▐ Special Credit to: Bloodimir / Mr.Articuno / Aka (References to create this Addon)");
            AddonMenu = Menu.AddSubMenu("▐  " + NomeChamp + " ▐", "Settings");
            AddonMenu.AddLabel("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| Combo");
            AddonMenu.Add("QCombo", new CheckBox("|| " + NomeChamp + " Usar / Use Q || (Active)", true));
            AddonMenu.Add("WCombo", new CheckBox("|| " + NomeChamp + " Usar / Use W || (Active)", true));
            AddonMenu.Add("ECombo", new CheckBox("|| " + NomeChamp + " Usar / Use E || (Targed)", true));
            AddonMenu.Add("RCombo", new CheckBox("|| " + NomeChamp + " Usar / Use R || (Active)", true));
            AddonMenu.AddLabel("Set the mode of Use Below Skills / Configure o Modo de Uso Abaixo das Habilidades");
            AddonMenu.AddLabel("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| Q & E");
            AddonMenu.AddLabel("Time to Turn Skills Q & E / Tempo para Ativar Habilidades Q & E");
            var ActiveSkills = AddonMenu.Add("ActiveSkillsMod", new Slider(" TEXT", 2, 1, 2));
            var TMode = new[] { NomeChamp, "A - Before activating Skills / Ativar Habilidades Antes", "B - After activating Skills / Ativar Habilidades Depois" };
            ActiveSkills.DisplayName = TMode[ActiveSkills.CurrentValue];
            ActiveSkills.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = TMode[changeArgs.NewValue]; };

            AddonMenu.AddLabel("|| Automation ||");
            AddonMenu.Add("AutoKS", new CheckBox("|| " + NomeChamp + " Using Automatically (Killsteal) ||", true));
            AddonMenu.Add("Auto Ignity", new CheckBox("|| " + NomeChamp + " Using Automatically (Ignity) ||", true));
            AddonMenu.Add("LastHitMin", new CheckBox("|| " + NomeChamp + " LaneClear (E LastHit Minions) ||", false));
            AddonMenu.Add("LaneClearFarmE", new CheckBox("|| " + NomeChamp + " LaneClear (E) ||", false));
            AddonMenu.Add("UseItems", new CheckBox("|| " + NomeChamp + " Use Items ||", true));
            AddonMenu.AddLabel("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||| Draw");
            AddonMenu.Add("DesabilitaDraw", new CheckBox("|| " + NomeChamp + " My Range || (Atk Range)", true));
            AddonMenu.Add("DesabilitaDrawQ", new CheckBox("|| " + NomeChamp + " Q Range || (My Spell)", false));
            AddonMenu.Add("DesabilitaDrawW", new CheckBox("|| " + NomeChamp + " W Range || (My Spell)", false));
            AddonMenu.Add("DesabilitaDrawE", new CheckBox("|| " + NomeChamp + " E Range || (My Spell)", true));
            AddonMenu.Add("DesabilitaDrawR", new CheckBox("|| " + NomeChamp + " R Range || (My Spell)", false));
            AddonMenu.Add("DesabilitaDrawLine", new CheckBox("|| Get Line TargetSelector || (Enemy)", true));
            AddonMenu.AddLabel("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||  SkinHack");
            AddonMenu.Add("UseSkinHack", new CheckBox("|| " + NomeChamp + "  Use SkinHack || (F5)", true));
            var SkinHack = AddonMenu.Add("SkinID", new Slider("SkinHack Select", 5, 0, 6));
            var ID = new[] { "Classic", "SkinHack 1", "SkinHack 2", "SkinHack 3", "SkinHack 4", "SkinHack 5", "SkinHack 6"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID[changeArgs.NewValue]; };;
          //  Player.SetModel("XinZhao");
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
            var Inimigo = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            if (AddonMenu["DesabilitaDraw"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(ObjectManager.Player.Position);
                //new Circle() { Color = Color.Black, BorderWidth = 2f, Radius = ObjectManager.Player.GetAutoAttackRange() }.Draw(ObjectManager.Player.Position);
            }

          // Drawing.DrawText(20, 20, System.Drawing.Color.DeepPink, "Timer: " + DateTime.Now.ToShortTimeString(), 40);

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
              Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 3, Color.Black); 
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
                                a.Health < 50 + 20 * Campeao.Level - (a.HPRegenRate / 5 * 3)))
            {
                Ignite.Cast(source);
                return;
            }
        }

//FARMING

        internal class Misc
        {
            private static AIHeroClient XinZhao
            {
                get { return ObjectManager.Player; }
            }

            public static float Qdmg(Obj_AI_Base target)
            {
                return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 15, 30, 45, 60, 75 }[Program.Q.Level] + (0.2f * Champion.FlatPhysicalDamageMod)));
            }

            public static float Edmg(Obj_AI_Base target)
            {
                return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 70, 110, 150, 190, 230 }[Program.E.Level] + (0.6f * Champion.FlatMagicDamageMod)));
            }

            public static float Rdmg(Obj_AI_Base target)
            {
                return Champion.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 75, 175, 275 }[Program.E.Level] + (1.0f * Champion.FlatPhysicalDamageMod)));
            }
        }

        public enum AttackSpell
        {
            E
        };

        public static Obj_AI_Base MinionLh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
                                                                                                            &&
                                                                                                            a.Type ==
                                                                                                            type
                                                                                                            &&
                                                                                                            a.Distance(
                                                                                                                Champion) <=
                                                                                                            Program.E
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
                                                                                                                        .E
                                                                                                                        .Range)
                                                                                                            &&
                                                                                                            a.Health <=
                                                                                                            Misc.Edmg(a));
        }

        public static void LastHit()
        {
            var qcheck = Program.AddonMenu["LastHitMin"].Cast<CheckBox>().CurrentValue;
            if (qcheck &&!E.IsReady()) return;
            {
                var minion = (Obj_AI_Minion)MinionLh(GameObjectType.obj_AI_Minion, AttackSpell.E);
                if (minion != null)
                {
                    Program.E.Cast(minion);
                }
            }
        }


        public static void LaneClear()
        {
            var enemyE = (Obj_AI_Minion)GetEnemy(Program.E.Range, GameObjectType.obj_AI_Minion);
             if (E.IsReady() && enemyE.IsValid() && AddonMenu["LaneClearFarmE"].Cast<CheckBox>().CurrentValue)
              {
                  Program.E.Cast(enemyE);
             }
      }

        public static void Jungle()
        {
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,E.Range)
                        .FirstOrDefault(e => e.IsValidTarget());

            if (E.IsReady() && monster.IsValidTarget(600))
            {
                E.Cast(monster);
            }

            if (W.IsReady() && monster.IsValidTarget(600))
            {
                W.Cast();
            }

            if (Q.IsReady() && monster.IsValidTarget(600))
            {
                Q.Cast();
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

        //////////////////////////////////////////////////////////////////////KS////////////////////////////////////////////////////////
        private static void Killsteal()
        {
            var enemy = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (AddonMenu["AutoKS"].Cast<CheckBox>().CurrentValue && E.IsReady())
            {
                foreach (var qtarget in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(E.Range) && !hero.IsDead && !hero.IsZombie))
                {
                    if (Campeao.GetSpellDamage(qtarget, SpellSlot.E) >= qtarget.Health && qtarget.Distance(Campeao) <= E.Range)
                    {
                        E.Cast(enemy);
                    }
                    if (AddonMenu["AutoKS"].Cast<CheckBox>().CurrentValue && R.IsReady())
                    {
                        foreach (var etarget in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(R.Range) && !hero.IsDead && !hero.IsZombie))
                        {
                            if (Campeao.GetSpellDamage(etarget, SpellSlot.R) >= etarget.Health &&
                                etarget.Distance(Campeao) <= R.Range)
                            {
                                R.Cast();
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

            if (Player.Instance.CountEnemiesInRange(E.Range) > 0) Killsteal();
            if (AddonMenu["UseSkinHack"].Cast<CheckBox>().CurrentValue)
            {
                LoadingSkin();
            }
            AutoIgnity();

            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (!Inimigo.IsValid()) return;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                 if (AddonMenu["QCombo"].Cast<CheckBox>().CurrentValue)
                {
                     if (Q.IsReady())
                        {
                            var SelectMode = Program.AddonMenu["ActiveSkillsMod"].DisplayName;
                            switch (SelectMode)
                            {
                                case "A - Before activating Skills / Ativar Habilidades Antes":
                                    if (Inimigo.IsValidTarget(600)) Q.Cast();
                                    break;
                                case "B - After activating Skills / Ativar Habilidades Depois":
                                    if (Inimigo.IsValidTarget(200)) Q.Cast();
                                    break;
                            }
                        }
                }
                if (AddonMenu["WCombo"].Cast<CheckBox>().CurrentValue)
                {
                    if (W.IsReady())
                    {
                        var SelectMode = Program.AddonMenu["ActiveSkillsMod"].DisplayName;
                        switch (SelectMode)
                        {
                            case "A - Before activating Skills / Ativar Habilidades Antes":
                                if (Inimigo.IsValidTarget(600)) W.Cast();
                                break;
                            case "B - After activating Skills / Ativar Habilidades Depois":
                                if (Inimigo.IsValidTarget(200)) W.Cast();
                                break;
                        }
                    }
                }
                if (AddonMenu["ECombo"].Cast<CheckBox>().CurrentValue)
                {
                    if (E.IsReady() && E.IsInRange(Inimigo))
                    {
                        E.Cast(Inimigo);
                    }
                }
                if (AddonMenu["RCombo"].Cast<CheckBox>().CurrentValue)
                {

                    if (R.IsReady() && Player.Instance.CountEnemiesInRange(187) > 1)
                    {
                        R.Cast();
                    }

                }

                if (AddonMenu["UseItems"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(400) && Tiamat.IsReady()) Tiamat.Cast();
                    if (Inimigo.IsValidTarget(400) && Hydra.IsReady()) Hydra.Cast();
                    if (Inimigo.IsValidTarget(200) && Youmuu.IsReady()) Youmuu.Cast();


                    if (Inimigo.IsValidTarget(200) && BOTRK.IsReady()) BOTRK.Cast(Inimigo);
                    if (Inimigo.IsValidTarget(550) && Bilgewater.IsReady()) Bilgewater.Cast(Inimigo);
                    if (Inimigo.IsValidTarget(200) && Hextech.IsReady()) Hextech.Cast(Inimigo);


                  
                }



















            }
        }
    }
}
