﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace Compiled
{
    [TestClass]
    public class Lifetime : Unity.Specification.Lifetime.SpecificationTests
    {
        public override IUnityContainer GetContainer()
        {
            return new UnityContainer(UnityContainer.BuildStrategy.Compiled);
        }
    }
}

namespace Resolved
{
    [TestClass]
    public class Lifetime : Unity.Specification.Lifetime.SpecificationTests
    {
        public override IUnityContainer GetContainer()
        {
            return new UnityContainer(UnityContainer.BuildStrategy.Resolved);
        }
    }
}

