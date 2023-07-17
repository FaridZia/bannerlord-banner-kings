﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannerKings.Behaviours.Schemes
{
    public class DefaultSchemes : DefaultTypeInitializer<DefaultSchemes, Scheme>
    {
        public Scheme Assassination = new Scheme("Assassination");
        public Scheme Sway = new Scheme("Sway");
        public Scheme ForgeClaim = new Scheme("ForgeClaim");
        public Scheme FabricateCrime = new Scheme("FabricateCrime");

        public override IEnumerable<Scheme> All => throw new NotImplementedException();

        public override void Initialize()
        {
        }
    }
}
