﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ESPCheat_wh
{
    class Program
    {
        private static Memory mem;
        private static int client_dll;

        static void Main(string[] args)
        {
            while (!GetDll()) { }
            //Console.WriteLine("Module found!");
            // bool flag = false;

            while (true)
            {
                int local_player = mem.Read<int>(client_dll + AdressBase.dwLocalPlayer);
                int player_team = mem.Read<int>(local_player + AdressBase.m_iTeamNum);

                for (int i = 0; i < 64; i++)
                {
                    int current_player = mem.Read<int>(client_dll + AdressBase.dwEntityList + i * 0x10);
                    int cp_glow_index = mem.Read<int>(current_player + AdressBase.m_iGlowIndex);
                    int current_player_team = mem.Read<int>(current_player + AdressBase.m_iTeamNum);
                    float cp_hp = mem.Read<int>(current_player + AdressBase.m_iHealth) / 100f;

                    if (current_player_team != player_team && current_player_team != 0)
                    {
                        DrawGlow(cp_glow_index, cp_hp);
                    }
                }
            }
        }


        private static bool GetDll()
        {
            try
            {
                Process csgo = Process.GetProcessesByName("csgo")[0];
                mem = new Memory("csgo");
                foreach (ProcessModule module in csgo.Modules)
                {
                    if (module.ModuleName == "client_panorama.dll")
                    {
                        client_dll = (int)module.BaseAddress;
                        Console.WriteLine("found!");
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }

        }
        private static void changeRank(int player_rank)
        {
            mem.Write<int>(player_rank, 5);
        }

        private static void ChangeHP(float player_hp)
        {
            mem.Write<int>((int)player_hp, 50);
        }

        private static void DrawGlow(int glow_index, float hp)
        {
            int glow_object = mem.Read<int>(client_dll + AdressBase.dwGlowObjectManager);
            mem.Write<float>((glow_object + (glow_index * 0x38) + 4), 1 - hp);
            mem.Write<float>((glow_object + (glow_index * 0x38) + 8), hp);
            mem.Write<float>((glow_object + (glow_index * 0x38) + 12), 0);
            mem.Write<float>((glow_object + (glow_index * 0x38) + 0x10), 210 / 100f);
            mem.Write<bool>((glow_object + (glow_index * 0x38) + 0x24), true);
            mem.Write<bool>((glow_object + (glow_index * 0x38) + 0x25), false);
        }
    }
    class AdressBase
    {
        public const Int32 cs_gamerules_data = 0x0;
        public const Int32 m_ArmorValue = 0xB368;
        public const Int32 m_Collision = 0x320;
        public const Int32 m_CollisionGroup = 0x474;
        public const Int32 m_Local = 0x2FBC;
        public const Int32 m_MoveType = 0x25C;
        public const Int32 m_OriginalOwnerXuidHigh = 0x31B4;
        public const Int32 m_OriginalOwnerXuidLow = 0x31B0;
        public const Int32 m_SurvivalGameRuleDecisionTypes = 0x1320;
        public const Int32 m_SurvivalRules = 0xCF8;
        public const Int32 m_aimPunchAngle = 0x302C;
        public const Int32 m_aimPunchAngleVel = 0x3038;
        public const Int32 m_angEyeAnglesX = 0xB36C;
        public const Int32 m_angEyeAnglesY = 0xB370;
        public const Int32 m_bBombPlanted = 0x99D;
        public const Int32 m_bFreezePeriod = 0x20;
        public const Int32 m_bGunGameImmunity = 0x3930;
        public const Int32 m_bHasDefuser = 0xB378;
        public const Int32 m_bHasHelmet = 0xB35C;
        public const Int32 m_bInReload = 0x3295;
        public const Int32 m_bIsDefusing = 0x391C;
        public const Int32 m_bIsQueuedMatchmaking = 0x74;
        public const Int32 m_bIsScoped = 0x3914;
        public const Int32 m_bIsValveDS = 0x75;
        public const Int32 m_bSpotted = 0x93D;
        public const Int32 m_bSpottedByMask = 0x980;
        public const Int32 m_bStartedArming = 0x33E0;
        public const Int32 m_bUseCustomAutoExposureMax = 0x9D9;
        public const Int32 m_bUseCustomAutoExposureMin = 0x9D8;
        public const Int32 m_bUseCustomBloomScale = 0x9DA;
        public const Int32 m_clrRender = 0x70;
        public const Int32 m_dwBoneMatrix = 0x26A8;
        public const Int32 m_fAccuracyPenalty = 0x3320;
        public const Int32 m_fFlags = 0x104;
        public const Int32 m_flC4Blow = 0x2990;
        public const Int32 m_flCustomAutoExposureMax = 0x9E0;
        public const Int32 m_flCustomAutoExposureMin = 0x9DC;
        public const Int32 m_flCustomBloomScale = 0x9E4;
        public const Int32 m_flDefuseCountDown = 0x29AC;
        public const Int32 m_flDefuseLength = 0x29A8;
        public const Int32 m_flFallbackWear = 0x31C0;
        public const Int32 m_flFlashDuration = 0xA410;
        public const Int32 m_flFlashMaxAlpha = 0xA40C;
        public const Int32 m_flLastBoneSetupTime = 0x2924;
        public const Int32 m_flLowerBodyYawTarget = 0x3A7C;
        public const Int32 m_flNextAttack = 0x2D70;
        public const Int32 m_flNextPrimaryAttack = 0x3228;
        public const Int32 m_flSimulationTime = 0x268;
        public const Int32 m_flTimerLength = 0x2994;
        public const Int32 m_hActiveWeapon = 0x2EF8;
        public const Int32 m_hMyWeapons = 0x2DF8;
        public const Int32 m_hObserverTarget = 0x3388;
        public const Int32 m_hOwner = 0x29CC;
        public const Int32 m_hOwnerEntity = 0x14C;
        public const Int32 m_iAccountID = 0x2FC8;
        public const Int32 m_iClip1 = 0x3254;
        public const Int32 m_iCompetitiveRanking = 0x1A84;       //ранг
        public const Int32 m_iCompetitiveWins = 0x1B88;
        public const Int32 m_iCrosshairId = 0xB3D4;
        public const Int32 m_iEntityQuality = 0x2FAC;
        public const Int32 m_iFOV = 0x31E4;
        public const Int32 m_iFOVStart = 0x31E8;
        public const Int32 m_iGlowIndex = 0xA428;
        public const Int32 m_iHealth = 0x100;
        public const Int32 m_iItemDefinitionIndex = 0x2FAA;
        public const Int32 m_iItemIDHigh = 0x2FC0;
        public const Int32 m_iMostRecentModelBoneCounter = 0x2690;
        public const Int32 m_iObserverMode = 0x3374;
        public const Int32 m_iShotsFired = 0xA380;
        public const Int32 m_iState = 0x3248;
        public const Int32 m_iTeamNum = 0xF4;
        public const Int32 m_lifeState = 0x25F;
        public const Int32 m_nFallbackPaintKit = 0x31B8;
        public const Int32 m_nFallbackSeed = 0x31BC;
        public const Int32 m_nFallbackStatTrak = 0x31C4;
        public const Int32 m_nForceBone = 0x268C;
        public const Int32 m_nTickBase = 0x342C;
        public const Int32 m_rgflCoordinateFrame = 0x444;
        public const Int32 m_szCustomName = 0x303C;
        public const Int32 m_szLastPlaceName = 0x35B0;
        public const Int32 m_thirdPersonViewAngles = 0x31D8;
        public const Int32 m_vecOrigin = 0x138;
        public const Int32 m_vecVelocity = 0x114;
        public const Int32 m_vecViewOffset = 0x108;
        public const Int32 m_viewPunchAngle = 0x3020;
        public const Int32 anim_overlays = 0x2980;
        public const Int32 clientstate_choked_commands = 0x4D28;
        public const Int32 clientstate_delta_ticks = 0x174;
        public const Int32 clientstate_last_outgoing_command = 0x4D24;
        public const Int32 clientstate_net_channel = 0x9C;
        public const Int32 convar_name_hash_table = 0x2F0F8;
        public const Int32 dwClientState = 0x588D9C;
        public const Int32 dwClientState_GetLocalPlayer = 0x180;
        public const Int32 dwClientState_IsHLTV = 0x4D40;
        public const Int32 dwClientState_Map = 0x28C;
        public const Int32 dwClientState_MapDirectory = 0x188;
        public const Int32 dwClientState_MaxPlayer = 0x388;
        public const Int32 dwClientState_PlayerInfo = 0x52B8;
        public const Int32 dwClientState_State = 0x108;
        public const Int32 dwClientState_ViewAngles = 0x4D88;
        public const Int32 dwEntityList = 0x4D3C7BC;
        public const Int32 dwForceAttack = 0x316DD80;
        public const Int32 dwForceAttack2 = 0x316DD8C;
        public const Int32 dwForceBackward = 0x316DDD4;
        public const Int32 dwForceForward = 0x316DDB0;
        public const Int32 dwForceJump = 0x51E0004;
        public const Int32 dwForceLeft = 0x316DDC8;
        public const Int32 dwForceRight = 0x316DDEC;
        public const Int32 dwGameDir = 0x6274F8;
        public const Int32 dwGameRulesProxy = 0x52532EC;
        public const Int32 dwGetAllClasses = 0xD4ED9C;
        public const Int32 dwGlobalVars = 0x588AA0;
        public const Int32 dwGlowObjectManager = 0x527DFA0;
        public const Int32 dwInput = 0x5187980;
        public const Int32 dwInterfaceLinkList = 0x8F4084;
        public const Int32 dwLocalPlayer = 0xD28B74;
        public const Int32 dwMouseEnable = 0xD2E718;
        public const Int32 dwMouseEnablePtr = 0xD2E6E8;
        public const Int32 dwPlayerResource = 0x316C10C;
        public const Int32 dwRadarBase = 0x517152C;
        public const Int32 dwSensitivity = 0xD2E5B4;
        public const Int32 dwSensitivityPtr = 0xD2E588;
        public const Int32 dwSetClanTag = 0x89D60;
        public const Int32 dwViewMatrix = 0x4D2E0E4;
        public const Int32 dwWeaponTable = 0x5188440;
        public const Int32 dwWeaponTableIndex = 0x324C;
        public const Int32 dwYawPtr = 0xD2E378;
        public const Int32 dwZoomSensitivityRatioPtr = 0xD33598;
        public const Int32 dwbSendPackets = 0xD386A;
        public const Int32 dwppDirect3DDevice9 = 0xA6030;
        public const Int32 find_hud_element = 0x26B0BD40;
        public const Int32 force_update_spectator_glow = 0x398642;
        public const Int32 interface_engine_cvar = 0x3E9EC;
        public const Int32 is_c4_owner = 0x3A4A70;
        public const Int32 m_bDormant = 0xED;
        public const Int32 m_flSpawnTime = 0xA360;
        public const Int32 m_pStudioHdr = 0x294C;
        public const Int32 m_pitchClassPtr = 0x51717D0;
        public const Int32 m_yawClassPtr = 0xD2E378;
        public const Int32 model_ambient_min = 0x58BDBC;
        public const Int32 set_abs_angles = 0x1CED30;
        public const Int32 set_abs_origin = 0x1CEB70;
    }
}
