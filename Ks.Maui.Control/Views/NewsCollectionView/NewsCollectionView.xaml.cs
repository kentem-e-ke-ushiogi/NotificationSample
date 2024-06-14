using CommunityToolkit.Mvvm.Input;
using Ks.MauiI.Control;
using NetStandardCommon;

namespace Ks.Maui.UI.Views;

/// <summary>お知らせ一覧表示用のコントロール</summary>
public partial class NewsCollectionView : ContentView
{
    /// <summary>
    /// タップでSourceが開かれた後に発生します。
    /// </summary>
    public event EventHandler? ItemSourceOpened;
    public NewsCollectionView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items),
        typeof(IEnumerable<NotificationItemModel>),
        typeof(NewsCollectionView),
        propertyChanged: ItemsChanged);

    public static readonly BindableProperty EmptyViewTextProperty = BindableProperty.Create(
        nameof(EmptyViewText),
        typeof(string),
        typeof(NewsCollectionView));

    /// <summary>
    /// 表示するアイテムのコレクション
    /// </summary>
    public IEnumerable<NotificationItemModel> Items
    {
        get => (IEnumerable<NotificationItemModel>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    /// <summary>
    /// Itemsがないときに表示するテキスト
    /// </summary>
    public string EmptyViewText
    {
        get => (string)GetValue(EmptyViewTextProperty);
        set => SetValue(EmptyViewTextProperty, value);
    }

    public IRelayCommand TapContentBoxCommand => new RelayCommand<object>(async (obj) =>
    {
        if (obj is not NotificationItemModel item) return;

        var res = await Browser.Default.OpenAsync(item.URL, BrowserLaunchMode.SystemPreferred);
        if (res)
        {
            // ToDo:既読の切替を行う
            item.IsReaded = true;
            NotificationUtils.SetReaded(item.Id);
            ItemSourceOpened?.Invoke(item, new EventArgs());
        }
    });

    private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not NewsCollectionView selfView || newValue is not IEnumerable<NotificationItemModel> items) return;

        selfView.TopBorder.IsVisible = items.Any();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await NotificationDialog.Show();
    }
}