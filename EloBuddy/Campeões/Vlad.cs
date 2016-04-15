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
    class Vladimir1
    {
        public static bool HasSpell(string s) { return Player.Spells.FirstOrDefault(o => o.SData.Name.Contains(s)) != null; }
        public static AIHeroClient Vladimir { get { return ObjectManager.Player; } }
        public static Spell.Active W, E;
        public static Spell.Skillshot R;
        public static Spell.Targeted Ignite, Q;
        public static Menu Menu;
        static Item Mercurial;
        public static  String NomeChamp = "Vladimir";
        public static int LastE;
        public static Text Text = new EloBuddy.SDK.Rendering.Text("", new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 8, System.Drawing.FontStyle.Bold));

        public Vladimir1()
        {
            Loading.OnLoadingComplete += Game_OnStart;
            EloBuddy.Hacks.RenderWatermark = false;
            Bootstrap.Init(null);
        }
        private static void Game_OnStart(EventArgs args)
        {
            if (Player.Instance.ChampionName != NomeChamp)  return;
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 150);
            E = new Spell.Active(SpellSlot.E, 610);
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 150);

            if (HasSpell("summonerdot")) Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            Mercurial = new Item(3137);
            //-------------------------------------------------------CHAT -----------------------------------------------------------
            Chat.Print("|| "+ NomeChamp + " Load || Credit: <font color='#FF0000'>UnrealSkill99</font></b> || Challenger Addon To EloBuddy", Color.White);
            //-------------------------------------------------------MENU ------------------------------------------------------------
            Menu = MainMenu.AddMenu(NomeChamp, NomeChamp);
            Menu.AddSeparator(5);
            Menu.AddLabel("〈〈〈〈 Special Thanks to: Bloodimir 〉〉〉〉");
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Combo 〉〉〉〉");
            Menu.Add("QCombo", new CheckBox("❐ " + NomeChamp + " Usar / Use Q (Targed)", true));
            Menu.Add("WCombo", new CheckBox("❐ " + NomeChamp + " Usar / Use W (Active)", true));
            Menu.Add("ECombo", new CheckBox("❐ " + NomeChamp + " Usar / Use E (Active)", true));
            Menu.Add("RCombo", new CheckBox("❐ " + NomeChamp + " Usar / Use R (Targed)", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Ultimate 〉〉〉〉");
            Menu.Add("REnemy", new Slider("Minimum for Combo R", 2, 1, 5));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Auto 〉〉〉〉");
            Menu.Add("AutoKS", new CheckBox("❐ " + NomeChamp + " Use Automatically (Killsteal)", true));
            Menu.Add("Auto Ignity", new CheckBox("❐ " + NomeChamp + " Use Automatically (Ignity)", true));
            Menu.Add("Estack", new CheckBox("❐ " + NomeChamp + " Auto Stack [E] (Passive)", true));
            Menu.Add("HpStack", new Slider("❐ " + NomeChamp + " Min % HP for Stack [E]", 20, 0, 100));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Farm 〉〉〉〉");
            Menu.Add("LastHitMin", new CheckBox("❐ " + NomeChamp + " LastHit (Q)", true));
            Menu.Add("LaneClearFarmQ", new CheckBox("❐ " + NomeChamp + " LaneClear (Q) ", true));
            Menu.Add("LaneClearFarmE", new CheckBox("❐ " + NomeChamp + " LaneClear (E) ", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 Draw 〉〉〉〉");
            Menu.Add("DesabilitaDraw", new CheckBox("❐ " + NomeChamp + " My Range (Atk Range)", true));
            Menu.Add("DesabilitaDrawQ", new CheckBox("❐ " + NomeChamp + " Q Range (My Spell)", true));
            Menu.Add("DesabilitaDrawW", new CheckBox("❐ " + NomeChamp + " W Range (My Spell)", false));
            Menu.Add("DesabilitaDrawE", new CheckBox("❐ " + NomeChamp + " E Range (My Spell)", false));
            Menu.Add("DesabilitaDrawR", new CheckBox("❐ " + NomeChamp + " R Range (My Spell)", true));
            Menu.Add("Txt", new CheckBox("❐ " + NomeChamp + " Show Text (Info)", true));
            Menu.Add("DesabilitaDrawLine", new CheckBox("Get Line TargetSelector (Enemy)", true));
            Menu.AddLabel("______________________________________________________________________________________");
            Menu.AddLabel("〈〈〈〈 SkinHack 〉〉〉〉");
            Menu.Add("UseSkinHack", new CheckBox("❐ " + NomeChamp + "  Use SkinHack", true));
            Menu.Add("SkinLoad", new KeyBind("Load Skin / Carrega Skin", false, KeyBind.BindTypes.HoldActive, 'N'));
            var SkinHack = Menu.Add("SkinID", new Slider("SkinHack Select", 5, 0, 6));
            var ID = new[] { "Classic", "SkinHack 1", "SkinHack 2", "SkinHack 3", "SkinHack 4", "SkinHack 5", "SkinHack 6"};
            SkinHack.DisplayName = ID[SkinHack.CurrentValue];
            SkinHack.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs) { sender.DisplayName = ID[changeArgs.NewValue]; };;
            Player.SetModel("Vladimir");
            Menu.AddLabel("______________________________________________________________________________________");
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
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
            }
        }
        private static void Game_OnDraw(EventArgs args)
        {
            if (true)
            {
                Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(30, -20);
                Text.Color = Color.White;
                Text.TextValue = " R In: [" + Menu["REnemy"].Cast<Slider>().CurrentValue.ToString() + "] Enemy";
                Text.Draw();

                Text.Position = Drawing.WorldToScreen(Player.Instance.Position) - new Vector2(30, -40);
                Text.Color = Color.White;
                Text.TextValue = " E Stack: [" + Menu["Estack"].Cast<CheckBox>().CurrentValue.ToString() +"]";
                Text.Draw();

                if (Menu["DesabilitaDraw"].Cast<CheckBox>().CurrentValue) new Circle() { Color = Color.Black, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 3f }.Draw(ObjectManager.Player.Position);
            }

            var Inimigo = TargetSelector.GetTarget(700, DamageType.Magical);
            Drawing.DrawText(20, 20, System.Drawing.Color.WhiteSmoke, "Timer: " + DateTime.Now.ToShortTimeString(), 40);
            if (Menu["DesabilitaDrawQ"].Cast<CheckBox>().CurrentValue)
            {
               new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = Q.Range }.Draw(ObjectManager.Player.Position);
               new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = Q.Range -3 }.Draw(ObjectManager.Player.Position);   
            }
            if (Menu["DesabilitaDrawW"].Cast<CheckBox>().CurrentValue)
            {
             new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = W.Range }.Draw(ObjectManager.Player.Position);
             new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = W.Range -3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawE"].Cast<CheckBox>().CurrentValue)
            {
             new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = E.Range }.Draw(ObjectManager.Player.Position);
             new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = E.Range -3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawR"].Cast<CheckBox>().CurrentValue)
            {
               new Circle() { Color = Color.Black, BorderWidth = 3f, Radius = R.Range }.Draw(ObjectManager.Player.Position);
               new Circle() { Color = Color.WhiteSmoke, BorderWidth = 3f, Radius = R.Range -3 }.Draw(ObjectManager.Player.Position);
            }
            if (Menu["DesabilitaDrawLine"].Cast<CheckBox>().CurrentValue) Drawing.DrawLine(ObjectManager.Player.Position.WorldToScreen(), Inimigo.Position.WorldToScreen(), 2, Color.Black); 
        }
        public static int Now {get { return (int)DateTime.Now.TimeOfDay.TotalMilliseconds; }}
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.NetworkId == Player.Instance.NetworkId && args.Slot == SpellSlot.E)
            {
                LastE = Now;
            }
        }
        public static void Stack()
        {
            if (Player.Instance.IsRecalling() || MenuGUI.IsChatOpen) return;
            // Now - LastCast
            var stackHp = Menu["HpStack"].Cast<Slider>().CurrentValue;
            if (Now - LastE >= 9900 && E.IsReady() && (Player.Instance.HealthPercent >= stackHp)) E.Cast();
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
            foreach (var source in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy && a.IsValidTarget(Ignite.Range) && a.Health < 50 + 20 * Vladimir.Level - (a.HPRegenRate / 5 * 3)))
            {
                Ignite.Cast(source);
                return;
            }
        }
        internal class Misc
        {
            private static AIHeroClient Vladimir
            { get { return ObjectManager.Player; } }
            public static float Qdmg(Obj_AI_Base target)
            {
                return Vladimir.CalculateDamageOnUnit(target, DamageType.Magical, (new float[] { 0, 90, 125, 160, 195, 230 }[Q.Level] + (0.6f * Vladimir.FlatMagicDamageMod)));
            }
            public static float Edmg(Obj_AI_Base target)
            {
                return Vladimir.CalculateDamageOnUnit(target, DamageType.Magical,(new float[] { 0, 60, 85, 110, 135, 160 }[E.Level] + (0.45f * Vladimir.FlatMagicDamageMod)));
            }
            public static float Rdmg(Obj_AI_Base target)
            {
                return Vladimir.CalculateDamageOnUnit(target, DamageType.Magical, (new float[] { 0, 168, 280, 392 }[E.Level] + (0.78f * Vladimir.FlatMagicDamageMod)));
            }
        }
        public enum AttackSpell {Q};
        public static Obj_AI_Base MinionLh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault(a => a.IsEnemy &&
            a.Type == type && a.Distance(Vladimir) <= Q.Range && !a.IsDead && !a.IsInvulnerable && a.IsValidTarget(Q.Range) && a.Health <= Misc.Qdmg(a));
        }
        public static void LastHit()
        {
            var qcheck = Menu["LastHitMin"].Cast<CheckBox>().CurrentValue;
            var minion = (Obj_AI_Minion)MinionLh(GameObjectType.obj_AI_Minion, AttackSpell.Q);
            if (!Q.IsReady()) return;
            if (minion != null) Q.Cast(minion);
        }
        public static void LaneClear()
        {
            var enemyE = (Obj_AI_Minion)GetEnemy(E.Range, GameObjectType.obj_AI_Minion);
            var enemyQ = (Obj_AI_Minion)GetEnemy(Q.Range, GameObjectType.obj_AI_Minion);
             if (Q.IsReady() && enemyQ.IsValid() && Menu["LaneClearFarmQ"].Cast<CheckBox>().CurrentValue) Q.Cast(enemyQ);
             if (E.IsReady() && enemyE.IsValid() && Menu["LaneClearFarmE"].Cast<CheckBox>().CurrentValue) E.Cast(); 
      }
        public static void Jungle()
        {
            var monster = EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position,Q.Range).FirstOrDefault(e => e.IsValidTarget());
            if (E.IsReady() && monster.IsValidTarget(E.Range)) E.Cast();
            if (Q.IsReady() && monster.IsValidTarget(Q.Range)) Q.Cast(monster);           
        }    
        public static void Rincombo()
        {
            foreach (var qtarget in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(R.Range) && !hero.IsDead && !hero.IsZombie))
            {
                var rtarget = TargetSelector.GetTarget(700, DamageType.Magical);
                if (Menu["RCombo"].Cast<CheckBox>().CurrentValue)
                {
                    if (R.IsReady() && rtarget.CountEnemiesInRange(R.Width) >= Menu["REnemy"].Cast<Slider>().CurrentValue) R.Cast(rtarget.ServerPosition);
                }
            }
        }
        private static void Killsteal()
        {
            var enemy = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
            if (Menu["AutoKS"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var x in EntityManager.Heroes.Enemies.Where(hero => hero.IsValidTarget(Q.Range) && !hero.IsDead && !hero.IsZombie))
                {
                    if (Vladimir.GetSpellDamage(x, SpellSlot.Q) >= x.Health && x.Distance(Vladimir) <= Q.Range) Q.Cast(enemy);
                    if (Vladimir.GetSpellDamage(x, SpellSlot.E) >= x.Health && x.Distance(Vladimir) <= E.Range) E.Cast(); 
                }
            }
        }
        public static void Combo()
        {
            var Inimigo = TargetSelector.GetTarget(700, DamageType.Magical);
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Q.IsReady() && Q.IsInRange(Inimigo) && (Menu["QCombo"].Cast<CheckBox>().CurrentValue)) Q.Cast(Inimigo);
                if (W.IsReady() && Player.Instance.CountEnemiesInRange(150) > 0 && Menu["WCombo"].Cast<CheckBox>().CurrentValue) W.Cast();
                if (E.IsReady() && Player.Instance.CountEnemiesInRange(610) > 0 && Menu["ECombo"].Cast<CheckBox>().CurrentValue) E.Cast();
              //  Orbwalker.OrbwalkTo(Game.CursorPos);
                Rincombo();
            }
        }
        private static void Game_OnUpdate(EventArgs args)
        {
            Combo();
            if (Player.Instance.CountEnemiesInRange(Q.Range) > 0) Killsteal();
            if (Menu["UseSkinHack"].Cast<CheckBox>().CurrentValue) LoadingSkin();
            if (Menu["Estack"].Cast<CheckBox>().CurrentValue && Player.Instance.CountEnemiesInRange(1000) > 0) Stack();
            AutoIgnity();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit)) LastHit();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) LaneClear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) Jungle();
        }
        }
    }

