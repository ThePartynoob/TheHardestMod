using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TheHardestMod.Npcs
{
    internal class SubspaceTripmine : NPC
    {
        [SerializeField]
        public PropagatedAudioManager AudMan;

        public override void Initialize()
        {
            base.Initialize();
            behaviorStateMachine.ChangeState(new SubspaceTripmine_Appear(this));
        }
    }

    internal class SubspaceTripmine_Base(SubspaceTripmine St) : NpcState(St)
    {
        protected SubspaceTripmine SubTrip = St;
    }

    internal class SubspaceTripmine_Appear(SubspaceTripmine St) : SubspaceTripmine_Base(St)
    {
        protected float TimeleftbeforeDisseapear = 6;
        public override void Enter()
        {
            base.Enter();
            St.AudMan.PlaySingle(MainClass.Instance.Snd_SubMine_Activate);
            
        }
        public override void Update()
        {
            base.Update();
            TimeleftbeforeDisseapear -= Time.deltaTime;
            St.spriteRenderer[0].color = Color.Lerp(St.spriteRenderer[0].color, new Color(0, 0, 0, 0), 0.08f * Time.deltaTime);
            if (TimeleftbeforeDisseapear < 0)
            {
                St.behaviorStateMachine.ChangeState(new SubspaceTripmine_Await(St));
                St.spriteRenderer[0].color = new Color(0, 0, 0, 0);
            }
        }
    }

    internal class SubspaceTripmine_Await(SubspaceTripmine St) : SubspaceTripmine_Base(St)
    {

        public override void Enter()
        {
            base.Enter();
            
        }
        public override void Update()
        {
            base.Update();
            var distance = (St.transform.position - Singleton<CoreGameManager>.Instance.GetPlayer(0).transform.position).magnitude;
            if (distance < 15) St.behaviorStateMachine.ChangeState(new SubspaceTripmine_Explode(St));
        }
    }

    internal class SubspaceTripmine_Explode(SubspaceTripmine St) : SubspaceTripmine_Base(St)
    {
        protected TimeScaleModifier tsm;
        protected float timeleft = 5;
        public override void Enter()
        {
            base.Enter();
            St.AudMan.PlaySingle(MainClass.Instance.Snd_SubMine_Explode);
            tsm = new TimeScaleModifier(1, 1, 0);
            Singleton<CoreGameManager>.Instance.GetComponent<ScoreManager>().AddScore(-450f, true, true, "Got caught by a subspace tripmine");

            Singleton<CoreGameManager>.Instance.GetPlayer(0).AddTimeScale(tsm);
            St.spriteRenderer[0].color = new Color(1, 1, 1, 1);
        }
        public override void Update()
        {
            base.Update();
            timeleft -= Time.deltaTime;
            if (timeleft < 0)
            {
                Singleton<CoreGameManager>.Instance.GetPlayer(0).RemoveTimeScale(tsm);
                St.Despawn();
            }
            
        }
    }
}
