using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour
{
    public GameObject cloudPrefab;
    [SerializeField] float cloudMaxSize;

    GameObject[] cloudAreaParent = new GameObject[9];

    public void AddCloud(int cloudIntensity,float maxDistance)
    {
        float randPosX, randPosY, randPosZ;

        float maxSize = cloudMaxSize + cloudIntensity * 2;

        for (int i = 0; i < cloudIntensity * 20; ++i)
        {
            float randSize = Random.Range(1, maxSize);
            if (cloudIntensity < 8)
            {
                randPosX = Random.Range(-maxDistance, maxDistance);
                randPosY = Random.Range(20000, 30000);
                randPosZ = Random.Range(-maxDistance, maxDistance);
            }
            else
            {
                randSize = Random.Range(maxSize * 0.5f, maxSize);
                randPosX = Random.Range(-maxDistance * 2.0f, maxDistance * 2.0f);
                randPosY = Random.Range(25000, 40000);
                randPosZ = Random.Range(-maxDistance * 2.0f, maxDistance * 2.0f);
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

        float maxSize = cloudMaxSize;

        float randPosX, randPosY, randPosZ;
        for (int i = 0; i < additionalCloud; ++i)
        {
            float randSize = Random.Range(maxSize / 2, maxSize);
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

    public void AddCloudWithButton(int noOfClouds, float maxDistance,int buttonNo)
    {
        if (!cloudAreaParent[buttonNo])
        {
            cloudAreaParent[buttonNo] =  new GameObject("cloudArea" + buttonNo.ToString());
            cloudAreaParent[buttonNo].transform.SetParent(transform.parent);
        }

        else if (cloudAreaParent[buttonNo])
        {
            ClearCloudByArea(buttonNo);
        }


        float randPosX, randPosY, randPosZ;
        float distanceFromEachGrid = maxDistance * 2 / 3f;
        int currZGrid = (buttonNo - 1) / 3;
        int currXGrid = buttonNo - 1;

        for (int i = 0; i < noOfClouds; ++i)
        {
            float randSize = Random.Range(cloudMaxSize * 0.5f, cloudMaxSize);

            randPosX = Random.Range(-maxDistance + distanceFromEachGrid * currXGrid, -maxDistance + distanceFromEachGrid * (currXGrid + 1)) - maxDistance * 2 * currZGrid;
            randPosY = Random.Range(25000, 30000);
            randPosZ = Random.Range(maxDistance - distanceFromEachGrid * currZGrid, maxDistance - distanceFromEachGrid * (currZGrid + 1));

            Vector3 toRotate = new Vector3(randPosX, randPosY, randPosZ) - transform.position;
            toRotate.Normalize();
            Quaternion desiredRotation = Quaternion.LookRotation(toRotate);


            var cloudVariant = Instantiate(cloudPrefab, new Vector3(randPosX, randPosY, randPosZ), desiredRotation);
            cloudVariant.transform.localScale = Vector3.one * randSize;
            cloudVariant.transform.parent = cloudAreaParent[buttonNo].transform;
        }
    }
    public void ClearCloud()
    {
        foreach(Transform cloud in this.transform)
        {
            Destroy(cloud.gameObject);
        }
    }
    public void ClearCloudByArea(int buttonNo)
    {
        foreach (Transform cloud in cloudAreaParent[buttonNo].transform)
        {
            Destroy(cloud.gameObject);
        }
    }

    public void ClearAllCloudByArea()
    {
        for(int i = 0;i< cloudAreaParent.Length;i++)
        { 
            Destroy(cloudAreaParent[i]);
        }
    }
}
