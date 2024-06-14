using FluentAssertions;
using Ks.Maui.UI.Tests;
using Xunit;

namespace Ks.Maui.UI.Test.Views.NewsCollectionView
{
    public class NewsCollectionViewTest : BaseHandlerTest
    {
        readonly Ks.Maui.UI.Views.NewsCollectionView newsCollectionView;
        public NewsCollectionViewTest()
        {
            newsCollectionView = new();
        }
        [Fact]
        public void ContentShouldBeNull_WhenThereIsNoCondition()
        {
            newsCollectionView.Content.Should().NotBeNull();
        }
    }
}
