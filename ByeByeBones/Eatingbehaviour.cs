using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using BoneworksModdingToolkit;
using StressLevelZero.AI;
using StressLevelZero.Combat;
using System.Collections;
using MelonLoader;

namespace ByeByeBones
{
    public class Eatingbehaviour : MonoBehaviour
    {
        public Eatingbehaviour(IntPtr intPtr) : base(intPtr) { }

        float timer;

        GameObject TeethObj;
        TextMeshPro TeethText;
        RectTransform TeethRect;

        Shader DefaultHandShader;

        void Awake()
        {
            TeethObj = new GameObject("TeethObj");
            TeethText = TeethObj.AddComponent<TextMeshPro>();
            TeethRect = TeethObj.GetComponent<RectTransform>();
            TeethRect.sizeDelta = new Vector2(0.5f, 0.5f);

            TeethObj.transform.parent = transform;
            TeethObj.transform.localPosition = new Vector3(0, 0, 0.19f);
            TeethObj.transform.rotation = transform.rotation;

            TeethText.text = "";
            TeethText.fontSize = 1;
            TeethText.color = Color.black;
            TeethText.fontStyle = FontStyles.Bold;
            TeethText.alignment = TextAlignmentOptions.Center;
        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.2f))
            {
                if (hit.collider.transform.root.gameObject.GetComponent<AIBrain>() != null)
                {
                    AIBrain brain = hit.collider.transform.root.gameObject.GetComponent<AIBrain>();

                    timer += Time.deltaTime;

                    if (timer > 1)
                    {
                        MelonCoroutines.Start(UIBite());
                        MelonCoroutines.Start(StartGlitch(brain));

                        timer = 0;
                    }
                }
            }
            else
            {
                timer = 0;
                TeethText.text = "";
            }
        }

        IEnumerator UIBite()
        {
            TeethText.text = "V V V\n^ ^ ^";
            yield return new WaitForSeconds(0.4f);
            TeethText.text = "V V V\n\n^ ^ ^";
            yield return new WaitForSeconds(0.1f);
        }

        IEnumerator StartGlitch(AIBrain brain)
        {
            Attack attack = new Attack()
            {
                damage = 0.05f,
                attackType = AttackType.Stabbing
            };

            foreach (Renderer renderer in brain.transform.root.GetComponentsInChildren<Renderer>())
            {
                renderer.material.shader = Shader.Find("SLZ/Falloff");
            }

            foreach (SkinnedMeshRenderer renderer in brain.transform.root.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                renderer.material.shader = Shader.Find("SLZ/Falloff");
            }

            while (!brain.puppetMaster.isDead)
            {
                foreach (Rigidbody rigidbody in brain._rigidbodies)
                {
                    Vector3 randomDirection = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                    rigidbody.useGravity = false;
                    rigidbody.AddForce(Vector3.up * 1.1f);
                    rigidbody.AddForce(randomDirection * 10f);
                }

                brain.puppetMaster.SetAnimationEnabled((UnityEngine.Random.value > 0.5f));
                brain.behaviour.sfx.pitchMultiplier = UnityEngine.Random.RandomRange(-10, 10);
                brain.behaviour.sfx.Pain(1);
                brain.behaviour.health.TakeDamage(1, attack);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
