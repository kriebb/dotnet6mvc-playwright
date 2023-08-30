using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace dotnet6mvcEcommerce.Playwright.tests
{



    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TutorialTest : PageTest
    {
        [Test]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            await Page.GotoAsync("https://playwright.dev");

            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

            // create a locator
            var getStarted = Page.GetByRole(AriaRole.Link, new() { Name = "Get started" });

            // Expect an attribute "to be strictly equal" to the value.
            await Expect(getStarted).ToHaveAttributeAsync("href", "/docs/intro");

            // Click the get started link.
            await getStarted.ClickAsync();

            // Expects the URL to contain intro.
            await Expect(Page).ToHaveURLAsync(new Regex(".*intro"));

            await Page.ScreenshotAsync(new PageScreenshotOptions() { Path= "C:\\git\\dotnet6mvc-playwright\\dotnet6mvcEcommerce.Playwright.tests\\bin\\Debug\\net6.0\\image.png" });
        }
    }
}