using UnityEngine;

public class AnimatedShaders : MonoBehaviour
{
    public Material DeviceScreen;
    public Material DeviceHighlights;
    public Material DeviceEnvGround;
    public Material DeviceEnvFog;

    void Start()
    {
        DeviceScreen.SetFloat("_Speed", 0.5f);

        DeviceHighlights.SetFloat("_Speed", 1.0f);

        DeviceEnvGround.SetVector("_PositionAnimation", new Vector4(0f, 0.2f, 0f, 0f));

        DeviceEnvFog.SetFloat("_layer1_Speed", 0.5f);
        DeviceEnvFog.SetFloat("_layer2_Speed", 1.0f);
    }

    void OnApplicationQuit()    
    {
        DeviceScreen.SetFloat("_Speed", 0f);
        DeviceHighlights.SetFloat("_Speed", 0f);
        DeviceEnvGround.SetVector("_PositionAnimation", Vector4.zero);
        DeviceEnvFog.SetFloat("_layer1_Speed", 0f);
        DeviceEnvFog.SetFloat("_layer2_Speed", 0f);
    }
}
