using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameControler
{
    private static bool m_IsInited = false;
    private static bool m_IsPaused = false;

    public static bool IsInited { get => m_IsInited; }
    public static bool IsPaused { get => m_IsPaused; }

    public static void TickGame()
    {
        //����������Ⱦ�߳�ִ�е�tick���߼��̵߳�tick��GameLogicThread.cs�ļ���ִ�С�
        //GfxSystem.Tick();
        //GfxModule.Skill.GfxSkillSystem.Instance.Tick();
        //GfxModule.Impact.GfxImpactSystem.Instance.Tick();
    }



}
