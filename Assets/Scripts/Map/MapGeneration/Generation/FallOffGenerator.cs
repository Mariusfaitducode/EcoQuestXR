using UnityEngine;

namespace MapGeneration.Generation
{
    public static class FalloffGenerator {
        

        public static float[,] GenerateFalloffMap(int size, float falloffStart, float falloffEnd) {
            float[,] map = new float[size,size];
        
            // Debug.Log(falloffStart);

            for (int i = 0; i < size; i++) {
                for (int j = 0; j < size; j++) {
                    float x = i / (float)size * 2 - 1;
                    float y = j / (float)size * 2 - 1;

                    float t = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));
                    // map [i, j] = Evaluate(t);
                
                    if (t < falloffStart) {
                        map [i, j] = 0;
                    } else if (t > falloffEnd) {
                        map [i, j] = 1;
                    }
                    else
                    {
                        // map[i, j] = Mathf.Clamp01((t - falloffStart) / (falloffEnd - falloffStart));
                    
                        map[i, j] = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(falloffStart, falloffEnd, t));
                    
                    }

                }
            }

            return map;
        }

        static float Evaluate(float value) {
            float a = 3;
            float b = 2.2f;

            return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
        }
    }
}