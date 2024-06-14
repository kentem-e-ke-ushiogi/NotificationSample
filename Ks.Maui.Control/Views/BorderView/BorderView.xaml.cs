using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Ks.Maui.UI.Views;

[Flags]
public enum Borders
{
    None = 0,
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
}
public partial class BorderView : ContentView
{
    private static readonly Color TappedColor = Colors.Yellow;
    private static readonly Color BorderColor = Colors.SkyBlue;
    private static readonly Color DisableColor = Colors.LightGray;

    public static readonly BindableProperty VisibleBordersProperty = BindableProperty.Create(nameof(VisibleBorders), typeof(Borders), typeof(BorderView), Borders.None, propertyChanged: OnVisibleBordersChanged);

    public static readonly BindableProperty IsArrowVisibleProperty = BindableProperty.Create(nameof(IsArrowVisible), typeof(bool), typeof(BorderView), false, propertyChanged: OnIsArrowVisibleChanged);

    public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(BorderView), null, propertyChanged: OnTapCommandChanged);

    public static readonly BindableProperty TapCommandParameterProperty = BindableProperty.Create(nameof(TapCommandParameter), typeof(object), typeof(BorderView));

    public static readonly BindableProperty ContentPaddingProperty = BindableProperty.Create(nameof(ContentPadding), typeof(Thickness), typeof(BorderView), new Thickness(10, 10));

    private readonly List<Action> InitActions = [];
    private readonly GridSeparateBorder LeftBorder = new();
    private readonly GridSeparateBorder TopBorder = new();
    private readonly GridSeparateBorder RightBorder = new();
    private readonly GridSeparateBorder BottomBorder = new();

    private bool _isTemplateApplied = false;
    private Grid _contentPresenterGrid = null!;
    private Grid _contentFrameGrid = null!;
    private Image? _arrowImage;

    public BorderView()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _contentFrameGrid = (Grid)GetTemplateChild("ContentFrameGrid");
        _contentPresenterGrid = (Grid)GetTemplateChild("ContentPresenterGrid");
        _isTemplateApplied = true;

        foreach (var init in InitActions)
        {
            init();
        }
        InitActions.Clear();
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(IsEnabled))
        {
            _contentFrameGrid.BackgroundColor = IsEnabled ? Colors.Transparent : DisableColor;
            _contentFrameGrid.IsEnabled = IsEnabled;
        }
    }

    private static void OnVisibleBordersChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (BorderView)bindable;
        if (!control._isTemplateApplied)
        {
            control.InitActions.Add(() => OnVisibleBordersChanged(bindable, oldValue, newValue));
            return;
        }
        var newBorders = (Borders)newValue;
        var targetGrid = control._contentFrameGrid;

        bool isLeftBorderVisible = newBorders.HasFlag(Borders.Left);
        control.LeftBorder.Update(isLeftBorderVisible, targetGrid, rowSpan: 3);

        bool isTopBorderVisible = newBorders.HasFlag(Borders.Top);
        control.TopBorder.Update(isTopBorderVisible, targetGrid, columnSpan: 3);

        bool isRightBorderVisible = newBorders.HasFlag(Borders.Right);
        control.RightBorder.Update(isRightBorderVisible, targetGrid, column: 2, rowSpan: 3);

        bool isBottomBorderVisible = newBorders.HasFlag(Borders.Bottom);
        control.BottomBorder.Update(isBottomBorderVisible, targetGrid, row: 2, columnSpan: 3);
    }

    private static void OnTapCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (BorderView)bindable;
        if (!control._isTemplateApplied)
        {
            control.InitActions.Add(() => OnTapCommandChanged(bindable, oldValue, newValue));
            return;
        }
        control._contentFrameGrid.GestureRecognizers.Clear();
        control._contentFrameGrid.Effects.Clear();

        if (newValue is ICommand command)
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += control.TapGesture_Tapped;
            control._contentFrameGrid.GestureRecognizers.Add(tapGesture);

            //var touchEffect = new TouchPlatformEffect() { IsReceiveTouch = true };
            //touchEffect.TouchAction += control.TouchEffect_TouchAction;
            //control._contentFrameGrid.Effects.Add(touchEffect);

            //var touchBehavior = new TouchBehavior();
            //touchBehavior.CurrentTouchStatusChanged += control.TouchBehavior_CurrentTouchStatusChanged;
            //control._contentFrameGrid.Behaviors.Add(touchBehavior);
        }
    }

    //private void TouchBehavior_CurrentTouchStatusChanged(object? sender, CommunityToolkit.Maui.Core.TouchStatusChangedEventArgs e)
    //{
    //    switch (e.Status)
    //    {
    //        case TouchStatus.Started:
    //            _contentPresenterGrid.BackgroundColor = TappedColor;
    //            break;
    //        case TouchStatus.Completed:
    //            TapCommand.Execute(TapCommandParameter);
    //            _contentPresenterGrid.BackgroundColor = Colors.Transparent;
    //            break;
    //        default:
    //            _contentPresenterGrid.BackgroundColor = Colors.Transparent;
    //            break;
    //    }
    //}

    private static void OnIsArrowVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (BorderView)bindable;
        if (!control._isTemplateApplied)
        {
            control.InitActions.Add(() => OnIsArrowVisibleChanged(bindable, oldValue, newValue));
            return;
        }
        var isArrowVisible = (bool)newValue;
        var targetGrid = control._contentPresenterGrid;
        targetGrid.ColumnDefinitions.Clear();

        if (isArrowVisible)
        {
            targetGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            targetGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            control._arrowImage ??= new Image()
            {
                Source = "arrow_forward_glay.png",
                Margin = new Thickness(5, 0, 0, 0),
                HeightRequest = 20,
                WidthRequest = 20
            };
            targetGrid.Add(control._arrowImage, 1, 0);
        }
        else
            targetGrid.Remove(control._arrowImage);
    }

    private void TapGesture_Tapped(object? sender, TappedEventArgs e)
    {
        if (TapCommand == null || !IsEnabled) return;

        TapCommand.Execute(TapCommandParameter);
    }

    public Borders VisibleBorders
    {
        get => (Borders)GetValue(VisibleBordersProperty);
        set => SetValue(VisibleBordersProperty, value);
    }

    public bool IsArrowVisible
    {
        get => (bool)GetValue(IsArrowVisibleProperty);
        set => SetValue(IsArrowVisibleProperty, value);
    }

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public object TapCommandParameter
    {
        get => GetValue(TapCommandParameterProperty);
        set => SetValue(TapCommandParameterProperty, value);
    }

    public Thickness ContentPadding
    {
        get => (Thickness)GetValue(ContentPaddingProperty);
        set => SetValue(ContentPaddingProperty, value);
    }

    private class GridSeparateBorder
    {
        private BoxView? _border;

        public void Update(bool isVisible, Grid grid, int column = 0, int row = 0, int columnSpan = 1, int rowSpan = 1)
        {
            if (isVisible)
            {
                _border ??= new BoxView() { Color = BorderColor };
                grid.Add(_border, column, row);
                grid.SetColumnSpan(_border, columnSpan);
                grid.SetRowSpan(_border, rowSpan);
            }
            else
            {
                grid.Remove(_border);
            }
        }
    }
}