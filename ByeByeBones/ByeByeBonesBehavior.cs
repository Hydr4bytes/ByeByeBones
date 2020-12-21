using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MelonLoader;
using StressLevelZero.AI;
using StressLevelZero.Combat;

namespace ByeByeBones
{
    class ByeByeBonesBehavior : MonoBehaviour
    {
        public ByeByeBonesBehavior(IntPtr intPtr) : base(intPtr) { }
        public UnhollowerBaseLib.Il2CppReferenceArray<AudioClip> SnapSounds = null;
        public UnhollowerBaseLib.Il2CppReferenceArray<AudioClip> OriginalDeathSounds = null;

        AIBrain brain;
        Attack attack;

        float NeckBreakForce;

        void Awake()
        {
            brain = GetComponent<AIBrain>();
            OriginalDeathSounds = brain.behaviour.sfx.death;

            attack = new Attack();
            attack.damage = 99999;

            NeckBreakForce = MelonPrefs.GetFloat("ByeByeBones", "NeckBreakForce");
        }

        void Update()
        {
            if (!brain.isDead && brain.puppetMaster.muscles[brain.puppetMaster.GetMuscleIndex(HumanBodyBones.Head)] != null)
            {
                if (brain.puppetMaster.muscles[brain.puppetMaster.GetMuscleIndex(HumanBodyBones.Head)].joint.currentTorque.y >= NeckBreakForce || brain.puppetMaster.muscles[brain.puppetMaster.GetMuscleIndex(HumanBodyBones.Head)].joint.currentTorque.y <= -NeckBreakForce)
                {
                    brain.behaviour.sfx.death = SnapSounds;
                    brain.behaviour.health.TakeDamage(1, attack);
                    brain.puppetMaster.muscles[brain.puppetMaster.GetMuscleIndex(HumanBodyBones.Head)].IgnoreAngularLimits(true);
                }
            } else
            {
                brain.behaviour.sfx.death = OriginalDeathSounds;
                if (brain.puppetMaster.muscles[brain.puppetMaster.GetMuscleIndex(HumanBodyBones.Head)] != null)
                    brain.puppetMaster.muscles[brain.puppetMaster.GetMuscleIndex(HumanBodyBones.Head)].IgnoreAngularLimits(false);
                Destroy(this);
            }
        }
    }
}
