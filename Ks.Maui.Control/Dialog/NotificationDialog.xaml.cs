using CommunityToolkit.Maui.Views;
using NetStandardCommon;

namespace Ks.MauiI.Control;

public partial class NotificationDialog : Popup
{
    /// <summary>ダイアログを閉じた後の処理 </summary>
    public  Action<bool>? CloseAction = null;
    /// <summary> お知らせアイテム </summary>
    public NotificationItemModel[] Items { get; }

    public NotificationDialog(NotificationItemModel[] items)
    {
        Items = items;
        InitializeComponent();
    }

    private void BtnOK_Clicked(object sender, EventArgs e)
    {
        Close();
        CloseAction?.Invoke(NotAlertAgainCheckBox.IsChecked);
    }

    /// <summary> お知らせダイアログ表示 </summary>
    public static async Task<object> Show()
    {
        var currentPage = GetCurrentPage();
        try
        {
            var items = await NotificationUtils.GetImportantNotifications();

            NotificationDialog dialog = new(items);
            return currentPage.ShowPopupAsync(dialog);
        }
        catch(Exception ex)
        {
            await currentPage.DisplayAlert("エラー", ex.Message + "\r\n" + ex.StackTrace, "OK");
            return Task.CompletedTask;
        }
    }

    private static Page GetCurrentPage()
    {
        if (Application.Current!.MainPage is Shell shell)
        {
            return shell.CurrentPage;
        }

        if (Application.Current.MainPage is NavigationPage nav)
        {
            return nav.CurrentPage;
        }

        if (Application.Current.MainPage is TabbedPage tabbed)
        {
            return tabbed.CurrentPage;
        }

        return Application.Current.MainPage!;
    }
}