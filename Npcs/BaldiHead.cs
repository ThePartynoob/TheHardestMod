using UnityEngine;
using TheHardestMod.ObjectExtensions;
using TheHardestMod;
using TheHardestMod.Others;


namespace TheHardestMod.Npcs
{
    internal class BaldiHeadNPC : NPC
    {

        [SerializeField]
        public PropagatedAudioManager AudMan;




        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new BaldiHead_Chase(this));
            navigationStateMachine.ChangeState(new NavigationState_Disabled(this));
            this.Navigator.SetSpeed(1);
            this.spriteRenderer[0].gameObject.layer = LayerMask.NameToLayer("Overlay");

        }
    }

    internal class BaldiHead_StateBase(BaldiHeadNPC baldihead) : NpcState(baldihead)
    {

        protected BaldiHeadNPC pand = baldihead;
        

    }
    internal class BaldiHead_Chase(BaldiHeadNPC Harbinger) : BaldiHead_StateBase(Harbinger)
    {
        protected float speed = 0f;
        protected float SpeedToGo = 150f;
        protected float timeBeforeChase = 1f;
        protected bool saw = false;
        private BaldiHeadNPC HarbingerPC = Harbinger;
        public override void Enter()
        {
            base.Enter();
            
            SpeedToGo = 40f;
            HarbingerPC.AudMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_HarbingerComing);
            HarbingerPC.AudMan.SetLoop(true);
            
            

        }

        public override void PlayerInSight(PlayerManager player)
                {
                    if (!saw) {
                        saw = true;
                        
                        SpeedToGo = 300;
                        
                    }
                }

        public override void Update()
                {
                    base.Update();
                     
                    
                    timeBeforeChase -= Time.deltaTime;
                    var distance = (HarbingerPC.transform.position - Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position).magnitude;
            var direction = (Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position - HarbingerPC.transform.position).normalized;

            if (Singleton<BaseGameManager>.Instance is MainGameManager)
            {
                if (timeBeforeChase < 0)
                {
                    HarbingerPC.transform.position += direction * speed * Time.deltaTime;
                    speed += Time.deltaTime / 15f;
                }
                else
                {
                    speed = 6f;
                }
            } else  if (Singleton<BaseGameManager>.Instance is CusChallengeGameManager)
            {
                if (timeBeforeChase < 0)
                {
                    HarbingerPC.transform.position += direction * speed * Time.deltaTime;
                    speed += Time.deltaTime / 30f;
                }
                else
                {
                    speed = 6f;
                }
            }
                    



                    

                }

        public override void OnStateTriggerEnter(Collider other, bool validCollision)
                {
                    base.OnStateTriggerEnter(other, validCollision);
                    if (other.gameObject.GetComponent<PlayerManager>() != null) {
                var a = HarbingerPC.gameObject.AddComponent<Baldi>();
                a.loseSounds = [new WeightedSoundObject {
                    selection = MainClass.Instance.Snd_SubMine_Explode,
                    weight = 999
                }];
                a.CaughtPlayer(Singleton<CoreGameManager>.Instance.GetPlayer(0));

            }

                }



    }


    
}