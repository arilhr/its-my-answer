using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestPlayerAnimator : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public GameObject playerModel;

    private Animator anim;

    private void Start()
    {
        if (pv.IsMine)
        {
            GameObject playerModelObject = PhotonNetwork.Instantiate("Player Models/" + playerModel.name, transform.position, Quaternion.identity);
            anim = playerModelObject.GetComponent<Animator>();
        }
    }

    public void Run()
    {
        anim.SetBool("isRunning", true);
    }

    public void Unrun()
    {
        anim.SetBool("isRunning", false);
    }

    public static GameObject GetPlayer(Player owner)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in playerObjects)
        {
            TestPlayerAnimator player = p.GetComponent<TestPlayerAnimator>();

            if (player != null)
            {
                if (player.photonView.Owner == owner)
                {
                    return p;
                }
            }
        }

        return null;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TestPlayerAnimator))]
public class TestPlayerAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TestPlayerAnimator pa = (TestPlayerAnimator)target;

        if (GUILayout.Button("RUN"))
        {
            pa.Run();
        }

        if (GUILayout.Button("UNRUN"))
        {
            pa.Unrun();
        }
    }
}
#endif