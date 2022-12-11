# Database Migrations
Add-Migration Initial -Output Database/Migrations

# RabbitMQ Docker Instance
docker run -d --hostname localhost --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9.26-management