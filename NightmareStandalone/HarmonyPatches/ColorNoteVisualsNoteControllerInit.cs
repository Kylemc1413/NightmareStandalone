using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using NightmareStandalone.Extensions;
using SiraUtil.Interfaces;
using IPA.Utilities;

namespace NightmareStandalone.HarmonyPatches
{
    [HarmonyPriority(Priority.High)]
    [HarmonyPatch(typeof(ColorNoteVisuals))]
    [HarmonyPatch("HandleNoteControllerDidInit")]
    class ColorNoteVisualsHandleNoteControllerDidInitEvent
    {
        static void Postfix(ColorNoteVisuals __instance, NoteControllerBase noteController, MeshRenderer[] ____arrowMeshRenderers, MeshRenderer[] ____circleMeshRenderers, MaterialPropertyBlockController[] ____materialPropertyBlockControllers, ref ColorManager ____colorManager)
        {
            if (!Plugin.Safe())
                return;
            ColorNoteVisualsExtensions.CNVDisplayType displayType = ColorNoteVisualsExtensions.CNVDisplayType.NORMAL;
            NoteBehaviour noteBehaviour = NoteBehaviour.GetNoteBehaviour(noteController, true);
            if (noteBehaviour != null)
            {
                Color c = noteBehaviour.Colour;
                if (noteBehaviour.CutBehaviour == NoteBehaviour.NoteCutBehaviour.BIDIRECTIONAL)
                {
                    displayType = ColorNoteVisualsExtensions.CNVDisplayType.BIDIRECTIONAL;
                }
                else if (noteBehaviour.CutBehaviour == NoteBehaviour.NoteCutBehaviour.DEFLECT)
                {
                    displayType = ColorNoteVisualsExtensions.CNVDisplayType.DEFLECT;
                }
                try
                {
                    SetNoteColour(__instance, c);
                    var colorable = noteController.gameObject.GetComponent<IColorable>();
                    if (colorable != null)
                        colorable.Color = c;
                //    __instance.SetDisplayType(c != Color.clear ? c : Color.white, ____circleGlowSpriteRenderer, displayType, ____noteController.noteData.cutDirection);

                }
                catch (Exception e)
                {
                    Logger.log.Error(e);
                }
            }
            else
            {
                Color c = ____colorManager.ColorForType(noteController.noteData.colorType);
           //     SetNoteColour(__instance, c);
       //         __instance.SetDisplayType(c, ____circleGlowSpriteRenderer, ColorNoteVisualsExtensions.CNVDisplayType.NORMAL, ____noteController.noteData.cutDirection);
            }


        }

        private static readonly int _colorId = Shader.PropertyToID("_Color");
        public static void SetNoteColour(ColorNoteVisuals noteVis, Color c)
        {
        //    SpriteRenderer ____arrowGlowSpriteRenderer = noteVis.GetField<SpriteRenderer>("_arrowGlowSpriteRenderer");
          //  SpriteRenderer ____circleGlowSpriteRenderer = noteVis.GetField<SpriteRenderer>("_circleGlowSpriteRenderer");
            MaterialPropertyBlockController[] ____materialPropertyBlockController = noteVis.GetField<MaterialPropertyBlockController[], ColorNoteVisuals>("_materialPropertyBlockControllers");
//            if (____arrowGlowSpriteRenderer != null) ____arrowGlowSpriteRenderer.color = c;
//            if (____circleGlowSpriteRenderer != null) ____circleGlowSpriteRenderer.color = c;
            foreach(var block in ____materialPropertyBlockController)
            {
                block.materialPropertyBlock.SetColor(_colorId, c);
                block.ApplyChanges();
            }

        }
    }


}
