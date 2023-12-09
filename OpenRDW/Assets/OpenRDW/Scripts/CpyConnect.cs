using System.Collections;
using System.IO;
using UnityEngine;

public class CpyConnect : MonoBehaviour
{
    public string filePath = "Assets/locrot_data.txt"; // Adjust the path as needed
    public float updateInterval = 0.1f; // Update interval in seconds

    private GameObject cubeObject;

    private void Start()
    {
        // Instantiate a cube object
        cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Start the coroutine to update cube position and rotation
        StartCoroutine(UpdateCubePositionAndRotation());
    }

    private IEnumerator UpdateCubePositionAndRotation()
    {
        while (true)
        {
            // Use File.Open with appropriate FileShare settings to handle sharing violations
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Use a StreamReader to read lines from the file
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    // Read data from the file
                    string line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] data = line.Split(' ');

                        // Check if there are enough elements in the array
                        if (data.Length >= 16)
                        {
                            // Attempt to parse position data
                            if (float.TryParse(data[0], out float posX) &&
                                float.TryParse(data[1], out float posY) &&
                                float.TryParse(data[2], out float posZ))
                            {
                                // Attempt to parse rotation matrix data
                                Matrix4x4 rotationMatrix = new Matrix4x4();
                                int matrixIndex = 3;
                                for (int i = 0; i < 3; i++)
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (float.TryParse(data[matrixIndex], out float matrixValue))
                                        {
                                            rotationMatrix[i, j] = matrixValue;
                                            matrixIndex++;
                                        }
                                        else
                                        {
                                            Debug.LogError("Error parsing rotation matrix data.");
                                            yield break; // Exit the coroutine on error
                                        }
                                    }
                                }

                                // Attempt to parse quaternion data
                                if (float.TryParse(data[12], out float quatX) &&
                                    float.TryParse(data[13], out float quatY) &&
                                    float.TryParse(data[14], out float quatZ) &&
                                    float.TryParse(data[15], out float quatW))
                                {
                                    // Update cube position
                                    cubeObject.transform.position = new Vector3(posX, posY, posZ);
                                    cubeObject.transform.rotation = new Quaternion(quatX, quatY, quatZ, quatW);
                                }
                                else
                                {
                                    Debug.LogError("Error parsing quaternion data.");
                                }
                            }
                            else
                            {
                                Debug.LogError("Error parsing position data.");
                            }
                        }
                        else
                        {
                            Debug.LogError("Insufficient data elements in the array.");
                        }
                    }

                    // Wait for the next update interval
                    yield return new WaitForSeconds(updateInterval);
                }
            }
        }
    }
}