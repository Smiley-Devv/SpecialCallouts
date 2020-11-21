﻿using System;
using Rage;
using Rage.Native;
using LSPD_First_Response.Mod.Callouts;
using LSPD_First_Response.Mod.API;
using System.Drawing;

namespace UnitedCallouts.Callouts
{
    [CalloutInfo("RobberyHL", CalloutProbability.Medium)]
    class RobberyHL : Callout
    {
        private Ped A1;
        private Ped A2;
        private Ped A3;
        private Ped A6;
        private Ped A7;
        private Ped A8;
        private Ped A9;
        private Blip B1;
        private Blip B2;
        private Blip B3;
        private Blip B6;
        private Blip B7;
        private Blip B8;
        private Blip B9;
        private Ped SWAT1;
        private Ped SWAT2;
        private Ped SWAT3;
        private Vehicle PoliceRiot;
        private Vector3 PoliceRiotSpawn = new Vector3(3635.675f, 3774.846f, 28.51558f);
        private Vector3 spawnPoint;
        private Vector3 SWAT1Spawn = new Vector3(3631.826f, 3774.041f, 28.51571f);
        private Vector3 SWAT2Spawn = new Vector3(3632.776f, 3772.018f, 28.51571f);
        private Vector3 SWAT3Spawn = new Vector3(3633.925f, 3768.781f, 28.51571f);
        private Vector3 JoinSwatVector;
        private bool JoinSWAT = false;
        private bool pursuitCreated = false;
        private LHandle pursuit;

        public override bool OnBeforeCalloutDisplayed()
        {
            spawnPoint = new Vector3(3616.228f, 3737.619f, 28.6901f);
            ShowCalloutAreaBlipBeforeAccepting(spawnPoint, 70f);
            AddMinimumDistanceCheck(10f, spawnPoint);
            CalloutMessage = "~b~Dispatch:~s~ Robbery at Humane Labs and Research";
            CalloutPosition = spawnPoint;
            Functions.PlayScannerAudioUsingPosition("ATTENTION_01 crime_robbery_with_a_firearm IN_OR_ON_POSITION", spawnPoint);
            return base.OnBeforeCalloutDisplayed();
        }

        public override bool OnCalloutAccepted()
        {
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~UnitedCallouts", "~y~Robbery at Human Labs and Research", "~b~Dispatch: ~w~The wanted Robbers are marked! Respond with ~r~Code 3");

            A1 = new Ped("g_m_m_chemwork_01", spawnPoint, 0f);
            A2 = new Ped("g_m_m_chemwork_01", spawnPoint, 0f);
            A3 = new Ped("g_m_m_chemwork_01", spawnPoint, 0f);
            A6 = new Ped("g_m_m_chemwork_01", spawnPoint, 0f);
            A7 = new Ped("g_m_m_chemwork_01", spawnPoint, 0f);
            A8 = new Ped("u_m_y_juggernaut_01", spawnPoint, 0f);
            A9 = new Ped("g_m_m_chemwork_01", spawnPoint, 0f);

            A8.CanRagdoll = false;
            NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(A8, false);
            NativeFunction.Natives.SetPedPathCanUseClimbovers(A8, true);
            Functions.SetPedCantBeArrestedByPlayer(A8, true);

            PoliceRiot = new Vehicle("RIOT", PoliceRiotSpawn, 0f);
            PoliceRiot.Heading = -174f;

            SWAT1 = new Ped("s_m_y_swat_01", SWAT1Spawn, 94f);
            SWAT2 = new Ped("s_m_y_swat_01", SWAT2Spawn, 94f);
            SWAT3 = new Ped("s_m_y_swat_01", SWAT3Spawn, 94f);

            SWAT1.Inventory.GiveNewWeapon(WeaponHash.CarbineRifle, 500, true);
            SWAT2.Inventory.GiveNewWeapon(WeaponHash.CarbineRifle, 500, true);
            SWAT3.Inventory.GiveNewWeapon(WeaponHash.CarbineRifle, 500, true);

            A1.IsPersistent = true;
            A2.IsPersistent = true;
            A3.IsPersistent = true;
            A6.IsPersistent = true;
            A7.IsPersistent = true;
            A8.IsPersistent = true;
            A9.IsPersistent = true;

            A1.BlockPermanentEvents = true;
            A2.BlockPermanentEvents = true;
            A3.BlockPermanentEvents = true;
            A6.BlockPermanentEvents = true;
            A7.BlockPermanentEvents = true;
            A8.BlockPermanentEvents = true;
            A9.BlockPermanentEvents = true;

            A1.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;
            A2.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;
            A3.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;
            A6.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;
            A7.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;
            A8.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;
            A9.RelationshipGroup = RelationshipGroup.AggressiveInvestigate;

            A1.Inventory.GiveNewWeapon("WEAPON_SMG", 5000, true);
            A2.Inventory.GiveNewWeapon("WEAPON_PISTOL", 5000, true);
            A3.Inventory.GiveNewWeapon("WEAPON_CARBINERIFLE", 5000, true);
            A6.Inventory.GiveNewWeapon("WEAPON_CARBINERIFLE", 5000, true);
            A7.Inventory.GiveNewWeapon("WEAPON_SMG", 5000, true);
            A8.Inventory.GiveNewWeapon("WEAPON_MINIGUN", 5000, true);
            A9.Inventory.GiveNewWeapon("WEAPON_SMG", 5000, true);

            A1.Armor = 400;
            A2.Armor = 300;
            A8.Armor = 1000;
            SWAT1.Armor = 200;
            SWAT2.Armor = 200;
            SWAT3.Armor = 200;

            B1 = A1.AttachBlip();
            B2 = A2.AttachBlip();
            B3 = A3.AttachBlip();
            B6 = A6.AttachBlip();
            B7 = A7.AttachBlip();
            B8 = A8.AttachBlip();
            B9 = A9.AttachBlip();
            B1.EnableRoute(Color.Yellow);
            return base.OnCalloutAccepted();
        }

