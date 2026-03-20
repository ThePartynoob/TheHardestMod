using UnityEngine;
using TheHardestMod.ObjectExtensions;
using TheHardestMod;


namespace TheHardestMod.Npcs
{
    internal class PandemoniumWanderNPC : NPC
    {

        [SerializeField]
        public PropagatedAudioManager AudMan;




        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new PandemoniumW_Chase(this));
            
            this.Navigator.SetSpeed(1);
            this.ec.map.AddArrow(this.Navigator.Entity, Color.black);

        }
    }

    internal class PandemoniumW_StateBase(PandemoniumWanderNPC pandemonium) : NpcState(pandemonium)
    {

        protected PandemoniumWanderNPC pand = pandemonium;
        

    }
    internal class PandemoniumW_Chase(PandemoniumWanderNPC pandemonium) : PandemoniumW_StateBase(pandemonium)
    {
        protected float speed = 0f;
        protected float SpeedToGo = 40f;
        protected float timeBeforeChase = 0f;
        protected float SafeTime = 3f;
        protected bool saw = false;
        protected bool deathInline = false;
        private PandemoniumWanderNPC pandemoniumPC = pandemonium;
        public override void Enter()
        {
            base.Enter();
            SpeedToGo = 25f;
            pandemoniumPC.AudMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_Sfx_Pandemonium_Moving);
            pandemoniumPC.AudMan.SetLoop(true);
            ChangeNavigationState(new NavigationState_WanderRandom(pandemoniumPC, 99));
            

        }

        public override void PlayerInSight(PlayerManager player)
                {
            saw = true;
                }
        public override void PlayerLost(PlayerManager player)
        {
            base.PlayerLost(player);
            saw = false;
        }

        public override void Update()
                {
                    base.Update();
                    
                    pandemoniumPC.Navigator.SetSpeed(speed);
                    timeBeforeChase -= Time.deltaTime;
                    var distance = (pandemoniumPC.transform.position - Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position).magnitude;
                    if (timeBeforeChase < 0) {
                        speed = SpeedToGo;
                    } else {
                        speed = 0f;
                    }
            if (saw) SafeTime -= Time.deltaTime;
            if (!saw) SafeTime = 3f;
            if (SafeTime <= 0 && !deathInline)
            {
                ChangeNavigationState(new NavigationState_TargetPlayer(pandemoniumPC, 9999, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position, true));
                deathInline = true;
                pandemoniumPC.AudMan.FlushQueue(true);
                pandemoniumPC.AudMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_Sfx_Pandemonium_Scream);
                SpeedToGo = 40;
                pandemoniumPC.AudMan.SetLoop(true);
                pand.spriteRenderer[0].gameObject.layer = LayerMask.NameToLayer("Overlay");
            } 
            if (deathInline)
            {
                SpeedToGo += Time.deltaTime * 2;
                ChangeNavigationState(new NavigationState_TargetPlayer(pandemoniumPC, 9999, Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position, true));
            }


                    

                }

        public override void OnStateTriggerEnter(Collider other, bool validCollision)
                {
                    base.OnStateTriggerEnter(other, validCollision);
                    if (other.gameObject.GetComponent<PlayerManager>() != null) {
                var a = pandemoniumPC.gameObject.AddComponent<Baldi>();
                a.loseSounds = [new WeightedSoundObject {
                    selection = MainClass.Instance.Snd_SubMine_Explode,
                    weight = 999
                }];
                a.CaughtPlayer(other.gameObject.GetComponent<PlayerManager>());
                    }

                }



    }


   
}