# GameProject

## Descrição

O **GameProject** é um projeto de jogo multiplayer em tempo real, desenvolvido utilizando ASP.NET Core para o backend e SignalR para a comunicação em tempo real entre o servidor e os clientes. O objetivo do jogo é permitir que jogadores se registrem, avancem turnos, iniciem uma contagem regressiva e determinem o vencedor com base em tempo acumulado.

### Funcionalidades

- **Registro de Jogadores**: Jogadores podem se registrar e entrar no jogo.
- **Contagem Regressiva**: Quando o número de jogadores for maior que 1, uma contagem regressiva de 30 segundos é iniciada.
- **Turnos dos Jogadores**: O jogo alterna entre os jogadores, dando-lhes um tempo acumulado durante seu turno.
- **Condição de Vitória**: O jogo termina quando sobra apenas um jogador, que é declarado vencedor.
- **Reinício do Jogo**: O jogo pode ser reiniciado a qualquer momento, limpando o estado atual e permitindo um novo jogo.

## Estrutura do Projeto

O projeto está dividido nas seguintes camadas:

- **GameProject.API**: Contém os controladores da API e a configuração do servidor web.
- **GameProject.Application**: Contém a lógica do jogo, incluindo a comunicação com o SignalR e a manipulação do estado do jogo.
- **GameProject.Domain**: Define os modelos de dados, como `Player` e `GameState`.
- **GameProject.Hubs**: Implementa o SignalR Hub para a comunicação em tempo real entre o servidor e os clientes.

## Detalhes Técnicos

### Backend

- **ASP.NET Core**: O backend é construído com o framework ASP.NET Core, utilizando a arquitetura de API REST.
- **SignalR**: Utilizado para comunicação em tempo real entre o servidor e os clientes, permitindo que atualizações do estado do jogo sejam enviadas instantaneamente para todos os jogadores conectados.
- **Services**: O jogo é gerenciado através de dois serviços principais:
  - **GameSessionService**: Gerencia os jogadores, permitindo adicionar, listar e limpar jogadores.
  - **GameService**: Gerencia a lógica do jogo, incluindo turnos, contagem regressiva e verificação de condições de vitória.
- **GameHub**: Um hub SignalR que envia e recebe mensagens dos clientes, notificando-os sobre o estado do jogo, jogadores, contagem regressiva, e mais.

### Estado do Jogo

- **Players**: Lista de jogadores que estão participando da sessão de jogo.
- **CurrentPlayer**: Jogador que está atualmente em turno.
- **GameStarted**: Indicador que determina se o jogo foi iniciado.
- **ContagemIniciada**: Indicador de que a contagem regressiva foi iniciada.
- **Status**: Mensagem de status atual do jogo, como "Aguardando jogadores" ou "Jogo em andamento".
- **AccumulatedTime**: Tempo acumulado de cada jogador durante seus turnos.

### Comunicação em Tempo Real

Utilizando SignalR, a comunicação entre o servidor e os clientes é feita em tempo real. As seguintes mensagens são enviadas aos clientes:

- **PlayersUpdated**: Atualização da lista de jogadores.
- **GameStateUpdated**: Atualização do estado do jogo.
- **GameStatusUpdated**: Atualização do status do jogo (por exemplo, "Vez do jogador X").
- **CountdownUpdated**: Atualização da contagem regressiva.
- **Error**: Notificação de erro caso ocorra algum problema.

### Fluxo de Jogo

1. Jogadores se registram via a API `POST /api/game/register`.
2. A contagem regressiva é iniciada quando o número de jogadores é maior que 1.
3. O jogo alterna entre os jogadores, acumulando tempo durante os turnos.
4. Quando o tempo de um jogador ultrapassa o limite (30 segundos), ele é removido da partida.
5. O jogo termina quando sobra apenas um jogador.
6. O jogo pode ser reiniciado a qualquer momento.

### Dependências

- **ASP.NET Core 6.0**
- **SignalR**
- **Entity Framework Core** 

---
