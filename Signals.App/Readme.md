# Database Migrations
Add-Migration Initial -Output Database/Migrations -Context SignalsContext

# RabbitMQ Docker Instance
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 --network local --hostname localhost -e RABBITMQ_DEFAULT_USER=dev -e RABBITMQ_DEFAULT_PASS=!Development123 rabbitmq:3.9.26-management

# Postgres Docker Instance
docker run -d --name postgres -p 5432:5432 --network local -e POSTGRES_USER=dev -e POSTGRES_PASSWORD=!Development123 postgres:15.2

# PgAdmin Docker Instance
docker run -d --name pgadmin -p 2345:80 --network local -e PGADMIN_DEFAULT_EMAIL=dev@dev.dev -e PGADMIN_DEFAULT_PASSWORD=!Development123 dpage/pgadmin4:6.20