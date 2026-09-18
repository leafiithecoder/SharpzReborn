using GorillaExtensions;
using Photon.Pun;
using SharpzReborn.Extensions;
using SharpzReborn.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SharpzReborn.Mods
{
    public class Visuals
    {
        // thank you Poison for the boneESP code
        private static readonly Dictionary<VRRig, List<LineRenderer>> boneESP = new Dictionary<VRRig, List<LineRenderer>>();
        public static readonly int[] bones = {
            4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21, 25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6, 10, 6, 14, 6, 16, 14, 12, 10, 9, 7
        };
        public static void BoneESP()
        {

            List<VRRig> toRemove = new List<VRRig>();

            foreach (var boness in boneESP.Where(boness => !VRRigExtensions.ActiveRigs.Contains(boness.Key)))
            {
                toRemove.Add(boness.Key);

                foreach (LineRenderer renderer in boness.Value)
                    UnityEngine.Object.Destroy(renderer);
            }

            foreach (VRRig rig in toRemove)
                boneESP.Remove(rig);

            foreach (var vrrig in VRRigExtensions.ActiveRigs.Where(vrrig => !vrrig.isLocal))
            {
                if (!boneESP.TryGetValue(vrrig, out List<LineRenderer> Lines))
                {
                    Lines = new List<LineRenderer>();

                    LineRenderer LineHead = vrrig.head.rigTarget.gameObject.GetOrAddComponent<LineRenderer>();
                    LineHead.material.shader = Shader.Find("GUI/Text Shader");
                    Lines.Add(LineHead);

                    for (int i = 0; i < 19; i++)
                    {
                        LineRenderer Line = vrrig.mainSkin.bones[bones[i * 2]].gameObject.GetOrAddComponent<LineRenderer>();
                        Line.material.shader = Shader.Find("GUI/Text Shader");
                        Lines.Add(Line);
                    }

                    boneESP.Add(vrrig, Lines);
                }

                LineRenderer liner = Lines[0];

                Color color = vrrig.playerColor;

                liner.startWidth = 0.0075f;
                liner.endWidth = 0.0075f;

                liner.startColor = color;
                liner.endColor = color;

                liner.SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
                liner.SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));

                for (int i = 0; i < 19; i++)
                {
                    liner = Lines[i + 1];

                    liner.startWidth = 0.0075f;
                    liner.endWidth = 0.0075f;

                    liner.startColor = color;
                    liner.endColor = color;

                    liner.material.shader = Shader.Find("GUI/Text Shader");

                    liner.SetPosition(0, vrrig.mainSkin.bones[bones[i * 2]].position);
                    liner.SetPosition(1, vrrig.mainSkin.bones[bones[i * 2 + 1]].position);
                }
            }
        }

        public static void Tracers()
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (GorillaGameManager.instance == null)
                return;

            foreach (var line in linePool)
                line.gameObject.SetActive(false);

            foreach (VRRig playerRig in VRRigExtensions.ActiveRigs)
            {
                if (playerRig.isLocal)
                    continue;

                Color lineColor = playerRig.playerColor;

                LineRenderer line = Visuals.GetLineRender();

                line.startColor = lineColor;
                line.endColor = lineColor;
                line.startWidth = 0.025f;
                line.endWidth = 0.025f;
                line.SetPosition(0, GorillaTagger.Instance.rightHandTransform.position);
                line.SetPosition(1, playerRig.transform.position);
            }
        }

        public static readonly List<LineRenderer> linePool = new List<LineRenderer>();

        public static GameObject lineRenderHolder;

        public static bool isLineRenderQueued = false;

        public static LineRenderer GetLineRender()
        {
            if (lineRenderHolder == null)
                lineRenderHolder = new GameObject("LineRender_Holder");

            LineRenderer finalRender = null;

            foreach (var line in linePool.Where(line => finalRender == null).Where(line => !line.gameObject.activeInHierarchy))
            {
                line.gameObject.SetActive(true);
                finalRender = line;
            }

            if (finalRender == null)
            {
                GameObject lineHolder = new GameObject("LineObject");
                lineHolder.transform.parent = lineRenderHolder.transform;

                LineRenderer newLine = lineHolder.AddComponent<LineRenderer>();
                newLine.material.shader = Shader.Find("GUI/Text Shader");
                newLine.startWidth = 0.025f;
                newLine.endWidth = 0.025f;
                newLine.positionCount = 2;
                newLine.useWorldSpace = true;

                linePool.Add(newLine);
                finalRender = newLine;
            }

            finalRender.gameObject.layer = lineRenderHolder.layer;

            return finalRender;
        }

        public static void ClearLinePool(bool destroy = false) // Set destroy when you disable a feature that needs a lot of lines
        {
            Debug.Log("eheheh I AM CLEARING THE LINE POOL");

            foreach (LineRenderer line in linePool)
            {
                if (destroy || isLineRenderQueued)
                    UnityEngine.Object.Destroy(line.gameObject);
                else
                    line.gameObject.SetActive(false);
            }

            if (destroy || isLineRenderQueued)
                linePool.Clear();
        }

        private static bool previousFullbrightStatus;
        public static void SetFullbrightStatus(bool fullBright)
        {
            if (fullBright)
            {
                previousFullbrightStatus = false;
                GameLightingManager.instance.SetCustomDynamicLightingEnabled(false);
            }
            else
            {
                if (previousFullbrightStatus)
                    GameLightingManager.instance.SetCustomDynamicLightingEnabled(true);
            }
        }
    }
}
