using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    public Color floorColor;
    public float toothstalkerSpeed;
    public float toothstalkerPauseDuration;
    public float toothstalkerDetectionRadius;
    public LevelConfig nextLevel;
}
