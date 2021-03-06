using System.IO;
using UnityEngine;
 
public class SR_RenderCamera : MonoBehaviour {
 
    public int FileCounter = -1;

    public int Max;

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
    
    void step()
    {
        if(FileCounter <= Max && FileCounter > 0){
            Camera Cam = GetComponent<Camera>();
    
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = Cam.targetTexture;
            
    
            Cam.Render();
            Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
            Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;
    
            var Bytes = Image.EncodeToJPG();
            Destroy(Image);
    
            File.WriteAllBytes(Application.dataPath + "/TrainingImages/" + (FileCounter-1) + ".jpg", Bytes);
            Debug.Log(FileCounter);
        }
        FileCounter++;
    }
}