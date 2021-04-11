using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using System.IO;
 using System;
 
 public class CarSpawner : MonoBehaviour {
     int NUM_SAMPLES = 1000;

     public GameObject van;
     public GameObject truck;
     public GameObject mustdang;
     public GameObject chevy;
     public GameObject sports;

     string path;
     string[] jsonString;
     public int count = 0;
     public int curr = 0;
     public int NumSpots = 8;

     public Dictionary<int, Vector3> dict;
     System.Random rnd = new System.Random();
     public Scene s;
     ArrayList currObjects = new ArrayList();
     // Use this for initialization
     ArrayList scenes;
     void Start () {
        path = Application.dataPath + "/TrainingImages/positions.txt";

        Dictionary<int, float> verticalPositions = new Dictionary<int, float>();
        verticalPositions[0] = 0f;
        verticalPositions[1] = 1.6f;
        verticalPositions[2] = 1.1f;
        verticalPositions[3] = 0.9f;
        verticalPositions[4] = 0.9f;

        dict = new Dictionary<int, Vector3>();
        for(int x = 0; x < 5; x++){
            dict[x*NumSpots + 0] = new Vector3(-13f+((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], -8.5f);
            dict[x*NumSpots + 1] = new Vector3(-13f+((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], -4.3f);
            dict[x*NumSpots + 2] = new Vector3(-13f+((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], -0.1f);
            dict[x*NumSpots + 3] = new Vector3(-13f+((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], 4.1f);
            dict[x*NumSpots + 4] = new Vector3(((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], -8.5f);
            dict[x*NumSpots + 5] = new Vector3(((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], -4.3f);
            dict[x*NumSpots + 6] = new Vector3(((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], -0.1f);
            dict[x*NumSpots + 7] = new Vector3(((float) rnd.NextDouble() * 1.0f) - 0.5f, verticalPositions[x], 4.1f);
        }

        scenes = new ArrayList();
        for(int x = 0; x < NUM_SAMPLES; x++){
            scenes.Add(new Scene(van, truck, mustdang, chevy, sports));
        }
        save(path);
     }

     private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            incrament();  
        }
    }
 
     // Update is called once per frame
     void incrament () {
         if(curr < NUM_SAMPLES){
            clearCars(currObjects);
            for(int x = 0; x < 8; x++){
                Scene s = (Scene) scenes[curr];
                if(s.lot[x] != null){
                    Car c = s.lot[x];
                    print("Placing car " + x);
                    print(s.lot[x]);
                    print(c);
                    placeCar(x, c.car);
                } 
                
            }
             curr += 1;
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

        GameObject item = Instantiate(car, dict[(constant * NumSpots) + place], Quaternion.identity);
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
        string massive_string = "";
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
                
            File.WriteAllText (filename, massive_string);
        }
    }
 } 
 
 

public class Scene{
        
    int MIN_CARS = 0;
    int MAX_CARS = 5;
    int LOTSIZE = 8;
    float SPAWN_RATE = 0.35f;

    public GameObject van;
     public GameObject truck;
     public GameObject mustdang;
     public GameObject chevy;
     public GameObject sports;


    public Dictionary<string, ArrayList> CARS = new Dictionary<string, ArrayList>();
    
    public Dictionary<int, Car> lot = new Dictionary<int, Car>();
    public Scene(GameObject v, GameObject t, GameObject m, GameObject c, GameObject s){ 
        van = v;
        truck = t;
        mustdang = m;
        chevy = c;
        sports = s;

        CARS["VAN"] = new ArrayList(){"Green", "Van", van};
        CARS["TRUCK"] = new ArrayList(){"Yellow", "Pick-up Truck", truck};
        CARS["MUSTANG"] = new ArrayList(){"Blue", "Sports Car", mustdang};
        CARS["CHEVY"] = new ArrayList(){"Brown", "Sedan", chevy};
        CARS["SPORTS"] = new ArrayList(){"Purple", "Sports Car", sports};
        
        ArrayList sample_pool = new ArrayList(CARS.Keys);
        reshuffle(sample_pool);

        System.Random rnd = new System.Random();

        for(int k = 0; k < LOTSIZE; k++){
            if(rnd.NextDouble() < SPAWN_RATE){
                if(sample_pool.Count > 0){
                    ArrayList car_info = CARS[(String) sample_pool[0]];
                    sample_pool.RemoveAt(0);
                    lot[k] = new Car((string) car_info[0], (string) car_info[1], (GameObject) car_info[2]);
                }
            }
            if(!lot.ContainsKey(k)){
                lot[k] = null;
            }
        }
    }

    private void reshuffle(ArrayList texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :slight_smile:
        for (int t = 0; t < texts.Count; t++ )
        {
            object tmp = texts[t];
            int r = UnityEngine.Random.Range(t, texts.Count);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }


}

public class Car{
    public string color;
    private string type;
    private string plate;

    public GameObject car;

    public Car(string car_color, string car_type, GameObject car_item){
        color = car_color;
        type = car_type;
        plate = generate_plate();
        car = car_item;
    }

    private string generate_plate(){
        return "";
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "0123456789";
        //return "".join(random.sample(alpha, 3)) + "-" + "".join(random.sample(numbers, 4))
    }
}