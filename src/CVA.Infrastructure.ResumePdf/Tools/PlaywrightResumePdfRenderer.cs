using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace CVA.Infrastructure.ResumePdf;

/// <summary>
/// Renders a given URL into a PDF using Playwright (Chromium).
/// </summary>
/// <param name="options">Resume PDF options.</param>
public sealed class PlaywrightResumePdfRenderer(IOptions<ResumePdfOptions> options) : IAsyncDisposable
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    /// <summary>
    /// Renders the specified URL into a PDF document.
    /// </summary>
    /// <param name="url">Page URL to render.</param>
    /// <param name="ct">Cancellation token.</param>
    public async Task<byte[]> RenderAsync(string url, CancellationToken ct)
    {
        await EnsureInitializedAsync(ct);

        var size = new ViewportSize { Width = 1280, Height = 720, };
        var contextOptions = new BrowserNewContextOptions { ViewportSize = size, };
        await using var context = await _browser!.NewContextAsync(contextOptions);

        var page = await context.NewPageAsync();

        await page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = options.Value.NavigationTimeoutSeconds * 1000
        });

        await page.EmulateMediaAsync(new PageEmulateMediaOptions { Media = Media.Print });

        return await page.PdfAsync(new PagePdfOptions
        {
            Format = "A4",
            PrintBackground = true,
            Margin = new Margin { Top = "12mm", Bottom = "12mm", Left = "10mm", Right = "10mm" }
        });
    }

    private async Task EnsureInitializedAsync(CancellationToken ct)
    {
        if (_browser is not null) return;

        await _initLock.WaitAsync(ct);
        try
        {
            if (_browser is not null) return;

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true,
                Args = ["--no-sandbox"]
            });
        }
        finally
        {
            _initLock.Release();
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
        _initLock.Dispose();
    }
}