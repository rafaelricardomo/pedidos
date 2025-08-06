# Envio de pedidos

O sistema de envio de pedidos é uma **API** que possibilita consultar e criar novos pedidos integrados com faturamento.

1. **Consultar pedido** - Consulta de pedido por identificador gerado na criação
2. **Criação de Pedido** - Criar um novo pedido com envio de integração


# Regras de negócio

1. Criar novo pedido com calculo de valores de itens e totais.
2. Envio de pedido para integração de faturamento.
3. Consultar pedido gerado pela criação.


# Débitos técnicos

1. Melhoria nos pontos de validações e padrão de retorno (ProblemDetails) dos endpoints das apis 
2. Implementar tratamentos de erros com logs e respostas nos endpoints.

# Instruções para docker desktop

1. Acesse a pasta "pedidos" do projeto via terminal com comando
**cd pedidos**

2. Execute o comando para construir a aplicação juntamente com a infra necessária
**docker compose up --build**

3. Acessa as pastas do projeto Pedidos.Infrastructure e utilize o script no sql server
**ScriptPedidosDb.sql**

4. Acesso endereço no seu navegador
**http://localhost:5000/swagger**
