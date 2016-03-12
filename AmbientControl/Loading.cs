using System.Linq;
using ICities;
using System;
using ColossalFramework;
using UnityEngine;
using ColossalFramework.UI;

namespace AmbientControl
{
 
    public class Loading : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);

            LightSettingsPanel.Initialize();
        }
    }
}
