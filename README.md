# docker-k8s-ws
A small docker-k8s-workshop

# Pre-requisites
- .NET 5
- Docker Desktop
- Azure Cli
- Valid Azure Subscription to deploy in Azure kubernetes service

# Setup couchbase
 - docker run -d --name db -p 8091-8094:8091-8094 -p 11210:11210 couchbase
 - Provide a meaniniful name to the cluster
 - Use Administrator as username and password as password to setup the cluster. Should you prefer a different username or password, maker sure to pass the same as env variables to the container of todo backend or update the docker-compose file accordingly.

 - Run todo backend by using the below command.
 ```
    docker run --rm -d --name todo-backend -e username=<username> -e password=<password> chandanghosh/todo-backend:v1
 ```

