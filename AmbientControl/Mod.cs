﻿using ICities;

namespace LightControl
{
    public class Mod : IUserMod
    {
        public string Name
        {
            get
            {
                return "Ambient Control";
            }
        }
        public string Description
        {
            get
            {
                return "Allows Adjustments to Ambient Lighting";
            }
        }
    }
}