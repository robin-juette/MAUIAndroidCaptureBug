using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Platform;

namespace AndroidCaptureBug;

public sealed record ViewCaptureResult(byte[] ImageBytes, int PixelWidth, int PixelHeight);

public sealed class ViewExportService
{
    public static async Task<ViewCaptureResult> CaptureViewAsync(IView graphicsView)
    {
        IScreenshotResult? captureResult = await graphicsView.CaptureAsync();
        if (captureResult is null)
        {
            throw new InvalidOperationException("The editor canvas could not be captured on this platform.");
        }

        await using Stream captureStream = await captureResult.OpenReadAsync();
        Microsoft.Maui.Graphics.IImage? capturedImage = PlatformImage.FromStream(captureStream);
        if (capturedImage is null)
        {
            throw new InvalidOperationException("The captured canvas image could not be decoded.");
        }

        int pixelWidth = checked((int)capturedImage.Width);
        int pixelHeight = checked((int)capturedImage.Height);
        using MemoryStream outputStream = new();
        capturedImage.Save(
            outputStream,
            Microsoft.Maui.Graphics.ImageFormat.Jpeg,
            0.92f);
        return new ViewCaptureResult(outputStream.ToArray(), pixelWidth, pixelHeight);
    }

}
