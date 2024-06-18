using NetStandardCommon;

namespace Ks.Maui.UI.Views;

/// <summary>お知らせ一覧表示用のコントロール</summary>
public partial class NewsCollectionView : ContentView
{
    public NewsCollectionView()
    {
        InitializeComponent();
        
    }
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        InitData();
    }
    private void InitData()
    {
        Task.Run(async () =>
        {
            try
            {
                if (Important)
                    Items = await NotificationUtils.GetImportantNotifications();
                else
                    Items = await NotificationUtils.GetAllNotifications();
                OnPropertyChanged(nameof(Items));
                UpdateEmptyView(null);
            }
            catch(Exception ex)
            {
                UpdateEmptyView(ex);
            }
        });
    }
    private void UpdateEmptyView(Exception? ex)
    {
        if (ex == null)
            EmptyViewText = "お知らせはありません。";
        else
            EmptyViewText = "ネットワークに接続することができないため、表示できません。";
        OnPropertyChanged(nameof(EmptyViewText));
    }

    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
        nameof(Items),
        typeof(IEnumerable<NotificationItemModel>),
        typeof(NewsCollectionView));

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

    public string EmptyViewText
    {
        get => (string)GetValue(EmptyViewTextProperty);
        set => SetValue(EmptyViewTextProperty, value);
    }

    public bool Important { get; set; }


    private void this_ItemSourceOpened(object sender, EventArgs e)
    {
        NotificationItemModel item = (NotificationItemModel)sender;
        item.IsReaded = true;
        NotificationUtils.SetReaded(item.Id);
    }
}