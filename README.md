# Sistema de Pizzaria (WPF + EF Core + SQLite)

## Como rodar
1. Abra a pasta `PizzariaApp` no Visual Studio 2022 (ou `dotnet build` / `dotnet run` pelo terminal, com o SDK do .NET 8 instalado).
2. Ao restaurar os pacotes NuGet, o pacote `Pomelo.EntityFrameworkCore.MySql` será baixado automaticamente (é o provedor de EF Core para MySQL).
3. O projeto já está configurado para se conectar ao MySQL definido em `Data/PizzariaContext.cs` (`ConnectionString`). Ao rodar pela primeira vez, `Database.EnsureCreated()` cria as tabelas (`Clientes`, `Produtos`, `Pedidos`, `ItensPedido`) nesse banco, caso ainda não existam, e o cardápio inicial é inserido — veja `Data/DbInitializer.cs`.
4. A janela inicial (`MainWindow`) tem dois botões:
   - **Novo Pedido (Atendimento)** → Tela 1.
   - **Painel da Cozinha** → Tela 2.
5. Abra as duas janelas ao mesmo tempo para simular o fluxo real: cadastre um pedido na Tela 1 e veja ele aparecer no Painel da Cozinha (atualiza automaticamente a cada 5 segundos).

## Sobre a string de conexão
⚠️ **Importante:** a string de conexão com usuário e senha está fixa no código (`Data/PizzariaContext.cs`) só para o projeto já vir "pronto para rodar" com o banco que você passou. Em um projeto real, evite deixar credenciais no código-fonte — prefira:
- Ler de uma variável de ambiente, ou
- Um arquivo `appsettings.json` / `App.config` que fique fora do controle de versão (`.gitignore`), ou
- Um cofre de segredos (ex.: Azure Key Vault, User Secrets do .NET).

Além disso, troque a senha do banco assim que possível, já que ela foi compartilhada em texto puro aqui na conversa.

## Estrutura
- `Models/` — Entidades: Cliente, Produto, Pedido, ItemPedido, enums StatusPedido/TamanhoPizza.
- `Data/` — `PizzariaContext` (DbContext) e `DbInitializer` (seed do cardápio).
- `Views/` — `MainWindow`, `NovoPedidoWindow` (Tela 1), `PainelCozinhaWindow` (Tela 2), e ViewModels de apoio (`ItemCarrinho`, `PedidoCardViewModel`).
- `Converters/` — Conversores de XAML (cor/texto do status) e `RelayCommand` para os botões de ação nos cards de pedido.

## Fluxo de status do pedido
`Pendente` → `EmPreparo` → `ProntoParaEntrega` → (`Entregue`, não exibido mais no painel)

Os botões "Em Preparo" e "Pronto para Entrega" ficam habilitados/desabilitados automaticamente conforme o status atual de cada pedido (via `PedidoCardViewModel`).
