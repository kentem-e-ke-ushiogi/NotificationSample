namespace Ks.Maui.UI.Views;

public partial class NewsCollectionViewDataTemplete : ContentView
{
    /// タップでSourceが開かれた後に発生します。
    /// </summary>
    public event EventHandler? ItemSourceOpened;

    public NewsCollectionViewDataTemplete()
	{
		InitializeComponent();
        PropertyChanged += NewsCollectionViewDataTemplete_PropertyChanged;
	}

    private void NewsCollectionViewDataTemplete_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == IsReadedProperty.PropertyName)
        {
            EllipseShape.IsVisible = !IsReaded;
        }
        else if (e.PropertyName == TitleProperty.PropertyName)
        {
            LblTitle.Text = Title;
        }
    }

    public static readonly BindableProperty IsReadedProperty = BindableProperty.Create(
            nameof(IsReaded),
            typeof(bool),
            typeof(NewsCollectionViewDataTemplete));

    public bool IsReaded
    {
        get => (bool)GetValue(IsReadedProperty);
        set => SetValue(IsReadedProperty, value);
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(NewsCollectionViewDataTemplete));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty LinkProperty = BindableProperty.Create(
        nameof(Link),
        typeof(string),
        typeof(NewsCollectionViewDataTemplete));

    public string Link
    {
        get => (string)GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var res = await Browser.Default.OpenAsync(Link, BrowserLaunchMode.SystemPreferred);
        if (res)
        {
            IsReaded = true;
            ItemSourceOpened?.Invoke(this.BindingContext, new EventArgs());
        }
    }
}