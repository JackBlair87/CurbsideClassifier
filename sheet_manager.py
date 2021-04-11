import gspread
from oauth2client.service_account import ServiceAccountCredentials
import random

class SheetManager():
    # use creds to create a client to interact with the Google Drive API
    def __init__(self) -> None:
        scope = ['https://spreadsheets.google.com/feeds',
                'https://www.googleapis.com/auth/drive']
        creds = ServiceAccountCredentials.from_json_keyfile_name('client_secret.json', scope)
        client = gspread.authorize(creds)

        # Find a workbook by name and open the first sheet
        # Make sure you use the right name here.
        self.sheet = client.open("Curbside Classifier").sheet1
        # self.sheet.clear()

    def add_order(self, space_number, car_type, order_number):
        # print(sheet.get_all_values())
        print('The order number is', order_number)
        row = [space_number, car_type, order_number]
        index = 2
        self.sheet.insert_row(row, index)
        # self.sheet.clear
    
    def add_scene(self, cars_in_spaces):
        self.sheet.clear()
        self.sheet.insert_row(["Space number", "Car type", "Order number"], 1)
        for car in cars_in_spaces:
            self.add_order(1, car, str(random.randrange(0, 1000)))