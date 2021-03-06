﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using XTable;


public class MountLoadTask : BaseLoadTask
{
    private IEquip component;
    private GameObject goInstance = null;
    public bool transferRef = false;
    public Renderer mainRender = null;

    public MountLoadTask(EPartType p, IEquip e) : base(p)
    {
        component = e;
    }

    public override void Load(ref FashionPositionInfo newFpi, HashSet<string> loadedPath)
    {
        if (!IsSamePart(ref newFpi))
        {
            if (MakePath(ref newFpi, loadedPath))
            {
                if (goInstance != null) GameObject.Destroy(goInstance);
                goInstance = XResources.Load<GameObject>(location, AssetType.Prefab);
                LoadFinish(this);
                ProcessTransfer();
            }
            else
            {
                if (string.IsNullOrEmpty(location))
                {
                    processStatus = EProcessStatus.EProcessing;
                    LoadFinish(this);
                }
            }
        }
    }

    private void LoadFinish(object o)
    {
        MountLoadTask mlt = o as MountLoadTask;
        if (mlt != null)
        {
            if (mlt.processStatus == EProcessStatus.EProcessing)
            {
                mlt.processStatus = EProcessStatus.EPreProcess;
            }
        }
        ProcessRender();
    }


    public override void PostLoad()
    {
        base.PostLoad();
    }

    public override void Reset()
    {
        base.Reset();
        if (goInstance != null)
        {
            XResources.Destroy(goInstance);
        }
        transferRef = false;
    }

    public void ProcessRender()
    {
        for (int i = 0; i < XCommon.tmpRender.Count; i++)
        {
            Renderer render = XCommon.tmpRender[i];
            render.enabled = true;
            Material mat = render.sharedMaterial;
            if (mat != null && mat.shader.renderQueue < 3000)
            {
                mainRender = render;
                render.shadowCastingMode = ShadowCastingMode.Off;
            }
            bool hasUIRimMask = render.sharedMaterial.HasProperty("_UIRimMask");
            if (hasUIRimMask)
            {
                render.material.SetVector("_UIRimMask", new Vector4(0, 0, 2, 0));
            }
        }
        XCommon.tmpRender.Clear();
    }

    public void ProcessTransfer()
    {
        goInstance.transform.parent = component.EntityObject.transform.FindChild(GetMountPoint());
        goInstance.transform.localPosition = Vector3.zero;
        goInstance.transform.localRotation = Quaternion.identity;
        goInstance.transform.localScale = Vector3.one;

    }

    private string GetMountPoint()
    {
        DefaultEquip.RowData data = component.data;
        string point = "";
        switch (part)
        {
            case EPartType.EMainWeapon:
                point = data.WeaponPoint;
                break;
            case EPartType.EWings:
                point = data.WingPoint;
                break;
            case EPartType.ETail:
                point = data.TailPoint;
                break;
            default:
                throw new System.Exception("err");
        }
        return point;
    }

}

