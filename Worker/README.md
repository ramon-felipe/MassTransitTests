Run dockerized rabbitMQ 
Command: docker run -d --hostname my-rabbit --name rabbit-management -p 5672:5672 -p 15672:15672 rabbitmq:3-management

We'll be able to access the management UI through: http://localhost:15672/