        public override void OnCalloutNotAccepted()
        {
            if (B1.Exists()) B1.Delete();
            if (B2.Exists()) B2.Delete();
            if (B3.Exists()) B3.Delete();
            if (B6.Exists()) B6.Delete();
            if (B7.Exists()) B7.Delete();
            if (B8.Exists()) B8.Delete();
            if (B9.Exists()) B9.Delete();
            if (A1.Exists()) A1.Delete();
            if (A2.Exists()) A2.Delete();
            if (A3.Exists()) A3.Delete();
            if (A6.Exists()) A6.Delete();
            if (A7.Exists()) A7.Delete();
            if (A8.Exists()) A8.Delete();
            if (A9.Exists()) A9.Delete();
            if (PoliceRiot.Exists()) { PoliceRiot.Delete(); }
            if (SWAT1.Exists()) { SWAT1.Delete(); }
            if (SWAT2.Exists()) { SWAT2.Delete(); }
            if (SWAT3.Exists()) { SWAT3.Delete(); }
            base.OnCalloutNotAccepted();
        }

        public override void Process()
        {
            GameFiber.StartNew(delegate
            {
                if (Game.LocalPlayer.Character.DistanceTo(PoliceRiot.Position) < 10f)
                {
                    SWAT1.Tasks.FollowToOffsetFromEntity(Game.LocalPlayer.Character, new Vector3(-1.5f, -1.5f, 0f));
                    SWAT2.Tasks.FollowToOffsetFromEntity(Game.LocalPlayer.Character, new Vector3(-1.5f, -1.5f, 0f));
                    SWAT3.Tasks.FollowToOffsetFromEntity(Game.LocalPlayer.Character, new Vector3(-1.5f, -1.5f, 0f));
                }
                if (Game.LocalPlayer.Character.DistanceTo(PoliceRiot.Position) < 10f)
                {
                    Game.DisplayHelp("~y~The SWAT-Team~w~ is going to support you against the robbers.", 5000);
                }
                if (Game.LocalPlayer.Character.Position.DistanceTo(spawnPoint) < 22f)
                {
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A1, Game.LocalPlayer.Character, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A2, Game.LocalPlayer.Character, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A3, Game.LocalPlayer.Character, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A6, Game.LocalPlayer.Character, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A7, Game.LocalPlayer.Character, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A8, Game.LocalPlayer.Character, 0, 1);
                    NativeFunction.CallByName<uint>("TASK_COMBAT_PED", A9, Game.LocalPlayer.Character, 0, 1);

                    A1.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    A2.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    A3.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    A6.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    A7.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    A8.Tasks.FightAgainst(Game.LocalPlayer.Character);
                    A9.Tasks.FightAgainst(Game.LocalPlayer.Character);

                    if (!JoinSWAT)
                    {
                        SWAT1.Position = Game.LocalPlayer.Character.Position; ;
                        SWAT2.Position = Game.LocalPlayer.Character.Position; ;
                        SWAT3.Position = Game.LocalPlayer.Character.Position; ;
                        JoinSWAT = true;
                    }
                }
                if (Game.LocalPlayer.Character.IsDead) End();
                if (Game.IsKeyDown(Settings.EndCall)) End();
                if (A1.IsDead && A1.IsDead && A2.IsDead && A3.IsDead && A6.IsDead && A7.IsDead && A8.IsDead && A9.IsDead) End();
                if (Functions.IsPedArrested(A1) && Functions.IsPedArrested(A2) && Functions.IsPedArrested(A3) && Functions.IsPedArrested(A6) && Functions.IsPedArrested(A7) && Functions.IsPedArrested(A8) && Functions.IsPedArrested(A9)) End();
            }, "RobberHL [UnitedCallouts]");
            base.Process();
        }

        public override void End()
        {
            if (B1.Exists()) B1.Delete();
            if (B2.Exists()) B2.Delete();
            if (B3.Exists()) B3.Delete();
            if (B6.Exists()) B6.Delete();
            if (B7.Exists()) B7.Delete();
            if (B8.Exists()) B8.Delete();
            if (B9.Exists()) B9.Delete();
            if (A1.Exists()) A1.Dismiss();
            if (A2.Exists()) A2.Dismiss();
            if (A3.Exists()) A3.Dismiss();
            if (A6.Exists()) A6.Dismiss();
            if (A7.Exists()) A7.Dismiss();
            if (A8.Exists()) A8.Dismiss();
            if (A9.Exists()) A9.Dismiss();
            if (SWAT1.Exists()) { SWAT1.Dismiss(); }
            if (SWAT2.Exists()) { SWAT2.Dismiss(); }
            if (SWAT3.Exists()) { SWAT3.Dismiss(); }
            if (PoliceRiot.Exists()) { PoliceRiot.Dismiss(); }
            Game.DisplayNotification("web_lossantospolicedept", "web_lossantospolicedept", "~w~UnitedCallouts", "~y~Robbery at Human Labs and Research", "~b~You: ~w~Dispatch we're code 4. Show me ~g~10-8.");
            Functions.PlayScannerAudio("ATTENTION_THIS_IS_DISPATCH_HIGH WE_ARE_CODE FOUR NO_FURTHER_UNITS_REQUIRED");
            base.End();
        }
    }
}