import time
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.common.exceptions import NoAlertPresentException

def test_login():
    driver = webdriver.Chrome()
    driver.get("http://localhost:18772")
    time.sleep(3)
    
    login_page_button = driver.find_element("xpath","""//*[@id="root"]/div/header/div/a[1]""")
    login_page_button.click()
    time.sleep(2)

    username_field = driver.find_element("xpath","""//*[@id="root"]/div/div/div/form/label[1]/input""")
    password_field = driver.find_element("xpath","""//*[@id="root"]/div/div/div/form/label[2]/input""")

    username_field.send_keys("Ala")
    password_field.send_keys("MaKota")

    login_button = driver.find_element("xpath","""//*[@id="root"]/div/div/div/form/input""")
    login_button.click()
    time.sleep(2)

    points_loc = 0
    if "Wyloguj" in driver.page_source:
        print('Zalogowano pomyślnie')
        points_loc += 1
    else:
        print('Logowanie nieudane')
        return -1

    main_site = driver.find_element("xpath","""//*[@id="root"]/div/header/a""")
    main_site.click()
    time.sleep(2)

    deep_divs = driver.find_elements("xpath","""/html/body/div/div/div/div/div""")
    divs_backwards = deep_divs[::-1]

    for _, div in enumerate(divs_backwards):
        offer_not_reserved = div.find_element("xpath", """//*[text()='Rezerwuj']""")

        if offer_not_reserved is not None:
            print(f"Znaleziono dostępna ofertę")
            offer_not_reserved.click()
            time.sleep(5)
            
            reserved_offers = driver.find_element("xpath", """/html/body/div/div/header/div/a""")
            reserved_offers.click()
            time.sleep(2)
            
            reserved_offer = driver.find_element("xpath", """// *[ @ id = "root"]/div/div/div/div/button""")
            reserved_offer.click()
            time.sleep(5)
            
            try:
                alert = driver.switch_to.alert
                alert_text = alert.text
                alert.accept()
            except NoAlertPresentException:
                print("Nie znaleziono wyskakującego komunikatu.")
                print("Test niepomyślny")
                return -1

            if "udana" in alert_text:
                print('Test oferty pomyślny, płatność zatwierdzona')
                ppoints_loc += 1
            elif "odrzucona" in alert_text:
                print('Test oferty pomyślny, płatność odrzucona')
                points_loc += 1
            else:
                print('Test oferty nieudane')
                return -1
            break
        else:
            pass

    else:
        print("Nie znaleziono wolnej oferty")
        return -1
    
    logout_button = driver.find_element("xpath", """//*[text()='Wyloguj']""")
    if logout_button is not None:
        logout_button.click()
        time.sleep(5)
        
        print('Wylogowano pomyślnie')
        points_loc += 1
    else:
        print('Nie znaleziono przycisku wylogowania')

    return points_loc

if __name__ == '__main__':
    cumulative_points = 0
    trials = 10
    
    for i in range(trials):
        max_points = 3
        points = test_login()
        cumulative_points = cumulative_points + points
        if points != -1:
            print(f'Test zakończony powodzeniem. Uzyskano: {points} punktów na {max_points}')
        else:
            print("Test zakończony niepowodzeniem")
    
    print(f"\n\nSuma punktów:{cumulative_points} / {max_points * trials}")