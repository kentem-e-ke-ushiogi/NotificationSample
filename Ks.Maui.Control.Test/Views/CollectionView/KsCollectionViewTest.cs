using Ks.Maui.UI.Views;
using Xunit;

namespace Ks.Maui.UI.Test.Views.CollectionView
{
    public class KsCollectionViewTest
    {
        readonly KsCollectionView ksCollectionView = new();
        [Fact]
        public void CheckDefaultValues()
        {
            Assert.Equal(-1, ksCollectionView.ScrollIndex);
            Assert.Null(ksCollectionView.EmptyView);
            Assert.False(ksCollectionView.IsForceScroll);
        }
    }
}
