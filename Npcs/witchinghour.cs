using UnityEngine;
using TheHardestMod.ObjectExtensions;
using TheHardestMod;


namespace TheHardestMod.Npcs
{
    internal class witchinghourNPC : NPC
    {

        [SerializeField]
        public PropagatedAudioManager AudMan;




        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new witchinghour_Chase(this));
            navigationStateMachine.ChangeState(new NavigationState_Disabled(this));
            this.Navigator.SetSpeed(1);
            this.spriteRenderer[0].gameObject.layer = LayerMask.NameToLayer("Overlay");

        }
    }

    internal class witchinghour_StateBase(witchinghourNPC witchinghour) : NpcState(witchinghour)
    {

        protected witchinghourNPC pand = witchinghour;
        

    }
    internal class witchinghour_Chase(witchinghourNPC Harbinger) : witchinghour_StateBase(Harbinger)
    {
        protected float speed = 0f;
        protected float SpeedToGo = 150f;
        protected float timeBeforeChase = 0f;
        protected float distanceleftbeforedeath = 410;
        protected bool saw = false;
        protected bool isdead = false;
        private witchinghourNPC HarbingerPC = Harbinger;
        public override void Enter()
        {
            base.Enter();
            HarbingerPC.navigationStateMachine.ChangeState(new NavigationState_Disabled(HarbingerPC));
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
            


                    if (timeBeforeChase < 0) {
                HarbingerPC.transform.position = Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position + new Vector3(0,0,distanceleftbeforedeath /20.5f + 3); ;
                speed += Time.deltaTime / 15f;
                distanceleftbeforedeath -= Time.deltaTime;
                    } else {
                        speed = 6f;
                    }

                    if (distanceleftbeforedeath < 50 && !isdead)
            {
                isdead = true;
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