#nullable enable


using UnityEngine;
using System;
using TMPro;
using MTM101BaldAPI.Components;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI;
using TheHardestMod.ObjectExtensions;
namespace TheHardestMod
{
    public class RandomEffectManager : MonoBehaviour
    {
        internal PlayerManager? pm = Singleton<CoreGameManager>.Instance.GetPlayer(0);
        internal string[] Effects = [
        "BaldiTempAnger", "PlayerSpeedBoost", "NegativeStamina","UnlimitedStamina", "Blinded"
        ,"Frozen", "HappyBaldi"];
        internal string ChoosenEffect = "";
        internal bool PlayedDrinkingSound = false;
        internal float DrinkingTimer = 2.5f;
        private Fog BlindnessFog = new Fog();
        float timer = -1f;
        ValueModifier RunWalkSpeedModifier = new ValueModifier(1f, 25f);
        void Start()
        {
            
            if (pm != null)
            {

                ChoosenEffect = Effects[UnityEngine.Random.Range(0, Effects.Length)];
                switch (ChoosenEffect)
                {
                    case "BaldiTempAnger":
                        {
                            timer = 20;
                            Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(timer);
                            
                            break;
                        }
                    case "PlayerSpeedBoost":
                        {
                            timer = 25;
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).GetMovementStatModifier().AddModifier("runSpeed", RunWalkSpeedModifier);
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).GetMovementStatModifier().AddModifier("walkSpeed", RunWalkSpeedModifier);
                            break;
                        }
                    case "NegativeStamina":
                        {
                            timer = 0;
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.stamina = -100;
                            break;
                        }
                    case "UnlimitedStamina":
                        {
                            timer = 100;
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.stamina = 100;
                            break;
                        }
                    case "Blinded":
                        {
                            BlindnessFog.color = Color.black;
                            BlindnessFog.startDist = 5;
                            BlindnessFog.maxDist = 5000;
                            BlindnessFog.strength = 0;
                            Singleton<BaseGameManager>.Instance.Ec.AddFog(BlindnessFog);
                            timer = 20;
                            break;
                        }
                    case "HappyBaldi":
                        {
                            timer = 0;
                            Singleton<BaseGameManager>.Instance.Ec.GetBaldi().Praise(6,false);
                            break;
                        }
                    case "Frozen":
                        {
                            timer = 25;
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).GetMovementStatModifier().AddModifier("runSpeed", RunWalkSpeedModifier);
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).GetMovementStatModifier().AddModifier("walkSpeed", RunWalkSpeedModifier);
                            break;
                        }
                }

            }
            else
            {
                Destroy(this);
            }
        }
        void Update()
        {
            if (timer != -1f)
            {
            	
                timer -= Time.deltaTime;

                switch (ChoosenEffect)
                {
                    case "BaldiTempAnger":
                        {
                            Singleton<BaseGameManager>.Instance.Ec.GetBaldi().GetAngry(-Time.deltaTime);
                            break;
                        }
                    case "PlayerSpeedBoost":
                        {
                            RunWalkSpeedModifier.addend = timer * 5;
                            RunWalkSpeedModifier.multiplier = 1;

                            break;
                        }
                    case "UnlimitedStamina":
                        {
                            Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.stamina = timer;

                            break;
                        }
                    case "Blinded":
                        {
                            BlindnessFog.maxDist = Mathf.Lerp(BlindnessFog.maxDist, 30, 0.01f);
                            BlindnessFog.strength = Mathf.Lerp(BlindnessFog.strength, 2, 0.01f);;
                            Singleton<BaseGameManager>.Instance.Ec.UpdateFog();
                            break;
                        }
                    case "Frozen":
                        {
                            RunWalkSpeedModifier.addend = 0;
                            RunWalkSpeedModifier.multiplier = -timer / 25 + 1f;
                            break;
                        }
                }
                if (timer <= 0)
                {
                    // reset functions depending current effect
                    switch (ChoosenEffect)
                    {
                        case "BaldiTempAnger":
                            {

                                break;
                            }
                        case "PlayerSpeedBoost":
                            {
                                Singleton<CoreGameManager>.Instance.GetPlayer(0).GetMovementStatModifier().RemoveModifier(RunWalkSpeedModifier);

                                break;
                            }
                        case "Frozen":
                            {
                                Singleton<CoreGameManager>.Instance.GetPlayer(0).GetMovementStatModifier().RemoveModifier(RunWalkSpeedModifier);

                                break;
                            }
                        case "UnlimitedStamina":
                            {


                                break;
                            }
                        case "Blinded":
                            {
                                Singleton<BaseGameManager>.Instance.Ec.RemoveFog(BlindnessFog);
                                Singleton<BaseGameManager>.Instance.Ec.FlickerLights(false);
                                Singleton<BaseGameManager>.Instance.Ec.UpdateFog();
                                break;

                            }
                    }




                    Destroy(this);
                }
            
            }
        }

    }




    public class ITM_RandomEffect : Item
    {
        public override bool Use(PlayerManager Pm)
        {
            
            if (Singleton<BaseGameManager>.Instance.Ec.GetBaldi() != null)
            {
                var e = Pm.gameObject.AddComponent<RandomEffectManager>();
                e.ChoosenEffect = e.Effects[UnityEngine.Random.Range(0, e.Effects.Length)];
                this.GetComponent<AudioManager>().PlaySingle(TheHardestMod.MainClass.Instance.Snd_Sfx_RandomEffect_Drink);

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
