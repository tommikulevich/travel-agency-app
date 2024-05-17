import json
import random
from datetime import datetime, timedelta, time
import json

def generate_flights():
    with open('data/hotels.json', 'r') as f:
        data = f.read()
        objects = data.split('\n')
        hotels = [json.loads(obj) for obj in objects if obj.strip()]

    with open('data/polishAirports.json', 'r') as f:
        airports = f.read().split('|')

    flights = []
    for hotel in hotels:
        destination = hotel['AirportName']
        duration = random.randint(90, 240)
        for i in range(4*4*3):  # 4 month, 4 weeks a month, 3 flights a week
            departure = random.choice(airports)
            date = datetime.now() + timedelta(days=i*(7/3)+1)
            date = date.combine(date, time(random.randint(0, 23), random.randint(0, 59), 00))

            num_of_seats_list = [189, 210,186,230,244]
            names_list = ['LOT', 'Ryanair Sun', 'EnterAir', 'WizzAir', 'RyanAir']
            DepartureTime= f"{date.strftime('%Y-%m-%d')} {date}"

            ArrivalTime = f"{date.strftime('%Y-%m-%d')} {date + timedelta(minutes=duration)}"

            flights.append({
                'Name': random.choice(names_list),
                'DeparturePlace': departure,
                'ArrivalPlace': destination,
                'DepartureTime': DepartureTime,
                'ArrivalTime': ArrivalTime,
                'NumOfSeats': random.choice(num_of_seats_list)
            })

    with open('data/flights_generated.json', 'w') as f:
        for flight in flights:
            f.write(json.dumps(flight) + "\n")

if __name__ == "__main__":
    generate_flights()
    print("End of generating flights")
