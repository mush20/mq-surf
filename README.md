## Mq Surf

### Start Redis MQ Server
`
docker-compose up`

### Run Mq.Host
on terminal

`cd Mq.Host && dotnet run`

### Run Mq.Client
on another terminal

`cd Mq.Client && dotnet run`


### Create new Product via message
`
curl --header "Content-Type: application/json" \
  --request POST \
  --data '{"Name":"xyz","Description":"xyz"}' \
  http://localhost:5000/api/values`
  
### Product list
Browse to http://localhost:5002/api/products

or

`curl http://localhost:5002/api/products`
