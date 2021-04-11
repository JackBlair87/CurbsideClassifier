import random
import json

CARS = {
    "VAN" : ("Green", "Van"),
    "TRUCK" : ("Yellow", "Pick-up Truck"),
    "MUSTANG" : ("Blue", "Sports Car"),
    "CHEVY" : ("Brown", "Sedan"),
    "SPORTS" : ("Purple", "Sports Car")
}
#COLOR_OPTIONS = ["Green", "Blue", "Yellow", "Brown", "Purple"]
#CAR_TYPES = ["Van", "Sedan", "Sports Car", "Pick-up Truck"]

MIN_CARS = 0
MAX_CARS = 5
LOTSIZE = 8
SPAWN_RATE = 0.35

NUM_SAMPLES = 1000
FILENAME = "test.txt"


class Scene:
    
    def __init__(self, ):
        self.lot_dict = {}
        sample_pool = list(CARS.items())
        random.shuffle(sample_pool)
        
        for k in range(LOTSIZE):
            if random.random() < SPAWN_RATE:
                if len(sample_pool) > 0:
                    _, car = sample_pool.pop(0)
                    self.lot_dict[k] = Car(car[0], car[1])
            if k not in self.lot_dict:
                self.lot_dict[k] = None
                
        #[print(num, str(elem)) for num, elem in self.lot_dict.items()]
    
    def save(self):
        dict_copy = {}
        for item, car in self.lot_dict.items():
            if car != None:
                dict_copy[item] = car.toJSON()
            else:
                dict_copy[item] = None
        return json.dumps(dict_copy)

class Car:   
    def __init__(self, color, car_type):
        self.color = color
        self.type = car_type
        self.plate = self.generate_random_plate()
    
    def generate_random_plate(self):
        alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        numbers = "0123456789"
        return "".join(random.sample(alpha, 3)) + "-" + "".join(random.sample(numbers, 4))
    
    def __str__(self):
        return "Car --> " + self.color
    
    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__, 
            sort_keys=True, indent=4)
    
           
if __name__=="__main__":
    # c = Car("Green", "Van")
    # print(c.toJSON())
    scenes = { }
    for x in range(NUM_SAMPLES):
        s = Scene()
        scenes[x] = s.save()
    with open(FILENAME, 'w') as out:  
        json.dump(scenes, out)
        