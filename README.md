# Automação de Validação do Título do Google

Este repositório contém a automação de teste desenvolvido por Leonardo Cunha para a segunda etapa do desafio técnico, com o objetivo específico de:

> Acessar `https://google.com` e validar se o título da página é exatamente **"Google"**.

A solução é implementada com: **C# + XUnit + Selenium WebDriver + FluentAssertions**.

---

## Dependências

As bibliotecas utilizadas são:

- `Selenium.WebDriver`  
  Controla navegadores (interface genérica: `IWebDriver`, `IWebElement`, `By`, etc).

- `Selenium.WebDriver.ChromeDriver`  
  Driver específico para Chrome. 

- `xunit`  
  Framework de testes que define e executa os testes unitários e de integração.

- `xunit.runner.visualstudio`  
  Integração para execução padrão via `dotnet test`.

- `FluentAssertions`  
  Biblioteca de assertions com sintaxe legível.

---

## Estrutura do teste

Arquivo: `GoogleTitleTests.cs`

```csharp
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

        public GoogleTitleTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);
        }

        [Fact(DisplayName = "Valida título da página do Google")]
        public void Google_HomePage_Title_ShouldBe_Google()
        {
      
            _driver.Navigate().GoToUrl("https://google.com");

            string title = _driver.Title;

            title.Should().Be("Google", because: "a página inicial do Google deve ter esse título exato");
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
```

---

## Explicação 

### `using` statements
Essas linhas no topo importam namespaces (bibliotecas) necessárias:

- `System`: classes base do .NET (`TimeSpan`, etc).  
- `OpenQA.Selenium`: interface genérica do Selenium (`IWebDriver`, `By`, etc).  
- `OpenQA.Selenium.Chrome`: classes específicas do Chrome (`ChromeDriver`, `ChromeOptions`).  
- `Xunit`: marca o método como teste e estrutura de teste.  
- `FluentAssertions`: permite fazer asserções legíveis como `title.Should().Be("Google")`.

### Configuração do WebDriver
- `ChromeOptions` é usado para ajustar o comportamento do navegador (ex.: desabilitar GPU, remover sandbox; headless opcional para ambientes sem interface).  
- `new ChromeDriver(options)`: instancia o navegador Chrome controlado.  
- Timeout de carregamento de página é definido para evitar waiting.

### Teste
- O atributo `[Fact]` identifica o método como caso de teste executável pelo XUnit.  
- A navegação é feita com `_driver.Navigate().GoToUrl(...)`.  
- O título é obtido via `_driver.Title`.  
- A validação usa FluentAssertions para garantir que o título seja exatamente **"Google"**; a mensagem `because:` dá contexto em caso de falha.

---

## Executando

### Pré-requisitos
- .NET SDK 7.0+ instalado  
- Google Chrome instalado  

### Passos

1. Restaurar pacotes:
   ```bash
   dotnet restore
   ```

2. Rodar os testes:
   ```bash
   dotnet test
   ```

O comando acima compila e executa o teste. O resultado esperado é um teste com status **Passed** se o título for exatamente **"Google"**.

---

## Considerações por sistema operacional

### Windows
- Instalar o .NET SDK via instalador oficial.  
- Chrome já instalado normalmente funciona; o Selenium Manager resolve o ChromeDriver automaticamente.  
- Execute `dotnet test` no terminal (PowerShell ou CMD).

### Ubuntu
- Instale o .NET SDK (exemplo para Ubuntu 22.04+):
  ```bash
  sudo apt update
  sudo apt install -y wget apt-transport-https
  wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  sudo apt update
  sudo apt install -y dotnet-sdk-7.0
  ```
- Instale o Chrome.  
- Para ambientes sem interface (server/CI), habilite o headless:
  ```csharp
  options.AddArgument("--headless=new");
  ```

---

## Saída esperada

Ao rodar:

```
> dotnet test

Test run for GoogleTitleTest.dll (.NETCoreApp,Version=v7.0)
Starting test execution, please wait...
GoogleTitleTest teste êxito (Xs)

Resumo do teste: total: 2; falhou: 0; bem-sucedido: 2; ignorado: 0; duração: Xs
Construir êxito em Xs
```

---

