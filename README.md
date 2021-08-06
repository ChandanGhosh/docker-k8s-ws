# docker-k8s-ws
A small docker-k8s-workshop

# Pre-requisites
- .NET 5
- Docker Desktop
- Azure Cli
- Valid Azure Subscription to deploy in Azure kubernetes service

# Setup couchbase
 Run couchbase single node pre-configured cluster locally using docker and explore the dashboard using http://localhost:8091
 ```sh
   docker run -d --name db -p 8091-8094:8091-8094 -p 11210:11210 couchbase-configured:latest
 ```
 Use ***Administrator*** as username and ***password*** as password to setup the cluster. Should you prefer a different username or password or optionally the bucket, maker sure to pass the same as env variables to the container. For example,
```sh
   docker run -it -d --name db \
    -e USERNAME=admin \
    -e PASSWORD=admin@password \
    -e BUCKET=todo \
    -p 8091-8094:8091-8094 \
    -p 11210:11210 \
    chandanghosh/couchbase-configured
 ```

 - Build todo backend by using the below command in the todoapp.backend directory
 ```sh
   docker build --rm --no-cache -t chandanghosh/todobackend-couchbase:net5 .
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

# Kubernetes
Use the docker-desktop for deploying the services locally first to check if they works!

```sh
  kubectl apply -f .\k8s\todoapp
```

Check the todo-service and how service type load balancer exposes public IP to localhost to access the website locally with http://localhost . To play with todo service, use the http://localhost/swagger endpoint. This helps without having a front-end ready when developing the backend part.

![swagger-ui](https://github.com/ChandanGhosh/docker-k8s-ws/raw/2-k8s/swagger.PNG "Swagger")
&nbsp;

Use the below command to inspect the couchbase server and open http://localhost:8091 . Username and password are Administrator/password

```sh
  kubectl port-forward deploy/couchbase-service 8091:8091
```
![couchbase-documents](https://github.com/ChandanGhosh/docker-k8s-ws/raw/2-k8s/couchbase.PNG "Couchbase dashboard")
&nbsp;

