using UnityEngine;
using MTM101BaldAPI.PlusExtensions;
using MTM101BaldAPI.Components;

namespace TheHardestMod
{
    internal class MetalPipeManager : MonoBehaviour {
        private TimeScaleModifier tsm = new TimeScaleModifier(0,4,1);
        private float timer = 20f;

        void Start() {
            timer = UnityEngine.Random.Range(10f,20f);
            tsm = new TimeScaleModifier(1f,1,1);
            Singleton<BaseGameManager>.Instance.Ec.AddTimeScale(tsm);
            
        }
        void Update() {
            timer -= Time.deltaTime;
            
            if (timer < 0) {
                Singleton<BaseGameManager>.Instance.Ec.RemoveTimeScale(tsm);
                Destroy(this);
            }
            if (timer < 7) {
                tsm.npcTimeScale = Mathf.Lerp(tsm.npcTimeScale,1,0.004f);
                tsm.playerTimeScale = Mathf.Lerp(tsm.playerTimeScale,1,0.004f);
            } else {
                tsm.npcTimeScale = Mathf.Lerp(tsm.npcTimeScale,0.25f,0.03f);
                tsm.playerTimeScale = Mathf.Lerp(tsm.playerTimeScale,1.5f,0.005f);
            }
        }
    }

    public class ITM_MetalPipe : Item
    {
        internal ItemObject NextItem;


        public override bool Use(PlayerManager pm)
        {

            if (pm.GetComponent<MetalPipeManager>() == null) {
                this.GetComponent<AudioManager>().PlaySingle(TheHardestMod.MainClass.Instance.MetalPipe_Sound);
                if (!ModifiersCategorySettings.Instance.i)
                {
                    pm.gameObject.AddComponent<MetalPipeManager>();
                    if (NextItem != null)
                    {
                        pm.itm.SetItem(NextItem, pm.itm.selectedItem);
                        return false;
                    }
                    else return true;
                }
                else return true;
            } else {
                return false;
            }
            




            
        }
        
    }
}