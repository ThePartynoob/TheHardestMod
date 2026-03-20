using MTM101BaldAPI;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace TheHardestMod.Events
{

    public class pandemoniumEvent : RandomEvent
    {
        RoomController CurrentRoom;
        bool isHarbinger = false;
        List<GameObject> Helpers = new();
        public override void Begin()
        {
            if (!isHarbinger)
            {
                Singleton<BaseGameManager>.Instance.Ec.SpawnNPC(TheHardestMod.MainClass.Instance.Pandemonium, CurrentRoom.AllEntitySafeCellsNoGarbage()[15].position);
            } else
            {
                Singleton<CoreGameManager>.Instance.audMan.PlaySingle(MainClass.Instance.Snd_HarbingerMusic);
                Singleton<BaseGameManager>.Instance.Ec.SpawnNPC(TheHardestMod.MainClass.Instance.Harbinger, CurrentRoom.AllEntitySafeCellsNoGarbage()[15].position);
            }

            foreach (Door Door in CurrentRoom.doors) 
            {
                Door.Block(false);
            }
            var e = FindObjectsOfType<HideableLocker>();

            foreach (var item in e)
            {
                var ee = new GameObject("BlueLockerIndicator");
                var em = new GameObject("renderer");
                em.layer = LayerMask.NameToLayer("Billboard");
                ee.layer = LayerMask.NameToLayer("Overlay");
                em.transform.parent = ee.transform;
                var spr = em.AddComponent<SpriteRenderer>();
                spr.sprite = MainClass.Instance.assetManager.Get<Sprite>("spr_helpHide");
                em.transform.localPosition = Vector3.zero;
                ee.transform.SetParent(item.transform);
                ee.transform.position = item.transform.position + new Vector3(0, 7, 0);
                Helpers.Add(ee);
            }
            StartCoroutine(FadeoutAll());



            base.Begin();
            
        }

        IEnumerator FadeoutAll()
        {
            yield return null;
            yield return new WaitForSeconds(15);
            foreach (var item in Helpers)
            {
                Destroy(item);
            }
            Helpers = new();
        }

        public override void AssignRoom(RoomController room)
        {
            base.AssignRoom(room);
            CurrentRoom = room;
            foreach (Door Door in CurrentRoom.doors) 
            {
                Door.Block(true);
            }
            
                    
                    
        }

        public override void Initialize(EnvironmentController controller, Random rng)
        {
            base.Initialize(controller, rng);
            SoundObject[] rndsnd = [MainClass.Instance.Snd_pd1, MainClass.Instance.Snd_pd2, MainClass.Instance.Snd_pd3];
            this.eventIntro = rndsnd[UnityEngine.Random.Range(0, 3)];
            if (UnityEngine.Random.Range(1, 100) == 1 && !ModifiersCategorySettings.Instance.k)
            {
                isHarbinger = true;
                this.eventIntro = MainClass.Instance.Snd_Har1;
            }
            else if (UnityEngine.Random.Range(1, 2) == 1 && ModifiersCategorySettings.Instance.k)
            {
                this.eventIntro = MainClass.Instance.Snd_Har1;
                isHarbinger = true;
            }
        }
    }
}