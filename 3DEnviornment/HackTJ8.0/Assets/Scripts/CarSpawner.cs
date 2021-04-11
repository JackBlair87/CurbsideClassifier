using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using System.IO;
 using System;
 
 public class CarSpawner : MonoBehaviour {
     public int NUM_SAMPLES;

     public GameObject van;
     public GameObject truck;
     public GameObject mustdang;
     public GameObject chevy;
     public GameObject sports;

     public int curr = 0;
     public int NumSpots = 8; 

     public Dictionary<int, Vector3> dict;
     ArrayList currObjects = new ArrayList(); //cars loaded in the scene
     // Use this for initialization
     ArrayList scenes;
     void Start () {
        string path = Application.dataPath + "/TrainingImages/positions.txt";

        Dictionary<int, float> verticalPositions = new Dictionary<int, float>();
        verticalPositions[0] = 0f;
        verticalPositions[1] = 1.6f;
        verticalPositions[2] = 1.1f;
        verticalPositions[3] = 0.9f;
        verticalPositions[4] = 0.9f;

        dict = new Dictionary<int, Vector3>();
        for(int x = 0; x < 5; x++){
            dict[x*NumSpots + 0] = new Vector3(-12.5f, verticalPositions[x], -8.5f);
            dict[x*NumSpots + 1] = new Vector3(-12.5f, verticalPositions[x], -4.3f);
            dict[x*NumSpots + 2] = new Vector3(-12.5f, verticalPositions[x], -0.1f);
            dict[x*NumSpots + 3] = new Vector3(-12.5f, verticalPositions[x], 4.1f);
            dict[x*NumSpots + 4] = new Vector3(-1f, verticalPositions[x], -8.5f);
            dict[x*NumSpots + 5] = new Vector3(-1f, verticalPositions[x], -4.3f);
            dict[x*NumSpots + 6] = new Vector3(-1f, verticalPositions[x], -0.1f);
            dict[x*NumSpots + 7] = new Vector3(-1f, verticalPositions[x], 4.1f);
        }

        scenes = new ArrayList();
        for(int x = 0; x < NUM_SAMPLES; x++)
            scenes.Add(new Scene(van, truck, mustdang, chevy, sports));
        save(path);
     }

     private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            step();  
        }
    }

    void Update(){
        step();
    }
 
     // Update is called once per frame
    public void step(){
        if(curr < NUM_SAMPLES){
            clearCars(currObjects);
            for(int x = 0; x < 8; x++){
                Scene s = (Scene) scenes[curr];
                if(s.lot[x] != null){
                    Car c = s.lot[x];
                    placeCar(x, c.car);
                } 
            }
            curr += 1;
        }
        else{
            Application.Quit();
            Debug.Break();
        }
     }

     void placeCar(int place, GameObject car){
         if(car == null){
             return;
         }
         int constant = 0;
         if(car == van)
             constant = 0;
         else if(car == truck)
             constant = 1;
         else if(car == mustdang)
             constant = 2;
        else if(car == chevy)
             constant = 3;
        else if(car == sports)
             constant = 4;

        Vector3 randomness = new Vector3(UnityEngine.Random.Range(0f, 0.5f)-0.25f, 0f, UnityEngine.Random.Range(0f, 1f)-0.5f);
        GameObject item = Instantiate(car, dict[(constant * NumSpots) + place] + randomness, Quaternion.identity);
        currObjects.Add(item);
        if(place > 3)
            item.transform.Rotate(0.0f, 270.0f, 0.0f, Space.Self);
        else
            item.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
     }

     void clearCars(ArrayList objects){
        foreach(GameObject item in objects){
            Destroy(item);
        }
        ArrayList currObjects = new ArrayList();
     }

    public void save(string filename){
        //lot
        string massive_string = "{\"all_car_models\": [\"Yellow\", \"Green\", \"Brown\", \"Purple\", \"Blue\", \"Empty\"], \"image_filename_labels\": {";
        for(int x = 0; x < NUM_SAMPLES; x++){
            Scene s = (Scene) scenes[x];
            Dictionary<int, string> cars = new Dictionary<int, string>();
            string big_string = "";
            for(int y = 0; y < 8; y++){
                if(s.lot[y] == null){
                    cars[y] = "null";
                }
                else{ 
                    cars[y] = s.lot[y].color;
                }
                string final = "\"" + y + "\":\"" + cars[y] + "\"";
                if(y != 7){
                    final += ",\n";
                }
                big_string += final;
            }
            massive_string += "\"" + x + ".png" + "\":{" + big_string + "}";
            if(x != NUM_SAMPLES-1){
                    massive_string += ",\n";
                }
                
            File.WriteAllText (filename, massive_string + "}}");
        }
    }
 } 