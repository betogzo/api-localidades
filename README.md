# API Localidades

API para cadastro e consulta de estados e municípios brasileiros.  

## ✅ Propósito deste projeto

Este projeto foi desenvolvido para participar do Desafio Dotnet organizado pelo [André Baltieri](https://github.com/andrebaltieri) da plataforma de ensino [balta.io](https://balta.io/). O objetivo era entregar uma Minimal API com C# e .NET 7 ou superior, seguindo uma lista de requisitos, se utilizando de boas práticas de desenvolvimento, documentação e também testes unitários. Apesar de ser uma aplicação relativamente pequena e simples em termos de funcionalidades, contém conceitos e tecnologias com as quais eu não era familiarizado, o que trouxe muitos desafios e consequentemente muito aprendizado. Agradecimentos ao Balta!  

## 🏷️ Features

Autenticação e autorização com JWT e Claims;  
Validações de dados;  
Respostas padronizadas e paginadas;   
Busca de Estados e Municípios por diversos parâmetros;   
Cache;  
Documentação completa com Swagger;   
Tratamento global de exceções;  
Cadastro de estados e municípios através de upload de arquivo *.xlsx ou *.xls (Excel);  

## 🚀 Começando

Sinta-se à vontade para clonar, usar e alterar o projeto como quiser.

### 📋 Pré-requisitos

```
.NET SDK versão 7.0 ou superior.
```

### 🔧 Instalação

```
git clone https://github.com/betogzo/api-localidades.git
cd api-localidades
dotnet restore
dotnet run
```

🗄️ Você pode crirar a estrutura inicial da base de dados rodando a migration do EF Core com o comando:

```
dotnet ef database update
```  

### ⚙️ Executando os testes

```
dotnet test
```

## 💡 Populando a base de dados

Você pode usar a planilha Excel presente na pasta "assets" deste repositório para testar o endopoint de "Upload" e consequentemente popular a base de dados com todos os Estados e Municípios do Brasil.  

## 🛠️ Tecnlogias utilizadas

* C# .NET 7
* Entity Framework Core (ORM)
* SQL Server
* Fluent Validation
* Azure Identity
* EPPlus
* Swagger
* xUnit / Moq
* Microsoft Visual Studio 2022  

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para detalhes.



