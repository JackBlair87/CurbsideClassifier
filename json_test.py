import json
  
# Data to be written
dictionary = {
    "all_car_models" : ["Yellow", 'Green', 'brown'],
    "image_filename_labels" : {'image_15.png' : (None, 'Yellow', None, None, None, None, None, None)},
}
  
# Serializing json 
json_object = json.dumps(dictionary)
  
# Writing to sample.json
with open("./raw_training_data/image_dictionary.json", "w") as outfile:
    outfile.write(json_object)