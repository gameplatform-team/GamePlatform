## Instalação - 

Use o [git](https://git-scm.com/) para clonar este repositório

```shell
git clone git@gitlab.sotran.com.br:sotran/api-tmov.git # SSH

git clone http://gitlab.sotran.com.br/sotran/api-tmov.git # HTTP
```

Download e instalação do Visual Studio 2017 ou 2019.

## Executar projeto 

- Selecionar o enviroment que deseja executar e o projeto irá executar com base no appsettings.[enviroment].json selecionado.

- Importante: Para executar no enviroment DEVELOPMENT, cada Squad deve descomentar as linhas de conexões com banco de dados e apontamentos do seu time, no arquivo `appsettings.development.json`

- Executar projeto com `Ctrl + F5` ou em executar no Visual Studio.

## Build do projeto

| Enviroment  | Descrição                                                      |
| ----------- | -------------------------------------------------------------- |
| development | Build para ambiente de desenvolvimento e apontamento local     |
| hml         | Build para ambiente de homologação e apontamento de homolog    |
| production  | Build para ambiente de desenvolvimento e apontamento produtivo |

## Documentação da API

Acesse a url `[SERVIDOR]/swagger/index.html`

## Build

O build da aplicação para execução em homologação ou produção é realizado pelo [Docker](https://www.docker.com/), fazendo a montagem da imagem e criação do container no servidor.

### Build docker compose

```bash
docker-compose -f docker-compose.yml up --build -d
```

### Build individual

```bash
# CRIAÇÃO DA IMAGEM
docker build -t "api-tmov:1.0.0" .

# REMOVER IMAGENS TEMPORÁRIAS QUE FORAM UTILIZADAS PARA O BUILD
docker image prune --filter label=stage=api-tmov-intermediate

# EXECUTAR APLICAÇÃO EM CONTAINER
## ASPNETCORE_ENVIRONMENT pode variar de acordo com o ambiente que está sendo executado
### production, hml, development ou test
docker run -itd -p 95:80 -v /var/log/api-tmov:/var/log/api-tmov -e ASPNETCORE_ENVIRONMENT=production --hostname=api-tmov --rm --name api-tmov api-tmov:1.0.0

# TESTAR SE O CONTAINER RESPONDE
curl -L http://0:95

# VISUALIZAR LOG DO CONTAINER
sudo docker container ls # (pegar o id do container)
sudo docker logs -f -t # + (id do container)

# LOG DA APLICAÇÃO
sudo docker exec -i -t (id do container) /bin/bash
tail -f 'c:\temp\nlog-all-2020-05-07.log'

# PARA APLICAÇÃO DO CONTAINER
sudo docker container ls # (pegar o id do container)
sudo docker container stop # + (id do container)
```


----

### Entity framework core

https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx
