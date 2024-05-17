import time
import json
import random
import re
from bs4 import BeautifulSoup as Soup
import chromedriver_autoinstaller
from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.common.exceptions import TimeoutException

def get_localization(localization):
    country = localization[0].find("span").text
    if len(localization) == 2:
        city = localization[1].find("span").text
    elif len(localization) == 3:
        city = localization[2].find("span").text
    else:
        city = ""
        
    return country, city

def get_room_number_info(offer_page):
    hotel_informations = offer_page.find("div", {"id": "accordion__body-HOTEL"}).find_all("div", "odd:rounded-md")
    room_number_info = [""]
    for i in range(len(hotel_informations)):
        if "Liczba pokojów" in hotel_informations[i].find("span").text.strip():
            room_number_info = hotel_informations[i].find_all("li")
            for j in range(len(room_number_info)):
                room_number_info[j] = room_number_info[j].text.strip()
                
    return room_number_info

def get_food_info(offer_page):
    # "none|breakfast|two-dishes|three-dishes|all-inclusive"
    try:
        food_informations = offer_page.find("div", {"id": "accordion__body-FOOD"}) \
            .find_all("article", class_="OfferDetailsBlock_offerDetailsBlock___fVtR")
    except:
        food = 'Not provided'
        return food
    
    food_title = [0] * len(food_informations)
    for i in range(len(food_informations)):
        food_title[i] = food_informations[i].find("h3", class_="heading")
        if food_title[i]:
            food_title[i] = food_title[i].text.strip()
            
    food = "none"
    food_eng = []
    for i in range(len(food_title)):
        if food_title[i]:
            if "all inclusive" in food_title[i].lower():
                food_eng.append("All-Inclusive")
            if "trzy posiłki" in food_title[i].lower():
                food_eng.append("FB")
            if "dwa posiłki" in food_title[i].lower():
                food_eng.append("HB")
            if "śniadanie" in food_title[i].lower():
                food_eng.append("Breakfast")
                
    if len(food_eng) > 0:
        food = random.choice(food_eng)
        
    return food

def get_room_number(offer_page):
    room_number_str = get_room_number_info(offer_page)[0]
    if room_number_str.isnumeric():
        return int(room_number_str)
    
    rooms_info = re.split(':|,| ', room_number_str)
    
    room_number = 0
    for i in range(len(rooms_info)):
        if rooms_info[i].isnumeric():
            room_number += int(rooms_info[i].strip())
            
    if room_number == 0:
        room_number = random.randrange(start=20, stop=100)
        
    return room_number

def get_room_info(offer_page, stars):
    room_number = get_room_number(offer_page)
    room_informations = offer_page.find("div", {"id": "accordion__body-ROOMS"})
    if room_informations:
        room_informations = room_informations.find_all("article", class_="OfferDetailsBlock_offerDetailsBlock___fVtR")
        room_title = [0] * len(room_informations)
        features = [""] * len(room_informations)
        capacity = [0] * len(room_informations)
        for i in range(len(room_informations)):
            room_title[i] = room_informations[i].find("h3", class_="heading")
            if room_title[i]:
                room_title[i] = room_title[i].text.strip()

                room_features = \
                    room_informations[i].find_all("li", class_="OfferDetailsBlock_offerDetailsBlockFeature__Xztov")
                for j in range(len(room_features)):
                    if "liczba osób" in \
                            room_features[j].find("span", class_="OfferDetailsBlock_offerDetailsBlockFeatureName__cviXr").text:
                        capacity[i] = len(room_features[j].find_all("svg"))
                        break
                if capacity[i] == 0:
                    capacity[i] = random.randrange(2, 4)

                room_content = \
                    room_informations[i].find_all("li", class_="OfferDetailsBlock_offerDetailsBlockSpecsListItem--room__lfzDw")
                for j in range(len(room_content)):
                    features[i] += room_content[j].find("span").text.strip()
                    if j < len(room_content) - 1:
                        features[i] += "|"
            else:
                return None

        rooms = [0] * len(room_title)
        number_of_rooms = room_number // len(rooms)
        for i in range(len(rooms)):
            if i == len(rooms) - 1:
                number_of_rooms = room_number - number_of_rooms * (len(rooms) - 1)
            base_price = 40 * stars + 10 * capacity[i]
            if "apartament" in room_title[i].lower():
                base_price *= 1.2
            rooms[i] = RoomTemplate(name=room_title[i], capacity=capacity[i], features=features[i],
                                              number_of_rooms=number_of_rooms, base_price=float(int(base_price)))
        return rooms
    else:
        return None

def get_hotel_images(offer_page, image_number):
    images = offer_page.find_all("div", class_="swiper-wrapper")[0].find_all("img")
    images_urls = [0] * image_number
    k = 0
    for i in range(len(images)):
        if k < image_number:
            if "Hotel" in images[i]['alt'] and "https" in images[i]['src']:
                images_urls[k] = images[i]['src']
                k += 1
                
    # Optional: save images on disk
    # for i in range(len(images_urls)):
    #     response = requests.get(images_urls[i])
    #     with open("images/image" + str(i) + ".jpg", "wb") as f:
    #         f.write(response.content)

    return images_urls

