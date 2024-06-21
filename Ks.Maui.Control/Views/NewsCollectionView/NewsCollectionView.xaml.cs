using NetStandardCommon;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
                {
                    var items = await NotificationUtils.GetImportantNotifications();
                    Items = new ObservableCollection<NotificationItemModel>(items);
                }
                else
                {
                    var items = await NotificationUtils.GetAllNotifications();
                    Items = new ObservableCollection<NotificationItemModel>(items);
                }
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


    public static readonly BindableProperty EmptyViewTextProperty = BindableProperty.Create(
        nameof(EmptyViewText),
        typeof(string),
        typeof(NewsCollectionView));

    /// <summary>
    /// 表示するアイテムのコレクション
    /// </summary>
    public ObservableCollection<NotificationItemModel> Items { get; set; }

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
        NotificationUtils.SetReaded(new int[] { item.Id });
    }

    public ICommand AllReadedCommand => new Command( () =>
    {
        List<int> ids = new List<int>();
        var targets = Items.ToArray();
        for (int i = 0; i < targets.Length; i ++)
        {
            var item = targets[i];
            if (!item.IsReaded)
            {
                item.IsReaded = true;
                ids.Add(item.Id);
                Items[i] = item;
            }
        }
        NotificationUtils.SetReaded(ids.ToArray());
    });
}