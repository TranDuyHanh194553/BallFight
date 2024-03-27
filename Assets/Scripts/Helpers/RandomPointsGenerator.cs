using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointsGenerator : MonoBehaviour
{
    public static float minDistance = 2f; // Khoảng cách tối thiểu giữa các điểm
    public static List<Vector3> randomPoints = new List<Vector3>();

    public static List<GameObject> GenerateObjectsByListPrefabs(List<GameObject> originPrefabs,  int numberOfPoints, Vector3 minPosition, Vector3 maxPosition)
    {
        List<GameObject> randomObjects = new List<GameObject>();
        randomPoints.Clear();

        for (int i = 0; i < numberOfPoints; i++)
        {

            int index = Random.Range(0, originPrefabs.Count);
            Vector3 randomVector3 = GenerateRandomPoint(minPosition, maxPosition);
            GameObject newObject = Instantiate(originPrefabs[index], randomVector3, originPrefabs[index].transform.rotation);
            randomPoints.Add(randomVector3);
            randomObjects.Add(newObject);
        }

        return randomObjects;
    }

    public static List<GameObject> GenerateObjectsByPrefab(GameObject originPrefab, int numberOfPoints, Vector3 minPosition, Vector3 maxPosition)
    {
        List<GameObject> randomObjects = new List<GameObject>();
        randomPoints.Clear();

        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 randomVector3 = GenerateRandomPoint(minPosition, maxPosition);
            GameObject newObject = Instantiate(originPrefab, randomVector3, originPrefab.transform.rotation);
            randomPoints.Add(randomVector3);
            randomObjects.Add(newObject);
        }

        return randomObjects;
    }


    public static Vector3 GenerateRandomPoint(Vector3 minPosition, Vector3 maxPosition)
    {
        int maxAttempts = 1000; // Số lần tạo điểm tối đa

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 newPoint = new Vector3(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                Random.Range(minPosition.z, maxPosition.z)
            );

            bool isTooClose = false;

            foreach (Vector3 existingPoint in randomPoints)
            {
                Debug.Log("GenerateRandomPoint newPoint distance:" + Vector3.Distance(newPoint, existingPoint));

                if (Vector3.Distance(newPoint, existingPoint) < minDistance)
                {
                    isTooClose = true;
                    break; // Thoát khỏi vòng lặp nếu quá gần
                }
            }

            if (!isTooClose)
            {
                randomPoints.Add(newPoint); // Thêm điểm mới vào danh sách
                return newPoint; // Trả về điểm mới tạo
            }
        }

        // Trả về một điểm bất kỳ nếu không tạo được điểm thỏa mãn
        return new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z)
        );
    }
}