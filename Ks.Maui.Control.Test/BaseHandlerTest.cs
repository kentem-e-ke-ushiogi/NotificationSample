// 以下を参考にしました。
//https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.UnitTests/BaseHandlerTest.cs

using CommunityToolkit.Maui;
using Ks.Maui.UI.Tests.Mocks;

namespace Ks.Maui.UI.Tests
{
    // All the code in this file is included in all platforms.
    public abstract class BaseHandlerTest
    {
        protected BaseHandlerTest()
        {
            CreateAndSetMockApplication(out var serviceProvider);
            ServiceProvider = serviceProvider;

            DispatcherProvider.SetCurrent(new MockDispatcherProvider());
        }
        
        protected IServiceProvider ServiceProvider { get; }
        static void CreateAndSetMockApplication(out IServiceProvider serviceProvider)
        {
            var appBuilder = MauiApp.CreateBuilder()
                                        .UseMauiApp<MockApplication>();

            var mauiApp = appBuilder.Build();

            var application = mauiApp.Services.GetRequiredService<IApplication>();
            serviceProvider = mauiApp.Services;

            application.Handler = new ApplicationHandlerStub();
            application.Handler.SetMauiContext(new HandlersContextStub(mauiApp.Services));
        }

        protected static TViewHandler CreateViewHandler<TViewHandler>(IView view, bool hasMauiContext = true)
        where TViewHandler : IViewHandler, new()
        {
            var mockViewHandler = new TViewHandler();
            mockViewHandler.SetVirtualView(view);

            if (hasMauiContext)
            {
                mockViewHandler.SetMauiContext(Application.Current?.Handler?.MauiContext ?? throw new NullReferenceException());
            }

            return mockViewHandler;
        }
    }
}
