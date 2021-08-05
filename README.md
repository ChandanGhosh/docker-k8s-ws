# docker-k8s-ws
A small docker-k8s-workshop

# Pre-requisites
- .NET 5
- Docker Desktop
- Azure Cli
- Valid Azure Subscription to deploy in Azure kubernetes service

# Setup couchbase
 
 ```sh
 docker run -d --name db -p 8091-8094:8091-8094 -p 11210:11210 couchbase-configured:latest
 ```
 Use Administrator as username and password as password to setup the cluster. Should you prefer a different username or password, maker sure to pass the same as env variables to the container of todo backend or update the docker-compose file accordingly. For example,
```sh
 docker run -it -d --name db \                                                                        130 ↵
  -e USERNAME=Administrator \     
  -e PASSWORD=password \
  -e BUCKET=todo \                 
  -p 8091-8094:8091-8094 \
  -p 11210:11210 \
  chandanghosh/couchbase-configured
 ```

 - Run todo backend by using the below command.
 ```sh
    docker run --rm -d --name todo-backend chandanghosh/todobackend-couchbase:net5
 ```

# Docker compose
Use the docke-compose to run multiple container automatically.

```sh
docker-compose up
```


