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
namespace EloBuddy
{
    class XinZhao
    {
        public static bool HasSpell(string s){  return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null; }
        public static AIHeroClient Hero { get { return ObjectManager.Player; }  }
        public AIHeroClient myTarget = null;
        public static Spell.Active W, Q, R;
        public static Spell.Targeted Ignite, E;
        public static Menu Menu;
        static Item Mercurial;
        static Item Tiamat, Hydra, BOTRK, Bilgewater, Hextech, Youmuu;
        public static String NomeHero = "XinZhao";

        public XinZhao()
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Bootstrap.Init(null);
        }
        private static void Game_OnStart(EventArgs args)
        {
            if (Player.Instance.ChampionName != NomeHero) return;
            Q = new Spell.Active(SpellSlot.Q, 180);
            W = new Spell.Active(SpellSlot.W, 180);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R, 187);
            if (HasSpell("summonerdot")) Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            Mercurial = new Item(3137);
            BOTRK = new Item(3153, 550);
            Bilgewater = new Item(3144, 550);
            Hydra = new Item(3074, 400);
            Tiamat = new Item(3077, 400);
            Hextech = new Item(3146,550);
            Youmuu = new Item(3142);
            //-------------------------------------------------------CHAT -----------------------------------------------------------
            Chat.Print("|| " + NomeHero + " Load || Credit: <font color='#FF0000'>UnrealSkill99</font></b> || Challenger Addon To EloBuddy", Color.White);
            //-------------------------------------------------------MENU ------------------------------------------------------------
            Menu = MainMenu.AddMenu(NomeHero, NomeHero);
            Menu.AddSeparator(5);
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Combo 〉〉〉〉");
            Menu.Add("QCombo", new CheckBox("❐ " + NomeHero + " Usar / Use Q (Active)", true));
            Menu.Add("WCombo", new CheckBox("❐ " + NomeHero + " Usar / Use W (Active)", true));
            Menu.Add("ECombo", new CheckBox("❐ " + NomeHero + " Usar / Use E (Targed)", true));
            Menu.Add("RCombo", new CheckBox("❐ " + NomeHero + " Usar / Use R (Active)", true));
            Menu.AddLabel("Set the mode of Use Below Skills / Configure o Modo de Uso Abaixo das Habilidades");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Q & E Active 〉〉〉〉");
            var ActiveSkills = Menu.Add("ActiveSkillsMod", new Slider(" TEXT", 2, 1, 2));
            var TMode = new[] { NomeHero, "A - Before activating Skills / Ativar Habilidades Antes", "B - After activating Skills / Ativar Habilidades Depois" };
            ActiveSkills.DisplayName = TMode[ActiveSkills.CurrentValue];
            ActiveSkills.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = TMode[changeArgs.NewValue]; };
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Ultimate 〉〉〉〉");
            Menu.Add("REnemy", new Slider("Minimum Enemy for Ultimate", 2, 1, 5));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Auto 〉〉〉〉");
            Menu.Add("AutoKS", new CheckBox("❐ " + NomeHero + " Use Auto (Killsteal)", true));
            Menu.Add("Auto Ignity", new CheckBox("❐ " + NomeHero + " Use Auto (Ignity)", true));
            Menu.Add("UseItems", new CheckBox("❐ " + NomeHero + " Use Auto Items ", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Farm 〉〉〉〉");
            Menu.Add("LastHitMin", new CheckBox("❐ " + NomeHero + " LastHit (E)", false));
            Menu.Add("LaneClearFarmE", new CheckBox("❐ " + NomeHero + " LaneClear (E)", false));
            Menu.Add("JClear", new CheckBox("❐ " + NomeHero + " Jungle (Q/W)", false));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Draw 〉〉〉〉");
            Menu.Add("DesabilitaDraw", new CheckBox("❐ " + NomeHero + " My Range (Atk Range)", true));
            Menu.Add("DesabilitaDrawQ", new CheckBox("❐ " + NomeHero + " Q Range (My Spell)", false));
            Menu.Add("DesabilitaDrawW", new CheckBox("❐ " + NomeHero + " W Range (My Spell)", false));
            Menu.Add("DesabilitaDrawE", new CheckBox("❐ " + NomeHero + " E Range (My Spell)", true));
            Menu.Add("DesabilitaDrawR", new CheckBox("❐ " + NomeHero + " R Range (My Spell)", false));
            Menu.Add("DesabilitaDrawLine", new CheckBox("Get Line TargetSelector (Enemy)", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 SkinHack 〉〉〉〉");
            Menu.Add("UseSkinHack", new CheckBox("❐ " + NomeHero + "  Use SkinHack", true));
            Menu.Add("SkinLoad", new KeyBind("Load Skin / Carrega Skin", false, KeyBind.BindTypes.HoldActive, 'N'));
            var SkinHack = Menu.Add("SkinID", new Slider("SkinHack Select", 5, 0, 6));
            var ID = new[] { "Classic", "SkinHack 1", "SkinHack 2", "SkinHack 3", "SkinHack 4", "SkinHack 5", "SkinHack 6"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID[changeArgs.NewValue]; };;
            Menu.AddLabel("______________________________________________________________________________________");
            Player.SetModel("XinZhao");
        }
        public static void LoadingSkin()
        {
            if (Menu.Get<KeyBind>("SkinLoad").CurrentValue)
            {
                var SkinHackSelect = Menu["SkinID"].DisplayName;
                switch (SkinHackSelect)
                {
                    case "Classic": Player.Instance.SetSkinId(0); break;
                    case "SkinHack 1": Player.Instance.SetSkinId(1); break;
                    case "SkinHack 2": Player.Instance.SetSkinId(2); break;
                    case "SkinHack 3": Player.Instance.SetSkinId(3); break;
                    case "SkinHack 4": Player.Instance.SetSkinId(4); break;
                    case "SkinHack 5": Player.Instance.SetSkinId(5); break;
                    case "SkinHack 6": Player.Instance.SetSkinId(6); break;
                }
                Chat.Print("|| SkinHack Load || <font color='#84e2ab'>Carregado / Load ModelHack " + SkinHackSelect.ToString() + "</font>", System.Drawing.Color.White);
            }
        }
        private static void Game_OnDraw(EventArgs args)
        {
            if (Menu["DesabilitaDraw"].Cast<CheckBox>().CurrentValue) new Circle() { Color = Color.Black, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(ObjectManager.Player.Position);
            var Inimigo = TargetSelector.GetTarget(700, DamageType.Magical);
            Drawing.DrawText(20, 20, System.Drawing.Color.WhiteSmoke, "Timer: " + DateTime.Now.ToShortTimeString(), 40);
            if (Menu["DesabilitaDrawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = Q.Range }.Draw(ObjectManager.Player.Position);
                new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = Q.Range - 3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawW"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = W.Range }.Draw(ObjectManager.Player.Position);
                new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = W.Range - 3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawE"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = E.Range }.Draw(ObjectManager.Player.Position);
                new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = E.Range - 3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = R.Range }.Draw(ObjectManager.Player.Position);
                new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = R.Range - 3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawLine"].Cast<CheckBox>().CurrentValue) Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 2, Color.Black);
        }
        public static Obj_AI_Base GetEnemy(float range, GameObjectType t)
        {
            switch (t)
            {
                case GameObjectType.AIHeroClient:
                    return EntityManager.Heroes.Enemies.OrderBy(a => a.Health).FirstOrDefault(a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
                default:
                    return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(a => a.Distance(Player.Instance) < range && !a.IsDead && !a.IsInvulnerable);
            }
        }
        public static void AutoIgnity()
        {
            if (!Menu["Auto Ignity"].Cast<CheckBox>().CurrentValue || !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) return;
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy && a.IsValidTarget(Ignite.Range) && a.Health < 50 + 20 * Hero.Level - (a.HPRegenRate / 5 * 3)))
            {
                Ignite.Cast(source);
                return;
            }
        }
        internal class Misc
        {
            private static AIHeroClient XinZhao
            {
                get { return ObjectManager.Player; }

            }

            public static float Qdmg(Obj_AI_Base target)
            {
                return Hero.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 15, 30, 45, 60, 75 }[Q.Level] + (0.2f * Hero.FlatPhysicalDamageMod)));
            }

            public static float Edmg(Obj_AI_Base target)
            {
                return Hero.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 70, 110, 150, 190, 230 }[E.Level] + (0.6f * Hero.FlatMagicDamageMod)));
            }

            public static float Rdmg(Obj_AI_Base target)
            {
                return Hero.CalculateDamageOnUnit(target, DamageType.Magical,
                    (new float[] { 0, 75, 175, 275 }[E.Level] + (1.0f * Hero.FlatPhysicalDamageMod)));
            }
        }
        public enum AttackSpell
        {
            E
        };
        public static Obj_AI_Base MinionLh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy
            && a.Type == type &&  a.Distance(Hero) <= XinZhao.E.Range && !a.IsDead && !a .IsInvulnerable && a.IsValidTarget(E.Range) && a.Health <= Misc.Edmg(a));
        }
        public static void LastHit()
        {
            var x = Menu["LastHitMin"].Cast<CheckBox>().CurrentValue;
            if (!E.IsReady()) return;
            var minion = (Obj_AI_Minion)MinionLh(GameObjectType.obj_AI_Minion, AttackSpell.E);
            if (x && minion != null) E.Cast(minion); 
        }
        public static void LaneClear()
        {
            var enemyE = (Obj_AI_Minion)GetEnemy(XinZhao.E.Range, GameObjectType.obj_AI_Minion);
            if (E.IsReady() && enemyE.IsValid() && Menu["LaneClearFarmE"].Cast<CheckBox>().CurrentValue) E.Cast(enemyE);
        }
        public static void Jungle()
        {
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,E.Range).FirstOrDefault(e => e.IsValidTarget());
            if (Menu["LaneClearFarmE"].Cast<CheckBox>().CurrentValue)
            {
                //if (E.IsReady() && monster.IsValidTarget(600)) E.Cast(monster);
                if (W.IsReady() && monster.IsValidTarget(600)) W.Cast();
                if (Q.IsReady() && monster.IsValidTarget(600)) Q.Cast();
            }
        }
        private static void Killsteal()
        {
            var enemy = TargetSelector.GetTarget(E.Range, DamageType.Magical);
            if (Menu["AutoKS"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var x in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(E.Range) && !hero.IsDead && !hero.IsZombie))
                {
                    if (Hero.GetSpellDamage(x, SpellSlot.E) >= x.Health && x.Distance(Hero) <= E.Range) E.Cast(enemy);
                    if (Hero.GetSpellDamage(x, SpellSlot.R) >= x.Health && x.Distance(Hero) <= R.Range) R.Cast();    
                }
            }
        }
        public static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (Menu["QCombo"].Cast<CheckBox>().CurrentValue && Q.IsReady())
            {
               var SelectMode = Menu["ActiveSkillsMod"].DisplayName;
               switch (SelectMode){
               case "A - Before activating Skills / Ativar Habilidades Antes": if (Inimigo.IsValidTarget(600)) Q.Cast(); break;
               case "B - After activating Skills / Ativar Habilidades Depois": if (Inimigo.IsValidTarget(200)) Q.Cast(); break;
               }
            }
            if (Menu["WCombo"].Cast<CheckBox>().CurrentValue && W.IsReady())
                {
                var SelectMode = XinZhao.Menu["ActiveSkillsMod"].DisplayName;
                switch (SelectMode){
                case "A - Before activating Skills / Ativar Habilidades Antes": if (Inimigo.IsValidTarget(600)) W.Cast(); break;
                case "B - After activating Skills / Ativar Habilidades Depois": if (Inimigo.IsValidTarget(200)) W.Cast(); break;
                }
            }
                if (Menu["ECombo"].Cast<CheckBox>().CurrentValue && E.IsReady() && E.IsInRange(Inimigo)) E.Cast(Inimigo);
                if (Menu["RCombo"].Cast<CheckBox>().CurrentValue && R.IsReady() && Player.Instance.CountEnemiesInRange(187) >= Menu["REnemy"].Cast<Slider>().CurrentValue) R.Cast();
                if (Menu["UseItems"].Cast<CheckBox>().CurrentValue)
                {
                    if (Inimigo.IsValidTarget(400) && Tiamat.IsReady()) Tiamat.Cast();
                    if (Inimigo.IsValidTarget(400) && Hydra.IsReady()) Hydra.Cast();
                    if (Inimigo.IsValidTarget(200) && Youmuu.IsReady()) Youmuu.Cast();

                    if (Inimigo.IsValidTarget(200) && BOTRK.IsReady()) BOTRK.Cast(Inimigo);
                    if (Inimigo.IsValidTarget(550) && Bilgewater.IsReady()) Bilgewater.Cast(Inimigo);
                    if (Inimigo.IsValidTarget(200) && Hextech.IsReady()) Hextech.Cast(Inimigo);
                } 
      }
        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHit();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungle();
            if (Player.Instance.CountEnemiesInRange(E.Range) > 0) Killsteal();
            if (Menu["UseSkinHack"].Cast<CheckBox>().CurrentValue) LoadingSkin();
            AutoIgnity();
        }
        }
    }

