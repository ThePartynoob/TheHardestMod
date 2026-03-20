using MTM101BaldAPI;


namespace TheHardestMod.Events
{
    internal class SubspaceTripmineEvent : RandomEvent
    {
        
        public override void Begin()
        {
            base.Begin();
            ec.SpawnNPC(MainClass.Instance.SubspaceTripmineNPC, ec.AllTilesNoGarbage(false, false)[UnityEngine.Random.Range(0, ec.AllTilesNoGarbage(false, false).Count)].position);
        }
    }
}
