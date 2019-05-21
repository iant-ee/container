﻿using System;
using System.Linq;
using System.Reflection;
using Unity.Builder;
using Unity.Registration;
using Unity.Resolution;

namespace Unity.Factories
{
    public class EnumerableResolver
    {
        #region Fields

        private static readonly MethodInfo EnumerableMethod =
            typeof(EnumerableResolver).GetTypeInfo()
                                      .GetDeclaredMethod(nameof(EnumerableResolver.Resolver));

        private static readonly MethodInfo EnumerableFactory =
            typeof(EnumerableResolver).GetTypeInfo()
                                      .GetDeclaredMethod(nameof(EnumerableResolver.ResolverFactory));

        #endregion


        #region TypeResolverFactory

        public static TypeFactoryDelegate Factory = (Type type, IRegistration policies) =>
        {
#if NETSTANDARD1_0 || NETCOREAPP1_0 || NET40
            var typeArgument = type.GetTypeInfo().GenericTypeArguments.First();
            if (typeArgument.GetTypeInfo().IsGenericType)
#else
            var typeArgument = type.GenericTypeArguments.First();
            if (typeArgument.IsGenericType)
#endif
            {
                return ((EnumerableFactoryDelegate)
                    EnumerableFactory.MakeGenericMethod(typeArgument)
                                     .CreateDelegate(typeof(EnumerableFactoryDelegate)))();
            }
            else
            {
                return (ResolveDelegate<BuilderContext>)
                    EnumerableMethod.MakeGenericMethod(typeArgument)
                                    .CreateDelegate(typeof(ResolveDelegate<BuilderContext>));
            }
        };

        #endregion


        #region Implementation

        private static object Resolver<TElement>(ref BuilderContext context)
        {
            return ((UnityContainer)context.Container).ResolveEnumerable<TElement>(context.Resolve,
                                                                                   context.Name);
        }

        private static ResolveDelegate<BuilderContext> ResolverFactory<TElement>()
        {
            Type type = typeof(TElement).GetGenericTypeDefinition();
            return (ref BuilderContext c) => ((UnityContainer)c.Container).ResolveEnumerable<TElement>(c.Resolve, type, c.Name);
        }

        #endregion


        #region Nested Types

        private delegate ResolveDelegate<BuilderContext> EnumerableFactoryDelegate();

        #endregion
    }
}
