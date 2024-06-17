using NetStandardCommon;

namespace Ks.Maui.UI.Views;

/// <summary>お知らせ一覧表示用のコントロール</summary>
public partial class NewsCollectionView : ContentView
{
    public NewsCollectionView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items),
        typeof(IEnumerable<NotificationItemModel>),
        typeof(NewsCollectionView));

    public string EmptyViewText => "お知らせはありません。";

    /// <summary>
    /// 表示するアイテムのコレクション
    /// </summary>
    public IEnumerable<NotificationItemModel> Items
    {
        get => (IEnumerable<NotificationItemModel>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    private void this_ItemSourceOpened(object sender, EventArgs e)
    {
        NotificationItemModel item = (NotificationItemModel)sender;
        item.IsReaded = true;
        NotificationUtils.SetReaded(item.Id);
    }
}