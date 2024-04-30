using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastorData : MonoBehaviour, IDataPersistance
{
    [SerializeField] DialogueComponent GoodEnd;
    [SerializeField] DialogueComponent NeutralEnd;
    [SerializeField] DialogueComponent BadEnd;




    public void LoadData(GameData data)
    {
        switch (data.GiftedDandelions)
        {
            case 6:

                Destroy(NeutralEnd);
                Destroy(BadEnd);

                break;

            case -6:

                Destroy(NeutralEnd);
                Destroy(GoodEnd);

                break;

            default:

                Destroy(GoodEnd);
                Destroy(BadEnd);

                break;
        }
    }

    public void SaveData(ref GameData data)
    {

    }
}
