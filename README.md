# 🧭 Travel Agency App

> ☣ **Warning:** This project was created during studies for educational purposes only. It may contain non-optimal solutions.

### 📑 About

This project is an application built using **a microservice architecture**. It helps organize tours and share information with travel agencies. Users can use the app to showcase offers, search for trips, and make reservations or purchases.

The application is developed on the **.NET 8.0** platform, with **RabbitMQ** as the message broker, aided by **Mass Transit**. It uses **PostgreSQL** for the database, and **React** for the frontend. The project incorporates **Saga** and **Event Sourcing** patterns. The Saga pattern handles transactions during reservations and resource rollbacks in case of failure, while Event Sourcing is used for booking rooms and flights.

### 🧩 Components

There are several key components, each responsible for a specific part of the application's functionality:

- **Frontend** - Displays information to users and facilitates communication with the API Gateway. Built with React, it runs in the user's browser and does not store data locally.
- **API Gateway** - Handles travel-related requests from the Frontend via REST API and communicates with the Trip Service using RabbitMQ. It also stores and authenticates user information in PostgreSQL.
- **Trip Service** - Aggregates data from the Hotel and Flight Services, manages trip information, and uses PostgreSQL for its database. It includes a Saga for managing the reservation process.
- **Hotel Service** - Manages events related to room reservations, returns booking status, and uses PostgreSQL for its database.
- **Flight Service** - Provides information on available flights, tracks available seats, and manages seat reservation events. It also uses PostgreSQL.
- **Payment Service** - Simulates payment transactions, returning the result of successful or failed payments.

### 🗄️ Databases (PostgreSQL)  

*Database schema for offers* 

<img src="_readme-img/1-TripDB.png?raw=true" width=150 alt="TripDB">

*Database schemas for hotels and rooms*

<img src="_readme-img/1-HotelDB.png?raw=true" width=450 alt="HotelDB">

*Database schemas for flights*

<img src="_readme-img/1-FlightDB.png?raw=true" width=300 alt="FlightDB">

*Database schema for customers*

<img src="_readme-img/1-UserDB.png?raw=true" width=150 alt="UserDB">

### 🚦 Saga

*Dependency diagram between services participating in the Saga*

<img src="_readme-img/2-Saga.png?raw=true" width=500 alt="Saga">

*State machine diagram*

<img src="_readme-img/2-States.png?raw=true" alt="State machine">

The Saga depicted in the diagram implements a trip reservation scenario. It reserves seats on a flight and a room in a hotel by creating events in the appropriate databases. Then, it waits for 60 seconds for a payment confirmation event. If everything goes correctly, the reservation is completed. 

In case of a booking error for the room or flight, or if the payment is not received, events are emitted to cancel the reservations made during the saga process.

### 🚀 Setup

Make sure you have Docker environment installed. Simply run `docker-compose up` in the project directory and open your browser to http://localhost:18772 to access the application (in Polish).

<img src="_readme-img/3-Frontend.png?raw=true" alt="Frontend">
