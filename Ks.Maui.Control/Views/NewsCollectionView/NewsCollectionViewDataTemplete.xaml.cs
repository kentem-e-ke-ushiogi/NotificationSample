using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ks.Maui.UI.Views;

public partial class NewsCollectionViewDataTemplete : ContentView
{
    /// <summary>
    /// タップ後の処理
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
            BrdRead.IsVisible = !IsReaded;
        }
        else if (e.PropertyName == TitleProperty.PropertyName)
        {
            LblTitle.Text = Title;
        }
        else if (e.PropertyName == DateProperty.PropertyName)
        {
            LblDate.Text = Date.ToString("yyyy.MM.dd");
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

    public static readonly BindableProperty DateProperty = BindableProperty.Create(
        nameof(Date),
        typeof(DateTime),
        typeof(NewsCollectionViewDataTemplete));

    public DateTime Date
    {
        get => (DateTime)GetValue(DateProperty);
        set => SetValue(DateProperty, value);
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