using TheHardestMod;
using TheHardestMod.ObjectExtensions;
using UnityEngine;
using static UnityEngine.GridBrushBase;


namespace TheHardestMod.Npcs
{
    internal class HarbingerNPC : NPC
    {

        [SerializeField]
        public PropagatedAudioManager AudMan;




        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new Harbinger_Chase(this));
            navigationStateMachine.ChangeState(new NavigationState_Disabled(this));
            this.Navigator.SetSpeed(1);
            this.spriteRenderer[0].gameObject.layer = LayerMask.NameToLayer("Overlay");

        }
    }

    internal class Harbinger_StateBase(HarbingerNPC Harbinger) : NpcState(Harbinger)
    {

        protected HarbingerNPC pand = Harbinger;
        

    }
    internal class Harbinger_Chase(HarbingerNPC Harbinger) : Harbinger_StateBase(Harbinger)
    {
        protected float speed = 0f;
        protected float SpeedToGo = 150f;
        protected float timeBeforeChase = 1f;
        protected bool saw = false;
        private HarbingerNPC HarbingerPC = Harbinger;
        public override void Enter()
        {
            base.Enter();
            
            SpeedToGo = 40f;
            HarbingerPC.AudMan.QueueAudio(TheHardestMod.MainClass.Instance.Snd_HarbingerComing);
            HarbingerPC.AudMan.SetLoop(true);
            if ((Singleton<ModifiersCategorySettings>.Instance.c && !Singleton<ModifiersCategorySettings>.Instance.a) || Singleton<ModifiersCategorySettings>.Instance.e) {
                HarbingerPC.Despawn();
            }
            

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


                    if (timeBeforeChase < 0) {
                HarbingerPC.transform.position += direction * speed * Time.deltaTime;
                speed += Time.deltaTime * 1f;
                    } else {
                        speed = 0f;
                    }



                    if (distance < 30 && Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.Frozen) {
                        HarbingerPC.behaviorStateMachine.ChangeState(new Harbinger_Minigame(HarbingerPC));
                    }

                }

        public override void OnStateTriggerEnter(Collider other, bool validCollision)
                {
                    base.OnStateTriggerEnter(other,validCollision);
                    if (other.gameObject.GetComponent<PlayerManager>() != null) {
                        Singleton<BaseGameManager>.Instance.Ec.GetBaldi().CaughtPlayer(Singleton<CoreGameManager>.Instance.GetPlayer(0));

                    }

                }



    }


    internal class Harbinger_Minigame(HarbingerNPC Harbinger) : Harbinger_StateBase(Harbinger) {
        protected float speed = 1f;
        private HarbingerNPC HarbingerPC = Harbinger;
        private HarbingerMinigame PanMini;
        private bool caught = false;
        public override void Enter()
        {
            base.Enter();
            
            

            SoundObject[] randomMus = [MainClass.Instance.Snd_mus_Minigame_Pand, MainClass.Instance.Snd_mus_Minigame_Pand2];
            Singleton<CoreGameManager>.Instance.SetLives(3, true);

            PanMini = Singleton<BaseGameManager>.Instance.gameObject.AddComponent<HarbingerMinigame>();

        }

        public override void Update()
                {
                    base.Update();
                      
                    
                    
                    if (!Singleton<CoreGameManager>.Instance.GetPlayer(0).plm.Entity.Frozen) {
                        speed = 250;
                    } else speed = 0;

                    if (PanMini.done) {
                        PanMini.Stop();
                        Singleton<CoreGameManager>.Instance.audMan.audioDevice.Stop();
                        HarbingerPC.Despawn();
                        Singleton<CoreGameManager>.Instance.AddPoints(150,0,true,true, true);
                    }
                    if (PanMini.failure && !caught) {
                        Singleton<BaseGameManager>.Instance.Ec.GetBaldi().CaughtPlayer(Singleton<CoreGameManager>.Instance.GetPlayer(0));
                        caught = true;
                        PanMini.Stop();
                    }

                }

        public override void OnStateTriggerEnter(Collider other, bool validCollision)
                {
            base.OnStateTriggerEnter(other, validCollision);
                    if (other.gameObject.GetComponent<PlayerManager>() != null) {
                        Singleton<BaseGameManager>.Instance.Ec.GetBaldi().CaughtPlayer(Singleton<CoreGameManager>.Instance.GetPlayer(0));
                        PanMini.Stop();
                    }

                }



    }
}