import json
import time
import datetime
import chromedriver_autoinstaller
from bs4 import BeautifulSoup as bs
from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.chrome.service import Service

class Flight:
    def __init__(self, departure_airport, arrival_airport, date, departure_time, arrival_time, travel_time):
        self.departure_airport = departure_airport
        self.arrival_airport = arrival_airport
        self.departure_date, self.arrival_date = self.get_dates(departure_time, arrival_time, date)
        travel_hour, travel_min = travel_time.split("h")[0], travel_time.split("h")[1].split("min")[0]
        self.travel_time = int(travel_hour) * 60 + int(travel_min)

    def get_dates(self, departure_time, arrival_time, date):
        date_day, date_month, date_year = map(int, date.split("."))
        departure_hour, departure_mins = map(int, departure_time.split(":"))
        arrival_hour, arrival_mins = map(int, arrival_time.split(":"))
        departure_date = datetime.datetime(date_year, date_month, date_day, departure_hour, departure_mins)
        arrival_date = datetime.datetime(date_year, date_month, date_day, arrival_hour, arrival_mins)
        
        if arrival_date < departure_date:
            arrival_date += datetime.timedelta(days=1)
            
        departure_date = departure_date.strftime("%d.%m.%Y %H:%M")
        arrival_date = arrival_date.strftime("%d.%m.%Y %H:%M")
        
        return departure_date, arrival_date

    def save(self):
        flight = {
            "departureAirport": self.departure_airport,
            "arrivalAirport": self.arrival_airport,
            "departureDate": self.departure_date,
            "arrivalDate": self.arrival_date,
            "travelTime": self.travel_time
        }

        with open('data/flights.json', 'a') as outfile:
            outfile.write(json.dumps(flight, default=str) + "\n")

class GetFlights:
    def __init__(self, url, click_more_times=4):
        self.delay = 10
        self.total_flights = 0
        self.polish_airports = []
        self.airports = {}
        self.url = url + "zaawansowana-wyszukiwarka-lotow"
        self.click_more_times = click_more_times
        self.driver = self.setup_driver()
        self.get_flights()

    def setup_driver(self):
        chromedriver_autoinstaller.install()
        options = webdriver.ChromeOptions()
        options.add_argument('--headless')
        options.add_experimental_option('excludeSwitches', ['enable-logging'])
        return webdriver.Chrome(service=Service(), options=options)

    def get_flights(self):
        self.driver.get(self.url)
        try:
            WebDriverWait(self.driver, self.delay).until(
                EC.presence_of_element_located((By.CLASS_NAME, 'charters-destination-item__button-text')))
            self.process_airports()
        except TimeoutException:
            print("Loading took too much time!")

        print("NUMBER OF FLIGHTS: " + str(self.total_flights))
        
        self.save_data()
        self.driver.close()

    def process_airports(self):
        airport_names = bs(self.driver.page_source, 'lxml').find_all("p", class_="charters-destination-item__name")
        airport_countries = bs(self.driver.page_source, 'lxml').find_all("p",
                                                                         class_="charters-destination-item__country")
        buttons = self.driver.find_elements(By.CLASS_NAME, "charters-destination-item__button-text")
        airports_number = len(buttons)

        for i in range(airports_number):
            if i > 0:
                self.driver.get(self.url)
                WebDriverWait(self.driver, self.delay).until(
                    EC.presence_of_element_located((By.CLASS_NAME, 'charters-destination-item__button-text')))
                buttons = self.driver.find_elements(By.CLASS_NAME, "charters-destination-item__button-text")
                
            self.airports[airport_names[i].text.strip()] = airport_countries[i].text.strip()
            self.driver.execute_script("arguments[0].click();", buttons[i])
            WebDriverWait(self.driver, self.delay).until(
                EC.presence_of_element_located((By.CLASS_NAME, 'charters-offers-list__load-more-wrapper')))

            for _ in range(self.click_more_times):
                load_more_button = bs(self.driver.page_source, "lxml").find("div",
                                                                            "charters-offers-list__load-more-wrapper")
                if load_more_button:
                    load_more_button = self.driver.find_element(By.CLASS_NAME,
                                                                "charters-offers-list__load-more-wrapper")
                    load_more_button = load_more_button.find_element(By.CLASS_NAME, "button")
                    if load_more_button is not None:
                        self.driver.execute_script("arguments[0].click();", load_more_button)
                        time.sleep(2.5)

            flights_page = bs(self.driver.page_source, 'lxml').find_all("div", class_="charters-offers-list__item")
            flights = []

            for f in range(len(flights_page)):
                flight, self.polish_airports = self.get_flight_pair(flights_page[f], self.polish_airports)
                flights += flight

            for flight in flights:
                flight.save()
                
            self.total_flights += len(flights)

    def get_flight_pair(self, flight_pair_page, polish_airports):
        first = flight_pair_page.find("div", class_="charters-offer-item__wrapper--departure")
        second = flight_pair_page.find("div", class_="charters-offer-item__wrapper--return")
        day1 = first.find("span", class_="charters-offer-flight-data__heading-date").text.strip()
        day2 = second.find("span", class_="charters-offer-flight-data__heading-date").text.strip()
        details1 = first.find_all("div", class_="charters-offer-flight-data__details")
        details2 = second.find_all("div", class_="charters-offer-flight-data__details")
        dep1 = details1[0].find_all("span")
        arr1 = details1[1].find_all("span")
        dep2 = details2[0].find_all("span")
        arr2 = details2[1].find_all("span")
        travel_time1 = first.find_all("span", class_="charters-offer-flight-data-description__value")[0].text.strip()
        travel_time2 = first.find_all("span", class_="charters-offer-flight-data-description__value")[0].text.strip()

        if dep1[1].text.strip() not in polish_airports:
            polish_airports += [dep1[1].text.strip()]

        if arr2[1].text.strip() not in polish_airports:
            polish_airports += [arr2[1].text.strip()]

        flight_pair = [Flight(departure_airport=dep1[1].text.strip(), arrival_airport=arr1[1].text.strip(),
                              date=day1, travel_time=travel_time1,
                              departure_time=dep1[0].text.strip(), arrival_time=arr1[0].text.strip()),

                       Flight(departure_airport=dep2[1].text.strip(), arrival_airport=arr2[1].text.strip(),
                              date=day2, travel_time=travel_time2,
                              departure_time=dep2[0].text.strip(), arrival_time=arr2[0].text.strip())]

        return flight_pair, polish_airports

    def save_data(self):
        with open('data/airports.json', 'a') as outfile:
            outfile.write(json.dumps(self.airports, default=str))
        with open('data/polishAirports.json', 'a') as outfile:
            for j in range(len(self.polish_airports)):
                line = self.polish_airports[j]
                if j < len(self.polish_airports) - 1:
                    line += "|"
                outfile.write(line)

if __name__ == "__main__":
    GetFlights(url="https://www.tui.pl/", click_more_times=0)
    print("End of scrapping flights")
