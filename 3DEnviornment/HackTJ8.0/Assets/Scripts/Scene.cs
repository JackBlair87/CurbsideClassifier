using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scene{
    int LOTSIZE = 8;
    float SPAWN_RATE = 0.35f;

    public Dictionary<string, ArrayList> CARS = new Dictionary<string, ArrayList>();
    
    public Dictionary<int, Car> lot = new Dictionary<int, Car>();
    public Scene(GameObject v, GameObject t, GameObject m, GameObject c, GameObject s){ 
        CARS["VAN"] = new ArrayList(){"Green", "Van", v};
        CARS["TRUCK"] = new ArrayList(){"Yellow", "Pick-up Truck", t};
        CARS["MUSTANG"] = new ArrayList(){"Blue", "Sports Car", m};
        CARS["CHEVY"] = new ArrayList(){"Brown", "Sedan", c};
        CARS["SPORTS"] = new ArrayList(){"Purple", "Sports Car", s};
        
        ArrayList sample_pool = new ArrayList(CARS.Keys);
        shuffle(sample_pool);

        for(int k = 0; k < LOTSIZE; k++){
            if(UnityEngine.Random.Range(0, 100) < (SPAWN_RATE*100)){
                if(sample_pool.Count > 0){
                    ArrayList car_info = CARS[(String) sample_pool[0]];
                    sample_pool.RemoveAt(0);
                    lot[k] = new Car((string) car_info[0], (string) car_info[1], (GameObject) car_info[2]);
                }
            }
            if(!lot.ContainsKey(k))
                lot[k] = null;
        }
    }

    private void shuffle(ArrayList vehicles)
    {
        for (int t = 0; t < vehicles.Count; t++ ){
            object tmp = vehicles[t];
            int r = UnityEngine.Random.Range(t, vehicles.Count);
            vehicles[t] = vehicles[r];
            vehicles[r] = tmp;
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
        //According to Virginia State Laws
        plate = "";
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string numbers = "0123456789";

        for(int x = 0; x < 3; x++)
            plate += alpha[(int) UnityEngine.Random.Range(0, alpha.Length)];
        plate += "-";
        for(int x = 0; x < 4; x++)
            plate += numbers[(int) UnityEngine.Random.Range(0, numbers.Length)];
        return plate;
    }
}
