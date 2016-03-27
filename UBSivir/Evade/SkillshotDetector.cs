using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using UBsivir.Evade;

namespace UBSivir.Evade
{
    internal static class SkillshotDetector
    {
        public delegate void OnDeleteMissileH(Skillshot skillshot, MissileClient missile);

        public delegate void OnDetectSkillshotH(Skillshot skillshot);

        public static List<Skillshot> detectedSkillShots = new List<Skillshot>();

        static SkillshotDetector()
        {
            Obj_AI_Base.OnProcessSpellCast += ObjAiHeroOnOnProcessSpellCast;

            GameObject.OnDelete += ObjSpellMissileOnOnDelete;
            GameObject.OnCreate += ObjSpellMissileOnOnCreate;
            GameObject.OnDelete += GameObject_OnDelete;

        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint)WindowMessages.KeyUp)
            {
                TriggerOnDetectSkillshot(
                    DetectionType.ProcessSpell, SpellDatabase.GetByName("TestSkillShot"), Environment.TickCount,
                    ObjectManager.Player.ServerPosition.To2D(), Game.CursorPos.To2D(), ObjectManager.Player);
            }
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid || sender.Team == ObjectManager.Player.Team)
            {
                return;
            }

            for (var i = detectedSkillShots.Count - 1; i >= 0; i--)
            {
                var skillshot = detectedSkillShots[i];
                if (skillshot.SpellData.ToggleParticleName != "" &&
                    sender.Name.Contains(skillshot.SpellData.ToggleParticleName))
                {
                    detectedSkillShots.RemoveAt(i);
                }
            }
        }

        private static void ObjSpellMissileOnOnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.IsValid || !(sender is MissileClient))
            {
                return; 
            }

            var missile = (MissileClient)sender;

#if DEBUG
            if (missile.SpellCaster is AIHeroClient)
            {
                Console.WriteLine(
                    Environment.TickCount + " Projectile Created: " + missile.SData.Name + " distance: " +
                    missile.StartPosition.Distance(missile.EndPosition) + "Radius: " +
                    missile.SData.CastRadiusSecondaryArray[0] + " Speed: " + missile.SData.MissileSpeed);
            }

