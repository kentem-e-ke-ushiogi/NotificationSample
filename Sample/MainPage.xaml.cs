using NetStandardCommon;

namespace NugetTestApp2
{
    public partial class MainPage : ContentPage
    {
        public NotificationItemModel[] Items { get; set; }
        public MainPage()
        {
            // 既読を保存する場所をアプリケーションフォルダーに設定
            string folder = GetJsonFolderPath();
            NotificationUtils.SetJsonFolderPath(folder);
            InitializeComponent();
        }

        private static string GetJsonFolderPath()
        {
            string folderpath = "";
#if ANDROID
            // "/storage/emulated/0/Android/data/com.companyname.NotificationSample/files"
            folderpath = MauiApplication.Context.GetExternalFilesDir(null)!.Path;
#endif
#if IOS
            folderpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
#endif
            return folderpath;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            EllipseShape.IsVisible = !EllipseShape.IsVisible;
            await Ks.MauiI.Control.NotificationDialog.Show();
        }
    }
}
