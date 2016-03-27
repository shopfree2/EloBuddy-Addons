using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;

namespace UBSivir
{
    class Mode
    {
        public static void Combo()
        {
            if (Config.ComboMenu["useQCombo"].Cast<CheckBox>().CurrentValue
                && Spells.Q.IsReady()
                && !Player.Instance.IsDashing())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
                if (target != null && target.IsValidTarget())
                {
                    Spells.Q.Cast(target);
                }
            }
            Event.Resetaa();
            if (Config.ComboMenu["useRCombo"].Cast<CheckBox>().CurrentValue
                      && Spells.R.IsReady())
            {               
                if (ObjectManager.Player.Position.CountAlliesInRange(1000) >= Config.ComboMenu["RhitCombo"].Cast<Slider>().CurrentValue
                  && ObjectManager.Player.Position.CountEnemiesInRange(2000) >= 4)
                {
                    Spells.R.Cast();
                }
            }
        }
        //Harass
        public static void Harass()
        {
            if (Config.HarassMenu["useQ"].Cast<CheckBox>().CurrentValue
                && Spells.Q.IsReady()
                && !Player.Instance.IsDashing())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Physical);
                if (target != null && target.IsValidTarget())
                {
                    Spells.Q.Cast(target);
                }
            }
            if (Config.HarassMenu["useW"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent > Config.HarassMenu["HrManager"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady())
            {
                Spells.W.Cast();
            }
        }
        //LaneClear
        public static void LaneClear()
        {
            var minion = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsEnemy && x.IsMinion && x.IsValidTarget(Spells.Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (minion != null) return;
            if (Config.LaneClear["useQLc"].Cast<CheckBox>().CurrentValue
              && Player.Instance.ManaPercent > Config.LaneClear["LcManager"].Cast<Slider>().CurrentValue
              && Spells.Q.IsReady()) return;
            {
                Spells.Q.Cast(minion);
            }
            if (Config.LaneClear["useWLc"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent > Config.LaneClear["LcManager"].Cast<Slider>().CurrentValue
                && minion.CountEnemiesInRange(700) >= Config.LaneClear["WhitLc"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady())
            {
                Spells.W.Cast();
            }
        }
        //Lasthit
        public enum AttackSpell
        { Q, W }
        private static Obj_AI_Base MinionQLh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault
                (a => a.IsEnemy
                && a.Type == type
                && a.Distance(Sivir) <= Spells.Q.Range
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(Spells.Q.Range)
                && a.Health <= Damages.QDamage(a));
        }

        private static Obj_AI_Base MinionWlh(GameObjectType type, AttackSpell spell)
        {
            return EntityManager.MinionsAndMonsters.EnemyMinions.OrderBy(a => a.Health).FirstOrDefault
                (a => a.IsEnemy
                && a.Type == type
                && a.Distance(Sivir) <= ObjectManager.Player.GetAutoAttackRange()
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(ObjectManager.Player.GetAutoAttackRange())
                && a.Health <= ObjectManager.Player.GetAutoAttackDamage(a));
        }

        public static void Lasthit()
        {
            if (Config.LasthitMenu["useQLh"].Cast<CheckBox>().CurrentValue
              && Spells.Q.IsReady()) return;
            {
                var qminion = (Obj_AI_Minion)MinionQLh(GameObjectType.obj_AI_Minion, AttackSpell.Q);
                if (qminion != null && Player.Instance.ManaPercent >= Config.LasthitMenu["LhManager"].Cast<Slider>().CurrentValue)
                {
                    Spells.Q.Cast(qminion.ServerPosition);
                }

                if (Config.LasthitMenu["useWLh"].Cast<CheckBox>().CurrentValue
                 && Spells.W.IsReady()) return;
                {
                    var wminion = (Obj_AI_Minion)MinionWlh(GameObjectType.obj_AI_Minion, AttackSpell.W);
                    if (wminion != null && Player.Instance.ManaPercent >= Config.LasthitMenu["LhManager"].Cast<Slider>().CurrentValue)
                    {
                        Spells.W.Cast();                        
                    }
                }
            }
        }
        //JungleClear
        public static void JungleClear()
        {
            var monster = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsMonster && x.IsValidTarget(Spells.Q.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (monster == null || !monster.IsValid) return;
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            if (Config.JungleClear["useQJc"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent > Config.JungleClear["JcManager"].Cast<Slider>().CurrentValue
                && Spells.Q.IsReady())
            {
                Spells.Q.Cast(monster);
            }

            var wmonster = ObjectManager.Get<Obj_AI_Minion>().Where(x => x.IsMonster && x.IsValidTarget(Spells.W.Range)).OrderBy(x => x.Health).FirstOrDefault();
            if (wmonster == null || !wmonster.IsValid) return;
            if (Orbwalker.IsAutoAttacking) return;
            Orbwalker.ForcedTarget = null;
            if (Config.JungleClear["useWJc"].Cast<CheckBox>().CurrentValue
                && Player.Instance.ManaPercent > Config.JungleClear["JcManager"].Cast<Slider>().CurrentValue
                && Spells.W.IsReady()
                && wmonster.CountEnemiesInRange(600) >= Config.JungleClear["WhitJc"].Cast<Slider>().CurrentValue)
            {
                if (Player.Instance.Mana > Config.JungleClear["JcEnergyManager"].Cast<Slider>().CurrentValue)
                {
                    Spells.W.Cast();
                }
            }
        }
        public static readonly AIHeroClient Sivir = ObjectManager.Player;
        //KillSteal
        public static void Killsteal()
        {
            if (Config.MiscMenu["useQKS"].Cast<CheckBox>().CurrentValue && Spells.Q.IsReady()) return;
            try
            {
                foreach (var kstarget in from qtarget in EntityManager.Heroes.Enemies.Where(
                    hero => hero.IsValidTarget(Spells.Q.Range) && !hero.IsDead && !hero.IsZombie)
                                        where Sivir.GetSpellDamage(qtarget, SpellSlot.Q) >= qtarget.Health
                                        select Spells.Q.GetPrediction(qtarget))
                {
                    Spells.Q.Cast(kstarget.CastPosition);
                }               
            }
            catch
            {
            }
        }
        public static void Useheal()
        {

            if (Config.ComboMenu["useheal"].Cast<CheckBox>().CurrentValue
             && Player.Instance.HealthPercent < Config.ComboMenu["manageheal"].Cast<Slider>().CurrentValue
             && ObjectManager.Player.CountEnemiesInRange(900) >= 1
             && Spells.heal.IsReady())
            {
                Spells.heal.Cast();
            }
            foreach (
                var ally in EntityManager.Heroes.Allies.Where(a => !a.IsDead))
            {
                if (Config.ComboMenu["usehealally"].Cast<CheckBox>().CurrentValue && ally.CountEnemiesInRange(800) >= 1
                    && ObjectManager.Player.Position.Distance(ally) < 800
                    && ally.HealthPercent <= Config.ComboMenu["managehealally"].Cast<Slider>().CurrentValue
                    && Spells.heal.IsReady())
                {
                    Spells.heal.Cast();
                }
            }
        }        
    }
}