#endif


            var unit = missile.SpellCaster;
            if (!unit.IsValid || (unit.Team == ObjectManager.Player.Team))
            {
                return;
            }

            var spellData = SpellDatabase.GetByMissileName(missile.SData.Name);
            if (spellData == null)
            {
                return;
            }
            var missilePosition = missile.Position.To2D();
            var unitPosition = missile.StartPosition.To2D();
            var endPos = missile.EndPosition.To2D();

            var direction = (endPos - unitPosition).Normalized();
            if (unitPosition.Distance(endPos) > spellData.Range || spellData.FixedRange)
            {
                endPos = unitPosition + direction * spellData.Range;
            }

            if (spellData.ExtraRange != -1)
            {
                endPos = endPos +
                         Math.Min(spellData.ExtraRange, spellData.Range - endPos.Distance(unitPosition)) * direction;
            }

            var castTime = Environment.TickCount - Game.Ping / 2 - (spellData.MissileDelayed ? 0 : spellData.Delay) -
                           (int)(1000 * missilePosition.Distance(unitPosition) / spellData.MissileSpeed);

            TriggerOnDetectSkillshot(DetectionType.RecvPacket, spellData, castTime, unitPosition, endPos, unit);
        }

        private static void ObjSpellMissileOnOnDelete(GameObject sender, EventArgs args)
        {
            if (!(sender is MissileClient))
            {
                return;
            }

            var missile = (MissileClient)sender;

            if (!(missile.SpellCaster is AIHeroClient))
            {
                return;
            }

            var unit = (AIHeroClient)missile.SpellCaster;
            if (!unit.IsValid || (unit.Team == ObjectManager.Player.Team))
            {
                return;
            }

            var spellName = missile.SData.Name;

            if (OnDeleteMissile != null)
            {
                foreach (var skillshot in detectedSkillShots)
                {
                    if (skillshot.SpellData.MissileSpellName == spellName &&
                        (skillshot.Unit.NetworkId == unit.NetworkId &&
                         (missile.EndPosition.To2D() - missile.StartPosition.To2D()).AngleBetween(skillshot.Direction) <
                         10) && skillshot.SpellData.CanBeRemoved)
                    {
                        OnDeleteMissile(skillshot, missile);
                        break;
                    }
                }
            }

#if DEBUG
            Console.WriteLine(
                "Missile deleted: " + missile.SData.Name + " D: " + missile.EndPosition.Distance(missile.Position));
#endif

            detectedSkillShots.RemoveAll(
                skillshot =>
                    (skillshot.SpellData.MissileSpellName == spellName ||
                     skillshot.SpellData.ExtraMissileNames.Contains(spellName)) &&
                    (skillshot.Unit.NetworkId == unit.NetworkId &&
                     ((missile.EndPosition.To2D() - missile.StartPosition.To2D()).AngleBetween(skillshot.Direction) < 10) &&
                     skillshot.SpellData.CanBeRemoved || skillshot.SpellData.ForceRemove)); // 
        }

        public static event OnDetectSkillshotH OnDetectSkillshot;

        public static event OnDeleteMissileH OnDeleteMissile;


        private static void TriggerOnDetectSkillshot(DetectionType detectionType,
            SpellData spellData,
            int startT,
            Vector2 start,
            Vector2 end,
            Obj_AI_Base unit)
        {
            var skillshot = new Skillshot(detectionType, spellData, startT, start, end, unit);

            if (OnDetectSkillshot != null)
            {
                OnDetectSkillshot(skillshot);
            }
        }

        private static void ObjAiHeroOnOnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.SData.Name == "dravenrdoublecast")
            {
                detectedSkillShots.RemoveAll(
                    s => s.Unit.NetworkId == sender.NetworkId && s.SpellData.SpellName == "DravenRCast");
            }

            if (!sender.IsValid || sender.Team == ObjectManager.Player.Team)
            {
                return;
            }
            var spellData = SpellDatabase.GetByName(args.SData.Name);

            if (spellData == null)
            {
                return;
            }

            var startPos = new Vector2();

            if (spellData.FromObject != "")
            {
                foreach (var o in ObjectManager.Get<GameObject>())
                {
                    if (o.Name.Contains(spellData.FromObject))
                    {
                        startPos = o.Position.To2D();
                    }
                }
            }
            else
            {
                startPos = sender.ServerPosition.To2D();
            }

            if (spellData.FromObjects != null && spellData.FromObjects.Length > 0)
            {
                foreach (var obj in ObjectManager.Get<GameObject>())
                {
                    if (obj.IsEnemy && spellData.FromObjects.Contains(obj.Name))
                    {
                        var start = obj.Position.To2D();
                        var end = start + spellData.Range * (args.End.To2D() - obj.Position.To2D()).Normalized();
                        TriggerOnDetectSkillshot(
                            DetectionType.ProcessSpell, spellData, Environment.TickCount - Game.Ping / 2, start, end,
                            sender);
                    }
                }
            }

            if (!startPos.IsValid())
            {
                return;
            }

            var endPos = args.End.To2D();

            if (spellData.SpellName == "LucianQ" && args.Target != null &&
                args.Target.NetworkId == ObjectManager.Player.NetworkId)
            {
                return;
            }

            var direction = (endPos - startPos).Normalized();
            if (startPos.Distance(endPos) > spellData.Range || spellData.FixedRange)
            {
                endPos = startPos + direction * spellData.Range;
            }

            if (spellData.ExtraRange != -1)
            {
                endPos = endPos +
                         Math.Min(spellData.ExtraRange, spellData.Range - endPos.Distance(startPos)) * direction;
            }


            TriggerOnDetectSkillshot(
                DetectionType.ProcessSpell, spellData, Environment.TickCount - Game.Ping / 2, startPos, endPos, sender);
        }
    }
}
