# Projetos .NET

Esta pasta contém projetos desenvolvidos em **.NET / C#**, sendo as aplicações presentes nela do tipo **Console**.

## Atenção: Smart App Control do Windows

Por serem aplicações do tipo **Console em .NET**, os projetos geram arquivos `.dll` durante o processo de compilação, que são utilizados para executar as aplicações.

Por exemplo, um projeto como `ComissaoVendas` pode gerar:

```text
bin\Debug\net10.0\ComissaoVendas.dll
```

Dependendo das configurações de segurança do Windows, o **Smart App Control (Controle Inteligente de Aplicativos)** pode bloquear a execução desses arquivos.

Nesse caso, ao executar:

```powershell
dotnet run
```

pode ser apresentado um erro semelhante a:

```text
System.IO.FileLoadException:
Uma política de Controle de Aplicativo bloqueou este arquivo.
(0x800711C7)
```

Esse bloqueio pode ocorrer mesmo que o projeto e seu código sejam legítimos. Trata-se de uma política de segurança do próprio Windows relacionada à confiança e reputação do arquivo.

## Como desabilitar o Smart App Control

Caso uma das aplicações seja bloqueada pelo Windows, acesse:

```text
Configurações
└── Privacidade e segurança
    └── Segurança do Windows
        └── Controle de aplicativos e navegador
            └── Controle Inteligente de Aplicativos
```

Na tela **Controle Inteligente de Aplicativos**, altere a opção para:

```text
Desativado
```

Depois, abra novamente o terminal e execute o projeto normalmente:

```powershell
dotnet run
```

## Observação de segurança

O **Smart App Control** é um mecanismo de segurança do Windows. Desativá-lo reduz uma camada de proteção do sistema.

Recomenda-se desativá-lo somente quando necessário e para executar código-fonte em que você confia.

## Execução dos projetos

Cada projeto possui seu próprio diretório. Entre no diretório do projeto desejado e execute:

```powershell
dotnet restore
dotnet build
dotnet run
```

### Exemplo

Para um projeto chamado `ComissaoVendas`:

```powershell
cd ComissaoVendas
dotnet run
```

Os mesmos procedimentos se aplicam aos demais projetos .NET presentes nesta pasta.
