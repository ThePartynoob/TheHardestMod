using MTM101BaldAPI.OptionsAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;

namespace TheHardestMod
{
    public class ModifiersCategorySettings : Singleton<ModifiersCategorySettings> {
        public bool a = false;
        public bool b = false;
        public bool c = false;
        public bool e = false;
        public bool g = false;
        public bool h = false;
        public bool i = false;
        public bool k = false;
        public int d = 0;
        public int f = 0;



    }


    public class ModifiersCategory : CustomOptionsCategory
    {

        MenuToggle modA;
        MenuToggle modB;
        MenuToggle modC;
        MenuToggle modE;
        MenuToggle modG;
        MenuToggle modH;
        MenuToggle modI;
        MenuToggle modK;

        AdjustmentBars modD;
        AdjustmentBars modF;

	   public override void Build()
        {
            

            // Insane modifier
            modA = CreateNewModifier("InsaneMode", "Insane mode", "mod_Insane",0);
            // one slot
            modB = CreateNewModifier("OneSlot", "One item only", "mod_OneItemSlot",1);
            // oo like speedrunning
            modC = CreateNewModifier("Speedrun", "Speedrun time", "mod_Speedrun",2);

            modE = CreateNewModifier("NoPande", "No pandemonium", "mod_noPande",3);



            modD = CreateNewModifierBar("Laps","Laps","mod_laps",4,4,Color.black);

            modF = CreateNewModifierBar("SpeedNpc","Speed of npcs","mod_sbNPCS",5,16,Color.red);


            

            modH = CreateNewModifier("Blind", "Blinded", "mod_blind", 6);

            modI = CreateNewModifier("broken metal pipes :(", "broken metal pipes", "mod_nopipes", 7);

            modK = CreateNewModifier("HarbingerBuff", "Buffed unfariness", "mod_harbingerbuff", 8);


            CreateApplyButton(() => SendData());
            
            modA.Set(ModifiersCategorySettings.Instance.a);
            modB.Set(ModifiersCategorySettings.Instance.b);
            modC.Set(ModifiersCategorySettings.Instance.c);
            modE.Set(ModifiersCategorySettings.Instance.e);
            
            modH.Set(ModifiersCategorySettings.Instance.h);
            modI.Set(ModifiersCategorySettings.Instance.i);
            modK.Set(ModifiersCategorySettings.Instance.k);
            modD.Adjust(ModifiersCategorySettings.Instance.d);
            modF.Adjust(ModifiersCategorySettings.Instance.f);


        }
        public void SendData() {
            ModifiersCategorySettings.Instance.a = modA.Value;
            ModifiersCategorySettings.Instance.b = modB.Value;
            ModifiersCategorySettings.Instance.c = modC.Value;
            ModifiersCategorySettings.Instance.d = (int)modD.ReflectionGetVariable("val");
            ModifiersCategorySettings.Instance.e = modE.Value;
            ModifiersCategorySettings.Instance.f = (int)modF.ReflectionGetVariable("val");
            
            ModifiersCategorySettings.Instance.h = modH.Value;
            ModifiersCategorySettings.Instance.i = modI.Value;
            ModifiersCategorySettings.Instance.k = modK.Value;



        }

        public MenuToggle CreateNewModifier(string id, string name,string tooltipKey,int order) {
            Vector3 originVec = new Vector3(40f,85f,0f);
            var toggle = CreateToggle(id, name, false,originVec + new Vector3(0,order*-29.5f,0),250f);
            AddTooltip(toggle,tooltipKey);
            
            return toggle;
        }

        public AdjustmentBars CreateNewModifierBar(string id, string name,string tooltipKey,int order, int length, Color textColor) {
            Vector3 originVec = new Vector3(40f,85f,0f);
            var bar = CreateBars(() => {
                
            },id, originVec + new Vector3(-30,order*-29.5f,0), length);
            CreateText(id + "_text", name, originVec + new Vector3(-125,order*-28,0),MTM101BaldAPI.UI.BaldiFonts.ComicSans24,TextAlignmentOptions.Left, new Vector2(250,10),textColor,false);
            AddTooltip(bar,tooltipKey);
            
            return bar;
        }



    }
}