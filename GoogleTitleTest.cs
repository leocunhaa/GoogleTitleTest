using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using FluentAssertions;

namespace GoogleTitleTest
{
    public class GoogleTitleTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly Xunit.Abstractions.ITestOutputHelper _output;

        public GoogleTitleTests(Xunit.Abstractions.ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Iniciando WebDriver...");

            var options = new ChromeOptions();
            // rodar sem interface se quiser (descomente para CI/headless)
            // options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

            _output.WriteLine("WebDriver iniciado com sucesso.");
        }

        [Fact(DisplayName = "Valida título da página do Google")]
        public void Google_HomePage_Title_ShouldBe_Google()
        {
            _output.WriteLine("Navegando para https://google.com");
            _driver.Navigate().GoToUrl("https://google.com");

            _output.WriteLine("Aguardando título estabilizar...");
            // tentar estabilizar título por alguns instantes
            Func<string> getTitle = () => _driver.Title;
            var timeout = TimeSpan.FromSeconds(5);
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.Elapsed < timeout)
            {
                _output.WriteLine($"Título atual: '{getTitle()}'");
                if (getTitle().Equals("Google", StringComparison.OrdinalIgnoreCase))
                {
                    _output.WriteLine("Título esperado encontrado.");
                    break;
                }
                System.Threading.Thread.Sleep(250);
            }

            string finalTitle = _driver.Title;
            _output.WriteLine($"Título final para verificação: '{finalTitle}'");

            _output.WriteLine("Validando que o título é exatamente 'Google'.");
            finalTitle.Should().Be("Google", because: "a página inicial do Google deve ter esse título exato");

            _output.WriteLine("Validação concluída com sucesso.");
        }

        public void Dispose()
        {
            _output.WriteLine("Finalizando WebDriver...");
            _driver.Quit();
            _driver.Dispose();
            _output.WriteLine("WebDriver finalizado.");
        }
    }
}
