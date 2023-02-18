# Database Migrations
Add-Migration Initial -Output Database/Migrations -Context SignalsContext

# RabbitMQ Docker Instance
docker run -d --hostname localhost --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9.26-management

# Postgres Docker Image
docker run -d --name postgres -p 5432:5432 --network=local -e POSTGRES_USER={{POSTGRES_USER}} -e POSTGRES_PASSWORD={{POSTGRES_PASSWORD}} postgres:15.2

# PgAdmin Docker Image
docker run -d --name pgadmin -p 2345:80 --network=local -e PGADMIN_DEFAULT_EMAIL={{PGADMIN_EMAIL}} -e PGADMIN_DEFAULT_PASSWORD={{PGADMIN_PASSWORD}} dpage/pgadmin4:6.20