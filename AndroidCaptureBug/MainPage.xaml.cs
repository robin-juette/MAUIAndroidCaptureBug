using CommunityToolkit.Maui.Storage;

namespace AndroidCaptureBug
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private async Task CaptureAndSaveAsync()
        {
            ViewExportService viewExportService = new();

            ViewCaptureResult captureResult = await ViewExportService.CaptureViewAsync(CoreLayout);

            byte[] outputBytes = captureResult.ImageBytes;
            string extension = ".jpg";
            string outputFileName = $"app{extension}";
            using MemoryStream outputStream = new(outputBytes, writable: false);

            FileSaverResult saveResult = await FileSaver.Default.SaveAsync(outputFileName, outputStream, CancellationToken.None);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _ = CaptureAndSaveAsync();
        }
    }
}
