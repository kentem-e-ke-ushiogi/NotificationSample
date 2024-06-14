// 以下を参考にしました。
// https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui.UnitTests/Mocks/MockApplication.cs

using Microsoft.Maui.Handlers;

namespace Ks.Maui.UI.Tests.Mocks
{
    class MockApplication : Application
    {
        public new Application? Current = null;
    }

    // Inspired by https://github.com/dotnet/maui/blob/main/src/Controls/tests/Core.UnitTests/TestClasses/ApplicationHandlerStub.cs
    class ApplicationHandlerStub : ElementHandler<IApplication, object>
    {
        public ApplicationHandlerStub() : base(Mapper)
        {
        }

        public static IPropertyMapper<IApplication, ApplicationHandlerStub> Mapper = new PropertyMapper<IApplication, ApplicationHandlerStub>(ElementMapper);

        protected override object CreatePlatformElement()
        {
            return new object();
        }
    }
}
