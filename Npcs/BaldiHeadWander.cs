using UnityEngine;
using TheHardestMod.ObjectExtensions;
using TheHardestMod;


namespace TheHardestMod.Npcs
{
    internal class BaldiHeadWanderNPC : NPC
    {

        [SerializeField]
        public PropagatedAudioManager AudMan;




        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new BaldiHeadWand_Chase(this));
            navigationStateMachine.ChangeState(new NavigationState_DoNothing(this,0));
            this.Navigator.SetSpeed(20);
            this.spriteRenderer[0].gameObject.layer = LayerMask.NameToLayer("Overlay");

        }
    }

    internal class BaldiHeadWand_StateBase(BaldiHeadWanderNPC baldihead) : NpcState(baldihead)
    {

        protected BaldiHeadWanderNPC pand = baldihead;
        

    }
    internal class BaldiHeadWand_Chase(BaldiHeadWanderNPC Harbinger) : BaldiHeadWand_StateBase(Harbinger)
    {
        protected float speed = 25f;
        protected float SpeedToGo = 150f;
        protected float timeBeforeChase = 1f;
        protected bool saw = false;
        private BaldiHeadWanderNPC HarbingerPC = Harbinger;
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
            HarbingerPC.Navigator.SetSpeed(speed);
            speed += Time.deltaTime / 30;
            HarbingerPC.navigationStateMachine.ChangeState(new NavigationState_TargetPlayer(HarbingerPC, 99, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position));



                    

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