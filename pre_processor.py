# import numpy as np
import os
import PIL
import PIL.Image
import PIL.ImageDraw
# import tensorflow as tf
# import tensorflow_datasets as tfds
import random
import json
import os
import time


import pathlib
data_dir = pathlib.Path('/Users/akashpamal/Documents/TJHSST/Grade11/HackTJ8.0/flower_photos')

SAMPLESIZE = 15
WHITE = (0, 0, 0)
LOT_SIZE = 8
# roses = list(data_dir.glob('roses/*'))
# type(data_dir.glob('roses/*'))
# roses
# PIL.Image.open(str(roses[0]))




def main():
    directory_path = './raw_training_data/'
    json_full_filename = './raw_training_data/image_dictionary.json'
    with open(json_full_filename) as f:
        # dictionary where the key is a string with the filename and the value is a tuple of length 8 that shows the cars in that space. If there's no car in that space, the tuple should contain None
        json_object = json.load(f)
        all_car_models, image_filename_labels = json_object['all_car_models'], json_object['image_filename_labels']
        image_filename_labels = {key : tuple([value_dictionary[str(index)] for index in range(LOT_SIZE)]) for key, value_dictionary in image_filename_labels.items()}
    
    image_output_dictionary = {elem : list() for elem in all_car_models}
    # image_output_dictionary['Empty'] = []

    print('Separating raw input into spaces...')
    # split up the lot into spaces and use the labels to add them to the dictonary above
    for image_filename, lot_keys in image_filename_labels.items():
        filename = os.path.join(directory_path, image_filename)
        lot_image = PIL.Image.open(filename)
        spaces = slice_lot(lot_image)
        for index, car_model in enumerate(lot_keys):
            image = spaces[index]
            if car_model is not None and car_model != 'null': # TODO change 'null' to None immediately after json read
                image_output_dictionary[car_model].append(image)
            else:
                image_output_dictionary['Empty'].append(image)
            # else:

    print('Done separating images')
    
    print('Saving processed images...')
    # save the files in the dictionary in a way that can be used for trainig
    save_directory_overall = './pre_processed_training_data/'
    for folder_name, save_image_list in image_output_dictionary.items():
        if len(save_image_list) == 0:
            print(f'NO DATA FOR "{folder_name}"". OMMITTING ENTRY. WILL NOT BE INCLUDED IN NEURAL NETWORK.')
        directory = os.path.join(save_directory_overall, folder_name)
        if not os.path.exists(directory):
            os.makedirs(directory)
        for index, image in enumerate(save_image_list):
            filename = os.path.join(directory, 'image_' + str(index)+'.png')
            image.save(filename, 'PNG')
    print('Done saving images')

    print('Preprocessing complete')

def sample_pixels(lotSpace):
    return list(zip(random.sample(range(256), k=SAMPLESIZE), random.sample(range(256), k=SAMPLESIZE)))
    
def slice_lot(lot_image):
    # start_left_edge = 16
    white_line_width = 3
    start_edges = [16, 73, 130, 185, 241]
    start_edges = [(start_edges[index], start_edges[index + 1] - white_line_width) for index in range(len(start_edges) - 1)]

    start_edges_temp = [elem for pixel_tuple in start_edges for elem in pixel_tuple]

    # for left_edge in start_edges_temp:
    #     pix = lot_image.load()
    #     print(left_edge)
    #     pix[(left_edge, 200)] = (255, 0, 0)

    # lot_image.show()

    # exit()

    cropped_images = []

    top_row_top_edge = 0
    top_row_bottom_edge = 100

    bottom_row_top_edge = 158
    bottom_row_bottom_edge = 256

    for left_edge, right_edge in start_edges:
        im1 = lot_image.crop((left_edge, top_row_top_edge, right_edge, top_row_bottom_edge))
        im1 = im1.resize((54, 98))
        cropped_images.append(im1)
    for left_edge, right_edge in start_edges:
        im2 = lot_image.crop((left_edge, bottom_row_top_edge, right_edge, bottom_row_bottom_edge))
        im2 = im2.resize((54, 98))
        cropped_images.append(im2)

    return cropped_images

if __name__ == '__main__':
    main()
    # im = PIL.Image.open('./raw_training_data/15.png')
    # slice_lot(im)