def get_hotel_airport(offer_page):
    airport = offer_page.find("div", {"id": "accordion__body-FLIGHT_INFO"})\
        .find_all("li", class_="flight-info-wrapper__item")
    airport = airport[0].find("div", class_="flight-info__part").find_all("span").pop().text.strip()
    
    return airport

def get_hotel_airport_based_on_country(country):
    with open('data/airports.json', 'r') as outfile:
        airports = json.load(outfile)
        hotel_airports = []
        for airport_name, airport_country in airports.items():
            if airport_country == country:
                hotel_airports += [airport_name]
        if len(hotel_airports) <= 0:
            return None
        hotel_airport = random.choice(hotel_airports)

    return hotel_airport

class Hotel:
    def __init__(self, name, country, city, stars, description, food, photo, rooms, airport):
        self.name = name
        self.country = country
        self.city = city
        self.stars = stars
        self.description = description
        self.food = food
        self.photo = photo
        self.rooms = rooms
        self.airport = airport

    def save(self):
        rooms = self.rooms_to_json()
        if rooms == "Not found":
            pass
        else:
            hotel = {
                "name": self.name,
                "Country": self.country,
                "city": self.city,
                "stars": self.stars,
                "description": self.description,
                "MealsType": self.food,
                "photo": self.photo,
                "Rooms": self.rooms_to_json(),
                "AirportName": self.airport
            }

            with open('data/hotels.json', 'a') as outfile:
                outfile.write(json.dumps(hotel) + "\n")

    def rooms_to_json(self):
        try:
            rooms_result = [0] * len(self.rooms)
            for i in range(len(self.rooms)):
                room = self.rooms[i]
                room_dict = dict()
                room_dict["name"] = room.name
                room_dict["Features"] = room.features
                room_dict["capacity"] = room.capacity
                room_dict["number_of_rooms"] = room.number_of_rooms
                rooms_result[i] = room_dict
            return rooms_result
        except:
            rooms_result = "Not found"
            return rooms_result

class RoomTemplate:
    def __init__(self, name, features, capacity, number_of_rooms, base_price):
        self.name = name
        self.features = features
        self.capacity = capacity
        self.number_of_rooms = number_of_rooms
        self.base_price = base_price

class GetOffers:
    def __init__(self, url, image_number=1, offer_number=350, delay=10):
        offers_number = 0
        
        chromedriver_autoinstaller.install()
        op = webdriver.ChromeOptions()
        op.add_argument('headless')
        driver = webdriver.Chrome(options=op)

        url += "wypoczynek/wyniki-wyszukiwania-samolot"
        print(url)
        driver.get(url)
        try:
            offers = []
            while len(offers) < offer_number:
                WebDriverWait(driver, delay).until(EC.presence_of_element_located((By.CLASS_NAME, 'results-container__button')))
                button_more = driver.find_element(By.CLASS_NAME, "results-container__button")
                if button_more is not None:
                    driver.execute_script("arguments[0].click();", button_more)
                    time.sleep(1)
                offers = Soup(driver.page_source, 'lxml').find_all("div", class_="offer-tile-wrapper")

            WebDriverWait(driver, delay).until(EC.presence_of_element_located((By.CLASS_NAME, 'offer-tile-wrapper')))
            for offer in offers:
                url_curr = url + offer.find("div", "offer-tile-header")['data-hotel-url']
                print(url_curr)
                driver.get(url_curr)
                offer_page = Soup(driver.page_source, "lxml")
                
                # if offer_page is None or offer_page.find("h1", class_="top-section__hotel-name") is None :
                #     break
                
                name = offer_page.find("h1", class_="top-section__hotel-name").text.strip()
                print(str(offers_number) + "\t" + str(name))
                country, city = get_localization(offer.find_all("li", class_="breadcrumbs__item"))
                print(country, city)
                stars = len(offer.find_all("li", class_="Rating_item__Yxpm_"))
                print(stars)
                description_element = offer_page.find("div", {"class": "col-sm-12 col-lg-9"}).find("div", {"class": "accordion"})
                if description_element and offer_page is not None:
                    description = str(offer_page.find("div", {"class": "HotelDescription_hotelDescription___1kS5"}).find("div",{"class": "font-bold"}).contents[0])
                    description = re.sub('<a href=\".*?\">', '', description)
                    food = get_food_info(offer_page)
                    photo = get_hotel_images(offer_page, image_number)
                    if image_number == 1:
                        photo = photo[0]
                    rooms = get_room_info(offer_page, stars)
                    is_flight = offer_page.find((By.CLASS_NAME, 'flight-info-wrapper'))
                    if is_flight is not None:
                        airport = get_hotel_airport(offer_page)
                    else:
                        airport = get_hotel_airport_based_on_country(country)

                    hotel_template = \
                        Hotel(name, country, city, stars, description, food, photo, rooms, airport)
                    hotel_template.save()
                    offers_number += 1

        except TimeoutException:
            print("Loading took too much time!")
        print("NUMBER OF HOTELS: " + str(offers_number))
        driver.close()

if __name__ == "__main__":
    GetOffers(url="https://www.tui.pl/", offer_number=200)
    print("End of scrapping hotels")
