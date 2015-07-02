using System;
using System.Collections.Generic;
using Nancy;
using Nancy.TinyIoc;

namespace NancyFxPlayground.MyApi.UnitTests
{
    internal class TestBootstrapper : MyApiBootstrapper
    {
        readonly Dictionary<Type, object> _dependencies;

        public TestBootstrapper()
        {
            _dependencies = new Dictionary<Type, object>();
        }
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            foreach (var dependency in _dependencies)
            {
                container.Register(dependency.Key, dependency.Value);
            }
        }

        public void OverrideDependency<T>(object instance)
        {
            _dependencies.Add(typeof(T), instance);
        }
    }
}