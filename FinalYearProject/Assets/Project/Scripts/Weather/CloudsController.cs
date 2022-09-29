using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour
{
    public GameObject cloudPrefab;

    public void AddCloud(int cloudIntensity,float maxDistance)
    {
        float maxSize = 50 + cloudIntensity;
        float randPosX, randPosY, randPosZ;
        for (int i = 0; i < cloudIntensity * 20; ++i)
        {
            float randSize = Random.Range(1, maxSize);
            if (cloudIntensity < 8)
            {
                randPosX = Random.Range(-maxDistance, maxDistance) / cloudIntensity;
                randPosY = Random.Range(20000, 30000);
                randPosZ = Random.Range(-maxDistance, maxDistance) / cloudIntensity;
            }
            else
            {
                randSize = Random.Range(maxSize * 0.5f, maxSize);
                randPosX = Random.Range(-maxDistance * 2.0f, maxDistance * 2.0f) / cloudIntensity;
                randPosY = Random.Range(25000, 40000);
                randPosZ = Random.Range(-maxDistance * 2.0f, maxDistance * 2.0f) / cloudIntensity;
            }

            Vector3 toRotate = new Vector3(randPosX,randPosY,randPosZ) - transform.position;
            toRotate.Normalize();
            Quaternion desiredRotation = Quaternion.LookRotation(toRotate);


            var cloudVariant = Instantiate(cloudPrefab, new Vector3(randPosX,randPosY,randPosZ), desiredRotation);
            cloudVariant.transform.localScale = new Vector3(randSize, randSize, randSize);//.one * randSize;
            cloudVariant.transform.parent = transform;
        }

        AddAdditionalCloud((int)1.5f * cloudIntensity, 10000, cloudIntensity);
    }

    public void AddAdditionalCloud(int additionalCloud,float maxDistance,int cloudIntensity)
    {
        if (cloudIntensity <= 0)
            additionalCloud = 0;
            
        float maxSize = 5;
        float randPosX, randPosY, randPosZ;
        for (int i = 0; i < additionalCloud; ++i)
        {
            float randSize = Random.Range(1, maxSize);
            if (cloudIntensity < 8)
            {
                randPosY = Random.Range(20000, 30000);
            }
            else
            {
                randPosY = Random.Range(25000, 30000);
                maxDistance = 10000;
            }
            randPosX = Random.Range(-maxDistance, maxDistance);
            randPosZ = Random.Range(-maxDistance, maxDistance);

            Vector3 toRotate = new Vector3(randPosX, randPosY, randPosZ) - transform.position;
            toRotate.Normalize();
            Quaternion desiredRotation = Quaternion.LookRotation(toRotate);


            var cloudVariant = Instantiate(cloudPrefab, new Vector3(randPosX, randPosY, randPosZ), desiredRotation);
            cloudVariant.transform.localScale = Vector3.one * randSize;
            cloudVariant.transform.parent = transform;
        }
    }

    public void ClearCloud()
    {
        foreach(Transform cloud in this.transform)
        {
            Destroy(cloud.gameObject);
        }
    }
}
