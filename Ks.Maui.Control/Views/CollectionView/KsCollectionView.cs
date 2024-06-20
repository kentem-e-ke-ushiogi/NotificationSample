namespace Ks.Maui.UI.Views;

public class KsCollectionView : CollectionView
{
    private readonly List<View> TempViews = [];

    public static readonly BindableProperty IsForceScrollProperty = BindableProperty.Create(
        nameof(IsForceScroll),
        typeof(bool),
        typeof(KsCollectionView),
        false,
        propertyChanged: (bindable, oldValue, newValue) => ((KsCollectionView)bindable).IsForceScroll = (bool)newValue);

    public static readonly BindableProperty EmptyViewTextProperty = BindableProperty.Create(
       nameof(EmptyViewText),
       typeof(string),
       typeof(KsCollectionView),
       propertyChanged: (bindable, oldValue, newValue) => ((KsCollectionView)bindable).EmptyViewText = (string)newValue);

    public static readonly BindableProperty ScrollIndexProperty = BindableProperty.Create(
       nameof(ScrollIndex),
       typeof(int),
       typeof(KsCollectionView),
       -1,
       propertyChanged: (bindable, oldValue, newValue) => ((KsCollectionView)bindable).ScrollIndex = (int)newValue);

    internal bool ScrollAnimated { get; set; } = true;

    public KsCollectionView()
    {
#if ANDROID
        // 画面の回転を繰り返すとItem部分だけ横幅が狭くなるときがある問題に対応しました。
        // Microsoft.Maui.Controls.CollectionViewで修正が入れば不要になります。
        SizeChanged += CollectionSizeChanged;
        ChildAdded += Collection_ChildAdded;
        ChildRemoved += Collection_ChildRemoved;
#endif
        PropertyChanged += CustomCollectionView_PropertyChanged;
    }

    private void CustomCollectionView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == IsForceScrollProperty.PropertyName)
            Onscroll();
        if (e.PropertyName == ScrollIndexProperty.PropertyName)
            OnScrollIndex();
        else if (e.PropertyName == EmptyViewTextProperty.PropertyName)
        {
#if ANDROID
            // デザイナで定義しても、AndroidのみEmptyViewが表示されません。
            // https://github.com/dotnet/maui/issues/10819
            // 不具合が修正されたら消します。
            EmptyView = EmptyViewText;
#endif
        }
    }

    public int ScrollIndex
    {
        get => (int)GetValue(ScrollIndexProperty);
        set => SetValue(ScrollIndexProperty, value);
    }
    public bool IsForceScroll
    {
        get => (bool)GetValue(IsForceScrollProperty);
        set => SetValue(IsForceScrollProperty, value);
    }
    public string EmptyViewText
    {
        get => (string)GetValue(EmptyViewTextProperty);
        set => SetValue(EmptyViewTextProperty, value);
    }
    private void Onscroll()
    {
        if (IsForceScroll)
        {
            if (SelectedItem == null)
                return;
            ScrollTo(SelectedItem, position: ScrollToPosition.MakeVisible, animate: ScrollAnimated);
        }
    }
    private void OnScrollIndex()
    {
        if (ScrollIndex == -1)
            return;
        ScrollTo(ScrollIndex, position: ScrollToPosition.MakeVisible, animate: ScrollAnimated);
    }
    private void Collection_ChildAdded(object? sender, ElementEventArgs e)
    {
        View grid = (View)e.Element;
        if (!TempViews.Contains(grid))
            TempViews.Add(grid);
    }
    private void Collection_ChildRemoved(object? sender, ElementEventArgs e)
    {
        View grid = (View)e.Element;
        TempViews.Remove(grid);
    }

    private void CollectionSizeChanged(object? sender, EventArgs e)
    {
        //グリッドの場合は複数列になるので、調整すると逆におかしくなる
        if (ItemsLayout is GridItemsLayout)
            return;

        AdjustItemWidth();
    }

    internal void AdjustItemWidth()
    {
        foreach (View item in TempViews)
        {
            //item.WidthRequest = Width;
        }
    }
}