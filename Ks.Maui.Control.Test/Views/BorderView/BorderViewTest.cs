using Ks.Maui.UI.Tests;
using Ks.Maui.UI.Views;
using Xunit;

namespace Ks.Maui.UI.Test.Views.BorderView
{
    public class BorderViewTest : BaseHandlerTest
    {
        readonly Ks.Maui.UI.Views.BorderView borderView = new();
        [Fact]
        public void CheckDefaultValues()
        {
            Assert.False(borderView.IsArrowVisible);
            Assert.Equal(Borders.None, borderView.VisibleBorders);
            Assert.Null(borderView.TapCommandParameter);
            Assert.Null(borderView.TapCommand);
        }
    }
}
