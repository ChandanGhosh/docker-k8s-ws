# docker-k8s-ws
A small docker-k8s-workshop

# Pre-requisites
- Linux Shell (***Windows users can use Ubuntu in WSL2***)
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

# Kubernetes-azure

- Login to azure subscription using azure cli
```sh
  az login
```

- get your subscription id

```sh
  # If jq not installed in the machine, install it.
  export SUBID=$(echo $(az account show) | jq -r '.id')
```

- Set the default variables as needed

```sh
export SUFFIX=$(echo $RANDOM)
export NODE_COUNT=2
export AKS_GRP=aks-grp-$SUFFIX
export LOC=centralindia
export AKS_NAME=aks-$SUFFIX
export TENANTID=$(echo $(az account show) | jq -r '.tenantId')
export SUBID=$(echo $(az account show) | jq -r '.id')
```
- Set default subscription

```sh
  az account set --subscription $SUBID
```
- Create azure group in your preferred location. $LOC used here is ***centralindia***

```sh
  az group create -n $AKS_GRP -l $LOC --tags label=$SUFFIX
```

- Create azure kubernetes service for our deployment

```sh
  az aks create -n $AKS_NAME -g $AKS_GRP --node-count $NODE_COUNT --network-plugin azure --generate-ssh-keys
```

- Get AKS credentials to work with kubectl

```sh
  az aks get-credentials --name $AKS_NAME -g $AKS_GRP
```

- Set kubernetes context to AKS now
```sh
  kubectl config use-context $AKS_NAME
```
- Check cluster information
```sh
  kubectl cluster-info
```

- Deploy our todo backend service and couchbase singlenode cluster in the azure kubernetes service as done in docker-desktop

```sh
  kubectl apply -f k8s/todoapp
```
- Now the todo backend will be automatically be exposed in the internet. Azure kubernetes service will create a public IP so that the app can be accessed from internet.

  Find the public IP of the app and access the app in the browser with the public IP

```sh
  kubectl get svc todo-service
```

- You should be able to do all the CRUD operations with the swagger endpoint as well as done in local docker-desktop cluster.

- Use the below command to inspect the couchbase server and open http://localhost:8091 . Username and password are Administrator/password

```sh
  kubectl port-forward deploy/couchbase-service 8091:8091
```

- ***[Optional]*** Once done, you should cleanup azure resources to save cost

```sh
  for rg in $(az group list --tag label=$SUFFIX --query '[].name' | jq -r '.[]'); \
  do echo "Delete Resource Group: ${rg}"; \
  az group delete -n ${rg}; done
```
- ***[Optional]*** To only delete the kubernetes cluster
 ```sh
  az aks delete --name $AKS_NAME --resource-group $AKS_GRP
```
