# GamePlatform

## 📋 Sobre o Projeto
GamePlatform é uma aplicação .NET 8.0 desenvolvida seguindo os princípios da Clean Architecture, oferecendo uma plataforma robusta e escalável para gerenciamento de jogos.

## 🏗️ Arquitetura

O projeto está estruturado em camadas:

- **GamePlatform.Api**: Camada de apresentação que expõe as APIs RESTful
- **GamePlatform.Application**: Contém a lógica de aplicação e casos de uso
- **GamePlatform.Domain**: Define as entidades de domínio e regras de negócio
- **GamePlatform.Infrastructure**: Implementa o acesso a dados e serviços externos
- **GamePlatform.Tests**: Projeto de testes unitários

## 🚀 Como Executar

### Pré-requisitos
- .NET SDK 8.0 ou superior
- Uma IDE compatível (recomendado: Visual Studio, JetBrains Rider ou VS Code)

### Passos para Execução

1. Clone o repositório:

```bash
git clone https://github.com/rafaelozelin/GamePlatform.git
```

2. Navegue até a pasta do projeto:
```bash
cd GamePlatform
``` 

3. Restaure as dependências:
```bash
dotnet restore
``` 

4. Execute a aplicação:
```bash
cd GamePlatform.Api
``` 
```bash
dotnet run
``` 

A API estará disponível em `http://localhost:5232`.

Você pode executar as requisições através do Swagger: `http://localhost:5232/swagger/index.html`.

## 🧪 Executando os Testes

Para executar os testes unitários:
```bash
dotnet test
```

## 🛠️ Tecnologias Utilizadas

- ASP.NET Core 8.0
- C# 12.0
- Clean Architecture
- Testes Unitários

## 📦 Estrutura da Solução

```plaintext
GamePlatform/
├── GamePlatform.Api/            # API endpoints e configurações
├── GamePlatform.Application/    # Casos de uso e lógica de aplicação
├── GamePlatform.Domain/         # Entidades e regras de negócio
├── GamePlatform.Infrastructure/ # Implementações de repositórios e serviços
└── GamePlatform.Tests/          # Testes unitários
```

## 🔄 CI/CD

O projeto utiliza GitHub Actions para automação de CI/CD, incluindo:
- Build e testes automatizados
- Build e push de imagem Docker
- Deploy automático para Azure Web App
