# Rota de Viagem #
Escolha a rota de viagem mais barata independente da quantidade de conex�es.
Para isso precisamos inserir as rotas.

# API
## CRUD de cadastro de ROTAS ##
* Dever� construir um endpoint de CRUD as rotas dispon�veis:
```
Origem: GRU, Destino: BRC, Valor: 10
Origem: BRC, Destino: SCL, Valor: 5
Origem: GRU, Destino: CDG, Valor: 75
Origem: GRU, Destino: SCL, Valor: 20
Origem: GRU, Destino: ORL, Valor: 56
Origem: ORL, Destino: CDG, Valor: 5
Origem: SCL, Destino: ORL, Valor: 20
```

## Explicando ## 
Uma viajem de **GRU** para **CDG** existem as seguintes rotas:

1. GRU - BRC - SCL - ORL - CDG ao custo de $40
2. GRU - ORL - CDG ao custo de $61
3. GRU - CDG ao custo de $75
4. GRU - SCL - ORL - CDG ao custo de $45

O melhor pre�o � da rota **1**, apesar de mais conex�es, seu valor final � menor.
O resultado da consulta deve ser: **GRU - BRC - SCL - ORL - CDG ao custo de $40**.

Sendo assim, o endpoint de consulta dever� efetuar o calculo de melhor rota.

# API .net core
1- Cadastro: CRUD de Rotas
2- Consulta: Dever� ter 2 campos para consulta de rota: **Origem-Destino** e exibir o resultado da consulta chamando a API
	
- Interface Rest (Obrigat�rio)
    A interface Rest dever� suportar o CRUD de rotas:
    - Manipula��o de rotas, dados podendo ser persistidos em arquivo, bd local, etc...
    - Consulta de melhor rota entre dois pontos.
	- Documento swagger
	- Testes unit�rios
	
  Exemplo:
  ```
  Consulte a rota: GRU-CGD
  Resposta: GRU - BRC - SCL - ORL - CDG ao custo de $40
  
  Consulte a rota: BRC-SCL
  Resposta: BRC - SCL ao custo de $5
  ```
