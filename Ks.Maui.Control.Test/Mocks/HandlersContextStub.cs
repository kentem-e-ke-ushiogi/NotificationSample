// 以下を参考にしました。



using Microsoft.Maui.Animations;

namespace Ks.Maui.UI.Tests.Mocks
{
    class HandlersContextStub : IMauiContext
    {
        public HandlersContextStub(IServiceProvider services)
        {
            Services = services;
            Handlers = Services.GetRequiredService<IMauiHandlersFactory>();
            AnimationManager = services.GetService<IAnimationManager>() ?? throw new NullReferenceException();
        }

        public IServiceProvider Services { get; }

        public IMauiHandlersFactory Handlers { get; }

        public IAnimationManager AnimationManager { get; }
    }
}
