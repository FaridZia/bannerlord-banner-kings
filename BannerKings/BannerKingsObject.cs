﻿using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace BannerKings
{
    public abstract class BannerKingsObject : MBObjectBase
    {
        protected TextObject description;
        protected TextObject name;

        protected BannerKingsObject(string stringId) : base(stringId)
        {
        }

        public TextObject Name => name;
        public TextObject Description => description;

        public void Initialize(TextObject name, TextObject description)
        {
            this.name = name;
            this.description = description;
        }

        public override bool Equals(object obj)
        {
            if (obj is BannerKingsObject kingsObject)
            {
                return kingsObject.StringId == StringId;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}